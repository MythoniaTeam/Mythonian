




namespace Mythonia.Game.Sprites
{
    /// <summary>
    /// 有位置、能够被绘制的对象
    /// </summary>
    public abstract class Sprite : DrawableGameComponent, INamed
    {
        #region Prop
        public string Name { get; init; }

        public MGame MGame => (MGame)Game;


        public ITexture Texture { get; protected set; }
        //public virtual Rectangle TextureSourceRange => Texture.GetSourceRange();

        protected MVec3 _position;
        protected Angle _direction;
        protected MVec2 _scale = (1, 1);
        //protected MVec2 _originRatio;
        public virtual MVec3 Position { get => _position; /*set => _position = value;*/ }
        public Angle Direction { get => _direction; /*set => _direction = value;*/ }
        public MVec2 Scale { get => _scale; /*set => _scale = value;*/ }
        //public MVec2 OriginRatio { get => _originRatio; set => _originRatio = value; }

        
        public Transform Transform
        {
            get => new(Position, Direction, Scale/*, _originRatio*/);
            //set => (_position, _direction, _scale) = value.ToTuple;
        }

        #endregion



        #region Constructors

        public Sprite(string name, MGame game, ITexture texture, Transform? transform = null) : base(game)
        {
            Name = name;
            Texture = texture;
            (_position, _direction, _scale) = (transform ?? new Transform(null)).ToTuple;

        }

        #endregion



        #region Methods

        public override void Draw(GameTime gameTime)
        {
            //base.Draw(gameTime);
            DrawTexture(MGame.SpriteBatch, MGame.Main.Camera);
        }

        public virtual void DrawTexture(SpriteBatch spriteBatch, Camera camera)
        {
            Texture?.Draw(spriteBatch, camera, Transform);
        }
        public virtual void DrawTexture(SpriteBatch spriteBatch)
        {
            Texture?.Draw(spriteBatch, Transform);
        }

        #endregion
    }
}
