



namespace Mythonia.Game.Sprites
{
    public struct Transform
    {
        public MVec3 Position { get; private set; }
        public Angle Direction { get; private set; }
        public MVec2 Scale { get; private set; }
        public bool FlipX { get; set; } 
        public bool FlipY { get; set; }

        public SpriteEffects SpriteEffects => (FlipX, FlipY) switch
        {
            (true, false) => SpriteEffects.FlipHorizontally,
            (false, true) => SpriteEffects.FlipVertically,
            _ => SpriteEffects.None
        };

        public (MVec3 Position, Angle Direction, MVec2 Scale, bool FlipX, bool FlipY) ToTuple => (Position, Direction, Scale, FlipX, FlipY);

        public Transform(MVec3? position = null, Angle direction = default, MVec2? scale = null, bool flipX = false, bool flipY = false)
        {

            Position = position ?? (0, 0, 0);
            Direction = direction;
            Scale = scale ?? (1, 1);
            FlipX = flipX;
            FlipY = flipY;
        }

        public static implicit operator (MVec3 Position, Angle Direction, MVec2 Scale, bool FlipX, bool FlipY)(Transform v) => v.ToTuple;
        public static implicit operator Transform((MVec3 Position, Angle Direction, MVec2 Scale, bool FlipX, bool FlipY) v) => new(v.Position,v.Direction,v.Scale, v.FlipX, v.FlipY);
        public static implicit operator Transform((MVec3 Position, Angle Direction, MVec2 Scale) v) => new(v.Position,v.Direction,v.Scale);


    }
}
