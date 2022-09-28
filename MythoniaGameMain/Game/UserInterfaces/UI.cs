



namespace Mythonia.Game.UserInterfaces
{
    public abstract class UI : Sprite
    {
        #region Transform
        public RectangleHitbox Rect { get; protected set; }

        public MVec2 GetPosByRatio(MVec2 ratio) => (MVec2)Position - OriginDisplacement + Size / 2 * ratio;
        public MVec2 GetPosByRatio(float x, float y) => GetPosByRatio((x, y));

        public MVec2 TopLeft     => GetPosByRatio(-1,  1);
        public MVec2 Top         => GetPosByRatio( 0,  1);
        public MVec2 TopRight    => GetPosByRatio( 1,  1);
        public MVec2 Left        => GetPosByRatio(-1,  0);
        public MVec2 Center      => GetPosByRatio( 0,  0);
        public MVec2 Right       => GetPosByRatio( 1,  0);
        public MVec2 BottomLeft  => GetPosByRatio(-1, -1);
        public MVec2 Bottom      => GetPosByRatio( 0, -1);
        public MVec2 BottomRight => GetPosByRatio( 1, -1);

        public override MVec3 Position { get => Father.GetPosByRatio(_posRatio) + _posDisplacement; /*set => throw new Exception("The Pos Prop of UI could not be set");*/ }

        protected MVec2 _posRatio;
        protected MVec2 _posDisplacement;
        //private MVec2 _size;
        public UITransform UITransform
        {
            get => new(_posRatio, _posDisplacement, Size);
            set => (_posRatio, _posDisplacement, Size) = value.ToTuple;
        }
        public virtual MVec2 Size
        {
            get => _size;
            set
            {
                _size = value;
            } 
        }
        private MVec2 _size;


        /// <summary>应该通过属性 <see cref="OriginRatio"/> 访问该字段</summary>
        private MVec2? _originRatio = null;
        public MVec2 OriginRatio 
        { 
            get => _originRatio ?? Texture?.OriginRatio ?? MVec2.Center;
            set
            {
                if(Texture is not null) Texture.OriginRatio = value;
                _originRatio = value;
            }
        }
        public MVec2 OriginDisplacement => OriginRatio * Size / 2;



        //private Func<MVec2> _getReferencePointMethod;
        

        #endregion



        public UICollection Father { get; init; }

        /// <summary>
        /// <see cref="DrawableGameComponent.DrawOrder"/> == 10000
        /// <para>
        /// 会按照 <paramref name="size"/> 调用 <see cref="SetHitbox(MVec2?)"/>，如果为 <see langword="null"/> 采用贴图尺寸<br/>
        /// 会自动设置 <see cref="Father"/>，如果不为 <see langword="null"/> 自动将 <see langword="this"/> 添加到父对象 (调用 <see cref="IUICollection.AddChild(UI)"/>)
        /// </para>
        /// <para>
        /// <b><see cref="IUICollection.AddChild(UI)"/>:</b><br/>
        /// <list type="bullet">
        /// <item>会绑定 <paramref name="child"/> 的 <see cref="UI.Resize"/> 方法</item>
        /// <item>如果 <paramref name="child"/> 也是 <see cref="IUICollection"/>，<br/>
        /// 会绑定 <paramref name="child"/> 的 <see cref="InvokeResize"/> (用于 激活 OnResize 事件 的方法) </item>
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="game"></param>
        /// <param name="father"></param>
        /// <param name="texture"></param>
        /// <param name="size"></param>
        /// <param name="transform"></param>
        public UI(string name, MGame game, UICollection father, ITexture texture, MVec2? origin = null, 
            MVec2? posRatio = null, MVec2? posDisplacement = null, MVec2? size = null, 
            /*UITransform? uiTransform = null,*/ Transform? transform = null) 
            : base(name, game, texture, transform: transform)
        {
            if (origin is MVec2 origin2)
            {
                 if (Texture is not null) Texture.OriginRatio = origin2;
                _originRatio = origin2;
            }

            try
            {
                UITransform = new(size ?? Texture.Size, posRatio, posDisplacement);
            }
            catch (NullReferenceException)
            {
                throw new Exception($"The object \"{Name}\" has neither Texture nor Size parameter");
            }

            DrawOrder = 10000;
            SetHitbox();

            Father = father;
            if(Father is not null) Father.AddChild(this);

            if(Texture is NineSliceTexture nineSlice) nineSlice.BoundedUI = this;

            //Resize();
            
        }

        //private void Window_ClientSizeChanged(object sender, EventArgs e)
        //{
        //    Resize();
        //}

        public virtual void SetHitbox()
        {
            Rect = new(MGame, () => (MVec2)Position, () => Size, IHitbox.Types.UI);
        }

        //public abstract void Resize();


        public override void Draw(GameTime gameTime)
        {
            //base.Draw(gameTime);
            DrawTexture(MGame.SpriteBatch);
        }
    }
}
