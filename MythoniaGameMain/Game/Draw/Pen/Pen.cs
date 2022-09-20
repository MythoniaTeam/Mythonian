namespace Mythonia.Game.Draw.Pen
{
    public class Pen : DrawableGameComponent
    {
        public static Pen Ins { get; set; }


        #region Prop

        public MGame MGame { get; set; }
        public Texture2D Texture { get; set; }
        public List<(Line Line, float Life)> LineList = new();

        #endregion



        #region Constructor

        public Pen(MGame game) : base(game)
        {
            Ins = this;
            MGame = game;
            Texture = MTextureManager.Ins.PX;
        }

        #endregion



        #region Methods

        public void DrawLine(MVec2 p1, MVec2 p2, float life = float.MaxValue, Color? color = null, float width = 1)
        {
            LineList.Add((new Line(p1, p2, color ?? Color.White, width), life));
        }


        public override void Update(GameTime gameTime)
        {
            for(int i = 0; i < LineList.Count; i++)
            {
                LineList[i] = (LineList[i].Line, LineList[i].Life - gameTime.CFDuration());
                if(LineList[i].Life < 0)
                {
                    LineList.RemoveAt(i);
                    i--;
                }
            }
        }

        public override void Draw(GameTime gametime)
        {

            foreach (var (line, _) in LineList)
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
