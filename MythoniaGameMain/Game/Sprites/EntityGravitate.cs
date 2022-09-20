



namespace Mythonia.Game.Sprites
{
    /// <summary>
    /// 受重力影响的实体
    /// </summary>
    public class EntityGravitate : Entity
    {

        #region Prop

        public Input Input => MGame.Main.Input;




        #region Prop - Physics


        public new RectangleHitbox Hitbox { get; protected set; }

        public bool OnGround { get; protected set; }
        public IList<RectangleHitbox> OnHitbox { get; protected set; }

        public bool JumpStatus { get; protected set; } = false;
        public int WalkStatus { get; protected set; } = 0;

        /// <summary>
        /// 横向移动的加速度
        /// </summary>
        public float WalkAcc { get; protected set; } = 0.3f;
        /// <summary>
        /// 最大横向移动速度
        /// </summary>
        public MLimit MaxWalkSpd { get; protected set; } = new(3);

        /// <summary>
        /// 重力加速度 (负数)
        /// </summary>
        public float Gravity { get; protected set;  } = -0.6f;
        /// <summary>
        /// 跳跃速度
        /// </summary>
        public float JumpVel { get; protected set; } = 7.5f;
        public float AutoStairClimbSpd { get; protected set; } = 5.5f;
        /// <summary>
        /// 最大坠落速度
        /// </summary>
        public float MaxFallingSpd { get; protected set; } = 9.5f;

        /// <summary>
        /// 跳跃次数限制
        /// </summary>
        public int JumpsCountMax { get; protected set; } = 1;
        /// <summary>
        /// 当前跳跃次数
        /// </summary>
        public int JumpsCount { get; protected set; } = 0;


        #endregion Prop - Physics

        #endregion



        public EntityGravitate(string name, MGame game, Map map, ITexture texture, MVec2? position = null, bool addToList = true) 
            : base(name, game, map, texture, position, addToList)
        {
        }


        #region Methods

        #region Methods - Y Movement

        /// <summary>
        /// 如果实体跳跃 (<paramref name="jump"/> == <see langword="true"/>), <br/>
        /// VelY 设为 <see cref="JumpInitSpd"/> 并减少跳跃次数
        /// </summary>
        /// <param name="jump"></param>
        protected virtual void Jump(bool jump)
        {
            if (jump)
            {
                //如果按下跳跃键
                if (JumpsCount < JumpsCountMax)
                {
                    //将速度设为 JumpInitSpd，按键时间设为 0 表示开始按键 (平时是 -1)
                    if(_velocity.Y < JumpVel) _velocity.Y = JumpVel;
                    JumpsCount++;
                }

            }

        }

        /// <summary>
        /// 施加重力的函数
        /// <para>
        /// <b><see langword="base"/>:</b><br/>
        /// 如果没有达到最大坠落速度 (VelY >= -<see cref="MaxFallingSpd"/>), VelY 增加重力 (<paramref name="gravity"/> 应为负数)
        /// </para>
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="gravity">重力加速度，负数为向下，正数向上</param>
        protected virtual void ApplyGravity(GameTime gameTime, float gravity)
        {

            if (_velocity.Y >= -MaxFallingSpd)
            {
                //增加重力
                _velocity.Y += gravity * gameTime.CFDuration();
                //如果超出最大坠落速度，限制到范围内
                if (_velocity.Y < -MaxFallingSpd) _velocity.Y = -MaxFallingSpd;
            }
        }

        #endregion


        #region Methods - X Movement

        /// <summary>
        /// 移动的函数
        /// <para>
        /// 如果实体移动 (<paramref name="walk"/> == <see langword="true"/>), VelX 加速度 (<see cref="WalkAcc"/>) 直到最大速度 (<see cref="MaxWalkSpd"/>)<br/>
        /// 如果速度超出范围, 或者停止移动, VelX 减速度 (同<see cref="WalkAcc"/>)
        /// </para>
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="walk"></param>
        protected virtual void Walk(GameTime gameTime, int walk)
        {
            if (!_velocity.X.IsInLimit(MaxWalkSpd) || walk == 0)
            {
                //如果速度超出范围，或松开移动键
                //如果 X 速度 < 减速度，直接归零
                if (MathF.Abs(_velocity.X) <= WalkAcc * gameTime.CFDuration())
                {
                    _velocity.X = 0;
                }
                else
                {
                    _velocity.X -= MathF.Sign(_velocity.X) * WalkAcc * gameTime.CFDuration();
                    //减速
                }
            }
            else if (WalkStatus != 0)
            {
                //如果速度没有超出范围，且按下移动键 (Status != 0)
                //加速度
                _velocity.X += WalkStatus * WalkAcc * gameTime.CFDuration();
                //如果加速度后超出范围，限制到范围内
                _velocity.X = MaxWalkSpd.Limit(_velocity.X);
            }
        }

        #endregion


        #region Methods - Move & HitCheck

        /// <summary>
        /// 分解 XY 速度，调用 <see cref="SpdDecompoMove(GameTime, MVec2, MVec2)"/> 移动，<br/>
        /// 根据返回值检测碰撞，设置 <see cref="OnGround"/> 等属性，调用碰撞体的 event 等
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns><see cref="OnGround"/></returns>
        protected virtual bool MoveHitAndGroundCheck(GameTime gameTime)
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
                    foreach(var hitboxY in hitboxesY)
                    {
                        if(hitboxY is RectangleHitboxEvented hitboxEvented)
                        {
                            hitboxEvented.SteppedBy(this);
                        }
                    }
                }
                _velocity.Y = 0;
            }
            var (hitX, _) = SpdDecompoMove(gameTime, _velocity * (1, 0), velRecord);
            if (hitX)
            {
                _velocity.X = 0;
            }
            return OnGround;
        }

        /// <summary>
        /// 分解速度移动, 检测碰撞, 如果碰撞，调用 <see cref="HitAction"/>，<br/>
        /// 根据返回值，如果需要，调用 <see cref="HitDichotomyMove(MVec2, RectangleHitbox)"/> 二分移动到合适的位置
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="vel"></param>
        /// <returns>
        /// (<see langword="bool"/>, <see cref="IList{T}"/><see cref="RectangleHitbox"/>)
        /// <list type="table">
        /// <item><term><see langword="bool"/>: </term><description>是否发生碰撞，碰撞会把速度清零，也与 OnGround 检测有关</description></item>
        /// <item><term><see cref="IList{RectangleHitbox}"/>: </term><description>发生碰撞的碰撞体</description></item>
        /// </list>
        /// </returns>
        protected (bool, RectangleHitbox[]) SpdDecompoMove(GameTime gameTime, MVec2 vel, MVec2 velRecord)
        {
            bool coll;
            //移动
            vel *= gameTime.CFDuration();
            _position += vel;

            //检查碰撞
            var hitboxes = HitUtility.GetHitRigidObjects(MGame.Main, Hitbox);
            RectangleHitbox[] hitboxes2 = null;
            if (hitboxes is not null)
            {
                hitboxes2 = new RectangleHitbox[hitboxes.Count];
                hitboxes.CopyTo(hitboxes2);
            }


            //如果不存在碰撞，返回假
            if (hitboxes is null) return (false, hitboxes2);


            //if (actionColl is not null) actionColl();

            var (coll2, endMethod) = HitAction(vel, hitboxes);
            coll = coll2;
            if (endMethod) return (coll, hitboxes2);


            //后退一半 vel
            vel /= 2.0f;
            _position -= vel;

            //二分移动，检测与碰撞体碰撞
            HitMove(hitboxes, _position, vel);


            return (coll, hitboxes2);
        }

        /// <summary>
        /// 与图格碰撞体碰撞 (重合) 后的行为，如自动上平台等
        /// <para>
        /// Invoke:<br/>
        /// 在 <see cref="SpdDecompoMove(GameTime, MVec2, MVec2)"/> 中，<br/>
        /// 调用 <see cref="HitUtility.GetHitTile(MGameMain, RectangleHitbox)"/> 后
        /// </para>
        /// <para>
        /// Base:<br/>
        /// 自动爬上一格平台
        /// </para>
        /// </summary>
        /// <returns>
        /// (<see cref="bool"/> Coll, <see cref="bool"/> EndMethod):<br/>
        /// <list type="table">
        /// <item><term>Coll</term> <description>调用其的函数 <see cref="SpdDecompoMove(GameTime, MVec2, MVec2)"/> 最终返回的值<br/>
        /// (是否发生碰撞，碰撞会把速度清零，也与 OnGround 检测有关)</description></item>
        /// <item><term>EndMethod</term> <description>返回后是否立刻 <b>结束 return</b> 调用其的函数</description></item>
        /// </list>
        /// </returns>
        protected virtual (bool Coll, bool EndMethod) HitAction(MVec2 vel, IList<RectangleHitbox> hitboxes)
        {
            if (vel.X != 0 && vel.Y <= 0)
            {
                var posTemp = _position;
                if (WalkStatus != 0 && OnGround)
                {
                    for(int i = 1; i <= 2; i++)
                    {

                        _position = posTemp.ChangeNew(y: 1 + 8 * i);
                        if (HitUtility.GetHitRigidObjects(MGame.Main, Hitbox) is null)
                        {
                            //如果向上移动一格后不会碰撞，那么将施加一个朝 Y 正方向的力
                            _velocity.Y += AutoStairClimbSpd * (i+1) / 3;
                            _position = posTemp;
                            //不碰撞
                            return (false, false);
                        }

                    }
                }
                _position = posTemp;
            }
            return (true, false);

        }

        /// <summary>
        /// 遍历 <paramref name="hitboxes"/> 内的所有 <see cref="RectangleHitbox"/>,<br/>
        /// 按照当前碰撞体移动后 (<see cref="HitDichotomyMove(MVec2, RectangleHitbox)"/> ), 是否与其他碰撞体碰撞
        /// </summary>
        /// <param name="hitboxes"></param>
        /// <param name="posTemp"></param>
        /// <param name="vel"></param>
        private void HitMove(IList<RectangleHitbox> hitboxes, MVec3 posTemp, MVec2 vel)
        {
            List<RectangleHitbox> removeList = new();

            HitDichotomyMove(vel, hitboxes[0]);
            hitboxes.RemoveAt(0);

            //遍历所有碰撞体
            foreach (var hitbox in hitboxes)
            {
                //如果仍然碰撞
                if (HitUtility.IsHit(Hitbox, hitbox))
                {
                    //位置重置
                    _position = posTemp;
                    //重新按照当前 Hitbox 以二分法移动
                    HitDichotomyMove(vel, hitbox);
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
            if (hitboxes.Count > 0) HitMove(hitboxes, posTemp, vel);
        }

        /// <summary>
        /// 用二分法, 更改坐标直到不碰撞
        /// </summary>
        /// <param name="vel"></param>
        /// <param name="hitbox"></param>
        private void HitDichotomyMove(MVec2 vel, RectangleHitbox hitbox)
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
            HitDichotomyMove(vel, hitbox);
        }
        #endregion

        /// <summary>
        /// 根据 <see cref="OnGround"/>，判断是否在地面上，并处理相关逻辑
        /// </summary>
        protected virtual void OnGroundAction(bool onGround, bool wasOnGround)
        {
            if (onGround)
            {
                JumpsCount = 0;
            }
            else if (wasOnGround)
            {
                //如果之前在地上，现在不在，说明离开地面
                //增加跳跃次数
                JumpsCount++;
            }
        }


        /// <summary>
        /// <para>
        /// <b><see langword="base">:</see></b><br/>
        /// 物理行为
        /// </para>
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //跳跃，横移，施加重力到 Vel
            Jump(JumpStatus);
            Walk(gameTime, WalkStatus);
            ApplyGravity(gameTime, Gravity);

            //根据 Vel 改变 Pos，检测碰撞
            bool wasOnGround = OnGround;
            OnGround = MoveHitAndGroundCheck(gameTime);

            //根据 OnGround 执行操作 (重置跳跃次数等)
            OnGroundAction(OnGround, wasOnGround);
        }

        #endregion
    }
}
