using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace piton
{
    public class GameLogic
    {
        private readonly int _gridSize;
        private readonly Sides _sides;

        public GameLogic(int gridSize)
        {
            _gridSize = gridSize;
            _sides = new Sides(gridSize);
        }

        public int[] MoveSnake(IEnumerable<Keys> keys, int[] snake)
        {
            var (_, _, fromLeft, _, fromRight, _, fromTop, _, fromBottom, _) =
                _sides.GetSides(snake, 0);

            int newHead;
            // Follow regular movement
            if (fromLeft) newHead = snake[0] + 1;
            else if (fromTop) newHead = snake[0] + _gridSize;
            else if (fromRight) newHead = snake[0] - 1;
            else if (fromBottom) newHead = snake[0] - _gridSize;
            else throw new Exception();

            // If player pressed any keys, turn the snake
            foreach (var key in keys)
                newHead = key switch
                {
                    Keys.W when !fromTop => snake[0] - _gridSize,
                    Keys.S when !fromBottom => snake[0] + _gridSize,
                    Keys.A when !fromLeft => snake[0] - 1,
                    Keys.D when !fromRight => snake[0] + 1,
                    _ => newHead
                };

            // Complete movement
            for (var i = snake.Length - 1; i >= 1; i--)
            {
                snake[i] = snake[i - 1];
            }

            snake[0] = newHead;
            return snake;
        }
    }
}