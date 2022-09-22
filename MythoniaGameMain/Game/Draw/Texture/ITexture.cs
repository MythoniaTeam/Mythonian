



namespace Mythonia.Game.Draw.Texture
{
    /// <summary>
    /// Texture的接口, 能返回一个 <see cref="Texture2D"/> 和 <see cref="Rectangle"/> 绘制范围
    /// </summary>
    public interface ITexture
    {
        Texture2D RawTexture { get; }

        MVec2 Size { get; }

        Rectangle GetSourceRange();

        void Draw(SpriteBatch spriteBatch, Camera camera, Transform transform)
        {
            Draw(spriteBatch, camera.Transform(transform));
        }

        void Draw(SpriteBatch spriteBatch, Transform transform);
    }
}
