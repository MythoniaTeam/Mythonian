using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mythonia.Sturctures
{
    public struct Angle
    {
        private float _degree = 0;
        public float Degree 
        { 
            get => _degree; 
            set
            {
                _degree = value;
                if (_degree is >= 360 or < 0)
                {
                    _degree %= 360;
                    if (_degree < 0) _degree += 360;
                } 
            }
        }
        public float Radium => _degree * MathF.PI / 180;


        #region Prop - Static

        public static Angle Left => new(0);
        public static Angle Top => new(90);
        public static Angle Right => new(180);
        public static Angle Bottom => new(270);

        #endregion



        public Angle(float degree = 0)
        {
            Degree = degree;
        }


        #region Operators

        public static Angle operator +(Angle a, Angle b) => new(a.Degree + b.Degree);   
        public static Angle operator +(Angle a, float b) => new(a.Degree + b);
        public static Angle operator +(float a, Angle b) => new(a + b.Degree);
        public static Angle operator -(Angle a, Angle b) => new(a.Degree - b.Degree);
        public static Angle operator -(Angle a, float b) => new(a.Degree - b);
        public static Angle operator -(float a, Angle b) => new(a - b.Degree);
        public static Angle operator *(Angle a, float b) => new(a.Degree * b);
        public static Angle operator *(float a, Angle b) => new(a * b.Degree);
        public static Angle operator /(Angle a, float b) => new(a.Degree / b);


        public static implicit operator float (Angle a) => a.Degree;
        public static implicit operator Angle (float a) => new(a);

        public static explicit operator MVec2 (Angle a) => new(MathF.Cos(a), MathF.Sin(a));

        #endregion

    }
}
