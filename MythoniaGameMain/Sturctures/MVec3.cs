


using System.Diagnostics.CodeAnalysis;

namespace Mythonia.Sturctures
{
    public struct MVec3
    {
        private Vector3 _v;

        public float X { get => _v.X; set => _v.X = value; }
        public float Y { get => _v.Y; set => _v.Y = value; }
        public float Z { get => _v.Z; set => _v.Z = value; }

        public Vector3 Vec { get => _v; set => _v = value; }

        public MVec3 Negative => -Vec;


        #region Constructors

        public MVec3() => _v = new();
        public MVec3(float x) => _v = new(x);
        public MVec3(float x, float y, float z) => _v = new(x, y, z);
        public MVec3(Vector2 v, float z = 0) => _v = new(v, z);
        public MVec3(Vector3 v) => _v = v;

        #endregion Constructors



        #region Methods

        public void Ceiling() => _v.Ceiling();
        public void Floor() => _v.Floor();
        public void Round() => _v.Round();

        public void Normalize() => _v.Normalize();

        public float Length() => _v.Length();
        public float LengthSquared() => _v.LengthSquared();

        public void Deconstruct(out float x, out float y, out float z) => _v.Deconstruct(out x, out y, out z);


        public MVec3 Replace(float? x = null, float? y = null, float? z = null)
        {
            if (x is float x2) X = x2;
            if (y is float y2) Y = y2;
            if (z is float z2) Z = z2;
            return this;
        }

        #endregion Methods



        #region Methods - Override

        public override string ToString() => _v.ToString();
        public override bool Equals([NotNullWhen(true)] object obj) => _v.Equals(obj);
        public override int GetHashCode() => _v.GetHashCode();

        #endregion



        #region Methods / Prop - Static

        public static MVec3 One => Vector3.One;
        public static MVec3 Zero => Vector3.Zero;
        public static MVec3 UnitX => Vector3.UnitX;
        public static MVec3 UnitY => Vector3.UnitY;
        public static MVec3 UnitZ => Vector3.UnitZ;


        public static MVec3 Negate(MVec3 v) => Vector3.Negate(v);

        public static MVec3 Max(MVec3 v1, MVec3 v2) => Vector3.Max(v1, v2);
        public static MVec3 Min(MVec3 v1, MVec3 v2) => Vector3.Min(v1, v2);

        public static MVec3 Add(MVec3 v1, MVec3 v2) => Vector3.Add(v1.Vec, v2.Vec);
        public static MVec3 Subtract(MVec3 v1, MVec3 v2) => Vector3.Subtract(v1.Vec, v2.Vec);
        public static MVec3 Multiply(MVec3 v1, float scalar) => Vector3.Multiply(v1, scalar);
        public static MVec3 Multiply(MVec3 v1, MVec3 v2) => Vector3.Multiply(v1, v2);
        public static MVec3 Divide(MVec3 v1, float scalar) => Vector3.Divide(v1, scalar);
        public static MVec3 Divide(MVec3 v1, MVec3 v2) => Vector3.Divide(v1, v2);



        #endregion



        #region Operators


        #region Operators - Arithmetic

        public static MVec3 operator -(MVec3 v) => v.Negative;

        public static MVec3 operator +(MVec3 v1, MVec3 v2) => Add(v1, v2);
        public static MVec3 operator -(MVec3 v1, MVec3 v2) => Subtract(v1, v2);
        public static MVec3 operator *(MVec3 v1, MVec3 v2) => Multiply(v1, v2);
        public static MVec3 operator *(MVec3 v1, float v2) => Multiply(v1, v2);
        public static MVec3 operator /(MVec3 v1, MVec3 v2) => Divide(v1, v2);
        public static MVec3 operator /(MVec3 v1, float v2) => Divide(v1, v2);

        #endregion


        #region Operators - Logical

        public static bool operator ==(MVec3 left, MVec3 right) => left.Equals(right);
        public static bool operator !=(MVec3 left, MVec3 right) => !(left == right);

        #endregion


        #region Operators - Type

        public static implicit operator MVec3 ((float x, float y, float z) v) => new(v.x, v.y, v.z);
        

        public static implicit operator MVec3 (Vector3 v) => new(v);

        public static implicit operator Vector3 (MVec3 v) => v.Vec;


        #endregion


        #endregion Operators

    }
}
