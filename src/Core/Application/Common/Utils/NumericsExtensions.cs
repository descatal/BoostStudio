using System.Numerics;

namespace BoostStudio.Application.Common.Utils;

public static class NumericsExtensions
{
    public static float[][] Expand(this Matrix4x4 source) =>
        [source.X.Expand(), source.Y.Expand(), source.Z.Expand(), source.W.Expand()];

    public static float[] Expand(this Vector4 source) => [source.X, source.Y, source.Z, source.W];

    public static Matrix4x4 ToMatrix(this Vector3 source) =>
        Matrix4x4.CreateFromYawPitchRoll(-source.Y, -source.X, -source.Z);
}
