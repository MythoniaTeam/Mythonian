



namespace Mythonia.Game.Sprites.UI
{
    public class Panel : UI
    {
        public Panel(MGame game, Map map, Transform transform = default) 
            : base("Panel", game, map, MTextureManager.Ins.Get<NineSliceTexture>("TestNineSlice"), transform)
        {

        }


    }
}
