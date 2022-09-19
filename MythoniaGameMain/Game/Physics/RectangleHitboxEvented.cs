



namespace Mythonia.Game.Physics
{
    public delegate void HitEvent(Sprite s);
    public class RectangleHitboxEvented : RectangleHitbox
    {

        public event HitEvent WhenBeStepped;

        public void SteppedBy(Sprite s)
        {
            if(WhenBeStepped is not null) WhenBeStepped(s);
        }


        public RectangleHitboxEvented(MGame game, Func<MVec2> getposmethod, MVec2 size, IHitbox.Types type) : base(game, getposmethod, size, type)
        {

        }
    }
}
