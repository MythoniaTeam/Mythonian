



namespace Mythonia.Game.Draw.Texture
{
    internal class TileTexture : MTexture
    {
        private static Tile.TextureBorderExtend[] BorderTypeList = new[]
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
        public TileTexture(ContentManager content, string name, Rectangle gridSize) : base(content, name)
        {
            SecAsTile(gridSize);
            
        }


        public MTexture SecAsTile(Rectangle gridSize)
        {
            List<string> subtextureNames = new();
            foreach (var borderType in BorderTypeList)
            {
                subtextureNames.Add(borderType.ToString());
            }
            SecTexture(9, 6, 47, subtextureNames);

            Size = gridSize.Size;
            return this;
        }
    }
}
