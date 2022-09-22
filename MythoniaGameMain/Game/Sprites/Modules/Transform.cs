



namespace Mythonia.Game.Sprites.Modules
{
    public struct Transform
    {
        public MVec3 Position { get; private set; }
        public Angle Direction { get; private set; }
        public MVec2 Scale { get; private set; }

        public SpriteEffects SpriteEffects => (Scale.X < 0, Scale.Y < 0) switch
        {
            (true, false) => SpriteEffects.FlipHorizontally,
            (false, true) => SpriteEffects.FlipVertically,
            _ => SpriteEffects.None
        };

        public (MVec3 Position, Angle Direction, MVec2 Scale) ToTuple => (Position, Direction, Scale);

        public Transform(MVec3? position = null, Angle direction = default, MVec2? scale = null)
        {

            Position = position ?? (0, 0, 0);
            Direction = direction;
            Scale = scale ?? (1, 1);
        }

        public static implicit operator (MVec3 Position, Angle Direction, MVec2 Scale)(Transform v) => v.ToTuple;
        public static implicit operator Transform((MVec3 Position, Angle Direction, MVec2 Scale) v) => new(v.Position,v.Direction,v.Scale);


    }
}
