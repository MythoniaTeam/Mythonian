



namespace Mythonia.Game.Sprites
{
    public abstract class Entity : Sprite
    {
        #region Prop

        public Map Map { get; init; }

        public MVec2 Velocity { get => _velocity; set => _velocity = value; }
        protected MVec2 _velocity = (0, 0);

        public abstract IHitbox Hitbox { get; }
        

        public EntityType Type { get; protected set; }


        #endregion


        #region 

        public Entity(string name, EntityType type, MGame game, Map map, ITexture texture, Transform? transform = null, bool addToList = true) 
            : base(name, game, texture, transform)
        {
            Map = map;

            Type = type;

            if(addToList) EntitiesManager.Ins.Add(this);

            if(this is IHealth iHealth)
            {
                iHealth.Health.OnDealDamage += Health_OnDealDamage;
                iHealth.Health.OnDeath += Health_OnDeath;
            }
        }

        private void Health_OnDealDamage(DamageInfo damage)
        {
            _velocity += damage.KnokBack;
        }

        private bool Health_OnDeath(DamageInfo damage)
        {
            _position = (0, 40, 0);
            ((IHealth)this).Health.HealthPoint = 80;
            return false;
        }
        #endregion


        public override string ToString()
        {
            return Name;
        }
    }
}
