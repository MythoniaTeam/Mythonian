﻿using Mythonia.Game.TileMap;



namespace Mythonia.Game
{
    /// <summary>
    /// 封装存储大部分游戏对象, 与 <see cref="Game.MGame"/> 主循环分开
    /// </summary>
    public class MGameMain
    {
        public Map TileMap { get; set; }
        public HitManager HitManager { get; set; }

        public MGame MGame { get; init; }

        public Camera Camera { get; set; }

        public MGameMain(MGame game, Rectangle tileSize)
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