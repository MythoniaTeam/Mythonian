using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mythonia.Game.Sprites
{
    public class EntitiesManager : List<Entity>
    {
        public static EntitiesManager Ins { get; private set; }

        public EntitiesManager()
        {
            Ins = this;
        }

    }
}
