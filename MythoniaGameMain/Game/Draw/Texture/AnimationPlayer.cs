



namespace Mythonia.Game.Draw.Texture
{
    /// <summary>
    /// 每个 <see cref="AnimationMeta"/> 和 <see cref="MTexture"/> 都是独立、无法修改的元数据，<br/>
    /// 但同一个 <see cref="AnimationMeta"/> 可以被数个 <see cref="AnimationPlayer"/> 引用。
    /// <para><see cref="AnimationPlayer"/> 是 <see cref="Sprite"/> 各自单独实例化的对象, 用于管理该对象动画贴图的播放 (当前播放的帧等)。</para>
    /// </summary>
    public class AnimationPlayer : ITexture
    {
        public AnimationMeta CurrentAnimation { get; set; }
        public MTexture Texture { get; init; }

        public MVec2 Size => Texture.Size;

        private float _timeCount;
        private float TimeCount
        {
            get => _timeCount;
            set
            {
                _timeCount = value;
                if (_timeCount >= CurrentAnimation.Duration)
                {
                    //如果播放到最后一帧
                    if (CurrentFrameNo + (int)(_timeCount / CurrentAnimation.Duration) >= CurrentAnimation.Length && !RepeatPlaying)
                    {
                        OnFinishPlaying(CurrentAnimation);
                        CurrentFrameNo = CurrentAnimation.Length - 1;
                        PlaySpeed = 0;
                    }
                    else
                    {
                        //增加帧数
                        CurrentFrameNo += (int)(_timeCount / CurrentAnimation.Duration);
                        _timeCount %= CurrentAnimation.Duration;
                    }
                }
            }
        }
        private int CurrentFrameNo
        {
            get => _currentFrameNo;
            set
            {
                _currentFrameNo = value % CurrentAnimation.Length;
            }
        }
        private int _currentFrameNo = 0;

        public float PlaySpeed { get; set; } = 1;

        public bool RepeatPlaying { get; set; } = true;

        public delegate void FinishPlaying(AnimationMeta current);
        public event FinishPlaying OnFinishPlaying;



        #region Constructors

        public AnimationPlayer(MGame game, MTexture texture, AnimationMeta animation, bool repeatPlaying = true)
        {
            Texture = texture;
            game.DrawManager.AddAnimationPlayer(this);
            CurrentAnimation = animation;
            RepeatPlaying = repeatPlaying;
        }

        #endregion



        #region Methods

        public void PlayAnimation(string name, bool repeatPlaying = true)
        {
            CurrentAnimation = Texture.Animations[name];
            CurrentFrameNo = 0;
            TimeCount = 0;
            PlaySpeed = 1;
            RepeatPlaying = repeatPlaying;
        }


        /// <summary>
        /// 每帧增加计时器
        /// <para>
        /// <b>调用:</b><br/>
        /// 在 <see cref="DrawManager.AnimationUpdate"/> 里
        /// </para>
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            TimeCount += gameTime.CFDuration() * PlaySpeed;
            
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera, Rectangle sourceRange, Transform transform)
        {
            var (screenPos, screenDirection, scale) = camera.Transform(transform).ToTuple;
            
            spriteBatch.Draw(RawTexture,
                (MVec2)screenPos,
                sourceRange,
                Color.White,
                screenDirection,
                Size / 2,
                scale.Abs,
                transform.SpriteEffects,
                0);
        }

        #endregion



        #region Implement - ITexture

        public Texture2D RawTexture => Texture.RawTexture;

        public Rectangle GetSourceRange() => CurrentAnimation[CurrentFrameNo];

        #endregion
    }
}
