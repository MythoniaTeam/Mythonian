



namespace Mythonia.Game
{
    public class MTime
    {
        public static int StandardFPS { get; set; } = 60;
    }

    public static class MTimeExtension
    {
        public static float CFDuration(this GameTime gameTime)
            => gameTime.CFToSecond() * MTime.StandardFPS;

        public static float CFToSecond(this GameTime gameTime) => (float)gameTime.ElapsedGameTime.TotalSeconds;



    }
}
