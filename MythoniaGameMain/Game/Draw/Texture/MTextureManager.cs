



namespace Mythonia.Game.Draw.Texture
{
    /// <summary>
    /// 单例类
    /// </summary>
    public class MTextureManager
    {
        public static MTextureManager Ins { get; private set; }


        #region LoadTextures
        public Texture2D PX { get; private set; }
        public SpriteFont TestCNFont { get; private set; }
        public Rectangle TileSize { get; init; } = new(0, 0, 16, 16);

        void LoadTextures()
        {
            PX = Content.Load<Texture2D>("Images/PX");

            Add(new TileTexture(MGame, Content, "Tile", TileSize));
            Add(new MTexture(MGame, Content, "TestPlayer"));
            Add(new MTexture(MGame, Content, "BouncingBomb").SecTexture(4).AddAnimation());
            Add(new MTexture(MGame, Content, "PressurePlate", new (string, Rectangle)[]
            {
                ("Plate", new(0, 0, 32, 6)),
                ("BaseUnActivate", new(0, 6, 32, 10)),
                ("BaseActivate", new(0, 16, 32, 10))
            }));
            Add(new MTexture(MGame, Content, "AimingLineVertical").
                SecTexture(17).
                AddAnimation(duration: 6.5f, frameEnd: 8).
                AddAnimation("Activate", 4.8f, frameStart: 9));

            TestCNFont = Content.Load<SpriteFont>("Fonts/File");

        }

        #endregion



        public MGame MGame { get; init; }
        public ContentManager Content => MGame.Content;



        private readonly List<MTexture> _textures = new();
        /// <summary>
        /// 获取一个 <see cref="MTexture"/>,<br/>
        /// 可以使用 <see cref="Get{T}(string)"/> 方法代替, 省略类型转换
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public MTexture this[string name]
        {
            get => _textures.Find(texture => texture.Name == name) ?? throw new Exception($"Texture \"{name}\" is not found");
        }

        /// <summary>
        /// 实例化<see cref="MGame"/> 后, 在 <see cref="MGame.LoadContent"/> 中实例化
        /// </summary>
        /// <param name="game"></param>

        public MTextureManager(MGame game)

        {
            Ins ??= this;
            MGame = game;
            LoadTextures();
        }

        public void Add(MTexture texture) => _textures.Add(texture);

        /// <summary>
        /// 获取一个指定类型的贴图
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T Get<T>(string name) where T : MTexture => (T)this[name];

    }
}
