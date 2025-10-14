using System.Numerics;
using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Application.Common.Models;
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

        foreach (var meshData in serializedNdp3Format.Meshes)
        {
            var mesh = new MeshBuilder<VertexPositionNormalTangent, VertexEmpty, VertexJoints4>(
                meshData.Name
            );

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

                var processedIndices = ModelUtils.ProcessVertexIndices(polygonData.Indices);

                var vertexGeometries = polygonData
                    .Vertices.Attributes.Select(attribute => new
                    {
                        attribute.Pos,
                        attribute.Normal,
                        attribute.Tangent,
                    })
                    .ToList();

                var vertexColorUv = polygonData
                    .Vertices.ColorUv.Select(colorUv => new
                    {
                        Color = colorUv.Color.ToVector(),
                        Uv = colorUv.Uv.ToVector(),
                    })
                    .ToList();

                var vertexBindings = polygonData
                    .Vertices.Attributes.SelectMany(attribute =>
                        attribute.BoneIndices.Zip(
                            attribute.BoneWeights,
                            (index, weight) => ((int)index, weight)
                        )
                    )
                    .ToArray();

                var index = 0;
                do
                {
                    var vertexGeometry1 = vertexGeometries[processedIndices[index++]];
                    var vertexGeometry2 = vertexGeometries[processedIndices[index++]];
                    var vertexGeometry3 = vertexGeometries[processedIndices[index++]];

                    primitiveBuilder.AddTriangle(
                        new VertexPositionNormalTangent(
                            vertexGeometry1.Pos.ToVector3(),
                            vertexGeometry1.Normal.ToVector3(),
                            vertexGeometry1.Tangent.ToVector()
                        ),
                        new VertexPositionNormalTangent(
                            vertexGeometry2.Pos.ToVector3(),
                            vertexGeometry2.Normal.ToVector3(),
                            vertexGeometry2.Tangent.ToVector()
                        ),
                        new VertexPositionNormalTangent(
                            vertexGeometry3.Pos.ToVector3(),
                            vertexGeometry3.Normal.ToVector3(),
                            vertexGeometry3.Tangent.ToVector()
                        )
                    );
                } while (processedIndices.Count > index);

                primitiveBuilder.TransformVertices(builder => builder.WithSkinning(vertexBindings));
            }

            if (request.VbnFile is null)
            {
                scene.AddRigidMesh(mesh, Matrix4x4.Identity);
                continue;
            }

            var boneData = serializedVbnFormat?.Bones ?? [];
            var joints = boneData
                .Select(data =>
                {
                    // var localRotation = Quaternion.CreateFromRotationMatrix(
                    //     Matrix4x4.Create(
                    //         x: data.InverseBoneTransformationMatrix.Row0.ToVector(),
                    //         y: data.InverseBoneTransformationMatrix.Row1.ToVector(),
                    //         z: data.InverseBoneTransformationMatrix.Row2.ToVector(),
                    //         w: data.InverseBoneTransformationMatrix.Row3.ToVector()
                    //     )
                    // );

                    // var x = new Vector4(
                    //     data.InverseBoneTransformationMatrix.Row0.X,
                    //     data.InverseBoneTransformationMatrix.Row1.X,
                    //     data.InverseBoneTransformationMatrix.Row2.X,
                    //     data.InverseBoneTransformationMatrix.Row3.X
                    // );
                    //
                    // var y = new Vector4(
                    //     data.InverseBoneTransformationMatrix.Row0.Y,
                    //     data.InverseBoneTransformationMatrix.Row1.Y,
                    //     data.InverseBoneTransformationMatrix.Row2.Y,
                    //     data.InverseBoneTransformationMatrix.Row3.Y
                    // );
                    //
                    // var z = new Vector4(
                    //     data.InverseBoneTransformationMatrix.Row0.Z,
                    //     data.InverseBoneTransformationMatrix.Row1.Z,
                    //     data.InverseBoneTransformationMatrix.Row2.Z,
                    //     data.InverseBoneTransformationMatrix.Row3.Z
                    // );
                    //
                    // var w = new Vector4(
                    //     data.InverseBoneTransformationMatrix.Row0.W,
                    //     data.InverseBoneTransformationMatrix.Row1.W,
                    //     data.InverseBoneTransformationMatrix.Row2.W,
                    //     data.InverseBoneTransformationMatrix.Row3.W
                    // );

                    var invBindMatrix = Matrix4x4.Create(
                        data.InverseBoneTransformationMatrix.Row0.ToVector(),
                        data.InverseBoneTransformationMatrix.Row1.ToVector(),
                        data.InverseBoneTransformationMatrix.Row2.ToVector(),
                        data.InverseBoneTransformationMatrix.Row3.ToVector()
                    );

                    var bindMatrix = Matrix4x4.Create(
                        data.BoneTransformationMatrix.Row0.ToVector(),
                        data.BoneTransformationMatrix.Row1.ToVector(),
                        data.BoneTransformationMatrix.Row2.ToVector(),
                        data.BoneTransformationMatrix.Row3.ToVector()
                    );

                    Matrix4x4.Decompose(
                        invBindMatrix,
                        out var scale,
                        out var rotation,
                        out var translation
                    );

                    Matrix4x4.Decompose(
                        bindMatrix,
                        out var scale2,
                        out var rotation2,
                        out var translation2
                    );

                    var localRotation = Quaternion.CreateFromYawPitchRoll(
                        data.TransformationMatrix.RotationX,
                        data.TransformationMatrix.RotationZ,
                        data.TransformationMatrix.RotationY
                    );

                    var localTranslation = new Vector3(
                        data.TransformationMatrix.TranslationX,
                        data.TransformationMatrix.TranslationY,
                        data.TransformationMatrix.TranslationZ
                    );

                    var localScale = new Vector3(
                        data.TransformationMatrix.ScaleX,
                        data.TransformationMatrix.ScaleY,
                        data.TransformationMatrix.ScaleZ
                    );

                    Matrix4x4.Invert(bindMatrix, out var invBindMatrixInv);

                    bindMatrix = bindMatrix.Round();
                    invBindMatrixInv = invBindMatrixInv.Round();

                    scale = scale.Round();
                    scale2 = scale2.Round();
                    translation = translation.Round();
                    translation2 = translation2.Round();
                    rotation = rotation.Round();
                    rotation2 = rotation2.Round();

                    localScale = localScale.Round();
                    localTranslation = localTranslation.Round();
                    localRotation = localRotation.Round();

                    if (!invBindMatrixInv.ApproximatelyEqual(invBindMatrix))
                    {
                        //
                    }

                    if (!scale.ApproximatelyEqual(localScale))
                    {
                        //

                        if (!scale2.ApproximatelyEqual(localScale))
                        {
                            //
                        }
                    }

                    if (!translation.ApproximatelyEqual(localTranslation))
                    {
                        //

                        if (!translation2.ApproximatelyEqual(localTranslation))
                        {
                            //
                        }
                    }

                    if (!rotation.ApproximatelyEqual(localRotation))
                    {
                        //

                        if (!rotation2.ApproximatelyEqual(localRotation))
                        {
                            //
                        }
                    }

                    var node = new NodeBuilder(data.Name);
                    // node.SetLocalTransform(
                    //     new AffineTransform(
                    //         scale: new Vector3(1, 1, 1),
                    //         rotation: new Quaternion(0, 0, 0, 1),
                    //         translation: new Vector3(0, 0, 0)
                    //     ),
                    //     true
                    // );

                    return (Node: node, InverseBindMatrix: bindMatrix);
                })
                .ToList();

            var boneHierarchyMap = boneData
                .Select((data, index) => new { index, data.ParentBoneIndex })
                .GroupBy(data => data.ParentBoneIndex)
                .ToDictionary(
                    grouping => grouping.Key,
                    grouping => grouping.Select(index => index.index).ToArray()
                );

            foreach ((int parentBoneIndex, int[] childBoneIndexes) in boneHierarchyMap)
            {
                var parentJoint = joints.ElementAtOrDefault(parentBoneIndex);

                if (parentJoint == default)
                    continue;

                foreach (var childBoneIndex in childBoneIndexes)
                {
                    var childJoint = joints.ElementAtOrDefault(childBoneIndex);

                    if (childJoint == default)
                        continue;

                    parentJoint.Node.AddNode(childJoint.Node);
                }
            }

            foreach ((NodeBuilder node, _) in joints)
            {
                node.WorldMatrix = Matrix4x4.Identity;
                node.LocalMatrix = Matrix4x4.Identity;
            }

            scene.AddSkinnedMesh(mesh, joints.ToArray());
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

public static class Extensions
{
    public static Vector3 Round(this Vector3 v, int decimals = 2) =>
        v with
        {
            X = (float)Math.Round(v.X, decimals),
            Y = (float)Math.Round(v.Y, decimals),
            Z = (float)Math.Round(v.Z, decimals),
        };

    public static Vector4 Round(this Vector4 v, int decimals = 2) =>
        v with
        {
            X = (float)Math.Round(v.X, decimals),
            Y = (float)Math.Round(v.Y, decimals),
            Z = (float)Math.Round(v.Z, decimals),
            W = (float)Math.Round(v.W, decimals),
        };

    public static Quaternion Round(this Quaternion q, int decimals = 2) =>
        q with
        {
            X = (float)Math.Round(q.X, decimals),
            Y = (float)Math.Round(q.Y, decimals),
            Z = (float)Math.Round(q.Z, decimals),
            W = (float)Math.Round(q.W, decimals),
        };

    public static Matrix4x4 Round(this Matrix4x4 v, int decimals = 2) =>
        v with
        {
            M11 = (float)Math.Round(v.M11, decimals),
            M12 = (float)Math.Round(v.M12, decimals),
            M13 = (float)Math.Round(v.M13, decimals),
            M14 = (float)Math.Round(v.M14, decimals),
            M21 = (float)Math.Round(v.M21, decimals),
            M22 = (float)Math.Round(v.M22, decimals),
            M23 = (float)Math.Round(v.M23, decimals),
            M24 = (float)Math.Round(v.M24, decimals),
            M31 = (float)Math.Round(v.M31, decimals),
            M32 = (float)Math.Round(v.M32, decimals),
            M33 = (float)Math.Round(v.M33, decimals),
            M34 = (float)Math.Round(v.M34, decimals),
            M41 = (float)Math.Round(v.M41, decimals),
            M42 = (float)Math.Round(v.M42, decimals),
            M43 = (float)Math.Round(v.M43, decimals),
            M44 = (float)Math.Round(v.M44, decimals),
        };

    public static bool ApproximatelyEqual(
        this Quaternion a,
        Quaternion b,
        float epsilon = 0.0001f
    ) =>
        Math.Abs(a.X - b.X) < epsilon
        && Math.Abs(a.Y - b.Y) < epsilon
        && Math.Abs(a.Z - b.Z) < epsilon
        && Math.Abs(a.W - b.W) < epsilon;

    public static bool ApproximatelyEqual(this Vector3 a, Vector3 b, float epsilon = 0.0001f) =>
        Math.Abs(a.X - b.X) < epsilon
        && Math.Abs(a.Y - b.Y) < epsilon
        && Math.Abs(a.Z - b.Z) < epsilon;

    public static bool ApproximatelyEqual(this Matrix4x4 a, Matrix4x4 b, float epsilon = 0.0001f) =>
        Math.Abs(a.M11 - b.M11) < epsilon
        && Math.Abs(a.M12 - b.M12) < epsilon
        && Math.Abs(a.M13 - b.M13) < epsilon
        && Math.Abs(a.M14 - b.M14) < epsilon
        && Math.Abs(a.M21 - b.M21) < epsilon
        && Math.Abs(a.M22 - b.M22) < epsilon
        && Math.Abs(a.M23 - b.M23) < epsilon
        && Math.Abs(a.M24 - b.M24) < epsilon
        && Math.Abs(a.M31 - b.M31) < epsilon
        && Math.Abs(a.M32 - b.M32) < epsilon
        && Math.Abs(a.M33 - b.M33) < epsilon
        && Math.Abs(a.M34 - b.M34) < epsilon
        && Math.Abs(a.M41 - b.M41) < epsilon
        && Math.Abs(a.M42 - b.M42) < epsilon
        && Math.Abs(a.M43 - b.M43) < epsilon
        && Math.Abs(a.M44 - b.M44) < epsilon;
}

[Mapper]
public static partial class Ndp3Mapper
{
    public static partial Vector4 ToVector(this VbnBinaryFormat.FloatV4 source);

    public static VbnBinaryFormat.FloatV4 Round(
        this VbnBinaryFormat.FloatV4 source,
        int decimals = 2
    )
    {
        source.X = (float)Math.Round(source.X, decimals);
        source.Y = (float)Math.Round(source.Y, decimals);
        source.Z = (float)Math.Round(source.Z, decimals);
        source.W = (float)Math.Round(source.W, decimals);
        return source;
    }

    public static VbnBinaryFormat.TransformMatrixData Round(
        this VbnBinaryFormat.TransformMatrixData source,
        int decimals = 2
    )
    {
        source.Row0 = source.Row0.Round();
        source.Row1 = source.Row1.Round();
        source.Row2 = source.Row2.Round();
        source.Row3 = source.Row3.Round();
        return source;
    }

    public static float[][] Expand(this VbnBinaryFormat.TransformMatrixData source) =>
        [source.Row0.Expand(), source.Row1.Expand(), source.Row2.Expand(), source.Row3.Expand()];

    public static float[] Expand(this VbnBinaryFormat.FloatV4 source) =>
        [source.X, source.Y, source.Z, source.W];

    public static float[][] Expand(this Matrix4x4 source) =>
        [source.X.Expand(), source.Y.Expand(), source.Z.Expand(), source.W.Expand()];

    public static float[] Expand(this Vector4 source) => [source.X, source.Y, source.Z, source.W];

    public static float[] Expand(this Vector3 source) => [source.X, source.Y, source.Z];

    public static float[] Expand(this Quaternion source) =>
        [source.X, source.Y, source.Z, source.W];

    public static Matrix4x4 ToMatrix(this Vector3 source) =>
        Matrix4x4.CreateFromYawPitchRoll(-source.Y, -source.X, -source.Z);

    public static Vector3 Invert(this Vector3 source) => new(-source.X, -source.Y, -source.Z);

    public static Matrix4x4 ToMatrix(this VbnBinaryFormat.TransformMatrixData source) =>
        Matrix4x4.Create(
            source.Row0.ToVector(),
            source.Row1.ToVector(),
            source.Row2.ToVector(),
            source.Row3.ToVector()
        );

    public static VertexPosition ToVertexPosition(this Ndp3BinaryFormat.Vector4 source) =>
        new(source.X, source.Y, source.Z);

    public static partial Vector3 ToVector3(this Ndp3BinaryFormat.Vector4 source);

    public static partial Vector4 ToVector(this Ndp3BinaryFormat.Vector4 source);

    public static Vector4 ToVector(this Ndp3BinaryFormat.Vector4Byte source) =>
        new(source.X, source.Y, source.Z, source.W);

    public static Vector2 ToVector(this Ndp3BinaryFormat.Vector2Byte source) =>
        new(source.X, source.Y);
}
