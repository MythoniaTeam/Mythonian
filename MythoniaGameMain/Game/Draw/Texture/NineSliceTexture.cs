

namespace Mythonia.Game.Draw.Texture
{
    public class NineSliceTexture : MTexture
    {
        private MVec2 SliceSize;

        private Rectangle[,] SliceRanges = new Rectangle[3,3];


        public NineSliceTexture(MGame game, ContentManager content, string name) : base(game, content, name)
        {
            NamedList<Frame> slices = new NamedList<Frame>();
            SliceSize = new MVec2(RawTexture.Width, RawTexture.Height) / 3;
            
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    SliceRanges[y, x] = new(new MVec2(x, y) * SliceSize, SliceSize);
                }
            }
        }



        public override void Draw(SpriteBatch spriteBatch, Camera camera, Rectangle sourceRange, Transform transform)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {

                    /*
                     * ⚠⚠⚠ 注意 ⚠⚠⚠
                     * 下面这个position是“每个切块相对于图像中心的偏移量”
                     * 我知道我写的这个很可能是错的
                     * 但我脑子稍微有点不够用了
                     */

                    MVec2 position = new MVec2(x, y) * (SliceSize + new MVec2(1, 1)) / 2;

                    Rectangle sliceRange = SliceRanges[x + 1, 1 - y];

                    Transform sliceTransform = new(
                        transform.Position + position,
                        transform.Direction,
                        new(
                            x == 0 ? transform.Scale.X : 1,
                            y == 0 ? transform.Scale.Y : 1
                        )
                    );

                    base.Draw(spriteBatch, camera, sliceRange, sliceTransform);
                }
            }
        }
    }
}
