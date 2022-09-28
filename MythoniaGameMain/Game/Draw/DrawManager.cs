



namespace Mythonia.Game.Draw
{
    public class DrawManager : GameComponent
    {
        public static DrawManager Ins { get; private set; }


        public MGame MGame => (MGame)Game;

        public DrawManager(MGame game) : base(game)
        {
            Ins ??= this;
        }



        private readonly List<AnimationPlayer> _animationPlayers = new();
        public void AddAnimationPlayer(AnimationPlayer player) => _animationPlayers.Add(player);

        /// <summary>
        /// 增加 <see cref="AnimationPlayer"/> 的计时变量
        /// </summary>
        /// <param name="gameTime"></param>
        public void AnimationUpdate(GameTime gameTime)
        {
            foreach (var player in _animationPlayers)
            {
                player.Update(gameTime);
            }
        }

        public override void Update(GameTime gameTime)
        {
            AnimationUpdate(gameTime);
        }

        /// <summary>
        /// 将贴图绘制到屏幕上的静态方法<br/>
        /// 会调用 <see cref="ITexture.GetSourceRange"/> 方法，获取对应帧图范围<br/>
        /// 不会对 <paramref name="transform"/> 做任何转换，里面的数据应为屏幕坐标系
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="transform">绘制的位置，旋转，缩放等信息。不会做转换，是最终绘制的值</param>
        public virtual void DrawToScreen(ITexture texture, SpriteBatch spriteBatch, Transform transform)
        {
            var (screenPos, screenDirection, scale) = transform.ToTuple;

            var (sourceRange, origin) = texture.GetDrawInfo();

            spriteBatch.Draw(texture.RawTexture,
                (MVec2)screenPos,
                sourceRange,
                Color.White,
                screenDirection.Radian,
                origin,
                scale.Abs,
                transform.SpriteEffects,
                0);
        }

        public Transform RectToScreenCoordinate(Transform rectCoordinate)
        {
            rectCoordinate.Position = rectCoordinate.Position + MGame.GraphicsDevice.Size() / 2;
            rectCoordinate.Position *= (1, -1, 1);
            rectCoordinate.Position = rectCoordinate.Position.Change(y: MGame.GraphicsDevice.Size().Y);
            return rectCoordinate;
        }
    }

}
