using System.Numerics;
using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Common.Utils;
using BoostStudio.Application.Exvs.Ndp3.Commands.Models;
using BoostStudio.Formats;
using Riok.Mapperly.Abstractions;
using SharpGLTF.Animations;
using SharpGLTF.Geometry;
using SharpGLTF.Geometry.VertexTypes;
using SharpGLTF.Materials;
using SharpGLTF.Scenes;
using SharpGLTF.Schema2;
using SharpGLTF.Transforms;
using FileInfo = BoostStudio.Application.Common.Models.FileInfo;

namespace BoostStudio.Application.Exvs.Ndp3.Commands;

// very good official writeup on gltf format:
// https://github.khronos.org/glTF-Tutorials/gltfTutorial/gltfTutorial_020_Skins.html
public record ConvertNdp3Command(Stream Ndp3File, Stream? VbnFile = null, string? FileName = null)
    : IRequest<FileInfo>;

public class ConvertNdp3CommandHandler(
    ICompressor compressor,
    INdp3BinarySerializer ndp3BinarySerializer,
    IVbnBinarySerializer vbnBinarySerializer
) : IRequestHandler<ConvertNdp3Command, FileInfo>
{
    public async ValueTask<FileInfo> Handle(
        ConvertNdp3Command request,
        CancellationToken cancellationToken
    )
    {
        var serializedNdp3Format = await ndp3BinarySerializer.DeserializeAsync(
            request.Ndp3File,
            cancellationToken
        );

        VbnBinaryFormat? serializedVbnFormat = null;
        if (request.VbnFile is not null)
        {
            serializedVbnFormat = await vbnBinarySerializer.DeserializeAsync(
                request.VbnFile,
                cancellationToken
            );
        }

        // create a scene
        var name = request.FileName ?? "model";
        var scene = new SceneBuilder(name);

        var boneData = serializedVbnFormat?.Bones ?? [];
        var joints = boneData
            .Select(data => new BoneData
            {
                Node = new NodeBuilder(data.Name),
                BindMatrix = data.BindMatrix.ToMatrix(),
                InverseBindMatrix = data.InverseBindMatrix.ToMatrix(),
                LocalRotation = data.LocalTransform.Rotation.ToVector(),
                LocalTranslation = data.LocalTransform.Translation.ToVector(),
                LocalScale = data.LocalTransform.Scale.ToVector(),
            })
            .ToList();

        var boneHierarchyMap = boneData
            .Select((data, index) => new { index, data.ParentBoneIndex })
            .GroupBy(data => data.ParentBoneIndex)
            .ToDictionary(
                grouping => grouping.Key,
                grouping => grouping.Select(index => index.index).ToArray()
            );

        // === BONE HIERARCHY TRANSFORMATION ===
        // Blender has two bone states:
        // - REST POSITION: The neutral "blueprint" skeleton, used for rigging/weight painting
        // - POSE POSITION: The animated/transformed skeleton, what we see during animation
        //
        // This code converts from Rest to Pose by:
        // 1. Taking each bone's inverse bind matrix (from Rest Position)
        // 2. Applying parent transformations down the chain (hierarchical)
        // 3. Computing world matrices so each bone knows its final position in 3D space
        //
        // WORLD MATRIX = the bone's absolute position/rotation/scale in 3D world space
        // (as opposed to local/relative to parent)

        // Traverse bone hierarchy and compute world-space transformations for each bone
        // This builds the pose by applying parent transformations down the bone chain
        foreach ((int parentBoneIndex, int[] childBoneIndexes) in boneHierarchyMap)
        {
            var parentJoint = joints.ElementAtOrDefault(parentBoneIndex);
            if (parentJoint is null)
                continue;

            // Get parent's world matrix - either use computed value or compute from local transform
            // WORLD MATRIX = bone's absolute transform in 3D space (not relative to parent)
            // This represents where the parent bone actually is in world coordinates
            var parentWorldMatrix =
                parentJoint.WorldMatrix
                ?? parentJoint.LocalRotation.ToMatrix() with
                {
                    Translation = parentJoint.LocalTranslation,
                };

            // Process all children of this parent bone
            // Each child needs parent's world transform applied to get correct Pose Position
            foreach (var childBoneIndex in childBoneIndexes)
            {
                var childJoint = joints.ElementAtOrDefault(childBoneIndex);
                if (childJoint is null)
                    continue;

                // Transform child from REST POSITION to POSE POSITION (world space):
                // 1. InverseBindMatrix: Converts from bind/rest pose to bone-local space
                //    (InverseBindMatrix undoes the Rest Position transform)
                // 2. Multiply by parent world: Applies parent chain transformations
                //    (This is why we need world matrices - bones inherit parent poses)
                var childWorldMatrix = childJoint.InverseBindMatrix * parentWorldMatrix;

                // Extract only the rotation component from world matrix for the node's local rotation
                // We decompose because we want world-space rotation but keep local translation/scale
                // (Translation and scale are kept as original local values)
                Matrix4x4.Decompose(childWorldMatrix, out _, out var worldRotation, out _);

                // Update the scene node with transform components:
                // - Local translation/scale remain unchanged (bone's own offset from parent)
                // - Rotation is set to the computed world-space rotation (accumulated from chain)
                childJoint
                    .Node.WithLocalTranslation(childJoint.LocalTranslation)
                    .WithLocalScale(childJoint.LocalScale)
                    .WithLocalRotation(worldRotation);

                // Compute and cache child's world matrix for its own children to use next
                // This becomes the parent matrix for the next level in the hierarchy
                // (Child bones need this parent's final world position to compute their own)
                var localTransformMatrix = childJoint.LocalRotation.ToMatrix();
                childJoint.WorldMatrix = parentWorldMatrix * localTransformMatrix;

                // Establish parent-child relationship in the scene graph
                // This ensures the bone hierarchy is properly structured
                parentJoint.Node.AddNode(childJoint.Node);
            }
        }

        var bindings = joints.Select(data => (data.Node, data.BindMatrix)).ToArray();
        foreach (var meshData in serializedNdp3Format.Meshes)
        {
            for (int polygonIndex = 0; polygonIndex < meshData.Polygons.Count; polygonIndex++)
            {
                var polygonData = meshData.Polygons[polygonIndex];

                // create mesh (vertex group), e.g. SHAPE_ROOT_3
                var mesh = new MeshBuilder<VertexPositionNormalTangent, VertexEmpty, VertexJoints4>(
                    $"{meshData.Name}_{polygonIndex}"
                );

                // from other NUD parsing logic, it seems like there's possibilities of more than one material
                // if that's the case we'll have to determine which primitives belongs to which material
                // for now it should be ok to just use 1 material
                var material = polygonData
                    .Materials.Where(data => data.Body is not null)
                    .Select((_, index) => new MaterialBuilder($"material_{index}"))
                    .FirstOrDefault();

                var primitiveBuilder = mesh.UsePrimitive(material);

                // create GLTFSharp compatible vertex data structure
                var vertices = polygonData
                    .Vertices.Attributes.Select(vertexAttribute =>
                    {
                        // TODO: add color and UV

                        var vertexPosition = new VertexPositionNormalTangent(
                            vertexAttribute.Pos.ToVector3(),
                            vertexAttribute.Normal.ToVector3(),
                            vertexAttribute.Tangent.ToVector()
                        );

                        var vertexBindings = vertexAttribute
                            .BoneIndices.Zip(
                                vertexAttribute.BoneWeights,
                                (index, weight) => ((int)index, weight)
                            )
                            .ToArray();

                        return new VertexBuilder<
                            VertexPositionNormalTangent,
                            VertexEmpty,
                            VertexJoints4
                        >(vertexPosition, new VertexEmpty(), new VertexJoints4(vertexBindings));
                    })
                    .ToList();

                // the vertex are just points, to connect them we need to use the provided vertex indices and connect them
                var index = 0;
                var processedIndices = ModelUtils.ProcessVertexIndices(polygonData.Indices);
                do
                {
                    // we construct a triangle based on the processed indices, every three indices = 1 triangle
                    // the accessor here will throw an error if the provided index is invalid, which is to be expected since we can't construct a shape based on wrong data
                    var vertex1 = vertices[processedIndices[index++]];
                    var vertex2 = vertices[processedIndices[index++]];
                    var vertex3 = vertices[processedIndices[index++]];
                    primitiveBuilder.AddTriangle(vertex1, vertex2, vertex3);
                } while (processedIndices.Count > index); // once all the indices are processed, we can stop the loop

                // if the command requestor only wants mesh without armature components, we just construct a rigid mesh
                if (request.VbnFile is null)
                {
                    scene.AddRigidMesh(mesh, Matrix4x4.Identity);
                    continue;
                }

                // it is ok to add the same bindings to each mesh since it technically is correct, each vertex group is bounded to the same sets of joints
                scene.AddSkinnedMesh(mesh, bindings);
            }
        }

        var tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

        try
        {
            Directory.CreateDirectory(tempDirectory);
            var fileName = Path.ChangeExtension(name, "glb");
            var filePath = Path.Combine(tempDirectory, fileName);

            var model = scene.ToGltf2();
            model.SaveGLB(filePath);
            // model.SaveGLTF(filePath);

            // var bytes = await compressor.CompressAsync(
            //     tempDirectory,
            //     CompressionFormats.Tar,
            //     cancellationToken
            // );

            var bytes = await File.ReadAllBytesAsync(filePath, cancellationToken);
            return new FileInfo(Data: bytes, FileName: fileName);
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
                Directory.Delete(tempDirectory, true);
        }
    }

    // just a data holder for my bone data transform
    private class BoneData
    {
        public required NodeBuilder Node { get; init; }
        public Vector3 LocalRotation { get; init; }
        public Vector3 LocalTranslation { get; init; }
        public Vector3 LocalScale { get; init; }
        public Matrix4x4 BindMatrix { get; init; }
        public Matrix4x4 InverseBindMatrix { get; init; }

        // mutable since we need this field to hold the updated WorldMatrix after applying the matrix transforms
        public Matrix4x4? WorldMatrix { get; set; }
    }
}

public static class ModelUtils
{
    private const int TriangleStripSeparator = 0xFFFF;

    /// <summary>
    /// Processes triangle strip vertex indices and converts them to individual triangles
    /// </summary>
    /// <param name="vertexIndices">The source vertex indices containing triangle strips</param>
    /// <returns>List of vertex indices for individual triangles</returns>
    public static List<ushort> ProcessVertexIndices(List<ushort> vertexIndices)
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
