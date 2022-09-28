namespace Mythonia.Game.Physics
{
    

    public static class HitUtility
    {

        public static bool IsHitableWith(this Entity e1, Entity e2)
        {
            if (e1.Type == EntityType.HostileProjectile && e2.Type == EntityType.Player) return true;
            return false;
        }

        public static void GetHitEntities(this Entity e, EntitiesManager entities = null, Action<Entity, Entity> hitAction = null)
        {
            entities ??= EntitiesManager.Ins;
            hitAction ??= EntitiesHitAction;
            foreach(var e2 in entities)
            {
                if (e.IsHitableWith(e2) && IsHit(e.Hitbox, e2.Hitbox))
                {
                    hitAction(e, e2);
                }
            }
        }

        public static void EntitiesHitAction(Entity e1, Entity e2)
        {
            if (e1.Type == EntityType.HostileProjectile && 
                e2.Type == EntityType.Player) 
                ((IHealth)e2).Health.DealDamage(((IDamage)e1).Damage);

        }


        #region Method - CheckHit

        public static bool IsHit(IHitbox h1, IHitbox h2)
        {
            if(h1 is RectangleHitbox rect1)
            {
                if (h2 is RectangleHitbox rect2)
                {
                    return IsHit(rect1, rect2);
                }
                else if (h2 is CircleHitbox cir2)
                {
                    return IsHit(rect1, cir2);
                }
            }
            else if(h1 is CircleHitbox cir1)
            {
                if (h2 is RectangleHitbox rect2)
                {
                    return IsHit(cir1, rect2);
                }
                else if (h2 is CircleHitbox cir2)
                {
                    return IsHit(cir1, cir2);
                }
            }

            //如果上面的语句没有匹配到合适的方法，说明传入了一个奇怪的类型(正常不会出现)
            throw new Exception($"An Unknown IHitbox implement type is Given: h1 is [{h1.GetType()}], h2 is [{h2.GetType()}]");
        }

        public static bool IsHit(RectangleHitbox rect, MVec2 p)
            => (rect.Position - p).Abs < rect.Size / 2;

        public static bool IsHit(RectangleHitbox rect1, RectangleHitbox rect2)
            => (rect1.Position - rect2.Position).Abs < (rect1.Size + rect2.Size) / 2;

        public static bool IsHit(CircleHitbox circle1, CircleHitbox circle2)
            => (circle1.Position - circle2.Position).LengthSquared() < (circle1.Radius + circle2.Radius) / 2;

        public static bool IsHit(CircleHitbox circle, RectangleHitbox rect)
        {
            MVec2 distance = (rect.Position - circle.Position).Abs;
            MVec2 rectSize = rect.Size / 2;

            //1. 把 circle 视为矩形粗判定
            if (distance >= rectSize + circle.Radius.ToVec()) return false;

            //2. 因为!(1)已经证实两个矩形碰撞, 只要 X Y 距离 其中之一在矩形尺寸的范围内 => 圆形必定与矩形碰撞
            else if (distance.X < rectSize.X || distance.Y < rectSize.Y) return true;

            //3. 如果!(1)(2), 与圆形最近的是矩形的一个角, 距离 - 矩形尺寸 / 2 = 圆形到矩形最近角的距离, 与圆形半径作比较
            else if ((distance - rectSize).LengthSquared() < MathF.Pow(circle.Radius, 2)) return true;

            //4. 否则不碰撞
            else return false;

        }

        public static bool IsHit(RectangleHitbox rect, CircleHitbox circle) => IsHit(circle, rect);

        #endregion


        /// <summary>
        /// 给定一个矩形碰撞体, 与 <see cref="Map"/> 的碰撞体检测碰撞
        /// </summary>
        /// <param name="hitbox">给定的碰撞体</param>
        /// <returns>
        /// <list type="table">
        /// <item><term>碰撞</term> <description>与之碰撞的 <see cref="IList{RectHitbox}"/> 碰撞体</description></item>
        /// <item><term>不碰撞</term> <description><see langword="null"/></description></item>
        /// </list>
        /// </returns>
        public static List<RectangleHitbox> GetHitTiles(MGameMain gameMain, RectangleHitbox hitbox)
        {
            MVec2 blTilePos = hitbox.BottomLeft / gameMain.Map.TileSizeVec;
            blTilePos.Floor();
            MVec2 trTilePos = hitbox.TopRight / gameMain.Map.TileSizeVec;
            trTilePos.Ceiling();

            List<RectangleHitbox> checkedHitboxes = new();
            List<RectangleHitbox> hitHitboxes = new();
            int trX = (int)trTilePos.X;
            int trY = (int)trTilePos.Y;
            for (int x = (int)blTilePos.X; x <= trX; x++)
            {
                for (int y = (int)blTilePos.Y; y <= trY; y++)
                {
                    if (gameMain.Map[x, y] != null)
                    {
                        var tileHitbox = gameMain.Map[x, y].Hitbox;
                        if (!checkedHitboxes.Contains(tileHitbox))
                        {
                            checkedHitboxes.Add(tileHitbox);
                            if (IsHit(hitbox, tileHitbox)) hitHitboxes.Add(tileHitbox);
                        }
                    }
                }
            }
            return hitHitboxes.IsEmpty() ? null : hitHitboxes;

        }
        /// <summary>
        /// 给定一个矩形碰撞体, 与 <see cref="Map"/>的碰撞体 和 <see cref="MGameMain.Entities"/>的碰撞体 检测碰撞
        /// <para>会先调用 <see cref="GetHitTiles(MGameMain, RectangleHitbox)"/></para>
        /// </summary>
        /// <param name="hitbox">给定的碰撞体</param>
        /// <returns>
        /// <list type="table">
        /// <item><term>碰撞</term> <description>与之碰撞的 <see cref="IList{RectHitbox}"/> 碰撞体</description></item>
        /// <item><term>不碰撞</term> <description><see langword="null"/></description></item>
        /// </list>
        /// </returns>
        public static List<RectangleHitbox> GetHitRigidObjects(MGameMain gameMain, RectangleHitbox hitbox)
        { 
            List<RectangleHitbox> hitHitboxes = GetHitTiles(gameMain, hitbox) ?? new();

            foreach (var entity in gameMain.Entities)
            {
                if(entity.Hitbox is RectangleHitbox entityHitbox && entity.Hitbox.Type is IHitbox.Types.Rigid)
                {
                    if (IsHit(hitbox, entityHitbox)) hitHitboxes.Add(entityHitbox);
                }
            }

            return hitHitboxes.IsEmpty() ? null : hitHitboxes;
        }



        #region Extension Methods

        public static bool IsHit(this ICollection<RectangleHitbox> hitboxes, RectangleHitbox hitbox)
        {
            foreach (var hitbox2 in hitboxes) if (IsHit(hitbox, hitbox2)) return true;
            return false;
        }

        #endregion

    }
}
