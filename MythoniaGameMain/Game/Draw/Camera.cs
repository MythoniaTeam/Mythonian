



namespace Mythonia.Game.Draw
{

    public class Camera : GameComponent
    {

        #region Props

        public MGame MGame { get; init; }

        public MVec3 Pos { get; set; }

        public MVec3 Target { get; set; }

        public float Focus { get; set; }

        public float Scale { get; set; }

        public Angle Direction { get; set; } = 0;

        public bool FollowPlayer { get; set; } = true;

        #endregion



        #region Constructor

        public Camera(MGame game, float? x, float? y, float? z = null, float? focus = null, Angle? direction = null, float? scale = null) : base(game)
        {
            MGame = game;

            Scale = scale ?? 1;
            Direction = direction ?? Angle.Left;
            Focus = focus ?? 100;
            Pos = new MVec3(x ?? 0, y ?? 0, z ?? 2 * Focus);
            Target = Pos;
        }

        #endregion



        #region Methods

        public Transform Transform(Transform transform)
        {

            float zScale = Scale / ((Pos.Z - transform.Position.Z - Focus) / Focus);
            MVec2 tranPos = (MVec2)(transform.Position - Pos) * zScale;

            tranPos.Rotation(-Direction);

            tranPos.Y *= -1;
            tranPos += MGame.GraphicsDevice.Size() / 2;
            Angle direction = transform.Direction - Direction;
            if(transform.Scale.X < 0 && transform.Scale.Y < 0) direction += 180f;
            direction *= -1;

            return (tranPos, direction, transform.Scale * zScale);

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Pos = MGame.Main.Player.Position.SetNew(z: Pos.Z);//方便测试先改成这样
            /*
            if (FollowPlayer)
            {
                MVec3 c;

                Player.Player _player = MGame.Main.Player;

                if (_player.Velocity.Length() == 0f)
                {
                    c = _player.Position;
                }
                else
                {
                    c = _player.Position + (_player.Velocity.Normalized * 100);
                }
                c.Z = Pos.Z;
                Target = c;
                if ((Target - Pos).Length() <= _player.Velocity.Length())
                {
                    Pos = Target;
                }
                else
                {
                    Pos += _player.Velocity;
                }
            };

            if ((Target - Pos).Length() <= 0.5)
            {
                Pos = Target;
            }
            else
            {
                Pos += (Target - Pos) / 50;
            }*/
        }

        #endregion

    }
}
