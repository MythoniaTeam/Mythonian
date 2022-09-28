



namespace Mythonia.Game.UserInterfaces
{
    public class UICollection : UI//<T> where T : UserInterface, IUserInterfaceCollection<T>
    {
        //public T ToUserInterface => (T)this;

        public UI this[string name] => Children[name];

        // public UI ToUI => (UI)this;


        public UICollection(string name, MGame game, UICollection father, ITexture texture, MVec2? origin = null,
            MVec2? posRatio = null, MVec2? posDisplacement = null, MVec2? size = null, 
            Transform? transform = null)
            : base(name, game, father, texture,
                  origin: origin,
                  posRatio: posRatio,
                  posDisplacement: posDisplacement,
                  size: size,
                  transform: transform)
        {

        }

        /// <summary>
        /// 将子对象绑定到自身上，
        /// <para>
        /// <list type="bullet">
        /// <item>会绑定 <paramref name="child"/> 的 <see cref="UI.Resize"/> 方法</item>
        /// <item>如果 <paramref name="child"/> 也是 <see cref="UICollection"/>，<br/>
        /// 会绑定 <paramref name="child"/> 的 <see cref="InvokeChildrenResize"/> (用于 激活 OnResize 事件 的方法) </item>
        /// </list>
        /// </para>
        /// <para>
        /// <b>调用:</b><br/>
        /// 在 <see cref="UI(string, MGame, UICollection, ITexture, MVec2?, Transform)"/> 构造函数中，<br/>
        /// 如果 <see cref="UI.Father"/> 不为 <see langword="null"/> 时自动调用，将自身添加到父对象中
        /// </para>
        /// </summary>
        /// <param name="child"></param>
        public void AddChild(UI child)
        {
            Children.Add(child);
            //AddChildrenResize(child.Resize);
            //if (child is IUICollection collection) AddChildrenResize(collection.InvokeChildrenResize);
            //InvokeResize();
        }
        public NamedList<UI> Children { get; init; } = new();

        /*public delegate void ResizeMethod();
        public void AddChildrenResize(ResizeMethod resizeMethod);
        public void RemoveChildrenResize(ResizeMethod resizeMethod);
        public void InvokeChildrenResize();*/

    }
}
