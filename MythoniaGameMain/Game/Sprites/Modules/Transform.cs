



namespace Mythonia.Game.Sprites.Modules
{
    public struct Transform
    {
        public MVec3 Position { get; set; }
        public Angle Direction { get; set; }
        public MVec2 Scale { get; set; } = (1, 1);

        //// <summary>表示原点位置的比例, (0, 0) 为正中心，(-1, -1) 为左下</summary>
        //public MVec2 OriginRatio { get; set; } = (0, 0);

        //public MVec2 GetDrawOrigin(Rectangle sourceRange) => (MVec2)sourceRange.Size.ToVector2() * (OriginRatio * (1, -1) + (1, 1)) / 2,

        public SpriteEffects SpriteEffects => (Scale.X < 0, Scale.Y < 0) switch
        {
            (true, false) => SpriteEffects.FlipHorizontally,
            (false, true) => SpriteEffects.FlipVertically,
            _ => SpriteEffects.None
        };

        public (MVec3 Position, Angle Direction, MVec2 Scale) ToTuple => (Position, Direction, Scale);

        public Transform(MVec3? position = null, Angle direction = default, MVec2? scale = null/*, MVec2? origin = null*/)
        {

            Position = position ?? (0, 0, 0);
            Direction = direction;
            Scale = scale ?? (1, 1);
            //OriginRatio = origin ?? (0, 0)
        }

        public static implicit operator (MVec3 Position, Angle Direction, MVec2 Scale)(Transform v) => v.ToTuple;
        public static implicit operator Transform((MVec3 Position, Angle Direction, MVec2 Scale) v) => new(v.Position,v.Direction,v.Scale);


    }
}
