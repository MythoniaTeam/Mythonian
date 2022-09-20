



namespace Mythonia.Game.Sprites
{
    public class Entity : Sprite
    {
        #region Prop
        public MVec2 Velocity { get => _velocity; set => _velocity = value; }
        protected MVec2 _velocity = (0, 0);

        public IHitbox Hitbox { get; set; }

        #endregion


        #region 

        public Entity(string name, MGame game, Map map, ITexture texture, MVec2? position = null, bool addToList = true) : base(name, game, map, texture, position)
        {
            if(addToList) EntitiesManager.Ins.Add(this);
        }
        #endregion
    }
}
