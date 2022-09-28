



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


        private bool _delaying = true;
        private float _timeCount;

#pragma warning disable IDE0052 // Remove unread private members
        private float TimeCount
        {
            get => _timeCount;
            set
            {
                _timeCount = value;
                if (_delaying)
                {
                    //如果达到时间，结束延迟阶段
                    if(_timeCount >= CurrentAnimation.Delay)
                    {
                        _delaying = false;
                        _timeCount %= CurrentAnimation.Delay;
                    }
                }
                //如果不是在延迟阶段，时间达到下一帧
                if (!_delaying && _timeCount >= CurrentAnimation.Duration)
                {
                    //如果播放到最后一帧，且不循环播放
                    if (CurrentFrameNo + (int)(_timeCount / CurrentAnimation.Duration) >= CurrentAnimation.Length && !RepeatPlaying)
                    {
                        //调用 event
                        OnFinishPlaying(CurrentAnimation);
                        //切换到最后一帧，停止播放
                        CurrentFrameNo = CurrentAnimation.Length - 1;
                        PlaySpeed = 0;
                    }
                    else
                    {
                        //增加帧数，_timeCount 取余
                        CurrentFrameNo += (int)(_timeCount / CurrentAnimation.Duration);
                        _timeCount %= CurrentAnimation.Duration;
                    }
                }
            }
        }
#pragma warning restore IDE0052 // Remove unread private members

        private int CurrentFrameNo
        {
            get => _currentFrameNo;
            set
            {
                if (!_delaying && CurrentAnimation.Delay > 0 && value >= CurrentAnimation.Length)
                {
                    _timeCount = 0;
                    _delaying = true;
                    _currentFrameNo = 0;
                }
                else _currentFrameNo = value % CurrentAnimation.Length;
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


        #endregion



        #region Implement - ITexture

        public Texture2D RawTexture => Texture.RawTexture;

        public MVec2 OriginRatio { get => Texture.OriginRatio; set => Texture.OriginRatio = value; }

        public Rectangle GetSourceRange() => CurrentAnimation[CurrentFrameNo];

        #endregion
    }
}
