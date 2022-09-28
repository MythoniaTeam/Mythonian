using Mythonia.Game.TileMap;



namespace Mythonia.Game
{
    /// <summary>
    /// 封装存储大部分游戏对象, 与 <see cref="Game.MGame"/> 主循环分开
    /// </summary>
    public class MGameMain
    {
        public MGame MGame { get; init; }

        public Input Input { get; init; }
        public TextManager Text { get; init; }
        public UIManager UserInterface { get; init; }
        public Pen Pen { get; set; }


        public Map Map { get; set; }


        public Camera Camera { get; set; }
        public Player Player { get; set; }

        public EntitiesManager Entities { get; init; } = new();


        public MGameMain(MGame game, Rectangle tileSize)
        {
            MGame = game;

            var font = MGame.Content.Load<SpriteFont>("Fonts/Default");
            Text = new TextManager(MGame, font);
            UserInterface = new UIManager(MGame);


            /*
             * 可通过Input[KeyName keyName]来访问这些按键的状态
             * 例：MGame.Main.Input[KeyName.Jump]
             * 返回一个int表示按键状态
             * 也可以使用这些返回布尔值的方法：KeyDown, KeyPress, KeyUp, KeyRelease
             */
            Input = new(MGame);

            Pen = new(MGame);

            Map = Map.StringToMap(MGame, tileSize, new string[]
            {


                @"                                                                 ##                                                                                                     #",
                @"                                                                  |                                                                                                     #",
                @"                                                                  |                                                                                                     #",
                @"                            @@  ## # #| @@@|                  ####|                                                                                                     #",
                @"                           @@@@#######||@@@|                      |                                                                                                     #",
                @"                          ##@@|@@@@@@@##@@@|                      |                                                                                                     #",
                @"                         ####|||@@ @ @# |||#                      |                                                                                                     #",
                @"                         ####|||# # @@##                #   ####  |                                                                                                     #",
                @"                          ##@@|##### @#                           |                                                                                                     #",
                @"                           |@@@ ### @@##                          |                                                                                                     #",
                @"                          |||@@#####@@##                          |                                                                                                     #",
                @"                           |@@@ # #  @#                   ####    |                                                                                                     #",
                @"                            @@                                    |                                                                                                     #",
                @"                                                                  |                                                                                                     #",
                @"                                                                  |                                                                                                     #",
                @"                                                        ####                                                                                                            #",
                @"                                                                                                                                                                        #",
                @"                                                                                                                                                                        #",
                @"            #####                                                                                                                                                       #",
                @"           ######                ####                ####                                                                                                               #",
                @"          #######            #####                                                                                                                                      #",
                @"         ########               ##                                                                                                                                      #",
                @"        #########               ##                                                                                                                                      #",
                @"   ######################################         #####                                                                                                                 #",
                @"                                                                                                                                                                        #",
                @"                                                                                                                                                                        #",
                @"                                                                                                                                                                        #",
                @"#########################################################################################################################################################################",
            });

            var pressurePlate = new PressurePlate(MGame, Map, Map.TileSizeVec * (4, 1));
            MGame.Components.Add(pressurePlate);

            Camera = new(MGame, 0, 0);

            Player = new Player(MGame, Map);


            MGame.Components.Add(Camera);
            MGame.Components.Add(Input);
            MGame.Components.Add(Map);
            MGame.Components.Add(Player);
            MGame.Components.Add(Text);
            MGame.Components.Add(Pen);
            MGame.Components.Add(UserInterface);
        }
    }
}
