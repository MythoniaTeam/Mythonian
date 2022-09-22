



namespace Mythonia.Game.Sprites.Projectiles
{
    

        

    public class BouncingBomb : EntityGravitate, IDamage
    {
        public DamageInfo Damage { get; set; }


        public BouncingBomb(MGame game, Map map, MVec2 position, float xVel, bool prototype = false) 
            : base("BouncingBomb", EntityType.HostileProjectile, game, map, MTextureManager.Ins["BouncingBomb"].PlayAnimation(), new(position), !prototype)
        {
            MaxFallingSpd = 18f;
            MaxWalkSpd = new(15);
            WalkAcc = 0;
            _velocity.X = MaxWalkSpd.Limit(xVel);//MaxWalkSpd;
            _velocity.Y = -10;
            RectHitbox = new(MGame, () => (MVec2)Position, Texture.Size, IHitbox.Types.Entity);

            Damage = new(10);
        }

        public bool Coll;

        protected override bool MoveHitAndGroundCheck(GameTime gameTime)
        {
            OnGround = false;
            Coll = false;

            //分解 X Y 速度移动
            MVec2 velRecord = _velocity;
            var (hitY, hitboxesY) = SpdDecompoMove(gameTime, _velocity * (0, 1), velRecord);
            if (hitY)
            {
                //如果向下移动且碰撞，表示在地面上
                if (velRecord.Y < 0)
                {
                    OnGround = true;
                    foreach (var hitboxY in hitboxesY)
                    {
                        if (hitboxY is RectangleHitboxEvented hitboxEvented)
                        {
                            hitboxEvented.SteppedBy(this);
                        }
                    }
                }
                _velocity.Y *= -0.87f;
                _velocity.X *= 0.95f;
                Coll = true;

            }
            var (hitX, _) = SpdDecompoMove(gameTime, _velocity * (1, 0), velRecord);
            if (hitX)
            {
                _velocity.X *= -0.95f;
                _scale.X *= -1;
                Coll = true;
            }
            return OnGround;
        }



        //float MaxSpeed => new MVec2(MaxWalkSpd, MaxFallingSpd).LengthSquared();
        public override void Update(GameTime gameTime)
        {
            var posRecord = (MVec2)Position;

            ((AnimationPlayer)Texture).PlaySpeed = MathF.Abs(Velocity.X) / MaxWalkSpd * 5;//Velocity.LengthSquared() * 5 / MaxSpeed; 
            //WalkStatus = MathF.Sign(Velocity.X);
            base.Update(gameTime);


            Pen.Ins.DrawLine((MVec2)Position, posRecord, 100);

            Damage.KnokBack = _velocity;
            this.GetHitEntities();
        }

        public bool PrototypeUpdate(GameTime gameTime)
        {
            var posRecord = (MVec2)Position;
            base.Update(gameTime);
            Pen.Ins.DrawLine((MVec2)Position, posRecord, 1);
            return Coll;

        }

    }
}
