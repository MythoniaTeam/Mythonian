using Mythonia.Game.TileMap;



namespace Mythonia.Game.Physics
{
    public class HitManager : DrawableGameComponent
    {
        public Main MGame => (Main)Game;

        public List<RectHitbox> TileHitboxes { get; private set; } = new();



        private Texture2D px;
        public HitManager(Main game, Map tilemap) : base(game)
        {
            px = game.Content.Load<Texture2D>("Images/PX");
            SetTileHitbox(tilemap);
        }

        private void SetTileHitbox(Map tilemap)
        {
            bool[,] tileChecked = new bool[tilemap.Width, tilemap.Height];

            //遍历每一个图格
            foreach (var tile in tilemap)
            {
                //如果图格已经检查过了, 跳过当前图格
                if (tile is null || tileChecked[(int)tile.MapIndex.X, (int)tile.MapIndex.Y]) continue;


                MVec2 indexFr = tile.MapIndex;
                MVec2 indexTo = tile.MapIndex;
                List<Tile> tilesInclude = new();


                var tile2 = tile;

                //遍历 x 直到边缘, 或直到没有碰撞体
                while (tile2?.HasColl ?? false)
                {
                    indexTo.X++;
                    tileChecked[(int)tile2.MapIndex.X, (int)tile2.MapIndex.Y] = true;
                    tilesInclude.Add(tile2);
                    tile2 = tile2.TileRight;
                }

                indexTo.X--;
        
                if (indexTo.X >= indexFr.X)
                {
                    //如果范围内至少包含一个图格 To >= Fr

                    //遍历 y 直到达到边缘, 或者跳出循环 (breakWhild)
                    bool breakWhile = false;
                    for (int y = (int)indexFr.Y + 1; y < tilemap.Height && !breakWhile; y++)
                    {
                        //如果下一层左右都有别的图格, 跳出循环
                        if ((tilemap[(int)indexFr.X, y]?.TileLeft?.HasColl ?? false) &&
                            (tilemap[(int)indexTo.X, y]?.TileRight?.HasColl ?? false))
                        {
                            indexTo.Y = y - 1;
                            breakWhile = true;
                            break;
                        }

                        //遍历 x, 如果有任意一个 x 没有碰撞体, 跳出循环
                        for (int x = (int)indexFr.X; x <= (int)indexTo.X; x++)
                        {
                            if (!(tilemap[x, y]?.HasColl ?? false))
                            {
                                indexTo.Y = y - 1;
                                breakWhile = true;
                                break;
                            }
                        }

                        //如果没有break, 把这些砖块设为checked
                        if (!breakWhile)
                        {
                            for (int x = (int)indexFr.X; x <= (int)indexTo.X; x++)
                            {
                                tileChecked[x, y] = true;
                                tilesInclude.Add(tilemap[x, y]);
                            }
                        }
                    }

                    var hitbox = new RectHitbox(MGame, () => ((indexTo + indexFr) / 2) * tilemap.TileSizeVec, (indexTo - indexFr + (1, 1)) * tilemap.TileSizeVec);
                    TileHitboxes.Add(hitbox);

                    //将碰撞体绑定在图格上
                    foreach (var tileInclude in tilesInclude)
                    {
                        tileInclude.Hitboxes.Add(hitbox);
                    }
                }
            }
        }



        #region Method - CheckColl

        public static bool IsCollided(RectHitbox rect1, RectHitbox rect2)
            => (rect1.Position - rect2.Position).Abs < (rect1.Size + rect2.Size) / 2;

        public static bool IsCollided(CircleHitbox cir1, CircleHitbox cir2)
            => (cir1.Position - cir2.Position).LengthSquared() < (cir1.Radius + cir2.Radius) / 2;

        public static bool IsCollided(CircleHitbox cir, RectHitbox rect)
        {
            MVec2 dis = (rect.Position - cir.Position).Abs;
            MVec2 rectSize = rect.Size / 2;

            //1. 把 cir 视为矩形粗判定
            if (dis >= rectSize + cir.Radius.ToVec()) return false;

            //2. 因为!(1)已经证实两个矩形碰撞, 只要 X Y 距离 其中之一在矩形尺寸的范围内 => 圆形必定与矩形碰撞
            else if (dis.X < rectSize.X || dis.Y < rectSize.Y) return true;

            //3. 如果!(1)(2), 与圆形最近的是矩形的一个角, 距离 - 矩形尺寸 / 2 = 圆形到矩形最近角的距离, 与圆形半径作比较
            else if ((dis - rectSize).LengthSquared() < MathF.Pow(cir.Radius, 2)) return true;

            //4. 否则不碰撞
            else return false;

        }

        public static bool IsCollided(RectHitbox rect, CircleHitbox cir) => IsCollided(cir, rect);

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
        public IList<RectHitbox> IsCollidedWithTile(RectHitbox hitbox)
        {
            List<RectHitbox> hitboxes = new();
            foreach(var tileHitbox in TileHitboxes)
            {
                if (IsCollided(hitbox, tileHitbox)) hitboxes.Add(tileHitbox);
            }
            return hitboxes.IsEmpty() ? null : hitboxes;
        }

        public override void Draw(GameTime gameTime)
        {
#if DEBUG
            if (MDebug.DrawTilesHitbox)
            {
                Color color = new(100, 20, 20, 150);
                foreach(var hitbox in TileHitboxes)
                {
                    color.R += (byte)(155 / TileHitboxes.Count);
                    color.G += (byte)(140 / TileHitboxes.Count);
                    color.B += (byte)(140 / TileHitboxes.Count);
                    hitbox.DrawHitbox(color);
                }
            }
#endif
        }

    }
}
