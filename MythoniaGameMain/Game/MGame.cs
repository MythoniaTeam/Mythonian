using System.Globalization;


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

        SpriteFont TestCNFont;
        protected override void Initialize()
        {
            Strings.Culture = new("zh-CN");// CultureInfo.CurrentCulture;
            SDebug.WriteLine(Strings.Culture);

            //IsFixedTimeStep = false;
            base.Initialize();

            SpriteBatch = new(GraphicsDevice);
            Main = new(this, TileSize);

            Window.BeginScreenDeviceChange(false);
            Window.EndScreenDeviceChange(GraphicsDevice.ToString(), 1366, 768);
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
            TestCNFont = Content.Load<SpriteFont>("Fonts/File");
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);



        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new(0, 10, 30));

            SpriteBatch.Begin();

            SpriteBatch.DrawString(TestCNFont, "这是一段测试文本", new(100, 100), Color.White);
            //SpriteBatch.DrawString(Main.Text.DefaultFont, "这是玩家", new(100, 100), Color.White);

            base.Draw(gameTime);

            SpriteBatch.End();
        }
    }
}
