﻿




namespace Mythonia.Game.Sprites.Sceneries
{
    public class PressurePlate : Entity
    {
        public bool Activate { get; private set; } = false;
        private sbyte _aniCount = -1;
        private float _originPosY;

        public override Rectangle TextureSourceRange => ((MTexture)Texture).Frames["Plate"].Range;

        public RectangleHitboxEvented HitboxEvented => (RectangleHitboxEvented)Hitbox;
        public PressurePlate(MGame game, Map map, MVec2 position) 
            : base("PressurePlate", game, map, game.TextureManager["PressurePlate"], position.Change(y: 5))
        {
            _originPosY = Position.Y;
            Hitbox = new RectangleHitboxEvented(MGame, () => (MVec2)Position, (24, 6), IHitbox.Types.Rigid);
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
            if(_aniCount >= 0)
            {
                _aniCount ++;
                _position.Y = _originPosY - MathF.Sin((_aniCount - StartTime) * MathF.PI / (2 * (EndTime - StartTime))) * Distance;
                if (_aniCount >= EndTime) _aniCount = -1;
            }
        }
    }


    public class PressurePlateBase : Entity
    {
        public bool Activate { get; private set; } = false;

        public override Rectangle TextureSourceRange => _textureSourceRange;
        public Rectangle _textureSourceRange;

        private sbyte _aniCount = -1;

        public PressurePlate PressurePlate { get; init; }

        public RectangleHitboxEvented HitboxEvented => (RectangleHitboxEvented)Hitbox;
        public PressurePlateBase(MGame game, Map map, MVec2 position) 
            : base("PressurePlate", game, map, game.TextureManager["PressurePlate"], position.ChangeNew(y: -3))
        {
            PressurePlate = new(game, map, position);
            game.Components.Add(PressurePlate);
            position.Change(y: -3);
            _textureSourceRange = ((MTexture)Texture).Frames["BaseUnActivate"].Range;
            Hitbox = new RectangleHitbox(MGame, () => position, (32, 10), IHitbox.Types.Rigid);
            PressurePlate.HitboxEvented.WhenBeStepped += PressurePlate_HitboxEvented_WhenBeStepped;
        }

        private void PressurePlate_HitboxEvented_WhenBeStepped(Sprite s)
        {
            if (!Activate)
            {
                Activate = true;
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
                    _textureSourceRange = ((MTexture)Texture).Frames["BaseActivate"].Range;
                    _aniCount = -1;
                }
            }
        }
    }
}
