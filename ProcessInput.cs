using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace piton
{
    public class ProcessInput
    {
        private readonly List<Keys> _batchedKeys = new List<Keys>();
        private KeyboardState _previousState;
        private readonly Keys[] _allKeys = {Keys.W, Keys.S, Keys.A, Keys.D};

        public void BatchInput()
        {
            var state = Keyboard.GetState();
            foreach (var key in _allKeys)
                if (state.IsKeyDown(key) && !_previousState.IsKeyDown(key))
                    _batchedKeys.Add(key);

            _previousState = state;
        }

        public Keys[] CollectInput()
        {
            var keys = _batchedKeys.ToArray();
            _batchedKeys.Clear();
            return keys;
        }

        public void ReturnUnusedKeys(Keys[] unusedKeys)
        {
            _batchedKeys.InsertRange(0, unusedKeys);
        }
    }
}