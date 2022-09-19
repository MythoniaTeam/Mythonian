



namespace Mythonia.Game.Physics
{
    public class CircleHitbox : IHitbox
    {
        public MVec2 Position => _getPosMethod();
        
        private readonly Func<MVec2> _getPosMethod;

        public float Radius { get; set; }
        public IHitbox.Types Type { get; private set; }


        public CircleHitbox(Func<MVec2> getposmethod, float radius, IHitbox.Types type)
        {
            _getPosMethod = getposmethod;
            Radius = radius;
            Type = type;
        }
    }
}
