using System.Globalization;


namespace Mythonia.Game
{
    public class MGame : XNA.Game
    {
        public GraphicsDeviceManager Graphics { get; init; }

        public MTextureManager TextureManager { get; private set; }
        public SpriteBatch SpriteBatch { get; set; }
        public DrawManager DrawManager { get; private set; }

        public MGameMain Main { get; set; }





        public MGame() : base()
        {

            Graphics = new(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
        }



        protected override void Initialize()
        {
            Strings.Culture = new("zh-CN");// CultureInfo.CurrentCulture;
            SDebug.WriteLine(Strings.Culture);

            base.Initialize();
            //IsFixedTimeStep = false;

            Main = new(this, MTextureManager.Ins.TileSize);
            SpriteBatch = new(GraphicsDevice);

            Window.BeginScreenDeviceChange(false);
            Window.EndScreenDeviceChange(GraphicsDevice.ToString(), 1366, 768);

        }

        protected override void LoadContent()
        {
            base.LoadContent();
            TextureManager = new(this);
            DrawManager = new(this);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);



        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new(0, 10, 30));

            SpriteBatch.Begin(samplerState: SamplerState.PointClamp);


            base.Draw(gameTime);

            SpriteBatch.End();
        }
    }
}
