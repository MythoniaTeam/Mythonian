



namespace Mythonia.Debug
{
    public class MDebug
    {

#if DEBUG
        public static bool DrawEntitiesHitbox { get; set; } = true;
#else          
        public static bool DrawEntitiesHitbox { get; set; } = false;
#endif
        
#if DEBUG
        public static bool DrawTilesHitbox { get; set; } = true;
#else          
        public static bool DrawTilesHitbox { get; set; } = false;
#endif

    }
}
