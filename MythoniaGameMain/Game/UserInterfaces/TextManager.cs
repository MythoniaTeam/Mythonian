

namespace Mythonia.Game.UserInterfaces
{
    public class TextManager : DrawableGameComponent
    {
        public static TextManager Ins { get; set; } = null;
        
        public MGame MGame => (MGame)Game;
        public MVec2 ScreenSize => MGame.GraphicsDevice.Size();



        private List<TextInstance> DebugText { get; init; } = new();

        public SpriteFont DefaultFont { get; init; }


        private class TextInstance
        {
            public Func<string> Text { get; init; }
            public float Life { get; private set; }

            public TextInstance(Func<string> text, float life)
            {
                Text = text;
                Life = life;
            }

            public bool LifeUpdate(GameTime gameTime)
            {
                if(Life != float.MaxValue)
                    Life -= gameTime.CFDuration(); 
                return Life <= 0;
            }

            public override string ToString()
            {
                return $"(Life: {Life}) {Text()}";
            }

            public static implicit operator string(TextInstance v) => v.Text();
        }

        public TextManager(MGame game, SpriteFont defaultFont) : base(game)
        {
            DefaultFont = defaultFont;
            if (Ins == null) Ins = this;
            DrawOrder = 20000;
        }

        public void WriteLine(Func<string> text, float? life = null)
        {
            DebugText.Add(new(text, life ?? float.MaxValue));
        }


        public override void Draw(GameTime gameTime)
        {
            MVec2 bound = (42, 30);
            MVec2 pos = bound;
            List<TextInstance> removeList = new();
            foreach (var text in DebugText)
            {
                MGame.SpriteBatch.DrawString(DefaultFont, text, pos, Color.White);
                pos.Y += 16;
                if (pos.Y > ScreenSize.Y - bound.Y)
                {
                    pos.Y = bound.Y;
                    pos.X += 250;
                }
                if (text.LifeUpdate(gameTime)) removeList.Add(text);
            }
            foreach(var remove in removeList)
            {
                DebugText.Remove(remove);
            }
        }

    }
}
