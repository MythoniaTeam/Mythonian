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


        public List<Sprite> Entities { get; init; }



        public MGame() : base()
        {

            Graphics = new(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
        }


        public FrameCounter FrameCounter { get; private set; }
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

            FrameCounter = new FrameCounter(this);
            Components.Add(FrameCounter);
            TextManager.Ins.WriteLine(() => $"FPS: {FrameCounter.AverageFramesPerSecond}");

        }

        protected override void LoadContent()
        {
            base.LoadContent();
            DrawManager = new(this);
            Components.Add(DrawManager);
            TextureManager = new(this);
        }

        public Stopwatch Stopwatch { get; init; } = new();
        public DateTime Time { get; private set; }
        public DateTime TimeNow { get; private set; }
        public double FPS { get; private set; }
        public int FrameCount { get; private set; } = -1;
        protected override void Update(GameTime gameTime)
        {
            //TextManager.Ins.WriteLine(() => $"{gameTime.ElapsedGameTime.TotalSeconds}");

            FrameCount++;
            /*if(FrameCount > 5 && FrameCount % 15 == 0)
            {
                /*Stopwatch.Stop();
                TimeNow = DateTime.Now;
                //FPS = Math.Round(Math.Min(99999, Stopwatch.Elapsed.TotalSeconds), 2);
                //FPS = Math.Round(Math.Min(99999, 15 / Stopwatch.Elapsed.TotalSeconds), 2);
                FPS = Math.Round(Math.Min(99999, 15 / (TimeNow - Time).TotalSeconds), 2);
                
                if(15 / (TimeNow - Time).TotalSeconds > 60)
                {
                    //SDebug.WriteLine("t");*
                if(FrameCounter.AverageFramesPerSecond > 60.5)
                {
                    string fps = FrameCounter.AverageFramesPerSecond.ToString();
                    string time = "";// (TimeNow - Time).TotalSeconds.ToString();

                    TextManager.Ins.WriteLine(() => $"FPS: {fps}, Time: {/*Math.Round((TimeNow - Time).TotalSeconds, 4)*time}", 200);
                }
                /*}

                Stopwatch.Restart();
                Time = TimeNow;*
            }*/
            base.Update(gameTime);



        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new(10, 0, 30));

            SpriteBatch.Begin(samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.Deferred);


            base.Draw(gameTime);

            SpriteBatch.End();
        }
    }
}
