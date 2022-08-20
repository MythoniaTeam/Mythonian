#nullable enable



namespace Mythonia.Game
{
    public class MTexture
    {
        private readonly string _name;
        public string Name => _name;

        public Texture2D Raw { get; init; }

        public MVec2 Size { get; set; }
        public float Width => Size.X;
        public float Height => Size.Y;


        public Dictionary<string, Rectangle> SubTextures { get; init; } = new();


        public MTexture(ContentManager content, string name, ICollection<(string Name, Rectangle Range)>? subtextures = null)
        {
            Raw = content.Load<Texture2D>("Images/" + name);
            Size = new MVec2(Raw.Width, Raw.Height);

            _name = name;
            if (subtextures is not null)
            foreach(var sub in subtextures)
            {
                SubTextures.Add(sub.Name, sub.Range);
            }
        }

        public MTexture SecTexture(int column, int row = 1, int? no = null, List<string>? subtexturesname = null)
        {
             MVec2 gridSize = Size / (column, row);


            subtexturesname ??= new();

            for (int i = subtexturesname.Count; i < no; i++) subtexturesname.Add(i.ToString());

            for(int y = 0; y < row; y++)
            {
                for(int x = 0; x < column; x++)
                {
                    int index = y * row + x;
                    if (index > no) break;
                    SubTextures.Add(subtexturesname[index], new((x, y) * gridSize, gridSize));
                }
            }

            return this;
        }

        public MTexture SecAsTile(Rectangle gridSize)
        {
            Size = gridSize.Size;
            Tile.TextureBorderExtend[] borderType = new[]
            {
                Tile.TextureBorderExtend.TL,
                Tile.TextureBorderExtend.T,
                Tile.TextureBorderExtend.TR,
                Tile.TextureBorderExtend.NoB,
                Tile.TextureBorderExtend.TL_Ag,
                Tile.TextureBorderExtend.TR_Ag,
                Tile.TextureBorderExtend.T_AgBR,
                Tile.TextureBorderExtend.T_AgBoth,
                Tile.TextureBorderExtend.T_AgBL,

                Tile.TextureBorderExtend.L,
                Tile.TextureBorderExtend.No,
                Tile.TextureBorderExtend.R,
                Tile.TextureBorderExtend.LR,
                Tile.TextureBorderExtend.BL_Ag,
                Tile.TextureBorderExtend.BR_Ag,
                Tile.TextureBorderExtend.B_AgTR,
                Tile.TextureBorderExtend.B_AgBoth,
                Tile.TextureBorderExtend.B_AgTL,

                Tile.TextureBorderExtend.BL,
                Tile.TextureBorderExtend.B,
                Tile.TextureBorderExtend.BR,
                Tile.TextureBorderExtend.NoT,
                Tile.TextureBorderExtend.L_AgBR,
                Tile.TextureBorderExtend.R_AgBL,
                Tile.TextureBorderExtend.No_AgBR,
                Tile.TextureBorderExtend.No_AgB,
                Tile.TextureBorderExtend.No_AgBL,

                Tile.TextureBorderExtend.NoR,
                Tile.TextureBorderExtend.TB,
                Tile.TextureBorderExtend.NoL,
                Tile.TextureBorderExtend.All,
                Tile.TextureBorderExtend.L_AgBoth,
                Tile.TextureBorderExtend.R_AgBoth,
                Tile.TextureBorderExtend.No_AgR,
                Tile.TextureBorderExtend.No_AgAll,
                Tile.TextureBorderExtend.No_AgL,

                Tile.TextureBorderExtend.No_AgNoTL,
                Tile.TextureBorderExtend.No_AgNoTR,
                Tile.TextureBorderExtend.No_AgTR_BL,
                Tile.TextureBorderExtend.No_AgTL_BR,
                Tile.TextureBorderExtend.L_AgTR,
                Tile.TextureBorderExtend.R_AgTL,
                Tile.TextureBorderExtend.No_AgTR,
                Tile.TextureBorderExtend.No_AgT,
                Tile.TextureBorderExtend.No_AgTL,

                Tile.TextureBorderExtend.No_AgNoBL,
                Tile.TextureBorderExtend.No_AgNoBR

            };
            for(int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    if (9 * y + x >= borderType.Length) break;
                    SecAsTile(gridSize, x, y, borderType[9 * y + x]);
                }
            }
            return this;
        }
        private void SecAsTile(Rectangle gridSize, int x, int y, Tile.TextureBorderExtend borderType)
        {
            SubTextures.Add(borderType.ToString(), new(gridSize.Width * x, gridSize.Height * y, gridSize.Width, gridSize.Height));
        }



        #region Operators - Type

        public static implicit operator Texture2D(MTexture texture) => texture.Raw;

        #endregion


    }
}
