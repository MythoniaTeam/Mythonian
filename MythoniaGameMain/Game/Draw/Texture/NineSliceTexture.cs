


namespace Mythonia.Game.Draw.Texture
{
    public class NineSliceTexture : MTexture, ICloneable
    {

        public MVec2 BorderSize { get; init; }

        public MVec2 MidSize { get; set; }

        public UI BoundedUI { get; set; }

        protected NineSliceTexture(MGame game, Texture2D rawTexture, string name, MVec2 size, MVec2 originRatio,
            NamedList<Frame> frames, NamedList<AnimationMeta> animations,
            MVec2 borderSize, MVec2 midSize)
            : base(game, rawTexture, name, size, originRatio, frames, animations)
        {
            BorderSize = borderSize;
            MidSize = midSize;
        }

        public NineSliceTexture(MGame game, ContentManager content, string name, MVec2 borderSize) : base(game, content, name)
        {
            BorderSize = borderSize;
            MidSize = Size - borderSize * 2;

            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {

                    MVec2 position = (Size + (MidSize + BorderSize) * (x, y)) / 2;
                    MVec2 size = (
                        x == 0 ? MidSize.X : BorderSize.X,
                        y == 0 ? MidSize.Y : BorderSize.Y
                    );

                    Frames.Add(new($"{x},{-y}", new(position - size / 2, size)));

                };
            };
        }

        public override void Draw(SpriteBatch spriteBatch, Camera camera, Transform transform)
        {
            base.Draw(spriteBatch, camera, transform);
        }


        public override void Draw(SpriteBatch spriteBatch, Transform transform)
        {
            if(Name == "UI/HealthBar")
            {
                int a = 1;
            }
            MVec2 originRatioRecord = OriginRatio;

            OriginRatio = MVec2.Center;

            MVec2 requiredMidSize = BoundedUI.Size - BorderSize * 2;
            transform.Scale = requiredMidSize / MidSize;
            transform.Position -= BoundedUI.OriginDisplacement;

            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {

                    PlayFrame($"{x},{y}");

                    MVec3 position = transform.Position + (requiredMidSize + BorderSize) / 2 * (x, y);
                    MVec2 scale = (
                        x == 0 ? transform.Scale.X : 1,
                        y == 0 ? transform.Scale.Y : 1
                    );

                    position.RotationXY(transform.Direction);
                    var transform2 = new Transform(position, transform.Direction, scale);

                    ITexture.Draw(this, spriteBatch, transform2);

                };
            };
            OriginRatio = originRatioRecord;

        }

        //private Rectangle _sourceRange;
        //public override Rectangle GetSourceRange()
        //{
        //    return _sourceRange;
        //}

        public new NineSliceTexture Clone()
        {
            return new NineSliceTexture(MGame, RawTexture, Name, Size, OriginRatio, Frames, Animations, BorderSize, MidSize);
        }
        object ICloneable.Clone() => Clone();

    }
}
