



namespace Mythonia.Game.Sprites.Projectiles
{
    public class BouncingBomb : EntityGravitate
    {
        public BouncingBomb(MGame game, Map map, MVec2 pos) : base("BouncingBomb", game,map, MTextureManager.Ins["BouncingBomb"].PlayAnimation(), pos)
        {
            MaxFallingSpd = 18f;
            MaxWalkSpd = new(8  );
            WalkAcc = 0;
            _velocity.X = MaxWalkSpd;
            _velocity.Y = -10;
            Hitbox = new(MGame, () => (MVec2)Position, Texture.Size, IHitbox.Types.Entity);
        }

        protected override bool MoveHitAndGroundCheck(GameTime gameTime)
        {
            OnGround = false;

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
                _velocity.Y *= -0.95f;
                _velocity.X *= 0.85f;
            }
            var (hitX, _) = SpdDecompoMove(gameTime, _velocity * (1, 0), velRecord);
            if (hitX)
            {
                _velocity.X *= -0.95f;
                _scale.X *= -1;
            }
            return OnGround;
        }

        //float MaxSpeed => new MVec2(MaxWalkSpd, MaxFallingSpd).LengthSquared();
        public override void Update(GameTime gameTime)
        {
            ((AnimationPlayer)Texture).PlaySpeed = MathF.Abs(Velocity.X) / MaxWalkSpd * 5;//Velocity.LengthSquared() * 5 / MaxSpeed; 
            WalkStatus = MathF.Sign(Velocity.X);
            base.Update(gameTime);
        }
    }
}
