



namespace Mythonia.Game.Physics
{
    public class RectangleHitboxEvented : RectangleHitbox
    {
        public delegate void HitEvent(Sprite s);

        public event HitEvent WhenBeStepped;

        public void SteppedBy(Sprite s)
        {
            WhenBeStepped(s);
        }


        public RectangleHitboxEvented(MGame game, Func<MVec2> getposmethod, MVec2 size, IHitbox.Types type) : base(game, getposmethod, size, type)
        {

        }
    }
}
