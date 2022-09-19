namespace Mythonia.Game.Pen
{
    public class Pen : DrawableGameComponent
    {

        #region Prop

        public MGame MGame { get; set; }
        public Texture2D Texture { get; set; }
        public List<Line> LineList = new();

        #endregion



        #region Constructor

        public Pen(MGame game) : base(game)
        {
            MGame = game;
            Texture = game.PX;
        }

        #endregion



        #region Methods

        public override void Draw(GameTime gametime)
        {

            foreach (Line line in LineList)
            {
                var (scrPos, direction, scale) =
                    MGame.Main.Camera.Transform(line.MidPoint, line.Direction, new(line.Length, line.Width));

                MGame.SpriteBatch.Draw(
                    Texture, scrPos, null, line.Color, direction.Radium, new(0.5f), scale, SpriteEffects.None, 0);
            }

        }

        #endregion

    }
}
