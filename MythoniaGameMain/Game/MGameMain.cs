using Mythonia.Game.TileMap;



namespace Mythonia.Game
{
    /// <summary>
    /// 封装存储大部分游戏对象, 与 <see cref="Game.MGame"/> 主循环分开
    /// </summary>
    public class MGameMain
    {
        public Map TileMap { get; set; }

        public MGame MGame { get; init; }

        public Player.Player Player { get; set; }

        public Camera Camera { get; set; }

        public MGameMain(MGame game, Rectangle tileSize)
        {
            MGame = game;

            Player = new Player.Player(MGame);

            Camera = new(MGame, Player, 0, 0);

            TileMap = Map.StringToMap(MGame, tileSize, new string[]
            {

                @"                                                                 ##  ",
                @"                                                                  |  ",
                @"                                                                  |  ",
                @"                                                              ####|  ",
                @"                                                                  |  ",
                @"                                                                  |  ",
                @"                                                                  |  ",
                @"                                                            ####  |  ",
                @"                                                                  |  ",
                @"                                                                  |  ",
                @"                                                                  |  ",
                @"                                                          ####    |  ",
                @"                                                                  |  ",
                @"                                                                  |  ",
                @"                                                                  |  ",
                @"                                                        ####         ",
                @"                                                                     ",
                @"                                                                     ",
                @"                                                                     ",
                @"                                                      ####           ",
                @"   ####                                                              ",
                @"     |      ###                                                      ",
                @"     |       |                                                       ",
                @"     |       |                                      ######           ",
                @"     |      ######                                                   ",
                @"  ######   ##    ##                                                  ",
                @" ##    #####      ##                                                 ",
                @"##                 ##################################################",
            });

            MGame.Components.Add(Camera);
            MGame.Components.Add(TileMap);
            MGame.Components.Add(Player);
        }
    }
}
