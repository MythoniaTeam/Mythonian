﻿



namespace Mythonia.Game.Sprites
{
    public class Player : Entity
    {



        #region Prop

        public Input Input => MGame.Main.Input;




        #region Prop - Physics


        public RectangleHitbox Hitbox { get; private set; }

        public bool OnGround { get; private set; }
        public IList<RectangleHitbox> OnHitbox { get; private set; }

        //public float JumpKeyPressTime = -1;
        public sbyte WalkKeyStatus = 0;
        /// <summary>判断玩家是否松开跳跃键 (从起跳开始)</summary>
        public bool JumpKeyReleased = true;


        private const float WalkAcc = 0.3f;
        private MLimit MaxWalkSpd = new(3);

        private const float Gravity = -0.5f;
        private const float JumpKeyPressAcc = 0.25f;
        private const float JumpInitSpd = 7f;
        private const float MaxFallingSpd = 8;

        private const int JumpsCountMax = 1;
        private int JumpsCount = 0;
        private const int LeaveGroundJumpTime = 8;
        private int LeaveGroundTimeCount = -1;
        private bool ApplyAutoJumpAcc = false;


        #endregion Prop - Physics

        #region Prop - Debug

#if DEBUG
        private float CFDuration = 0;
#endif

        #endregion Prop - Debug


        #endregion



        #region Constructor

        public Player(MGame game, Map map) : base("Player", game, map, game.TextureManager["BouncingBomb"].PlayAnimation(), (0,40))
        {
            Scale = (2, 2);
            Hitbox = new(MGame, () => (MVec2)Position, Texture.Size * Scale);

            TextManager.Ins.WriteLine(() => $"Player Vel.X: {MathF.Round(_velocity.X, 2)}");
            TextManager.Ins.WriteLine(() => $"Player Vel.Y: {MathF.Round(_velocity.Y, 2)}");
            TextManager.Ins.WriteLine(() => $"CF Duration: {MathF.Round(CFDuration, 2)}");
        }

        #endregion



        #region Methods

#if DEBUG
        private bool _f11Down = false;
        private bool _f10Down = false;
        private void DebugUpdate()
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
        }
#endif

               

        public override void Update(GameTime gameTime)
        {
            CFDuration = gameTime.CFDuration();

            base.Update(gameTime);

#if DEBUG
            DebugUpdate();
#endif
            

            if (Input.KeyDown(KeyName.Jump))
            {
                int keyPressTime = Input[KeyName.Jump];
                if (keyPressTime <= 12)
                {
                    //如果按下跳跃键
                    if(JumpsCount < JumpsCountMax)
                    {
                        //将速度设为 JumpInitSpd，按键时间设为 0 表示开始按键 (平时是 -1)
                        _velocity.Y = JumpInitSpd;
                        JumpKeyReleased = false;
                        JumpsCount++;
                    }
                    else if (JumpKeyReleased)
                    {
                        var posTemp = _position;
                        _position += _velocity.SetNew(y: -1.2f * Hitbox.Size.Y);
                        
                        //如果再次按下按键且前下方有碰撞体，开始加速下坠
                        if(!ApplyAutoJumpAcc && HitUtility.GetHitTile(Map, Hitbox) != null)
                        {
                            ApplyAutoJumpAcc = true;
                            TextManager.Ins.WriteLine(() => "ApplyAutoJumpAcc", 100);
                        }
                        _position = posTemp;
                    }
                    
                    if (ApplyAutoJumpAcc)
                    {
                        if(keyPressTime <= 6 && !OnGround && _velocity.Y < 0)
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

            if (!JumpKeyReleased && _velocity.Y > 0)
            {
                //如果按下跳跃键, 且 Y 速度 > 0
                //增加抵消后的重力
                _velocity.Y += (Gravity + JumpKeyPressAcc) * gameTime.CFDuration();
            }
            else if (_velocity.Y >= -MaxFallingSpd)
            {
                //如果松开跳跃键 或 Y 速度 <= 0 且未达到最大坠落速度
                //增加重力
                _velocity.Y += Gravity * gameTime.CFDuration();
                //如果超出最大坠落速度，限制到范围内
                if (_velocity.Y < -MaxFallingSpd) _velocity.Y = -MaxFallingSpd;
            }



            //检测 [左右键] 设置 WalkKeyStatus
            WalkKeyStatus = (Input.KeyDown(KeyName.Left), Input.KeyDown(KeyName.Right)) switch
            {
                (true, true) or (false, false) => 0,
                (true, false) => -1, //按下左键
                (false, true) => 1, //按下右键
            };

            if (!_velocity.X.IsInLimit(MaxWalkSpd) || WalkKeyStatus == 0)
            {
                //如果速度超出范围，或松开移动键
                //如果 X 速度 < 减速度，直接归零
                if (MathF.Abs(_velocity.X) <= WalkAcc * gameTime.CFDuration())
                {
                    _velocity.X = 0;
                }
                else
                {
                    //减速
                    _velocity.X -= MathF.Sign(_velocity.X) * WalkAcc * gameTime.CFDuration();
                }
            }
            else if (WalkKeyStatus != 0)
            {
                //如果速度没有超出范围，且按下移动键 (Status != 0)
                //加速度
                _velocity.X += WalkKeyStatus * WalkAcc * gameTime.CFDuration();
                //如果加速度后超出范围，限制到范围内
                _velocity.X = MaxWalkSpd.Limit(_velocity.X);
            }



            bool wasOnGround = OnGround;
            OnGround = false;

            //分解 X Y 速度移动
            MVec2 velRecord = _velocity;
            if (Move(gameTime, _velocity * (0, 1), velRecord))
            {
                //如果向下移动且碰撞，表示在地面上
                if (_velocity.Y < 0) OnGround = true;
                _velocity.Y = 0;
            }
            if (Move(gameTime, _velocity * (1, 0), velRecord))
            {
                _velocity.X = 0;
            }
            //_velocity.X *= ResisX;



            if (OnGround)
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
                if (LeaveGroundTimeCount > LeaveGroundJumpTime) JumpsCount++;
            }

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
        private bool Move(GameTime gameTime, MVec2 vel, MVec2 velRecord)
        {
            bool coll;
            //移动
            vel *= gameTime.CFDuration();
            _position += vel;

            //检查碰撞
            var hitboxes = HitUtility.GetHitTile(Map, Hitbox);


            //如果不存在碰撞，返回
            if (hitboxes is null) return false;

            coll = true;

            //if (actionColl is not null) actionColl();


            if (vel.Y > 0)
            {
                var posTemp2 = _position;
                int range = 4;
                int i = 1;
                while (true)
                {
                    _position = posTemp2.ChangeNew(x: i);
                    //如果不发生碰撞，返回
                    if (!hitboxes.IsHit(Hitbox)) 
                        return false;

                    i += MathF.Sign(i);
                    if (i == range + 1) i = -1;
                    if (i == -range - 1) break;
                }
                _position = posTemp2;
            }
            else if (vel.X != 0)
            {
                var posTemp2 = _position;
                /*if(velRecord.Y < 0)
                {
                    int range = 4;
                    int i = 1;
                    while (true)
                    {
                        _position = posTemp2.ChangeNew(y: i);
                        //如果不发生碰撞，返回
                        if (!hitboxes.IsHit(Hitbox))
                        {
                            if (HitUtility.GetHitTile(Map, Hitbox) is null)
                                return false;
                        }

                        i++;
                        if (i == range + 1) break;

                    }
                }*/

                //自动上一格平台
                if (WalkKeyStatus != 0 && OnGround)
                {
                    _position = posTemp2.ChangeNew(y: 17);
                    if (HitUtility.GetHitTile(Map, Hitbox) is null)
                    {
                        //如果向上移动一格后不会碰撞，那么将施加一个朝 Y 正方向的力
                        coll = false;
                        _velocity.Y += 4.5f;
                    }
                    
                }

                        _position = posTemp2;
            }
            

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

            

            return coll;
        }

        /// <summary>
        /// 遍历 <paramref name="hitboxes"/> 内的所有 <see cref="RectangleHitbox"/>,<br/>
        /// 检测当前坐标 (<see cref="DichotomyMoveX(float, RectangleHitbox)"/> 后), 是否与其他碰撞体碰撞
        /// </summary>
        /// <param name="hitboxes"></param>
        /// <param name="posTemp"></param>
        /// <param name="vel"></param>
        private void CheckMove(IList<RectangleHitbox> hitboxes, MVec3 posTemp, MVec2 vel)
        {
            List<RectangleHitbox> removeList = new();

            //遍历所有碰撞体
            foreach (var hitbox in hitboxes)
            {
                //如果仍然碰撞
                if (HitUtility.IsHit(Hitbox, hitbox))
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
        private void DichotomyMove(MVec2 vel, RectangleHitbox hitbox)
        {
            if (MathF.Abs(vel.LengthSquared()) < 0.01f)
            {
                //如果 | vel | < 0.1, 退后 整个vel, 结束递归
                if (HitUtility.IsHit(hitbox, Hitbox))
                    _position -= vel;
                return;
            }

            vel /= 2.0f;
            if (HitUtility.IsHit(hitbox, Hitbox))
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
            //var (scrPos, _, _) = MGame.Main.Camera.Transform(Position);
            //MGame.SpriteBatch.Draw(Texture, scrPos, null, Color.White, 0, Texture.Size / 2, 1, SpriteEffects.None, 0);
            DrawManager.Ins.Draw(this);

#if DEBUG
            //if (MDebug.DrawEntitiesHitbox)
            //{
            //    Hitbox.DrawHitbox(new(150,255,160,150));
            //    //HitboxFoot.DrawHitbox(new(50,0, 0, 100));
            //}
#endif
        }

        #endregion

    }
}
