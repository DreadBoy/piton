using System.Collections.Generic;

namespace piton
{
    public class Sides
    {
        private readonly int _gridSize;

        public Sides(int gridSize)
        {
            _gridSize = gridSize;
        }

        public (
            bool isHead,
            bool isTail,
            bool fromLeft,
            bool toRight,
            bool fromRight,
            bool toLeft,
            bool fromTop,
            bool toBottom,
            bool fromBottom,
            bool toTop
            ) GetSides(int[] snake, int index)
        {
            if (index == 0)
                return (
                    true,
                    false,
                    snake[0] - 1 == snake[1],
                    false,
                    snake[0] + 1 == snake[1],
                    false,
                    snake[0] - _gridSize == snake[1],
                    false,
                    snake[0] + _gridSize == snake[1],
                    false
                );
            if (index == snake.Length - 1)
                return (
                    false,
                    true,
                    false,
                    snake[index] + 1 == snake[index - 1],
                    false,
                    snake[index] - 1 == snake[index - 1],
                    false,
                    snake[index] + _gridSize == snake[index - 1],
                    false,
                    snake[index] - _gridSize == snake[index - 1]
                );
            return (
                false,
                false,
                snake[index] - 1 == snake[index + 1],
                snake[index] + 1 == snake[index - 1],
                snake[index] + 1 == snake[index + 1],
                snake[index] - 1 == snake[index - 1],
                snake[index] - _gridSize == snake[index + 1],
                snake[index] + _gridSize == snake[index - 1],
                snake[index] + _gridSize == snake[index + 1],
                snake[index] - _gridSize == snake[index - 1]
            );
        }
    }
}