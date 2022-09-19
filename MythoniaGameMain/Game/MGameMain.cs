using Mythonia.Game.TileMap;



namespace Mythonia.Game
{
    /// <summary>
    /// 封装存储大部分游戏对象, 与 <see cref="Game.MGame"/> 主循环分开
    /// </summary>
    public class MGameMain
    {
        public Map TileMap { get; set; }

        public Camera Camera { get; set; }
        public Player Player { get; set; }


        public Input Input { get; init; }
        public TextManager Text { get; init; }

        public MGame MGame { get; init; }


        public Pen.Pen Pen { get; set; }

        public MGameMain(MGame game, Rectangle tileSize)
        {
            MGame = game;

            /*
             * 可通过Input[KeyName keyName]来访问这些按键的状态
             * 例：MGame.Main.Input[KeyName.Jump]
             * 返回一个int表示按键状态
             * 也可以使用这些返回布尔值的方法：KeyDown, KeyPress, KeyUp, KeyRelease
             */
            Input = new(MGame, new Keys[] {
                Keys.A,
                Keys.D,
                Keys.W,
                Keys.S,
                Keys.L,
                Keys.K
            });

            Player = new(MGame);

            Pen = new(MGame);

            TileMap = Map.StringToMap(MGame, tileSize, new string[]
            {


                @"                                                                 ##  ",
                @"                                                                  |  ",
                @"                                                                  |  ",
                @"                            @@  ## # #| @@@|                  ####|  ",
                @"                           @@@@#######||@@@|                      |  ",
                @"                          ##@@|@@@@@@@##@@@|                      |  ",
                @"                         ####|||@@ @ @# |||#                      |  ",
                @"                         ####|||# # @@##                #   ####  |  ",
                @"                          ##@@|##### @#                           |  ",
                @"                           |@@@ ### @@##                          |  ",
                @"                          |||@@#####@@##                          |  ",
                @"                           |@@@ # #  @#                   ####    |  ",
                @"                            @@                                    |  ",
                @"                                                                  |  ",
                @"                                                                  |  ",
                @"                                                        ####         ",
                @"                                                                     ",
                @"                                                                     ",
                @"            #####                                                    ",
                @"           ######                ####                ####            ",
                @"          #######            #####                                   ",
                @"         ########               ##                                   ",
                @"        #########               ##                                   ",
                @"   ######################################         #####              ",
                @"                                                                     ",
                @"                                                                     ",
                @"                                                                     ",
                @"#####################################################################",
            });

            var font = MGame.Content.Load<SpriteFont>("Fonts/Default");
            Text = new TextManager(MGame, font);


            Camera = new(MGame, 0, 0);

            Player = new Player(MGame, TileMap);


            MGame.Components.Add(Camera);
            MGame.Components.Add(Input);
            MGame.Components.Add(TileMap);
            MGame.Components.Add(Player);
            MGame.Components.Add(Text);
            MGame.Components.Add(Pen);
        }
    }
}
