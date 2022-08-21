



namespace Mythonia.Game
{
    public class Camera
    {

        #region Props

        public MGame MGame { get; init; }

        public MVec3 Pos { get; set;  }

        public float Focus { get; set; }

        public float Scale { get; set; }

        public Angle Direction { get; set; } = 0;

        #endregion



        #region Constructor

        public Camera(MGame game, float? x, float? y, float? z = null, float? focus = null, Angle? direction = null, float? scale = null)
        {
            MGame = game;

            Scale = scale ?? 1;
            Direction = direction ?? Angle.Left;
            Focus = focus ?? 100;
            Pos = new MVec3(x ?? 0, y ?? 0, z ?? 2 * Focus);
        }

        #endregion



        #region Methods

        public (MVec2 scrPos, Angle direction, MVec2 scale) Transform(MVec3 pos, Angle direction = default, MVec2? scale = null)
        {

            float zScale = Scale / ((Pos.Z - pos.Z - Focus) / Focus);
            MVec2 tranPos = (MVec2)(pos - Pos) * zScale;

            tranPos.Rotation(-Direction);

            tranPos.Y *= -1;
            tranPos += MGame.GraphicsDevice.Size() / 2;

            return (tranPos, direction - Direction, scale ?? MVec2.One * zScale);

        }

        #endregion

    }
}
