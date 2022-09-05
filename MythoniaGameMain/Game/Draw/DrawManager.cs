



namespace Mythonia.Game.Draw
{
    public class DrawManager
    {
        public static DrawManager Ins { get; private set; }


        public MGame MGame { get; init; }

        public DrawManager(MGame game)
        {
            Ins ??= this;
            MGame = game;
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


        public void Draw(SpriteBatch spriteBatch, Camera camera, MTexture texture, Rectangle sourceRange, Transform transform)
        {
            var (screenPos, screenDirection, scale, _, _) = camera.Transform(transform).ToTuple;
            
            spriteBatch.Draw(texture, 
                (MVec2)screenPos, 
                sourceRange, 
                Color.White, 
                screenDirection, 
                texture.Size / 2, 
                scale, 
                transform.SpriteEffects,
                0);
        }
        public void Draw(Sprite sprite, SpriteBatch spriteBatch = null, Camera camera = null)
        {
            spriteBatch ??= MGame.SpriteBatch;
            camera ??= MGame.Main.Camera;
            Draw(spriteBatch, camera, sprite.Texture, sprite.TextureSourceRange, sprite.Transform);
        }

    }

}
