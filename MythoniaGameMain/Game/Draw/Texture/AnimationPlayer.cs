



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


        private float _timeCount = 0;
        private int CurrentFrameNo
        {
            get => _currentFrameNo;
            set
            {
                _currentFrameNo = CurrentFrameNo;
                if (_currentFrameNo >= CurrentAnimation.Length) _currentFrameNo = 0;
            }
        }
        private int _currentFrameNo = 0;

        public float PlaySpeed { get; set; } = 1;



        public AnimationPlayer(MGame game, MTexture texture)
        {
            Texture = texture;
            game.DrawManager.AddAnimationPlayer(this);
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
            _timeCount += gameTime.CFDuration();
            if(_timeCount >= CurrentAnimation.Duration)
            {
                CurrentFrameNo += (int)(_timeCount / CurrentAnimation.Duration);
                _timeCount %= CurrentAnimation.Duration;
            }
        }



        #region Implement - ITexture

        public Texture2D RawTexture => Texture.RawTexture;

        public Rectangle GetSourceRange() => CurrentAnimation[CurrentFrameNo];

        #endregion
    }
}
