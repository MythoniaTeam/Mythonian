﻿using Mythonia.Game.TileMap;



namespace Mythonia.Game
{
    /// <summary>
    /// 封装存储大部分游戏对象, 与 <see cref="Game.MGame"/> 主循环分开
    /// </summary>
    public class MGameMain
    {

        public Camera Camera { get; set; }

        public Input Input { get; set; }

        public Map TileMap { get; set; }

        public MGame MGame { get; init; }

        public Player.Player Player { get; set; }

        public MGameMain(MGame game, Rectangle tileSize)
        {
            MGame = game;

            Camera = new(MGame, 0, 0);

            Input = new(MGame, new Keys[] {
                Keys.A,
                Keys.D,
                Keys.W,
                Keys.S,
                Keys.L,
                Keys.K
            });

            Player = new Player.Player(MGame);

            TileMap = Map.StringToMap(MGame, tileSize, new string[]
            {
                @"   ####            ##   ",
                @"     |             #    ",
                @"     |             ##   ",
                @"     |             #    ",
                @"     |             ##   ",
                @"  ######           #    ",
                @" ##    ##############   ",
                @"##                 #### ",
            });

            MGame.Components.Add(Camera);
            MGame.Components.Add(Input);
            MGame.Components.Add(TileMap);
            MGame.Components.Add(Player);
        }
    }
}
