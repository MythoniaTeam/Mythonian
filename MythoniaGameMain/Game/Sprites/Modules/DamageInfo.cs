using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mythonia.Game.Sprites.Modules
{
    public class DamageInfo : ICloneable
    {
        public int DamageValue { get; set; }

        public MVec2 KnokBack { get; set; }


        public DamageInfo(int damage, MVec2? knokBack = null)
        {
            DamageValue = damage;
            if (knokBack is MVec2 knokBack2) KnokBack = knokBack2;
        }

        public DamageInfo Clone()
        {
            return new(DamageValue, KnokBack);
        }
        object ICloneable.Clone() => Clone();
    }
}
