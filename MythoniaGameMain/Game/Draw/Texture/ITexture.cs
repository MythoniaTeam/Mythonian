



namespace Mythonia.Game.Draw.Texture
{
    /// <summary>
    /// Texture的接口, 能返回一个 <see cref="Texture2D"/> 和 <see cref="Rectangle"/> 绘制范围
    /// </summary>
    public interface ITexture
    {
        public Texture2D RawTexture { get; }

        public MVec2 Size { get; }

        
        public MVec2 OriginRatio { get; set; }


        protected Rectangle GetSourceRange();
        public (Rectangle SourceRange, MVec2 Origin) GetDrawInfo()
        {
            var sourceRange = GetSourceRange();
            var origin = (MVec2)sourceRange.Size.ToVector2() * (OriginRatio * (1, -1) + (1, 1)) / 2;
            return (sourceRange, origin);
        }



        /// <summary>
        /// <inheritdoc cref="Draw(ITexture, SpriteBatch, Camera, Transform)"/>
        /// </summary>
        /// <param name="transform"><see cref="Transform.Position"/> 是世界坐标系，实际绘制位置受 <see cref="Camera"/> 影响</param>
        public void Draw(SpriteBatch spriteBatch, Camera camera, Transform transform)
        {
            Draw(this, spriteBatch, camera, transform);
        }


        /// <summary>
        /// <inheritdoc cref="Draw(ITexture, SpriteBatch, Transform)"/>
        /// </summary>
        /// <param name="transform"><see cref="Transform.Position"/> 是基于以中心为原点的平面直角坐标系</param>
        public void Draw(SpriteBatch spriteBatch, Transform transform)
        {
            Draw(this, spriteBatch, transform);
        }


        /// <summary>
        /// 调用 <see cref="Camera.Transform(Transform)"/> 方法，将 世界坐标 透过 <paramref name="camera"/> 转换为 屏幕坐标系<br/>
        /// 然后调用 <see cref="DrawManager.DrawToScreen(ITexture, SpriteBatch, Transform)"/> 方法，绘制贴图
        /// <para>
        /// </para>
        /// </summary>
        /// <param name="transform"><see cref="Transform.Position"/> 是世界坐标系，实际绘制位置受 <see cref="Camera"/> 影响</param>
        public static void Draw(ITexture texture, SpriteBatch spriteBatch, Camera camera, Transform transform)
        {
            DrawManager.Ins.DrawToScreen(texture, spriteBatch, camera.Transform(transform));
        }


        /// <summary>
        /// 调用 <see cref="DrawManager.RectToScreenCoordinate(Transform)"/> 方法，将 中心为原点的平面直角坐标系，转换为 屏幕坐标系<br/>
        /// 然后调用 <see cref="DrawManager.DrawToScreen(ITexture, SpriteBatch, Transform)"/> 方法，绘制贴图
        /// </summary>
        /// <param name="transform"><see cref="Transform.Position"/> 是基于以中心为原点的平面直角坐标系</param>
        public static void Draw(ITexture texture, SpriteBatch spriteBatch, Transform transform)
        {
            DrawManager.Ins.DrawToScreen(texture, spriteBatch, DrawManager.Ins.RectToScreenCoordinate(transform));
        }
    }
}
