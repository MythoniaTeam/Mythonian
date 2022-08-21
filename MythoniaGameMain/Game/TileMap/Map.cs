



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
                try
                {
                    return _tiles[_X, _Y];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
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



        public Tile this[int x, int y]
        {
            get
            {
                try
                {
                    return _tiles[x, y];
                }
                catch (Exception)
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

        public Map(MGame game, Rectangle tilesize, Tile[,] tiles) : base(game)
        {
            TileSize = tilesize;
            _tiles = tiles;
        }

        #endregion Constructor



        #region Methods

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


        public override void Draw(GameTime gameTime)
        {
            foreach(var tile in this)
            {
                if (tile != null) tile.Draw(MGame.SpriteBatch, MGame.Main.Camera);
               
            }
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
