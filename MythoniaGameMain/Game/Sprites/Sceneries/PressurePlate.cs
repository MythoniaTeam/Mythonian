




namespace Mythonia.Game.Sprites.Sceneries
{

    public class PressurePlate : Entity
    {
        public bool Activate { get; private set; } = false;


        private sbyte _aniCount = -1;

        public TopPlate Plate { get; init; }

        public override IHitbox Hitbox => HitboxRect;
        public RectangleHitbox HitboxRect { get; set; }


        public event HitEvent OnActivate;

        public MTexture MTexture => (MTexture)Texture;

        public PressurePlate(MGame game, Map map, MVec2 position) 
            : base("PressurePlate", EntityType.Scenery, game, map, game.TextureManager["PressurePlate"], new(position.ChangeNew(y: -3)))
        {
            Plate = new(game, map, position);
            game.Components.Add(Plate);
            position.Change(y: -3);
            MTexture.PlayFrame("BaseInactive");
            HitboxRect = new RectangleHitbox(MGame, () => position, () => (32, 10), IHitbox.Types.Rigid);
            Plate.HitboxEvented.WhenBeStepped += PressurePlate_HitboxEvented_WhenBeStepped;
        }

        private void PressurePlate_HitboxEvented_WhenBeStepped(Sprite s)
        {
            if (!Activate)
            {
                Activate = true;
                if(OnActivate is not null) OnActivate(s);
                _aniCount = 0;
            }
        }

        private const byte ChangeLightTime = 10;
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (_aniCount >= 0)
            {
                _aniCount++;
                if(_aniCount == ChangeLightTime)
                {
                    MTexture.PlayFrame("BaseActive");
                    _aniCount = -1;
                }
            }
        }




        public class TopPlate : Entity
        {
            public bool Activate { get; private set; } = false;
            private sbyte _aniCount = -1;
            private readonly float _originPosY;


            public override IHitbox Hitbox => HitboxEvented;
            public RectangleHitboxEvented HitboxEvented { get; set; }

            public TopPlate(MGame game, Map map, MVec2 position)
                : base("PressurePlate", EntityType.Scenery, game, map, game.TextureManager["PressurePlate"].PlayFrame("Plate"), new(position.Change(y: 5)))
            {
                _originPosY = Position.Y;
                HitboxEvented = new RectangleHitboxEvented(MGame, () => (MVec2)Position, () => (24, 6), IHitbox.Types.Rigid);
                HitboxEvented.WhenBeStepped += HitboxEvented_WhenBeStepped;

                TextManager.Ins.WriteLine(() => $"Pressure Plate Activate: {Activate}");
            }

            private void HitboxEvented_WhenBeStepped(Sprite s)
            {
                if (!Activate)
                {
                    Activate = true;
                    _aniCount = 0;
                }
            }

            private const byte StartTime = 1;
            private const byte EndTime = 12;
            private const byte Distance = 4;
            public override void Update(GameTime gameTime)
            {
                base.Update(gameTime);
                if (_aniCount >= 0)
                {
                    _aniCount++;
                    _position.Y = _originPosY - MathF.Sin((_aniCount - StartTime) * MathF.PI / (2 * (EndTime - StartTime))) * Distance;
                    if (_aniCount >= EndTime) _aniCount = -1;
                }
            }
        }
    }
}
