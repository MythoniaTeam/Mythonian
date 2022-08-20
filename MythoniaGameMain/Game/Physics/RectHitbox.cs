



namespace Mythonia.Game.Physics
{
    public class RectHitbox : IHitbox
    {
        public Main MGame { get; init; }

        public MVec2 Position => _getPosMethod();

        private readonly Func<MVec2> _getPosMethod;

        public MVec2 Size { get; set; }


        public RectHitbox(Main game, Func<MVec2> getposmethod, MVec2 size)
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
                MGame.MainGame.Camera.Transform(Position, scale: Size);
            MGame.SpriteBatch.Draw(MGame.PX, scrPos, null, color, 0, new(0.5f, 0.5f), scale, SpriteEffects.None, 0);
        }
    }
}
