

namespace Mythonia.Game.UserInterface
{
    public class TextManager : DrawableGameComponent
    {
        public MGame MGame => (MGame)Game;
        public MVec2 ScreenSize => MGame.GraphicsDevice.Size();


        public List<Func<string>> DebugText { get; init; } = new();

        public SpriteFont DefaultFont { get; init; }

        public TextManager(MGame game, SpriteFont defaultFont) : base(game)
        {
            DefaultFont = defaultFont;
        }

        public override void Draw(GameTime gameTime)
        {
            MVec2 bound = (42, 30);
            MVec2 pos = bound;
            foreach (var text in DebugText)
            {
                MGame.SpriteBatch.DrawString(DefaultFont, text(), pos, Color.White);
                pos.Y += 16;
                if (pos.Y > ScreenSize.Y - bound.Y)
                {
                    pos.Y = bound.Y;
                    pos.X += 250;
                }
            }
        }

    }
}
