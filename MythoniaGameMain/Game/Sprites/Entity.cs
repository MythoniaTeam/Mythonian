



namespace Mythonia.Game.Sprites
{
    public class Entity : Sprite
    {
        #region Prop
        public MVec2 Velocity { get => _velocity; set => _velocity = value; }
        protected MVec2 _velocity = (0, 0);

        #endregion


        #region 

        public Entity(string name, MGame game, Map map, ITexture texture, MVec2? position = null) : base(name, game, map, texture, position)
        {
        }
        #endregion
    }
}
