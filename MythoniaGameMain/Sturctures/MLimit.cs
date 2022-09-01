using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mythonia.Sturctures
{
    public struct MLimit
    {
        public float LimitValue { get; set; }

        public MLimit(float limitValue)
        {
            LimitValue = limitValue;
        }


        //public bool IsInLimit(float v) => MathF.Abs(v) <= LimitValue;

        public float Limit(float v)
        {
            if (!v.IsInLimit(this))
                v = MathF.Sign(v) * LimitValue;
            //如果在范围外，把v的值限制到限制值
            return v;
        }



        public static implicit operator float(MLimit v) => v.LimitValue;
    }

    public static class MLimitExtensions
    {
        /// <summary>
        /// 给定一个 <see cref="MLimit"/>, 判断值是否位于 Limit 的范围内
        /// </summary>
        /// <param name="v"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static bool IsInLimit(this float v, MLimit limit) => MathF.Abs(v) <= limit.LimitValue;
    }
}
