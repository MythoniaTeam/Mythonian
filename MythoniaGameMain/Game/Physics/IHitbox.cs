



namespace Mythonia.Game.Physics
{
    public interface IHitbox
    {
        public MVec2 Position { get; }

        public enum Types
        {
            UI,
            /// <summary>
            /// [刚体] 碰撞时会被推挤，无法穿过 (如图格)
            /// </summary>
            Rigid, 
            /// <summary>
            /// [实体] 碰撞时会被推挤，但不视为图格，无法判定为站立等
            /// </summary>
            Entity,
            /// <summary>
            /// [触发器] 不会推挤
            /// </summary>
            Trigger,
        }

        public Types Type { get; }
    }
}
