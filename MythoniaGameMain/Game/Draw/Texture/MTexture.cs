#nullable enable



namespace Mythonia.Game.Draw.Texture
{
    public class MTexture: ITexture
    {
        private readonly string _name;
        public string Name => _name;

        public Texture2D RawTexture { get; init; }

        /// <summary>
        /// 单帧图的尺寸 (没有帧图则是整图尺寸)
        /// </summary>
        public MVec2 Size { get; protected set; }
        public float Width => Size.X;
        public float Height => Size.Y;


        /// <summary>
        /// 帧图的名称和对应范围
        /// </summary>
        public Dictionary<string, Rectangle> SubTextures { get; init; } = new();



        public MTexture(ContentManager content, string name, ICollection<(string Name, Rectangle Range)>? subtextures = null)
        {
            RawTexture = content.Load<Texture2D>("Images/" + name);
            Size = new MVec2(RawTexture.Width, RawTexture.Height);

            _name = name;
            if (subtextures is not null)
            foreach(var sub in subtextures)
            {
                SubTextures.Add(sub.Name, sub.Range);
            }
        }



        /// <summary>
        /// 按照行、列数分割帧图, 保存到 <see cref="SubTextures"/> 中
        /// </summary>
        /// <param name="column">列数</param>
        /// <param name="row">行数</param>
        /// <param name="no">帧图数 (在多行帧图，最后一行没有填满的情况需要提供，否则会出现空白帧的情况)</param>
        /// <param name="subtexturesname">帧图的名称 (从左到右，从上到下，缺失的会以 "# + 数字编号" 命名)</param>
        /// <returns><see langword="this"/></returns>
        public MTexture SecTexture(int column, int row = 1, int? no = null, IList<string>? subtexturesname = null)
        {
             MVec2 gridSize = (RawTexture.Width / column, RawTexture.Height / row);

            no ??= column * row;
            subtexturesname ??= new List<string>();

            int count = subtexturesname.Count;
            for (int i = count; i < no; i++) subtexturesname.Add((i-count).ToString());

            for(int y = 0; y < row; y++)
            {
                for(int x = 0; x < column; x++)
                {
                    int index = y * column + x;
                    if (index >= no) break;
                    SubTextures.Add(subtexturesname[index], new((x, y) * gridSize, gridSize));
                }
            }

            return this;
        }




        #region Implement - ITexture

        public Rectangle GetSourceRange()
        {
            return new(new(0, 0), Size);
        }

        #endregion



        #region Operators - Type

        public static implicit operator Texture2D(MTexture texture) => texture.RawTexture;

        #endregion


    }
}
