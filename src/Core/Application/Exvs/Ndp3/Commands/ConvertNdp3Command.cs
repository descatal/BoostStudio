using System.Numerics;
using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Formats;
using SharpGLTF.Geometry;
using SharpGLTF.Geometry.VertexTypes;
using SharpGLTF.Materials;
using SharpGLTF.Schema2;

namespace BoostStudio.Application.Exvs.Ndp3.Commands;

public record ConvertNdp3Command(Stream File) : IRequest<byte[]>;

public class ConvertNdp3CommandHandler(INdp3BinarySerializer ndp3BinarySerializer)
    : IRequestHandler<ConvertNdp3Command, byte[]>
{
    public async ValueTask<byte[]> Handle(
        ConvertNdp3Command request,
        CancellationToken cancellationToken
    )
    {
        var serializedFormat = await ndp3BinarySerializer.DeserializeAsync(
            request.File,
            cancellationToken
        );

        var mesh = new MeshBuilder<VertexPosition>("mesh");

        foreach (var meshData in serializedFormat.Meshes)
        {
            foreach (var polyData in meshData.Polygons)
            {
                var materials = polyData
                    .Materials.Select((x, i) => new MaterialBuilder("material"))
                    .ToList();

                foreach (var material in materials)
                {
                    var prim = mesh.UsePrimitive(material);
                    // prim.AddTriangle();
                }
            }
        }

        // create a scene
        var scene = new SharpGLTF.Scenes.SceneBuilder();
        scene.AddRigidMesh(mesh, Matrix4x4.Identity);

        var temporaryFileName = Path.GetTempFileName();
        try
        {
            var model = scene.ToGltf2();
            model.SaveAsWavefront(temporaryFileName);
            return await File.ReadAllBytesAsync(temporaryFileName, cancellationToken);
        }
        finally
        {
            if (File.Exists(temporaryFileName))
                File.Delete(temporaryFileName);
        }
    }
}
