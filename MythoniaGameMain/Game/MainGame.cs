using Mythonia.Game.TileMap;



namespace Mythonia.Game
{
    public class MainGame
    {
        public Map TileMap { get; set; }
        public HitManager HitManager { get; set; }

        public Main MGame { get; init; }

        public Camera Camera { get; set; }

        public MainGame(Main game, Rectangle tileSize)
        {
            MGame = game;
            Camera = new(MGame, 0, 0);

            TileMap = Map.StringToMap(MGame, tileSize, new string[]
            {
                @"   ####             ",
                @"     |      ###     ",
                @"     |       |      ",
                @"     |       |      ",
                @"     |      ######  ",
                @"  ######   ##       ",
                @" ##    #####        ",
                @"##                  ",
            });
            HitManager = new(MGame, TileMap);

            MGame.Components.Add(TileMap);
            MGame.Components.Add(new Player.Player(MGame));
            MGame.Components.Add(HitManager);
        }
    }
}
