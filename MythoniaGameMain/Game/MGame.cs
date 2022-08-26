


namespace Mythonia.Game
{
    public class MGame : XNA.Game
    {
        public GraphicsDeviceManager Graphics { get; init; }

        public MTextureManager TextureManager { get; init; }
        public SpriteBatch SpriteBatch { get; set; }

        public MGameMain Main { get; set; }



        public Rectangle TileSize { get; init; } = new(0, 0, 16, 16);


        public MGame() : base()
        {
            Graphics = new(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            TextureManager = new(this);
        }


        //Texture2D tSprite;
        

        protected override void Initialize()
        {

            //IsFixedTimeStep = false;
            base.Initialize();

            SpriteBatch = new(GraphicsDevice);
            Main = new(this, TileSize);

            //tSprite = Content.Load<Texture2D>(@"Images\RECTANGLE");

            //MTextureManager.Add("")

        }

        public Texture2D PX { get; private set; }
        protected override void LoadContent()
        {
            base.LoadContent();

            PX = Content.Load<Texture2D>("Images/PX");

            TextureManager.AddTileTexture("Tile", TileSize);
                
            TextureManager.AddNewTexture("TestPlayer");
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);



        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new(0,10,30));

            SpriteBatch.Begin();

            base.Draw(gameTime);

            SpriteBatch.End();
        }
    }
}
