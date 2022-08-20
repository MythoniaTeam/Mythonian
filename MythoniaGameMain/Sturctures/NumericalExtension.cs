



namespace Mythonia.Sturctures
{
    public static class NumericalExtension
    {
        public static Vector2 Rotation(this Vector2 v, Angle rotation)
        {
            float sin = MathF.Sin(rotation);
            float cos = MathF.Cos(rotation);
            return new(v.X * cos + v.Y * sin, v.Y * cos - v.X * cos);

        }

        public static Vector2 ToV2(this Vector3 v) => new(v.X, v.Y);

        public static (float, float) ToFloat(this Vector2 v) => (v.X, v.Y);

        public static MVec2 ToVec(this float v) => new(v);

        public static float Normalize(this float v) => v switch
        {
            > 0 => 1,
              0 => 0,
            < 0 => -1,
            _ => float.NaN,
        };


    }
}
