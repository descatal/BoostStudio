using System.Numerics;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Application.Common.Models;
using SharpGLTF.Geometry;
using SharpGLTF.Geometry.VertexTypes;
using SharpGLTF.Materials;

namespace BoostStudio.Application.Exvs.Ndp3.Commands;

public record ConvertNdp3Command(Stream Ndp3File, Stream? VbnFile = null, string? FileName = null)
    : IRequest<byte[]>;

public class ConvertNdp3CommandHandler(
    INdp3BinarySerializer ndp3BinarySerializer,
    ICompressor compressor
) : IRequestHandler<ConvertNdp3Command, byte[]>
{
    public async ValueTask<byte[]> Handle(
        ConvertNdp3Command request,
        CancellationToken cancellationToken
    )
    {
        var serializedNdp3Format = await ndp3BinarySerializer.DeserializeAsync(
            request.Ndp3File,
            cancellationToken
        );

        // create a scene
        var name = request.FileName ?? "model";
        var scene = new SharpGLTF.Scenes.SceneBuilder(name);

        foreach (var meshData in serializedNdp3Format.Meshes)
        {
            var mesh = new MeshBuilder<VertexPosition>(meshData.Name);

            foreach (var polygonData in meshData.Polygons)
            {
                // from other NUD parsing logic, it seems like there's possibilities of more than one material
                // if that's the case we'll have to determine which primitives belongs to which material
                // for now it should be ok to just use 1 material
                var material = polygonData
                    .Materials.Where(data => data.Body is not null)
                    .Select((_, index) => new MaterialBuilder($"material_{index}"))
                    .FirstOrDefault();

                var primitiveBuilder = mesh.UsePrimitive(material);

                var vertexPositions = polygonData
                    .Vertices.Select(data => new VertexPosition(data.Pos.X, data.Pos.Y, data.Pos.Z))
                    .ToList();

                var processedIndices = ProcessVertexIndices(polygonData.VertexIndices);
                var index = 0;
                do
                {
                    var vertex1 = vertexPositions[processedIndices[index++]];
                    var vertex2 = vertexPositions[processedIndices[index++]];
                    var vertex3 = vertexPositions[processedIndices[index++]];

                    primitiveBuilder.AddTriangle(vertex1, vertex2, vertex3);
                } while (processedIndices.Count > index);
            }

            scene.AddRigidMesh(mesh, Matrix4x4.Identity);
        }

        var tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

        try
        {
            Directory.CreateDirectory(tempDirectory);
            var filePath = Path.Combine(tempDirectory, name);

            var model = scene.ToGltf2();
            model.SaveGLTF(filePath);

            return await compressor.CompressAsync(
                tempDirectory,
                CompressionFormats.Tar,
                cancellationToken
            );
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
                Directory.Delete(tempDirectory, true);
        }
    }

    private const int TriangleStripSeparator = 0xFFFF;

    /// <summary>
    /// Processes triangle strip vertex indices and converts them to individual triangles
    /// </summary>
    /// <param name="vertexIndices">The source vertex indices containing triangle strips</param>
    /// <returns>List of vertex indices for individual triangles</returns>
    private static List<ushort> ProcessVertexIndices(List<ushort> vertexIndices)
    {
        ArgumentNullException.ThrowIfNull(vertexIndices);

        if (vertexIndices.Count < 3)
            return vertexIndices;

        var resultIndices = new List<ushort>();
        var position = 0;

        // Initialize sliding window of two vertices
        var slidingWindow = (vertexIndices[position++], vertexIndices[position++]);
        var isClockwise = true;

        while (position < vertexIndices.Count)
        {
            var currentVertex = vertexIndices[position++];

            if (currentVertex == TriangleStripSeparator)
            {
                // Check if end of list
                if (position + 1 >= vertexIndices.Count)
                {
                    continue;
                }

                // Reset for new triangle strip
                slidingWindow = (vertexIndices[position++], vertexIndices[position++]);
                isClockwise = true;
            }
            else
            {
                isClockwise = !isClockwise;

                (ushort firstVertex, ushort secondVertex) = slidingWindow;

                // Check for degenerate triangle - all vertices must be distinct
                if (new HashSet<ushort> { firstVertex, secondVertex, currentVertex }.Count == 3)
                {
                    List<ushort> vertexRange = isClockwise
                        ? [currentVertex, secondVertex, firstVertex]
                        : [secondVertex, currentVertex, firstVertex];

                    resultIndices.AddRange(vertexRange);
                }

                // Shift window for next iteration
                slidingWindow = (secondVertex, currentVertex);
            }
        }

        return resultIndices;
    }
}
