using System.Numerics;
using BoostStudio.Formats;
using Riok.Mapperly.Abstractions;

namespace BoostStudio.Application.Exvs.Ndp3.Commands.Models;

[Mapper]
public static partial class Ndp3Mapper
{
    public static Matrix4x4 ToMatrix(this VbnBinaryFormat.Matrix4x4 source) =>
        Matrix4x4.Create(
            source.Row0.ToVector(),
            source.Row1.ToVector(),
            source.Row2.ToVector(),
            source.Row3.ToVector()
        );

    public static partial Vector4 ToVector(this VbnBinaryFormat.Vector4 source);

    public static partial Vector3 ToVector(this VbnBinaryFormat.Vector3 source);

    public static partial Vector3 ToVector3(this Ndp3BinaryFormat.Vector4 source);

    public static partial Vector4 ToVector(this Ndp3BinaryFormat.Vector4 source);
}
