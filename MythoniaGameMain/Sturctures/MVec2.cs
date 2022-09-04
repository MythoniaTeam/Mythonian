



using System.Diagnostics.CodeAnalysis;

namespace Mythonia.Sturctures
{
    public struct MVec2 : ICloneable
    {
        private Vector2 _v;

        public float X { get => _v.X; set => _v.X = value; }
        public float Y { get => _v.Y; set => _v.Y = value; }

        public Vector2 Vec { get => _v; set => _v = value; }

        public MVec2 Negative => -Vec;

        public MVec2 Abs => (MathF.Abs(X), MathF.Abs(Y));

        public MVec2 Normalized
        {
            get
            {
                MVec2 v = Clone();
                v.Normalize();
                return v;
            }
        }

        public Angle Direction => MathF.Atan2(Y, X) * 180 / MathF.PI;

        public (float X, float Y) ToFloat => (X, Y);



        #region Constructors

        public MVec2() => _v = new();
        public MVec2(float x) => _v = new(x);
        public MVec2(float x, float y) => _v = new(x, y);

        public MVec2(Vector2 v) => _v = v;

        #endregion Constructors



        #region Enum - 3 * 3 Dir

        public enum Dir9
        {
            BottomLeft,
            Bottom,
            BottomRight,
            Left,
            Center,
            Right,
            TopLeft,
            Top,
            TopRight,
        }

        public static implicit operator MVec2 (Dir9 dir) => dir switch
        {
            Dir9.BottomLeft  => new(-1, -1),
            Dir9.Bottom      => new(0,  -1),
            Dir9.BottomRight => new(1,  -1),

            Dir9.Left   => new(-1, 0),
            Dir9.Center => new(0,  0),
            Dir9.Right  => new(1,  0),

            Dir9.TopLeft  => new(-1, 1),
            Dir9.Top      => new(0,  1),
            Dir9.TopRight => new(1,  1),

            _ => throw new Exception($"MVec2.Dir9 doesn't contain the value \"{dir}\"")
        };

        public Dir9 GetQuadrant()
        {
            Vector2 v = Clone().Vec;
            v.X.Normalize();
            v.Y.Normalize();
            return v.ToFloat() switch
            {
                (-1, -1) => Dir9.BottomLeft,
                ( 0, -1) => Dir9.Bottom,
                ( 1, -1) => Dir9.BottomRight,
                (-1,  0) => Dir9.Left,
                ( 0,  0) => Dir9.Center,
                ( 1,  0) => Dir9.Right,
                (-1,  1) => Dir9.TopLeft,
                ( 0,  1) => Dir9.Top,
                ( 1,  1) => Dir9.TopRight,
                _ => throw new Exception($"When GetQuadrant(), an Unexpected Value {v} was given"),
            };
        }

        public static explicit operator Dir9 (MVec2 v) => v.GetQuadrant();


        #endregion



        #region Methods

        public void Ceiling() => _v.Ceiling();
        public void Floor() => _v.Floor();
        public void Round() => _v.Round();

        public void Normalize() => _v.Normalize();

        public float Length() => _v.Length();
        public float LengthSquared() => _v.LengthSquared();

        public void Deconstruct(out float x, out float y) => _v.Deconstruct(out x, out y);


        public MVec2 Rotation(Angle rotation) => _v.Rotation(rotation);

        /// <summary>
        /// 将当前向量的 X / Y 增加给定的值, 然后返回.
        /// <para>
        /// <b>Example: </b><br/>
        /// <c><see langword="new"/> <see cref="MVec2"/>(2, 3).Change(<paramref name="y"/>: 10)</c><br/>
        /// <i>(将 Y 增加 10 并返回, 结果应为向量 (2, 13) )</i>
        /// </para>
        /// </summary>
        /// <param name="x">X增加的值, 如果留空, 则为 <see langword="null"/>, 不会修改原有值</param>
        /// <param name="y">Y增加的值, 如果留空, 则为 <see langword="null"/>, 不会修改原有值</param>
        /// <returns>X / Y 改变后的向量</returns>
        public MVec2 Change(float? x = null, float? y = null)
        {
            if (x is float x2) X += x2;
            if (y is float y2) Y += y2;
            return this;
        }
        /// <summary>
        /// 将当前向量的 X / Y 替换为给定的值, 然后返回.
        /// <para>
        /// <b>Example: </b><br/>
        /// <c><see langword="new"/> <see cref="MVec2"/>(2, 3).Replace(<paramref name="y"/>: 10)</c><br/>
        /// <i>(将 Y 值设为 10 并返回, 结果应为向量 (2, 10) )</i>
        /// </para>
        /// </summary>
        /// <param name="x">X替换的值, 如果留空, 则为 <see langword="null"/>, 不会修改原有值</param>
        /// <param name="y">Y替换的值, 如果留空, 则为 <see langword="null"/>, 不会修改原有值</param>
        /// <returns>X / Y 改变后的向量</returns>
        public MVec2 Replace(float? x = null, float? y = null)
        {
            if (x is float x2) X = x2;
            if (y is float y2) Y = y2;
            return this;
        }


        #endregion Methods



        #region Methods - Override

        public override string ToString() => _v.ToString();
        public override bool Equals([NotNullWhen(true)] object obj) => _v.Equals(obj);
        public override int GetHashCode() => _v.GetHashCode();

        object ICloneable.Clone() => Clone();
        public MVec2 Clone() => new(X, Y);

        #endregion



        #region Methods / Prop - Static

        public static MVec2 One => Vector2.One;
        public static MVec2 Zero => Vector2.Zero;
        public static MVec2 UnitX => Vector2.UnitX;
        public static MVec2 UnitY => Vector2.UnitY;

        public static MVec2 Negate(MVec2 v) => Vector2.Negate(v);

        public static MVec2 Max(MVec2 v1, MVec2 v2) => Vector2.Max(v1, v2);
        public static MVec2 Min(MVec2 v1, MVec2 v2) => Vector2.Min(v1, v2);

        public static MVec2 Add(MVec2 v1, MVec2 v2) => Vector2.Add(v1.Vec, v2.Vec);
        public static MVec2 Subtract(MVec2 v1, MVec2 v2) => Vector2.Subtract(v1.Vec, v2.Vec);
        public static MVec2 Multiply(MVec2 v1, float scalar) => Vector2.Multiply(v1, scalar);
        public static MVec2 Multiply(MVec2 v1, MVec2 v2) => Vector2.Multiply(v1, v2);
        public static MVec2 Divide(MVec2 v1, float scalar) => Vector2.Divide(v1, scalar);
        public static MVec2 Divide(MVec2 v1, MVec2 v2) => Vector2.Divide(v1, v2);

        



        #endregion



        #region Operators


        #region Operators - Arithmetic

        public static MVec2 operator -(MVec2 v) => v.Negative;

        public static MVec2 operator +(MVec2 v1, MVec2 v2) => Add(v1, v2);
        public static MVec2 operator -(MVec2 v1, MVec2 v2) => Subtract(v1, v2);
        public static MVec2 operator *(MVec2 v1, MVec2 v2) => Multiply(v1, v2);
        public static MVec2 operator *(MVec2 v1, float v2) => Multiply(v1, v2);
        public static MVec2 operator /(MVec2 v1, MVec2 v2) => Divide(v1, v2);
        public static MVec2 operator /(MVec2 v1, float v2) => Divide(v1, v2);

        #endregion


        #region Operators - Logical

        public static bool operator ==(MVec2 left, MVec2 right) => left.Equals(right);
        public static bool operator !=(MVec2 left, MVec2 right) => !(left == right);
        public static bool operator <=(MVec2 left, MVec2 right) => left.X <= right.X && left.Y <= right.Y;
        public static bool operator >=(MVec2 left, MVec2 right) => left.X >= right.X && left.Y >= right.Y;
        public static bool operator <(MVec2 left, MVec2 right) => left.X < right.X && left.Y < right.Y;
        public static bool operator >(MVec2 left, MVec2 right) => left.X > right.X && left.Y > right.Y;


        #endregion


        #region Operators - Type

        public static implicit operator MVec2 ((float X, float Y) v) => new(v.X, v.Y);
        public static explicit operator (float X, float Y) (MVec2 v) => v.ToFloat;


        public static implicit operator MVec2 (Vector2 v) => new(v);
        public static implicit operator Vector2 (MVec2 v) => v.Vec;

        public static implicit operator MVec2(Point v) => new(v.ToVector2());
        public static implicit operator Point(MVec2 v) => v.Vec.ToPoint();


        public static explicit operator MVec2 (MVec3 v) => new(v.X, v.Y);
        public static implicit operator MVec3 (MVec2 v) => new(v.X, v.Y, 0);



        #endregion


        #endregion Operators

    }
}
