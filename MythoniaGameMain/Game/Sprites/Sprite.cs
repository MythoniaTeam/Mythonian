﻿




namespace Mythonia.Game.Sprites
{
    public class Sprite : DrawableGameComponent
    {
        #region Prop
        public string Name { get; init; }

        public MGame MGame => (MGame)Game;
        public Map Map { get; init; }


        public ITexture Texture { get; protected set; }
        public virtual Rectangle TextureSourceRange => Texture.GetSourceRange();

        protected MVec3 _position;
        protected Angle _direction;
        protected MVec2 _scale = (1, 1);
        public MVec3 Position { get => _position; set => _position = value; }
        public Angle Direction { get => _direction; set => _direction = value; }
        public MVec2 Scale { get => _scale; set => _scale = value; }
        
        public Transform Transform
        {
            get => new(_position, _direction, _scale);
            set => (_position, _direction, _scale) = value.ToTuple;
        }

        #endregion



        #region Constructors

        public Sprite(string name, MGame game, Map map, ITexture texture, MVec3? position = null) : base(game)
        {
            Map = map;
            Name = name;
            Texture = texture;
            _position = position ?? (0, 0, 0);
        }

        #endregion



        #region Methods

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            DrawTexture(MGame.SpriteBatch, MGame.Main.Camera);
        }

        public virtual void DrawTexture(SpriteBatch spriteBatch = null, Camera camera = null)
        {
            spriteBatch ??= MGame.SpriteBatch;
            camera ??= MGame.Main.Camera;
            Texture.Draw(spriteBatch, camera, TextureSourceRange, Transform);
        }

        #endregion
    }
}
