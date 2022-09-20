


namespace Mythonia.Game.Draw.Texture
{
    public class NineSliceTexture : MTexture
    {

        public MVec2 Border;

        public NineSliceTexture(MGame game, ContentManager content, string name, MVec2 border) : base(game, content, name)
        {
            Border = border;

            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {

                    MVec2 position = Size + (Size + border) * (x, y) / 2;
                    MVec2 size = (
                        x == 0 ? Size.X - 2 * border.X : border.X,
                        y == 0 ? Size.Y - 2 * border.Y : border.Y
                    );

                    Frames.Add(new($"{x},{y}", new(position - size / 2, size)));

                };
            };
        }



        public override void Draw(SpriteBatch spriteBatch, Camera camera, Rectangle _, Transform transform)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {

                    Rectangle range = Frames[$"{x},{y}"].Range;

                    MVec3 position = transform.Position + (Size - Border) * (x, y) / 2;
                    MVec2 scale = (
                        x == 0 ? transform.Scale.X : 1,
                        y == 0 ? transform.Scale.Y : 1
                    );

                    transform = new(position, transform.Direction, scale);

                    base.Draw(spriteBatch, camera, range, transform);

                };
            };
        }

    }
}
