#nullable enable



namespace Mythonia.Game.Draw.Texture
{
    public class MTexture: ITexture, INamed, ICloneable
    {
        public string Name { get; init; }

        public MGame MGame { get; init; }

        public Texture2D RawTexture { get; init; }

        /// <summary>
        /// 单帧图的尺寸 (没有帧图则是整图尺寸)
        /// </summary>
        public MVec2 Size { get; set; }
        public float Width => Size.X;
        public float Height => Size.Y;


        /// <summary>
        /// 帧图的名称和对应范围
        /// </summary>
        public NamedList<Frame> Frames { get; init; } = new();

        public NamedList<AnimationMeta> Animations { get; init; } = new();

        public Frame CurrentFrame { get; set; }
        public MVec2 OriginRatio { get; set; }


        #region Constructors

        protected MTexture(MGame game, Texture2D rawTexture, string name, MVec2 size, MVec2 originRatio, NamedList<Frame> frames, NamedList<AnimationMeta> animations)
        {
            Name = name;
            RawTexture = rawTexture;
            MGame = game;
            Size = size;
            Frames = frames;
            Animations = animations;
            OriginRatio = originRatio;
            CurrentFrame = Frames[0];
        }

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
            else
            {
                Frames.Add(new("Default", new(new(0, 0), Size)));
            }
            CurrentFrame = Frames[0];
        }

        #endregion



        #region Method - Initialization

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
            Frames.RemoveAll(_ => true);

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

            PlayFrame(0);
            return this;
        }        


        public MTexture AddAnimation(string name, float duration, float delay, int[] frames)
        {
            Animations.Add(new(this, name, duration, delay, frames));
            return this;
        }
        public MTexture AddAnimation(string name = "Default", float duration = 10, float delay = 0, int frameStart = 0, int? frameEnd = null)
        {
            int frameEnd2 = frameEnd ?? Frames.Count - 1;
            int[] frames = new int[frameEnd2 - frameStart + 1];
            for (int i = 0; i < frames.Length; i++)
            {
                frames[i] = i + frameStart;
            }
            AddAnimation(name, duration, delay, frames);
            return this;
        }
        public MTexture AddAnimations((string name, float duration, float delay, int[] frames)[] animations)
        {
            foreach(var (name, duration, delay, frames) in animations)
            {
                AddAnimation(name, duration, delay, frames);
            }

            return this;
        }
        public MTexture AddAnimations(IList<(string name, float duration, float delay, int frameStart, int frameEnd)> animations)
        {
            foreach(var (name, duration, delay, frameStart, frameEnd) in animations)
            {
                AddAnimation(name, duration, delay, frameStart, frameEnd);
            }

            return this;
        }

        #endregion



        public AnimationPlayer PlayAnimation(string name = "Default")
        {
            return new(MGame, this, Animations[name]);
        }

        public MTexture PlayFrame(string name)
        {
            CurrentFrame = Frames[name];
            return this;
        }
        public MTexture PlayFrame(int index)
        {
            CurrentFrame = Frames[index];
            return this;
        }




        public virtual void Draw(SpriteBatch spriteBatch, Camera camera, Transform transform)
        {
            ITexture.Draw(this, spriteBatch, camera, transform);
        }
        public virtual void Draw(SpriteBatch spriteBatch, Transform transform)
        {
            ITexture.Draw(this, spriteBatch, transform);
        }
        





        #region Implement - ITexture

        public virtual Rectangle GetSourceRange()
        {
            return CurrentFrame.Range;
        }

        #endregion

        public MTexture Clone()
        {
            return new MTexture(MGame, RawTexture, Name, Size, OriginRatio, Frames, Animations);
        }
        object ICloneable.Clone() => Clone();



        #region Operators - Type

        public static implicit operator Texture2D(MTexture texture) => texture.RawTexture;

        #endregion


    }
}
