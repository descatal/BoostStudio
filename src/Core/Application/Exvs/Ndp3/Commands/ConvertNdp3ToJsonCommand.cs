using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Common.Utils;
using BoostStudio.Application.Exvs.Ndp3.Commands.Models;
using FileInfo = BoostStudio.Application.Common.Models.FileInfo;

namespace BoostStudio.Application.Exvs.Ndp3.Commands;

public record ConvertNdp3ToJsonCommand(Stream Ndp3File, Stream VbnFile) : IRequest<FileInfo>;

public class ConvertNdp3ToJsonCommandHandler(
    ICompressor compressor,
    INdp3BinarySerializer ndp3BinarySerializer,
    IVbnBinarySerializer vbnBinarySerializer
) : IRequestHandler<ConvertNdp3ToJsonCommand, FileInfo>
{
    public async ValueTask<FileInfo> Handle(
        ConvertNdp3ToJsonCommand request,
        CancellationToken cancellationToken
    )
    {
        var serializedNdp3Format = await ndp3BinarySerializer.DeserializeAsync(
            request.Ndp3File,
            cancellationToken
        );

        var serializedVbnFormat = await vbnBinarySerializer.DeserializeAsync(
            request.VbnFile,
            cancellationToken
        );

        var boneData = serializedVbnFormat.Bones ?? [];

        var objects = serializedNdp3Format
            .Meshes.SelectMany(meshData =>
                meshData.Polygons.Select(polygonData =>
                {
                    Dictionary<string, List<(long vertexIndex, float weight)>> boneInfluences = [];
                    var vertexBindings = polygonData
                        .Vertices.Attributes.Select(
                            (attribute, vertexIndex) =>
                            {
                                var weights = attribute
                                    .BoneIndices.Zip(
                                        attribute.BoneWeights,
                                        (index, weight) => ((int)index, weight)
                                    )
                                    .ToList();

                                return new { vertexIndex, weights };
                            }
                        )
                        .ToList();

                    foreach (var pair in vertexBindings)
                    {
                        foreach ((int index, float weight) in pair.weights)
                        {
                            if (weight == 0)
                                continue;

                            var bone = boneData.ElementAtOrDefault(index);
                            var boneName = bone?.Name.Replace("\0", string.Empty).Trim();

                            if (string.IsNullOrWhiteSpace(boneName))
                                continue;

                            boneInfluences.TryGetValue(boneName, out var influence);
                            influence ??= [];

                            influence.Add((pair.vertexIndex, weight));
                            boneInfluences[boneName] = influence;
                        }
                    }

                    var vertexGeometries = polygonData
                        .Vertices.Attributes.Select(attribute => new
                        {
                            attribute.Pos,
                            attribute.Normal,
                            attribute.Tangent,
                            attribute.Bitangent,
                        })
                        .ToList();

                    return new MeshObjectVm
                    {
                        Name = meshData.Name,
                        VertexIndices = ModelUtils
                            .ProcessVertexIndices(polygonData.Indices)
                            .ToArray(),
                        Positions =
                        [
                            new DataVm
                            {
                                Name = "Position0",
                                Data = new
                                {
                                    Vector3 = vertexGeometries.Select(geo =>
                                        new[] { geo.Pos.X, geo.Pos.Y, geo.Pos.Z }
                                    ),
                                },
                            },
                        ],
                        Tangents =
                        [
                            new DataVm
                            {
                                Name = "Tangent0",
                                Data = new
                                {
                                    Vector3 = vertexGeometries.Select(geo =>
                                        new[] { geo.Tangent.X, geo.Tangent.Y, geo.Tangent.Z }
                                    ),
                                },
                            },
                        ],
                        Normals =
                        [
                            new DataVm
                            {
                                Name = "Normal0",
                                Data = new
                                {
                                    Vector3 = vertexGeometries.Select(geo =>
                                        new[] { geo.Normal.X, geo.Normal.Y, geo.Normal.Z }
                                    ),
                                },
                            },
                        ],
                        BoneInfluences = boneInfluences
                            .Select(pair => new BoneInfluenceVm
                            {
                                BoneName = pair.Key,
                                VertexWeights = pair
                                    .Value.Select(tuple => new VertexWeightVm
                                    {
                                        VertexIndex = tuple.vertexIndex,
                                        VertexWeight = tuple.weight,
                                    })
                                    .ToArray(),
                            })
                            .ToArray(),
                    };
                })
            )
            .ToArray();

        var bones = boneData
            .Select(data =>
            {
                var transform = data.InverseBindMatrix.ToMatrix();
                return new BoneVm
                {
                    Name = data.Name.Replace("\0", string.Empty).Trim(),
                    Transform = transform.Expand(),
                    ParentIndex = data.ParentBoneIndex == 268435455 ? null : data.ParentBoneIndex,
                    InverseBindMatrix = data.InverseBindMatrix.ToMatrix(),
                    Translation = data.LocalTransform.Translation.ToVector(),
                    Rotation = data.LocalTransform.Rotation.ToVector(),
                };
            })
            .ToArray();

        var boneHierarchyMap = boneData
            .Select((data, index) => new { index, data.ParentBoneIndex })
            .GroupBy(data => data.ParentBoneIndex)
            .ToDictionary(
                grouping => grouping.Key,
                grouping => grouping.Select(index => index.index).ToArray()
            );

        foreach ((int parentBoneIndex, int[] childBoneIndexes) in boneHierarchyMap)
        {
            var parentBone = bones.ElementAtOrDefault(parentBoneIndex);
            if (parentBone is null)
                continue;

            var cumulativeMatrix =
                parentBone.CumulativeMatrix
                ?? parentBone.Rotation.ToMatrix() with
                {
                    Translation = parentBone.Translation,
                };

            foreach (var childBoneIndex in childBoneIndexes)
            {
                var childBone = bones.ElementAtOrDefault(childBoneIndex);

                if (childBone is null)
                    continue;

                var transformedMatrix = childBone.InverseBindMatrix * cumulativeMatrix;
                transformedMatrix.Translation = childBone.Translation;
                childBone.Transform = transformedMatrix.Expand();

                var childMatrix = childBone.Rotation.ToMatrix();

                var childCumulativeMatrix = cumulativeMatrix * childMatrix;
                childBone.CumulativeMatrix = childCumulativeMatrix;
            }
        }

        var serializerOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.Never,
            WriteIndented = true,
        };
        var model = new MeshVm { Objects = objects };
        var skeleton = new SkeletonVm { Bones = bones };
        var modelJson = JsonSerializer.Serialize(model, serializerOptions);
        var skeletonJson = JsonSerializer.Serialize(skeleton, serializerOptions);

        var tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

        try
        {
            Directory.CreateDirectory(tempDirectory);
            var modelFilePath = Path.Combine(tempDirectory, "model.json");
            var skeletonFilePath = Path.Combine(tempDirectory, "skeleton.json");

            await File.WriteAllTextAsync(modelFilePath, modelJson, cancellationToken);
            await File.WriteAllTextAsync(skeletonFilePath, skeletonJson, cancellationToken);

            var bytes = await compressor.CompressAsync(
                tempDirectory,
                CompressionFormats.Tar,
                cancellationToken
            );

            return new FileInfo(Data: bytes, FileName: "model.tar");
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
                Directory.Delete(tempDirectory, true);
        }
    }
}

public record MeshVm
{
    [JsonPropertyName("major_version")]
    public int MajorVersion { get; set; } = 1;

    [JsonPropertyName("minor_version")]
    public int MinorVersion { get; set; } = 0;

    [JsonPropertyName("objects")]
    public MeshObjectVm[] Objects { get; set; } = [];

    [JsonPropertyName("is_vs2")]
    public bool IsVs2 { get; set; } = true;
}

public record MeshObjectVm
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("subindex")]
    public int Subindex { get; set; } = 0;

    [JsonPropertyName("parent_bone_name")]
    public string ParentBoneName { get; set; } = string.Empty;

    [JsonPropertyName("sort_bias")]
    public int SortBias { get; set; } = 0;

    [JsonPropertyName("disable_depth_write")]
    public bool DisableDepthWrite { get; set; } = false;

    [JsonPropertyName("disable_depth_test")]
    public bool DisableDepthTest { get; set; } = false;

    [JsonPropertyName("vertex_indices")]
    public ushort[] VertexIndices { get; set; } = [];

    [JsonPropertyName("positions")]
    public DataVm[] Positions { get; set; } = [];

    [JsonPropertyName("normals")]
    public DataVm[] Normals { get; set; } = [];

    [JsonPropertyName("binormals")]
    public DataVm[] Binormals { get; set; } = [];

    [JsonPropertyName("tangents")]
    public DataVm[] Tangents { get; set; } = [];

    [JsonPropertyName("texture_coordinates")]
    public DataVm[] TextureCoordinates { get; set; } = [];

    [JsonPropertyName("color_sets")]
    public DataVm[] ColorSets { get; set; } = [];

    [JsonPropertyName("bone_influences")]
    public BoneInfluenceVm[] BoneInfluences { get; set; } = [];
}

public record BoneInfluenceVm
{
    [JsonPropertyName("bone_name")]
    public required string BoneName { get; set; }

    [JsonPropertyName("vertex_weights")]
    public VertexWeightVm[] VertexWeights { get; set; } = [];
}

public record VertexWeightVm
{
    [JsonPropertyName("vertex_index")]
    public long VertexIndex { get; set; }

    [JsonPropertyName("vertex_weight")]
    public float VertexWeight { get; set; }
}

public record DataVm
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("data")]
    public required object Data { get; set; }
}

public record SkeletonVm
{
    [JsonPropertyName("major_version")]
    public int MajorVersion { get; set; } = 1;

    [JsonPropertyName("minor_version")]
    public int MinorVersion { get; set; } = 0;

    [JsonPropertyName("bones")]
    public BoneVm[] Bones { get; set; } = [];
}

public record BoneVm
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("transform")]
    public float[][] Transform { get; set; } = [];

    [JsonPropertyName("parent_index")]
    public int? ParentIndex { get; set; } = null;

    [JsonPropertyName("billboard_type")]
    public string BillboardType { get; set; } = "Disabled";

    [JsonIgnore]
    public Matrix4x4? CumulativeMatrix { get; set; }

    [JsonIgnore]
    public Matrix4x4 InverseBindMatrix { get; set; } = Matrix4x4.Identity;

    [JsonIgnore]
    public Vector3 Translation { get; set; } = Vector3.Zero;

    [JsonIgnore]
    public Vector3 Rotation { get; set; } = Vector3.Zero;
}
