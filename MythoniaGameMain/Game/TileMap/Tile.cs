using Mythonia.Game.Physics;
#nullable enable


namespace Mythonia.Game.TileMap
{

    public class Tile : Sprite
    {

        public int Id { get; init; }

        //public override Rectangle TextureSourceRange => ((MTexture)Texture).Frames[TextureBorderType.ToString()].Range;

        public TextureBorderExtend TextureBorderType { get; private set; } = TextureBorderExtend.All;


        private bool _hasColl;
        public bool HasColl
        {
            get => _hasColl;
            set
            {
                _hasColl = value;
            }
        }

        public RectangleHitbox? Hitbox { get; set; }

        public TileTexture TileTexture => (TileTexture)Texture;



        #region Prop Map

        public Map Map { get; init; }

        private readonly MVec2? _mapIndex;

        public MVec2 MapIndex => _mapIndex ?? throw new IndexOutOfRangeException($"The Tile {this} doesn't have an index (not in a map)");
        public Tile? GetTile(MVec2 displacement) => (_mapIndex is MVec2 index /*&& _map is Map map*/) ? Map[index + displacement] : null;

        public Tile? TileTopLeft  => GetTile(MVec2.Dir9.TopLeft);
        public Tile? TileTop      => GetTile(MVec2.Dir9.Top);
        public Tile? TileTopRight => GetTile(MVec2.Dir9.TopRight);
        public Tile? TileLeft  => GetTile(MVec2.Dir9.Left);
        public Tile? TileRight => GetTile(MVec2.Dir9.Right);
        public Tile? TileBottomLeft  => GetTile(MVec2.Dir9.BottomLeft);
        public Tile? TileBottom      => GetTile(MVec2.Dir9.Bottom);
        public Tile? TileBottomRight => GetTile(MVec2.Dir9.BottomRight);

        #endregion


        #region Prop - Static

        public static readonly List<char> IdCharList = new(new char[]
        {
            ' ',
            '#',
            '|',
            '@'
        });

        #endregion



        #region Constructor

        private Tile(MGame game, int id, Map map, MVec2 index) 
            : base($"Tile-{index}", game, game.TextureManager.Get<TileTexture>("Tile"), new(index * map.TileSizeVec))
        {
            TileTexture.BoundedTile = this;

            Id = id;

            Map = map;
            _mapIndex = index;//map?.FindIndex(this);

            if (id != 0) HasColl = true; else HasColl = false;

            //Texture = game.TextureManager["Tile"];
            //Texture = game.TextureManager[$"Tile_{Id}"];
        }

        public static Tile? ConstructTile(MGame game, char symbol, Map map, MVec2 index)
            => ConstructTile(game, IdCharList.IndexOf(symbol), map, index);

        public static Tile? ConstructTile(MGame game, int id, Map map, MVec2 index)
            => id is 0 ? null :
                new(game, id, map, index);
        

        #endregion



        #region Enum

        public enum TextureBorder
        {
            All = 0b1111,

            NoT = 0b0111,
            NoL = 0b1011,
            NoR = 0b1101,
            NoB = 0b1110,

            BR = 0b0011,
            BL = 0b0101,
            LR = 0b0110,
            TB = 0b1001,
            TR = 0b1010,
            TL = 0b1100,

            B = 0b0001,
            R = 0b0010,
            L = 0b0100,
            T = 0b1000,

            No = 0b0000

        }

        public enum TextureBorderExtend
        {
            All = 0b11111111,


            NoT = 0b01111111,
            NoL = 0b10111111,
            NoR = 0b11011111,
            NoB = 0b11101111,


            BR = 0b00110111,//1个空角
            BR_Ag = 0b00111111,

            BL = 0b01011011,//1个空角
            BL_Ag = 0b01011111,

            LR = 0b01101111,

            TB = 0b10011111,

            TR = 0b10101101,//1个空角
            TR_Ag = 0b10101111,

            TL = 0b11001110,//1个空角
            TL_Ag = 0b11001111,


            B = 0b00010011,//2个空角
            B_AgTR = 0b00010111,
            B_AgTL = 0b00011011,
            B_AgBoth = 0b00011111,

            R = 0b00100101,//2个空角
            R_AgBL = 0b00100111,
            R_AgTL = 0b00101101,
            R_AgBoth = 0b00101111,

            L = 0b01001010,//2个空角
            L_AgBR = 0b01001011,
            L_AgTR = 0b01001110,
            L_AgBoth = 0b01001111,

            T = 0b10001100,//2个空角
            T_AgBR = 0b10001101,
            T_AgBL = 0b10001110,
            T_AgBoth = 0b10001111,


            No = 0b00000000,//4个空角

            No_AgBR = 0b00000001,
            No_AgBL = 0b00000010,
            No_AgTR = 0b00000100,
            No_AgTL = 0b00001000,

            No_AgB     = 0b00000011,
            No_AgR     = 0b00000101,
            No_AgL     = 0b00001010,
            No_AgT     = 0b00001100,
            No_AgTL_BR = 0b00001001,
            No_AgTR_BL = 0b00000110,

            No_AgNoTL = 0b00000111,
            No_AgNoTR = 0b00001011,
            No_AgNoBL = 0b00001101,
            No_AgNoBR = 0b00001110,

            No_AgAll = 0b00001111,

        }

        #endregion



        #region Methods

        public void UpdateTexture()
        {
            TextureBorderType = GetTextureBorderExtend();
        }

        public TextureBorder GetTextureBorder()
        {
            return (
                TextureConnectable(TileTop),
                TextureConnectable(TileLeft),
                TextureConnectable(TileRight),
                TextureConnectable(TileBottom)
            ) switch
            {
                (false, false, false, false) => TextureBorder.All,

                (true, false, false, false) => TextureBorder.NoT,
                (false, true, false, false) => TextureBorder.NoL,
                (false, false, true, false) => TextureBorder.NoR,
                (false, false, false, true) => TextureBorder.NoB,

                (false, false, true, true) => TextureBorder.TL,
                (false, true, false, true) => TextureBorder.TR,
                (false, true, true, false) => TextureBorder.TB,
                (true, false, false, true) => TextureBorder.LR,
                (true, false, true, false) => TextureBorder.BL,
                (true, true, false, false) => TextureBorder.BR,

                (false, true, true, true) => TextureBorder.T,
                (true, false, true, true) => TextureBorder.L,
                (true, true, false, true) => TextureBorder.R,
                (true, true, true, false) => TextureBorder.B,

                (true, true, true, true) => TextureBorder.No,
            };
        }

        public TextureBorderExtend GetTextureBorderExtend()
        {
            return (
                TextureConnectable(TileTop),
                TextureConnectable(TileLeft),
                TextureConnectable(TileRight),
                TextureConnectable(TileBottom)
            ) switch
            {
                (false, false, false, false) => TextureBorderExtend.All,

                (true, false, false, false) => TextureBorderExtend.NoT,
                (false, true, false, false) => TextureBorderExtend.NoL,
                (false, false, true, false) => TextureBorderExtend.NoR,
                (false, false, false, true) => TextureBorderExtend.NoB,


                (false, false, true, true) => //左上角不连接
                TextureConnectable(TileBottomRight) switch
                {
                    true => TextureBorderExtend.TL,
                    false => TextureBorderExtend.TL_Ag
                },

                (false, true, false, true) => //右上角不连接
                TextureConnectable(TileBottomLeft) switch
                {
                    true => TextureBorderExtend.TR,
                    false => TextureBorderExtend.TR_Ag
                },

                (false, true, true, false) => TextureBorderExtend.TB, //上下不连接
                (true, false, false, true) => TextureBorderExtend.LR, //左右不连接

                (true, false, true, false) => //左下不连接
                TextureConnectable(TileTopRight) switch
                {
                    true => TextureBorderExtend.BL,
                    false => TextureBorderExtend.BL_Ag
                },

                (true, true, false, false) => //右下不连接
                TextureConnectable(TileTopLeft) switch
                {
                    true => TextureBorderExtend.BR,
                    false => TextureBorderExtend.BR_Ag
                },


                (false, true, true, true) => //上方不连接
                (TextureConnectable(TileBottomLeft), TextureConnectable(TileBottomRight)) switch
                {
                    (true, true) => TextureBorderExtend.T,
                    (true, false) => TextureBorderExtend.T_AgBR,//右下不连接
                    (false, true) => TextureBorderExtend.T_AgBL,//左下不连接
                    (false, false) => TextureBorderExtend.T_AgBoth,
                },

                (true, false, true, true) => //左侧不连接
                (TextureConnectable(TileTopRight), TextureConnectable(TileBottomRight)) switch
                {
                    (true, true) => TextureBorderExtend.L,
                    (true, false) => TextureBorderExtend.L_AgBR,//右上不连接
                    (false, true) => TextureBorderExtend.L_AgTR,//右下不连接
                    (false, false) => TextureBorderExtend.L_AgBoth,
                },

                (true, true, false, true) => //右侧不连接
                (TextureConnectable(TileTopLeft), TextureConnectable(TileBottomLeft)) switch
                {
                    (true, true) => TextureBorderExtend.R,
                    (true, false) => TextureBorderExtend.R_AgBL,//左上不连接
                    (false, true) => TextureBorderExtend.R_AgTL,//左下不连接
                    (false, false) => TextureBorderExtend.R_AgBoth,
                },

                (true, true, true, false) => //下方不连接
                (TextureConnectable(TileTopLeft), TextureConnectable(TileTopRight)) switch
                {
                    (true, true) => TextureBorderExtend.B,
                    (true, false) => TextureBorderExtend.B_AgTR,
                    (false, true) => TextureBorderExtend.B_AgTL,
                    (false, false) => TextureBorderExtend.B_AgBoth,
                },


                (true, true, true, true) => (
                TextureConnectable(TileTopLeft),
                TextureConnectable(TileTopRight),
                TextureConnectable(TileBottomLeft),
                TextureConnectable(TileBottomRight)
                ) switch
                {
                    (false, false, false, false) => TextureBorderExtend.No_AgAll,

                    (true, false, false, false) => TextureBorderExtend.No_AgNoTL,
                    (false, true, false, false) => TextureBorderExtend.No_AgNoTR,
                    (false, false, true, false) => TextureBorderExtend.No_AgNoBL,
                    (false, false, false, true) => TextureBorderExtend.No_AgNoBR,

                    (false, false, true, true) => TextureBorderExtend.No_AgT,
                    (false, true, false, true) => TextureBorderExtend.No_AgL,
                    (false, true, true, false) => TextureBorderExtend.No_AgTL_BR,
                    (true, false, false, true) => TextureBorderExtend.No_AgTR_BL,
                    (true, false, true, false) => TextureBorderExtend.No_AgR,
                    (true, true, false, false) => TextureBorderExtend.No_AgB,

                    (false, true, true, true) => TextureBorderExtend.No_AgTL,
                    (true, false, true, true) => TextureBorderExtend.No_AgTR,
                    (true, true, false, true) => TextureBorderExtend.No_AgBL,
                    (true, true, true, false) => TextureBorderExtend.No_AgBR,

                    (true, true, true, true) => TextureBorderExtend.No
                }

            };
        }

        public virtual bool TextureConnectable(Tile? tile) => tile?.Id == Id;


        #region Methods - Override

        public override string ToString()
        {
            //return ((char)this).ToString();
            return $"({MapIndex} - {TextureBorderType}";
        }

        #endregion



        #region Methods - Game Component

        /*public void Draw(SpriteBatch spriteBatch, Camera cam)//, GameTime gameTime)
        {
            //var (scrPos, _, scale) = cam.Transform(new(Position));
            //spriteBatch.Draw(Texture, scrPos, Texture.SubTextures[TextureBorderType.ToString()], Color.White, 0, Texture.Size / 2, scale, SpriteEffects.None, 0);
            DrawManager.Ins.Draw(this);
        }*/

        #endregion



        #endregion Methods



        #region Operator - Type


        public static explicit operator int(Tile tile) => tile.Id;
        public static explicit operator char(Tile tile) => IdCharList[tile.Id];

        #endregion

    }
}
