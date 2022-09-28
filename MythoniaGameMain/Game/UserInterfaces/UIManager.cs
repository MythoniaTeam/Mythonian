




namespace Mythonia.Game.UserInterfaces
{
    public class UIManager : UICollection
    {
        public static UIManager Ins { get; private set; }

        //public MGame MGame => (MGame)Game;


        //MVec2 IUICollection.Position => (0, 0);
        //public MVec2 Size => MGame.GraphicsDevice.Size();

        public override MVec3 Position => (0, 0, 0);
        public override MVec2 Size { get => MGame.GraphicsDevice.Size(); set => base.Size = value; }

        public UIManager(MGame game): base("UIManager", game, null, null,
            size: game.GraphicsDevice.Size())
        {
            Ins = this;
            //MGame.Window.ClientSizeChanged += Window_ClientSizeChanged; 
        }

        //private void Window_ClientSizeChanged(object sender, EventArgs e)
        //{
        //    Resize();
        //}

        //public void AddChild(UI child)
        //{
        //    Children.Add(child);
        //    OnResize += child.Resize;
        //}

        //public override void Resize()
        //{
        //    SetHitbox(Size);
        //    if (OnResize is not null) OnResize();
        //}

        public override void Draw(GameTime gameTime)
        {
            int i = 1;
        }

        #region Implement - IUserInterfaceCollection

        

        //private event IUICollection.ResizeMethod OnResize;

        //public void AddChildrenResize(IUICollection.ResizeMethod resizeMethod) => OnResize += resizeMethod;
        //public void RemoveChildrenResize(IUICollection.ResizeMethod resizeMethod) => OnResize -= resizeMethod;
        //public void InvokeChildrenResize() => OnResize?.Invoke();

        #endregion
    }
}
