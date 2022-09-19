

namespace Mythonia.Game
{
    public enum KeyName
    {
        Left,
        Right,
        Up,
        Down,
        Jump,
        Attack,
        Debug,
    }

    public class Input : GameComponent
    {

        #region Props

        public KeyCodeList KeyCode { get; init; } = new(new Keys[] {
                Keys.Left,
                Keys.Right,
                Keys.Up,
                Keys.Down,
                Keys.X,
                Keys.Z,
                Keys.Space
            });

        private readonly int[] _keyStates;

        public int this[KeyName keyName] => _keyStates[(int)keyName];

        #endregion



        #region Constructor

        public Input(MGame game/*, Keys[] keyCode*/) : base(game)
        {
            //KeyCode = new(keyCode);
            _keyStates = new int[KeyCode.Length];
        }

        #endregion



        #region Methods

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            KeyboardState key = Keyboard.GetState();

            for (int i = 0; i < _keyStates.Length; i++)
            {
                if (key.IsKeyDown(KeyCode[(KeyName)i]))
                {
                    if (_keyStates[i] < 0) _keyStates[i] = 0;

                    _keyStates[i] += 1;//(int)gameTime.CFDuration();
                }
                else
                {
                    if (_keyStates[i] > 0) _keyStates[i] = 0;

                    _keyStates[i] -= 1;//(int)gameTime.CFDuration();
                };
            };
        }

        /// <summary>
        /// 按键是否被按住
        /// </summary>
        public bool KeyDown(KeyName keyName) => this[keyName] > 0;

        /// <summary>
        /// 按键是否处于按下的一瞬间
        /// </summary>
        public bool KeyPress(KeyName keyName) => this[keyName] == 1;

        /// <summary>
        /// 按键是否没有被按下
        /// </summary>
        public bool KeyUp(KeyName keyName) => this[keyName] < 0;

        /// <summary>
        /// 按键是否处于松开的一瞬间
        /// </summary>
        public bool KeyRelease(KeyName keyName) => this[keyName] == -1;

        #endregion

    }

    public class KeyCodeList
    {
        private readonly Keys[] _keyCodes;

        public Keys this[KeyName keyName] { get => _keyCodes[(int)keyName]; set => _keyCodes[(int)keyName] = value; }

        public int Length => _keyCodes.Length;

        public KeyCodeList(Keys[] keys) => _keyCodes = keys;
    }
}
