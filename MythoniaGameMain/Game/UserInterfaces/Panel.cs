



namespace Mythonia.Game.UserInterfaces
{
    public class Panel : UICollection
    {
        #region Implement - IUserInterfaceCollection

        

        //private event IUICollection.ResizeMethod OnResize;

        //public void AddChildrenResize(IUICollection.ResizeMethod resizeMethod) => OnResize += resizeMethod;
        //public void RemoveChildrenResize(IUICollection.ResizeMethod resizeMethod) => OnResize -= resizeMethod;
        //public void InvokeChildrenResize() => OnResize?.Invoke();

        #endregion


        public Panel(MGame game, UICollection father, NineSliceTexture texture, MVec2 origin,
            MVec2? size = null, MVec2? posRatio = null, MVec2? posDisplacement = null, 
            Transform? transform = null) 
            : base("Panel", game, father, MTextureManager.Ins.Get<NineSliceTexture>("UI/TestNineSlice")/*texture*/, 
                  origin: MVec2.Left/*origin*/, size: size, posRatio: posRatio, posDisplacement: posDisplacement, transform: transform)
        {//MTextureManager.Ins.Get<NineSliceTexture>("TestNineSlice")
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Size = Size.Change(x: -0.1f);
        }

        //public override void Resize()
        //{
        //    OnResize();
        //}
    }
}
