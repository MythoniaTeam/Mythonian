namespace Mythonia.Game.Pen
{
    public class Line
    {

        #region Props

        public MVec2 P1 { get; set; }
        public MVec2 P2 { get; set; }
        public float Width { get; set; }
        public Color Color { get; set; }

        public MVec2 MidPoint => (P1 + P2) / 2;
        public Angle Direction => (P1 - P2).Direction;
        public float Length => (P1 - P2).Length();

        #endregion



        #region Constructor

        public Line(MVec2 p1, MVec2 p2, Color color, float width = 1)
        {
            P1 = p1;
            P2 = p2;
            Color = color;
            Width = width;
        }

        #endregion

    }
}
