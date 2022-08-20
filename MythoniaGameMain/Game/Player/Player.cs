



namespace Mythonia.Game.Player
{
    public class Player : DrawableGameComponent
    {



        #region Prop

        public Main MGame => (Main)Game;
        public HitManager HitManager => MGame.MainGame.HitManager;

        public MTexture Texture { get; private set; }


        #region Prop - Physics

        public MVec2 Position { get => _position; set => _position = value; }
        private MVec2 _position = (0, 16);
        public MVec2 Velocity { get => _velocity; set => _velocity = value; }
        private MVec2 _velocity = (0, 0);

        public RectHitbox HitboxFoot { get; private set; }
        public RectHitbox Hitbox { get; private set; }

        public bool OnGround { get; private set; }
        public IList<RectHitbox> OnHitbox { get; private set; }

        public float JumpKeyPressTime = -1;
        public bool WalkKeyPressed = false;


        private const float Acc = 0.4f;
        private const float Gravity = -0.4f;
        private const float JumpAcc = 0.7f;
        private const float JumpInitSpd = 8;
        private const float JumpAccTime = 15;
        private const float Resis = 0.89f;

        #endregion Prop - Physics

        #endregion



        #region Constructor

        public Player(Main game) : base(game)
        {
            Texture = game.TextureManager["TestPlayer"];
            Hitbox = new(MGame, () => Position, Texture.Size);
            HitboxFoot = new(MGame, () => 
                Position - (0, Texture.Height / 2), 
                (Texture.Width, 3)
            );
        }

        #endregion



        #region Methods

#if DEBUG
        private bool _f11Down = false;
        private bool _f10Down = false;
        private void DebugUpdate(KeyboardState key)
        {
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
        }
#endif

               

        public override void Update(GameTime gameTime)
        {
            

            if (HitManager.IsCollidedWithTile(HitboxFoot) is IList<RectHitbox> ground)
            {
                OnHitbox = ground;
                OnGround = true;
                if (_velocity.Y < 0) _velocity.Y = 0;
            }
            else
            {
                OnGround = false;
                _velocity.Y += Gravity;
            }


            KeyboardState key = Keyboard.GetState();

#if DEBUG
            DebugUpdate(key);
#endif

            if (key.IsKeyDown(Keys.R))
            {
                Position = (0, 20);
                Velocity = (0, 0);
            }


            if (key.IsKeyDown(Keys.W))
            {
                //如果在地面上, 将速度设为 JumpInitSpd, 按键时间设为 0 表示开始按键 (平时是 -1)
                if (OnGround)
                {
                    _velocity.Y = JumpInitSpd;
                    JumpKeyPressTime = 0;
                }

                //如果 按下跳跃键 (>=0), 且按键时间 < JumpAccTime
                //不在地面, 且正在向上移动 (y 速度 > 0)
                else if (JumpKeyPressTime is >= 0 and < JumpAccTime && _velocity.Y > 0)
                {
                    //如果本帧加速后，总加速时长超出上限，只增加剩余的部分
                    if (JumpKeyPressTime + gameTime.CFDuration() >= JumpAccTime)
                    {
                        _velocity.Y += JumpAcc * (JumpAccTime - JumpKeyPressTime);
                    }
                    //否则增加 JumpAcc
                    else
                    {
                        _velocity.Y += JumpAcc * gameTime.CFDuration();
                    }
                    //增加按键时间    
                    JumpKeyPressTime += gameTime.CFDuration();
                }
            }
            else
            {
                //如果不按下跳跃键, 将按键时间设为 -1
                if (JumpKeyPressTime >= 0) JumpKeyPressTime = -1;
            }

            WalkKeyPressed = false;
            if (key.IsKeyDown(Keys.A))
            {
                _velocity.X -= Acc * gameTime.CFDuration();
                WalkKeyPressed = true;
            }
            if (key.IsKeyDown(Keys.D))
            {
                _velocity.X += Acc * gameTime.CFDuration();
                WalkKeyPressed = true;
            }

            if (!WalkKeyPressed && MathF.Abs(_velocity.X) < 1)
            {
                _velocity.X = 0;
            }


            if (Move(gameTime, _velocity * (1, 0)))
            {
                _velocity.X = 0;
            }
            if(Move(gameTime, _velocity * (0, 1)))
            {
                _velocity.Y = 0;
            }
            _velocity *= Resis;
        }

        /// <summary>
        /// 移动, 如果碰撞, 移动到合适的位置
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="vel"></param>
        /// <returns><list type="table">
        /// <item><term><see langword="true"/></term> <description>发生碰撞</description></item>
        /// <item><term><see langword="false"/></term> <description>没有发生碰撞</description></item>
        /// </list></returns>
        private bool Move(GameTime gameTime, MVec2 vel)
        {
            bool coll = false;

            //移动
            vel *= gameTime.CFDuration();
            _position += vel;

            //检查碰撞
            var hitboxes = HitManager.IsCollidedWithTile(Hitbox);
            //如果存在碰撞
            if (hitboxes is not null)
            {
                coll = true;
                //后退一半 vel
                vel /= 2.0f;
                _position -= vel;

                //记录当前位置
                var posTemp = _position;

                //按照 hitboxes[0] 用二分法移动
                DichotomyMove(vel, hitboxes[0]);

                //移除 hitboxes[0]
                hitboxes.RemoveAt(0);

                //检测是否仍与其他碰撞体碰撞
                CheckMove(hitboxes, posTemp, vel);

            }

            return coll;
        }

        /// <summary>
        /// 遍历 <paramref name="hitboxes"/> 内的所有 <see cref="RectHitbox"/>,<br/>
        /// 检测当前坐标 (<see cref="DichotomyMoveX(float, RectHitbox)"/> 后), 是否与其他碰撞体碰撞
        /// </summary>
        /// <param name="hitboxes"></param>
        /// <param name="posTemp"></param>
        /// <param name="vel"></param>
        private void CheckMove(IList<RectHitbox> hitboxes, MVec2 posTemp, MVec2 vel)
        {
            List<RectHitbox> removeList = new();

            //遍历所有碰撞体
            foreach (var hitbox in hitboxes)
            {
                //如果仍然碰撞
                if (HitManager.IsCollided(Hitbox, hitbox))
                {
                    //位置重置
                    _position = posTemp;
                    //重新按照当前 Hitbox 以二分法移动
                    DichotomyMove(vel, hitbox);
                    removeList.Add(hitbox);
                    //跳出循环
                    break;
                }

                //如果不碰撞，把该碰撞体加入移除列表
                removeList.Add(hitbox);
            }

            foreach (var remove in removeList)
                hitboxes.Remove(remove); //移除需要移除的对象

            //再次检测
            if(hitboxes.Count > 0) CheckMove(hitboxes, posTemp, vel);
        }

        /// <summary>
        /// 用二分法, 更改X坐标直到不碰撞
        /// </summary>
        /// <param name="vel"></param>
        /// <param name="hitbox"></param>
        private void DichotomyMove(MVec2 vel, RectHitbox hitbox)
        {
            if (MathF.Abs(vel.LengthSquared()) < 0.01f)
            {
                //如果 | vel | < 0.1, 退后 整个vel, 结束递归
                if (HitManager.IsCollided(hitbox, Hitbox))
                    _position -= vel;
                return;
            }

            vel /= 2.0f;
            if (HitManager.IsCollided(hitbox, Hitbox))
            {
                //如果碰到了, 退后 一半vel
                _position -= vel;
            }
            else
            {
                //如果没碰到, 前进 一半vel
                _position += vel;
            }
            //递归调用 MoveX, vel为原先的一半
            DichotomyMove(vel, hitbox);
        }





        
        public override void Draw(GameTime gameTime)
        {
            var (scrPos, _, _) = MGame.MainGame.Camera.Transform(Position);
            MGame.SpriteBatch.Draw(Texture, scrPos, null, Color.White, 0, Texture.Size / 2, 1, SpriteEffects.None, 0);

#if DEBUG
            if (MDebug.DrawEntitiesHitbox)
            {
                Hitbox.DrawHitbox(new(150,255,160,150));
                HitboxFoot.DrawHitbox(new(50,0, 0, 100));
            }
#endif
        }

        #endregion

    }
}
