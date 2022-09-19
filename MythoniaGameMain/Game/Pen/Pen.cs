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
            Texture = MTextureManager.Ins.PX;
        }

        #endregion



        #region Methods

        public override void Draw(GameTime gametime)
        {

            foreach (Line line in LineList)
            {
                var (scrPos, direction, scale) =
                    MGame.Main.Camera.Transform(new(line.MidPoint, line.Direction, new(line.Length, line.Width))).ToTuple;

                MGame.SpriteBatch.Draw(
                    Texture, (MVec2)scrPos, null, line.Color, direction.Radian, new(0.5f), scale, SpriteEffects.None, 0);
            }

        }

        #endregion

    }
}
