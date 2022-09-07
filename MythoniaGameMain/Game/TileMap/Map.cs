



using System.Collections;

namespace Mythonia.Game.TileMap
{

    public class MapEnum : IEnumerator<Tile>
    {
        private readonly Tile[,] _tiles;

        private int _X = -1;
        private int _Y = 0;

        public Tile Current
        {
            get
            {
                return _tiles[_X, _Y];
            }
        }

        object IEnumerator.Current => Current;


        public MapEnum(Tile[,] tiles)
        {
            _tiles = tiles;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }


        public bool MoveNext()
        {
            _X++;
            if(_X >= _tiles.GetLength(0))
            {
                _X = 0;
                _Y++;
            }
            return !(_Y >= _tiles.GetLength(1));
        }

        public void Reset()
        {
            _X = -1;
            _Y = 0;
        }
    }



    public class Map : DrawableGameComponent, IEnumerable<Tile>
    {
        public MGame MGame => Game as MGame;

        private readonly Tile[,] _tiles;


        public int Width => _tiles.GetLength(0);
        public int Height => _tiles.GetLength(1);

        public Rectangle TileSize { get; init; }
        public MVec2 TileSizeVec => TileSize.Size;

        public List<RectangleHitbox> MapHitboxes { get; private set; } = new();



        public Tile this[int x, int y]
        {
            get
            {
                if(x >= 0 && x < _tiles.GetLength(0) && y >= 0 && y < _tiles.GetLength(1))
                {
                    return _tiles[x, y];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                _tiles[x, y] = value;
            }
        }
        public Tile this[MVec2 v]
        {
            get => this[(int)v.X, (int)v.Y];
            set => this[(int)v.X, (int)v.Y] = value;
        }



        #region Constructor

        private Map(MGame game, Rectangle tilesize, Tile[,] tiles) : base(game)
        {
            TileSize = tilesize;
            _tiles = tiles;
        }

        #endregion Constructor



        #region Methods

        private void SetTileHitbox()
        {
            bool[,] tileChecked = new bool[Width, Height];

            //遍历每一个图格
            foreach (var tile in this)
            {
                //如果图格已经检查过了, 跳过当前图格
                if (tile is null || tileChecked[(int)tile.MapIndex.X, (int)tile.MapIndex.Y] || !tile.HasColl) continue;


                MVec2 indexFr = tile.MapIndex;
                MVec2 indexTo = tile.MapIndex;
                List<Tile> tilesInclude = new();


                var tile2 = tile;

                //遍历 x 直到边缘, 或直到没有碰撞体
                while (tile2?.HasColl ?? false)
                {
                    indexTo.X++;
                    //把方块设为已检查
                    tileChecked[(int)tile2.MapIndex.X, (int)tile2.MapIndex.Y] = true;
                    //当前方块包含在内
                    tilesInclude.Add(tile2);

                    //把当前方块 设为右边的方块
                    tile2 = tile2.TileRight;
                }

                indexTo.X--;

                if (indexTo.X >= indexFr.X)
                {
                    //如果范围内至少包含一个图格 To >= Fr

                    //遍历 y 直到达到边缘, 或者跳出循环 (breakWhild)
                    bool breakWhile = false;
                    for (int y = (int)indexFr.Y + 1; y < Height && !breakWhile; y++)
                    {
                        //如果下一层左右都有别的图格, 跳出循环
                        if ((this[(int)indexFr.X, y]?.TileLeft?.HasColl ?? false) &&
                            (this[(int)indexTo.X, y]?.TileRight?.HasColl ?? false))
                        {
                            //indexTo.Y = y - 1;
                            breakWhile = true;
                        }

                        //遍历 x, 如果有任意一个 x 没有碰撞体, 跳出循环
                        for (int x = (int)indexFr.X; x <= (int)indexTo.X; x++)
                        {
                            if (!(this[x, y]?.HasColl ?? false))
                            {
                                //indexTo.Y = y - 1;
                                breakWhile = true;
                                break;
                            }
                        }

                        //如果没有break, 把这些砖块设为checked
                        if (!breakWhile)
                        {
                            indexTo.Y++;
                            for (int x = (int)indexFr.X; x <= (int)indexTo.X; x++)
                            {
                                tileChecked[x, y] = true;
                                tilesInclude.Add(this[x, y]);
                            }
                        }
                    }

                    var hitbox = new RectangleHitbox(MGame, () => ((indexTo + indexFr) / 2) * TileSizeVec, (indexTo - indexFr + (1, 1)) * TileSizeVec);
                    MapHitboxes.Add(hitbox);

                    //将碰撞体绑定在图格上
                    foreach (var tileInclude in tilesInclude)
                    {
                        tileInclude.Hitbox = hitbox;
                    }
                }
            }
        }



        /// <summary>
        /// 给定一个 <seealso cref="Tile"/> <paramref name="tile"/>，返回该 <see cref="Tile"/> 于该 <see cref="Map"/> 中的下标
        /// </summary>
        /// <param name="tile">给定的图格</param>
        /// <returns>
        /// 对应 <seealso cref="Tile"/> 的的下标 <b>(x, y)</b><br/>
        /// <list type="table"><item>
        /// <term><see langword="null"/></term> <description>图格 <paramref name="tile"/> 不存在于当前 <see cref="Map"/> 中</description>
        /// </item></list>
        /// </returns>
        public MVec2? FindIndex(Tile tile)
        {
            for(int y = 0; y < _tiles.GetLength(1); y++)
            {
                for (int x = 0; x < _tiles.GetLength(0); x++)
                {
                    if (_tiles[x, y] == tile) return (x, y);
                }
            }
            return (-1, 0);
        }

        public void UpdateTileTexture()
        {
            foreach (var tile in this)
            {
                if (tile != null) tile.UpdateTexture();

            }
        }

        public override void Initialize()
        {
            SetTileHitbox();
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var tile in this)
            {
                if (tile != null) tile.Draw(gameTime);

            }
#if DEBUG
            if (MDebug.DrawTilesHitbox)
            {
                Color color;
                int j = 0;
                foreach (var hitbox in MapHitboxes)
                {
                    int i = j * 10;
                    color = (j % 7) switch
                    {
                        0 => new(100 + i % 155, 50 + i % 155, 50 + i % 155),
                        1 => new(50 + i % 155, 100 + i % 155, 50 + i % 155),
                        2 => new(50 + i % 155, 50 + i % 155, 100 + i % 155),
                        3 => new(100 + i % 155, 100 + i % 155, 50 + i % 155),
                        4 => new(100 + i % 155, 50 + i % 155, 100 + i % 155),
                        5 => new(50 + i % 155, 100 + i % 155, 100 + i % 155),
                        6 => new(100 + i % 155, 100 + i % 155, 100 + i % 155),
                        _ => throw new Exception()
                    };
                    color = new(color, 150);
                    hitbox.DrawHitbox(color);
                    j++;
                }
            }
#endif
        }


        #region Static Methods

        public static Map StringToMap(MGame game, Rectangle tilesize, string[] dataRows)
        {
            /*data = data.Replace("\r", "");
            data = data.Replace("\t", "");
            data = data.Trim('\n');
            string[] dataRows = data.Split('\n');*/
            for(int y = 0; y < MathF.Floor(dataRows.Length / 2.0f); y++)
            {
                MainExtension.Swap(ref dataRows[y], ref dataRows[^(y+1)]);
            }


            var tiles = new Tile[dataRows[0].Length, dataRows.Length];
            Map map = new(game, tilesize, tiles);

            for(int row = 0; row < dataRows.Length; row++)
            {
                if (dataRows[0].Length != dataRows[row].Length)
                    throw new Exception("The String Given have differen length at each lines");

                for(int col = 0; col < dataRows[row].Length; col++)
                {
                    map[col, row] = Tile.ConstructTile(game, dataRows[row][col], map, (col, row));
                }
            }
            map.UpdateTileTexture();
            map.Initialize();

            return map;
        }

        #endregion Static Methods



        #region Override Methods

        public override string ToString() => ToString(false);
        
        public string ToString(bool isid)
        {
            string str = "";

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    str += isid ? this[x, y].Id : this[x, y].ToString();
                }
                str += "\r\n";
            }

            return str;
        }


        #endregion

        #endregion Methods


        #region Implement - IEnumerable



        IEnumerator<Tile> IEnumerable<Tile>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public MapEnum GetEnumerator() => new(_tiles);


        #endregion
    }
}
