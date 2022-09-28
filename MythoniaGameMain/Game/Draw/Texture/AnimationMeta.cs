



namespace Mythonia.Game.Draw.Texture
{
    /// <summary>
    /// 动画对象的元数据，不应被修改。<br/>
    /// 不包含「当前播放的帧」「播放速度」等动画播放数据。
    /// <para>应该实例化一个 <see cref="AnimationPlayer"/> 以管理动画的播放。</para>
    /// </summary>
    public class AnimationMeta : INamed
    {
        public string Name { get; init; }

        /// <summary>
        /// 每帧持续的时间
        /// </summary>
        public float Duration { get; init; }

        /// <summary>
        /// 开始播放，和循环播放之间的间隔时间
        /// </summary>
        public float Delay { get; init; }

        /// <summary>
        /// 动画包含的帧数
        /// </summary>
        public int Length => FramesNo.Length;
        /// <summary>
        /// 每一帧在原贴图内的范围
        /// </summary>
        public int[] FramesNo { get; init; }
        public MTexture Texture { get; init; }


        public Rectangle this[int index] => Texture.Frames[FramesNo[index]].Range;

        public AnimationMeta(MTexture texture, string name, float duration, float delay, int[] frames)
        {
            Name = name;
            Texture = texture;
            Delay = delay;
            Duration = duration;
            FramesNo = frames;
        }


    }
}
