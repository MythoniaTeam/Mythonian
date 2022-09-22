



namespace Mythonia.Game.Sprites
{
    public class Player : EntityGravitate, IHealth
    {



        #region Prop


        public HealthInfo Health { get; init; } = new(100, 0);


        #region Prop - Physics



        //public float JumpKeyPressTime = -1;
        /// <summary>判断玩家是否松开跳跃键 (从起跳开始)</summary>
        public bool JumpKeyReleased { get; private set; } = true;

        public float JumpKeyPressAcc { get; protected set; } = 0.25f;
        public int LeaveGroundJumpTime { get; protected set;  } = 8;
        public int LeaveGroundTimeCount { get; protected set; } = -1;
        public bool ApplyAutoJumpAcc { get; protected set; } = false;


        #endregion Prop - Physics

        #region Prop - Debug


        #endregion Prop - Debug


        #endregion



        #region Constructor

        public Player(MGame game, Map map) 
            : base("Player", EntityType.Player, game, map, game.TextureManager["TestPlayer"]/*game.TextureManager["BouncingBomb"].PlayAnimation()*/, new((0,40, 0)))
        {
            JumpsCountMax = 4;
            MaxWalkSpd = new(4);
            WalkAcc = 0.55f;
            JumpVel = 6.6f;
            JumpKeyPressAcc = 0.37f;

            //Scale = (2, 2);
            RectHitbox = new(MGame, () => (MVec2)Position, Texture.Size, IHitbox.Types.Entity);

#if DEBUG
            TextManager.Ins.WriteLine(() => $"Player Vel.X: {MathF.Round(_velocity.X, 2)}");
            TextManager.Ins.WriteLine(() => $"Player Vel.Y: {MathF.Round(_velocity.Y, 2)}");
            TextManager.Ins.WriteLine(() => $"Jumps Count: {JumpsCount}");
            TextManager.Ins.WriteLine(() => $"Health: {Health.HealthPoint}");

            Health.HealthPoint = 80;
            Health.Defence = 8;

            //game.Components.Add(new Panel(MGame, Map, new((400, 280, 0), scale: (300, 200))));
#endif

        }

        #endregion



        #region Methods

#if DEBUG
        private bool _f11Down = false;
        private bool _f10Down = false;
        private void DebugUpdate(GameTime gameTime)
        {
            KeyboardState key = Keyboard.GetState();
            if (key.IsKeyDown(Keys.F10))
            {
                if (!_f10Down)
                {
                    MDebug.DrawTilesHitbox = !MDebug.DrawTilesHitbox;
                    _f10Down = true;
                }
            }
            else
            {
                _f10Down = false;
            }

            if (key.IsKeyDown(Keys.F11))
            {
                if (!_f11Down)
                {
                    MDebug.DrawEntitiesHitbox = !MDebug.DrawEntitiesHitbox;
                    _f11Down = true;
                }
            }
            else
            {
                _f11Down = false;
            }


            if (key.IsKeyDown(Keys.R))
            {
                Position = (0, 40, 0);
                Velocity = (0, 0);
            }

            if (Input[KeyName.Debug] > 0)
            {
                _xVel = Input[KeyName.Debug] / 10f;
                var prototype = new BouncingBomb(MGame, Map, (MVec2)Position + (0, 200), _xVel * MathF.Sign(Transform.Scale.X), true);
                int collCount = 0;
                for(int i = 0; i < 120; i++)
                {
                    if (prototype.PrototypeUpdate(gameTime)) collCount++;
                    if (collCount == 4 && i < 100) i = 100;
                }
            }
            else if (Input[KeyName.Debug] == -1)
            {
                MGame.Components.Add(new BouncingBomb(MGame, Map, (MVec2)Position + (0, 200), _xVel * MathF.Sign(Transform.Scale.X)));
                //MGame.Components.Add(_aimingLine = new AimingLineVertical(MGame, Map));
            }
            if (Input[KeyName.Down] == 1)
            {
                //_aimingLine.Activate();
            }

        }
        float _xVel;
        //AimingLineVertical _aimingLine;
#endif


        public override void Update(GameTime gameTime)
        {
            var posRecord = (MVec2)Position;
#if DEBUG
            DebugUpdate(gameTime);
#endif
            JumpStatus = Input.KeyDown(KeyName.Jump);

            //检测 [左右键] 设置 WalkKeyStatus
            WalkStatus = (Input.KeyDown(KeyName.Left), Input.KeyDown(KeyName.Right)) switch
            {
                (true, true) or (false, false) => 0,
                (true, false) => -1, //按下左键
                (false, true) => 1, //按下右键
            };
            if (WalkStatus == -1 && MathF.Sign(_scale.X) == 1 ||
                WalkStatus == 1 && MathF.Sign(_scale.X) == -1) _scale.X *= -1;
            
            
            base.Update(gameTime);

            Pen.Ins.DrawLine((MVec2)Position, posRecord, 100);
        }


        protected override void Jump(bool jump)
        {
            if (jump)
            {
                int keyPressTime = Input[KeyName.Jump];
                if (keyPressTime <= 12)
                {
                    //如果按下跳跃键
                    if (JumpKeyReleased && JumpsCount < JumpsCountMax)
                    {
                        //将速度设为 JumpInitSpd，按键时间设为 0 表示开始按键 (平时是 -1)
                        _velocity.Y = JumpVel;
                        JumpKeyReleased = false;
                        JumpsCount++;
                    }
                    else if (JumpKeyReleased)
                    {
                        var posTemp = _position;
                        _position += _velocity.SetNew(y: -1.2f * RectHitbox.Size.Y);

                        //如果再次按下按键且前下方有碰撞体，开始加速下坠
                        if (!ApplyAutoJumpAcc && HitUtility.GetHitTiles(MGame.Main, RectHitbox) != null)
                        {
                            ApplyAutoJumpAcc = true;
                            TextManager.Ins.WriteLine(() => "ApplyAutoJumpAcc", 100);
                        }
                        _position = posTemp;
                    }

                    if (ApplyAutoJumpAcc)
                    {
                        if (keyPressTime <= 6 && !OnGround && _velocity.Y < 0)
                            _velocity.Y -= 1f;
                        else ApplyAutoJumpAcc = false;
                    }

                }
            }
            else
            {
                //如果松开跳跃键, 设为假
                JumpKeyReleased = true;
            }
        }

        protected override void ApplyGravity(GameTime gameTime, float gravity)
        {
            if (!JumpKeyReleased && _velocity.Y > 0)
            {
                //如果按下跳跃键, 且 Y 速度 > 0
                //增加抵消后的重力
                _velocity.Y += (Gravity + JumpKeyPressAcc) * gameTime.CFDuration();
            }
            else
            {
                base.ApplyGravity(gameTime, gravity);
            }
        }

        protected override void OnGroundAction(bool onGround, bool wasOnGround)
        {
            if (onGround)
            {
                //如果在地面，重置计时器 (设为-1不启用)
                LeaveGroundTimeCount = -1;
                JumpsCount = 0;
            }
            else if (wasOnGround)
            {
                //如果之前在地面上，移动后不在，开始计算
                LeaveGroundTimeCount = 0; //开始计算离开地面的时长 (平时是 -1)
            }

            if (LeaveGroundTimeCount >= 0)
            {
                //如果开始计时，把计时器加一
                LeaveGroundTimeCount += 1;
                if (JumpsCount == 0 && LeaveGroundTimeCount > LeaveGroundJumpTime)
                {
                    JumpsCount++;
                    LeaveGroundTimeCount = -1;
                }
            }
        }

        protected override (bool Coll, bool EndMethod) HitAction(MVec2 vel, IList<RectangleHitbox> hitboxes)
        {
            //如果向上移动
            if (vel.Y > 0)
            {
                var posTemp2 = _position;
                int range = 4;
                int i = 1;

                //尝试 X 移动 1 ~ 4, -1 ~ -4，是否能躲避碰撞
                
                while (true)
                {
                    //移动
                    _position = posTemp2.ChangeNew(x: i);
                    //如果不发生碰撞，返回，结束后面碰撞检测的部分
                    if (!hitboxes.IsHit(RectHitbox))
                        return (false, true);

                    i += MathF.Sign(i);
                    if (i == range + 1) i = -1;
                    if (i == -range - 1) break;
                }
                //如果仍然碰撞，恢复原来的位置
                _position = posTemp2;
            }
            //自动上一格平台
            else return base.HitAction(vel, hitboxes);
            return (true, false);
            
        }



        
        public override void Draw(GameTime gameTime)
        {
            //var (scrPos, _, _) = MGame.Main.Camera.Transform(Position);
            //MGame.SpriteBatch.Draw(Texture, scrPos, null, Color.White, 0, Texture.Size / 2, 1, SpriteEffects.None, 0);
            //DrawManager.Ins.Draw(this);
            base.Draw(gameTime);

#if DEBUG
            if (MDebug.DrawEntitiesHitbox)
            {
                RectHitbox.DrawHitbox(new(150, 255, 160, 150));
                //HitboxFoot.DrawHitbox(new(50,0, 0, 100));
            }
#endif
        }

        #endregion

    }
}
