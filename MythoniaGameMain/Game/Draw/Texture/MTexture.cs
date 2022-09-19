#nullable enable



namespace Mythonia.Game.Draw.Texture
{
    public class MTexture: ITexture, INamed
    {
        public string Name { get; init; }

        public MGame MGame { get; init; }

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
        public NamedList<Frame> Frames { get; init; } = new();

        public NamedList<AnimationMeta> Animations { get; init; } = new();



        public MTexture(MGame game, ContentManager content, string name, ICollection<(string Name, Rectangle Range)>? subtextures = null)
        {
            MGame = game;

            RawTexture = content.Load<Texture2D>("Images/" + name);
            Size = new MVec2(RawTexture.Width, RawTexture.Height);

            Name = name;
            if (subtextures is not null)
            foreach(var sub in subtextures)
            {
                Frames.Add(new(sub.Name, sub.Range));
            }
        }



        /// <summary>
        /// 按照行、列数分割帧图, 保存到 <see cref="Frames"/> 中
        /// </summary>
        /// <param name="column">列数</param>
        /// <param name="row">行数</param>
        /// <param name="no">帧图数 (在多行帧图，最后一行没有填满的情况需要提供，否则会出现空白帧的情况)</param>
        /// <param name="subtexturesname">帧图的名称 (从左到右，从上到下，缺失的会以 "# + 数字编号" 命名)</param>
        /// <returns><see langword="this"/></returns>
        public MTexture SecTexture(int column, int row = 1, int? no = null, IList<string>? subtexturesname = null)
        {
            Size = (RawTexture.Width / column, RawTexture.Height / row);

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
                    Frames.Add(new(subtexturesname[index], new((x, y) * Size, Size)));
                }
            }

            return this;
        }

        

        public MTexture AddAnimation(string name, float duration, int[] frames)
        {
            Animations.Add(new(this, name, duration, frames));
            return this;
        }
        public MTexture AddAnimation(string name = "Default", float duration = 10, int frameStart = 0, int? frameEnd = null)
        {
            int frameEnd2 = frameEnd ?? Frames.Count - 1;
            int[] frames = new int[frameEnd2 - frameStart + 1];
            for (int i = 0; i < frames.Length; i++)
            {
                frames[i] = i + frameStart;
            }
            AddAnimation(name, duration, frames);
            return this;
        }

        public MTexture AddAnimations((string name, float duration, int[] frames)[] animations)
        {
            foreach(var (name, duration, frames) in animations)
            {
                AddAnimation(name, duration, frames);
            }

            return this;
        }
        

        public MTexture AddAnimations(IList<(string name, float duration, int frameStart, int frameEnd)> animations)
        {
            foreach(var (name, duration, frameStart, frameEnd) in animations)
            {
                AddAnimation(name, duration, frameStart, frameEnd);
            }

            return this;
        }

        public AnimationPlayer PlayAnimation(string name = "Default")
        {
            return new(MGame, this, Animations[name]);
        }

        public virtual void Draw(SpriteBatch spriteBatch, Camera camera, Rectangle sourceRange, Transform transform)
        {
            var (screenPos, screenDirection, scale) = camera.Transform(transform).ToTuple;
#if DEBUG
            if (Name == "BouncingBomb")
            {
                //int a = 1;
            }
#endif
            spriteBatch.Draw(RawTexture,
                (MVec2)screenPos,
                sourceRange,
                Color.White,
                screenDirection.Radian,
                sourceRange.Size.ToVector2() / 2,
                scale.Abs,
                transform.SpriteEffects,
                0);
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
