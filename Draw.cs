using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace piton
{
    public class Draw
    {
        private readonly Sides _sides;
        private readonly int _textureSize;
        private readonly int _tileSize;
        private readonly SpriteBatch _spriteBatch;
        private readonly Texture2D _spritesheet;

        public Draw(SpriteBatch spriteBatch, Texture2D spritesheet, int gridSize, int textureSize, int tileSize)
        {
            _sides = new Sides(gridSize);
            _textureSize = textureSize;
            _tileSize = tileSize;
            _spritesheet = spritesheet;
            _spriteBatch = spriteBatch;
        }

        public (int, int) tail = (0, 0);
        public (int, int) middle = (1, 0);
        public (int, int) cornerBR = (0, 1);
        public (int, int) cornerBL = (1, 1);
        public (int, int) cornerTR = (2, 1);
        public (int, int) cornerTL = (3, 1);
        public (int, int) head = (2, 0);
        public (int, int) food = (3, 0);

        public (int x, int y) GetSprite(int[] snake, int index)
        {
            if (index < 0 || index >= snake.Length)
                throw new ArgumentOutOfRangeException("index");
            var (isHead, isTail, fromLeft, toRight, fromRight, toLeft, fromTop, toBottom, fromBottom, toTop) =
                _sides.GetSides(snake, index);
            if (isHead)
                return head;
            if (isTail)
                return tail;

            if (fromLeft && toRight || fromRight && toLeft || fromTop && toBottom || fromBottom && toTop)
                return middle;
            if (fromBottom && toRight || fromRight && toBottom)
                return cornerBR;
            if (fromBottom && toLeft || fromLeft && toBottom)
                return cornerBL;
            if (fromTop && toRight || fromRight && toTop)
                return cornerTR;
            if (fromTop && toLeft || fromLeft && toTop)
                return cornerTL;
            throw new ArgumentOutOfRangeException("index");
        }

        public int GetRotation(int[] snake, int index)
        {
            if (index < 0 || index >= snake.Length)
                throw new ArgumentOutOfRangeException("index");
            var (isHead, isTail, fromLeft, toRight, fromRight, toLeft, fromTop, toBottom, fromBottom, toTop) =
                _sides.GetSides(snake, index);
            if (isHead)
            {
                if (fromLeft) return 0;
                if (fromTop) return 1;
                if (fromRight) return 2;
                if (fromBottom) return 3;
            }

            if (isTail)
            {
                if (toRight) return 0;
                if (toBottom) return 1;
                if (toLeft) return 2;
                if (toTop) return 3;
            }

            return 0;
        }

        private Effect _currentEffect;

        public void Begin()
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            _currentEffect = null;
        }

        public void End()
        {
            _spriteBatch.End();
        }

        public void DrawTile(Vector2 position, (int x, int y) source, float rotation = 0, Effect effect = null)
        {
            if (effect != _currentEffect)
            {
                _currentEffect = effect;
                _spriteBatch.End();
                _spriteBatch.Begin(
                    SpriteSortMode.Deferred,
                    BlendState.AlphaBlend,
                    SamplerState.PointClamp,
                    effect: _currentEffect
                );
            }

            _spriteBatch.Draw(_spritesheet, position * _tileSize + Vector2.One * _tileSize * 0.5f,
                new Rectangle(_textureSize * source.x, 385 * source.y, _textureSize, _textureSize),
                Color.White, rotation, Vector2.One * 0.5f * _textureSize, Vector2.One / _textureSize * _tileSize,
                SpriteEffects.None, 0);
        }
    }
}