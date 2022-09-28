



namespace Mythonia.Game.Physics
{
    public class RectangleHitbox : IHitbox
    {
        public MGame MGame { get; init; }

        public MVec2 Position => _getPosMethod();

        private readonly Func<MVec2> _getPosMethod;

        public MVec2 Size => _getSizeMethod(); //{ get; set; }
        private readonly Func<MVec2> _getSizeMethod;

        public MVec2 BottomLeft => Position - Size / 2;
        public MVec2 TopRight => Position + Size / 2;

        public IHitbox.Types Type { get; private set; }


        public RectangleHitbox(MGame game, Func<MVec2> getPosMethod, Func<MVec2> getSizeMethod, IHitbox.Types type)
        {
            MGame = game;
            _getPosMethod = getPosMethod;
            _getSizeMethod = getSizeMethod;
            Type = type;
        }



        public override string ToString()
        {
            MVec2 fr = Position - Size / 2;
            MVec2 to = Position + Size / 2;

            return $"({fr.X}, {fr.Y}) ({to.X}, {to.Y})";
        }

        public void DrawHitbox(Color color)
        {
            var (scrPos, scrDir, scale) =
                MGame.Main.Camera.Transform(new(Position)).ToTuple;
            MGame.SpriteBatch.Draw(MTextureManager.Ins.PX, (MVec2)scrPos, null, color, scrDir.Radian, new MVec2(1, 1) / 2, scale * Size, SpriteEffects.None, 1);
        }
    }
}
