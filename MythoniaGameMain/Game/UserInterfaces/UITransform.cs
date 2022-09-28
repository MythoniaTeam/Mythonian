



namespace Mythonia.Game.UserInterfaces
{
    public struct UITransform
    {
        public MVec2 PosRatio { get; set; }
        public MVec2 PosDisplacement { get; set; }
        public MVec2 Size { get; set; }

        public (MVec2 PosRatio, MVec2 PosDisplacement, MVec2 Size) ToTuple => (PosRatio, PosDisplacement, Size);

        public UITransform(MVec2 size, MVec2? posRatio = null, MVec2? posDisplacement = null)
        {
            PosRatio = posRatio ?? new();
            PosDisplacement = posDisplacement ?? new();
            Size = size;
        }

    }
}
