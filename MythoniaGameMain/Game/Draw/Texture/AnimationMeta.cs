



namespace Mythonia.Game.Draw.Texture
{
    /// <summary>
    /// 动画对象的元数据，不应被修改。<br/>
    /// 不包含「当前播放的帧」「播放速度」等动画播放数据。
    /// <para>应该实例化一个 <see cref="AnimationPlayer"/> 以管理动画的播放。</para>
    /// </summary>
    public class AnimationMeta
    {
        public string Name { get; init; }

        /// <summary>
        /// 每帧持续的时间
        /// </summary>
        public float Duration { get; set; }

        /// <summary>
        /// 动画包含的帧数
        /// </summary>
        public int Length => Frames.Count;
        /// <summary>
        /// 每一帧在原贴图内的范围
        /// </summary>
        public IList<Rectangle> Frames { get; init; }
        public MTexture Texture { get; init; }


        public Rectangle this[int index] => Frames[index];

        public AnimationMeta(MTexture texture, string name, IList<Rectangle> frames)
        {
            Name = name;
            Texture = texture;
            Frames = frames;
        }


    }
}
