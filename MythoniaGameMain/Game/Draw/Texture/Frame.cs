




namespace Mythonia.Game.Draw.Texture
{
    public class Frame : INamed
    {
        public string Name { get; init; }

        public Rectangle Range { get; init; }

        public Frame (string name, Rectangle range)
        {
            Name = name;
            Range = range;
        }
    }
}
