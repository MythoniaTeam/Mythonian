



namespace Mythonia.Game.Sprites.UI
{
    public class UI : Sprite
    {
        public UI(string name, MGame game, Map map, ITexture texture, Transform transform = default) 
            : base(name, game, map, texture, transform)
        {

        }


        public override void Draw(GameTime gameTime)
        {
            //base.Draw(gameTime);
            DrawTexture(MGame.SpriteBatch);
        }
    }
}
