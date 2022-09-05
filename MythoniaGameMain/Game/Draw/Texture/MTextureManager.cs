



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

            Add(new TileTexture(Content, "Tile", TileSize));
            Add(new MTexture(Content, "TestPlayer"));
            Add(new MTexture(Content, "BouncingBomb").SecTexture(4));

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
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public MTextureManager(MGame game)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
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
