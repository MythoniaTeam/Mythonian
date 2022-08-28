



namespace Mythonia.Game.Physics
{
    public class RectangleHitbox : IHitbox
    {
        public MGame MGame { get; init; }

        public MVec2 Position => _getPosMethod();

        private readonly Func<MVec2> _getPosMethod;

        public MVec2 Size { get; set; }


        public MVec2 BottomLeft => Position - Size / 2;
        public MVec2 TopRight => Position + Size / 2;


        public RectangleHitbox(MGame game, Func<MVec2> getposmethod, MVec2 size)
        {
            MGame = game;
            _getPosMethod = getposmethod;
            Size = size;
        }



        public override string ToString()
        {
            MVec2 fr = Position - Size / 2;
            MVec2 to = Position + Size / 2;

            return $"({fr.X}, {fr.Y}) ({to.X}, {to.Y})";
        }

        public void DrawHitbox(Color color)
        {
            var (scrPos, _, scale) =
                MGame.Main.Camera.Transform(Position, scale: Size);
            MGame.SpriteBatch.Draw(MGame.PX, scrPos, null, color, 0, new(0.5f, 0.5f), scale, SpriteEffects.None, 0);
        }
    }
}
