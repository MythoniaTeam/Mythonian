



namespace Mythonia.Game.UserInterfaces
{
    public class HealthBar : UICollection
    {

        public static int Length = 66;



        #region Implement - IUserInterfaceCollection

        

        //private event IUICollection.ResizeMethod OnResize;

        //public void AddChildrenResize(IUICollection.ResizeMethod resizeMethod) => OnResize += resizeMethod;
        //public void RemoveChildrenResize(IUICollection.ResizeMethod resizeMethod) => OnResize -= resizeMethod;
        //public void InvokeChildrenResize() => OnResize?.Invoke();

        #endregion

        Border_ Border { get; init; }
        Bar_ Bar { get; init; }
        HeartIcon_ HeartIcon { get; init; }



        public HealthBar(MGame game, UICollection father, Func<float> getHealthMethod, Func<float> getMaxHealthMethod)
            : base("HealthBar", game, father, null, 
                  size: MTextureManager.Ins.Get<MTexture>("UI/HealthBar_Border").Size,
                  origin: MVec2.TopLeft, posRatio: MVec2.TopLeft, posDisplacement: MVec2.BottomRight * (20, 12))
        {
            Length = (int)MTextureManager.Ins.Get<MTexture>("UI/HealthBar_Border").Size.X - 20;

            //Transform.OriginRatio
            //Texture.OriginRatio = MVec2.TopLeft;
            _scale = (1, 1);

            Border = new Border_(MGame, this);
            Bar = new Bar_(MGame, this, getHealthMethod, getMaxHealthMethod);
            HeartIcon = new HeartIcon_(MGame, this);

            Children.Add(Border);
            Children.Add(Bar);
            Children.Add(HeartIcon);

            MGame.Components.Add(Border);
            MGame.Components.Add(Bar);
            MGame.Components.Add(HeartIcon);

            //Father.RemoveChildrenResize(Resize);
            //Border = new(game, this);
            //AddChildrenResize(Resize);
            //HeartIcon = new(game, this);

            //Resize();
            //InvokeChildrenResize();
        }

        public override void Update(GameTime gameTime)
        {
            //var originalSizeX = Rect.Size.X;
            //_posDisplacement.X = + Rect.Size.X / 2;

        }
        /*public override void Draw(GameTime gameTime)
        {
            Border.Draw(gameTime);
            base.Draw(gameTime);
            HeartIcon.Draw(gameTime);
        }*/

        //public override void Resize()
        //{
        //}


        public class Bar_ : UI
        {
            public HealthBar HealthBar { get; init; }

            public Func<float> GetHealthMethod { get; init; }
            public Func<float> GetMaxHealthMethod { get; init; }

            public Bar_(MGame game, HealthBar healthBar, Func<float> getHealthMethod, Func<float> getMaxHealthMethod)
                : base("HealthBar_Bar", game, healthBar, MTextureManager.Ins.Get<NineSliceTexture>("UI/HealthBar"),
                      origin: MVec2.Left, posRatio: MVec2.Left, posDisplacement: (10, 0))
            {
                //((NineSliceTexture)Texture).BoundedUI = this;

                HealthBar = healthBar;

                GetHealthMethod = getHealthMethod;
                GetMaxHealthMethod = getMaxHealthMethod;
            }

            public override void Update(GameTime gameTime)
            {
                base.Update(gameTime);
                Size = (GetHealthMethod() / GetMaxHealthMethod() * (Length - 2) + 2, Rect.Size.Y);

            }
        }

        public class Border_ : UI
        {
            public HealthBar HealthBar { get; init; }

            public Border_(MGame game, HealthBar healthBar)
                : base("HealthBar_Border", game, healthBar, MTextureManager.Ins.Get<MTexture>("UI/HealthBar_Border"),
                      origin: MVec2.Left, posRatio: MVec2.Left)
            {
                HealthBar = healthBar;
            }


            //public override void Resize()
            //{
            //    _position = Father.ToUI.Father.ToUI.TopLeft + (10, -15);
            //}
        }

        public class HeartIcon_: UI
        {
            public HealthBar HealthBar => (HealthBar)Father;
            public HeartIcon_(MGame game, HealthBar healthBar)
                : base("HealthBar_Heart", game, healthBar, MTextureManager.Ins.Get<MTexture>("UI/HealthBar_Heart").PlayAnimation(),
                      origin: MVec2.Center, posRatio: MVec2.Left)
            {
            }

            //public override void Resize()
            //{
            //    _position = HealthBar.Border.Position.ChangeNew(x: -32);
            //}
        }
    }
}
