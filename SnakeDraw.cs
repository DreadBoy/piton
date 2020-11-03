using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace piton
{
    public class SnakeDraw
    {
        private readonly int _gridSize;

        public SnakeDraw(int gridSize)
        {
            _gridSize = gridSize;
        }

        private (int, int) tail = (0, 0);
        private (int, int) middle = (1, 0);
        private (int, int) cornerBR = (0, 1);
        private (int, int) cornerBL = (1, 1);
        private (int, int) cornerTR = (2, 1);
        private (int, int) cornerTL = (3, 1);
        private (int, int) head = (2, 0);
        private (int, int) food = (3, 0);

        public (int x, int y) GetSprite(List<int> snake, int index)
        {
            if (index < 0 || index >= snake.Count)
                throw new ArgumentOutOfRangeException("index");
            var (isHead, isTail, fromLeft, toRight, fromRight, toLeft, fromTop, toBottom, fromBottom, toTop) =
                GetSides(snake, index);
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

        public int GetRotation(List<int> snake, int index)
        {
            if (index < 0 || index >= snake.Count)
                throw new ArgumentOutOfRangeException("index");
            var (isHead, isTail, fromLeft, toRight, fromRight, toLeft, fromTop, toBottom, fromBottom, toTop) =
                GetSides(snake, index);
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

        private (
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
            ) GetSides(List<int> snake, int index)
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
            if (index == snake.Count - 1)
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