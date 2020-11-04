using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace piton
{
    public class GameLogic
    {
        private readonly int _gridSize;
        private readonly Sides _sides;
        private readonly Random _random;

        public GameLogic(int gridSize)
        {
            _gridSize = gridSize;
            _sides = new Sides(gridSize);
            _random = new Random();
        }

        public (int[] snake, Keys[] unusedKeys, int[] trail) MoveSnake(Keys[] keys, int[] snake, int[] trail)
        {
            var unusedKeys = new Keys[0];
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
            for (int i = 0; i < keys.Length; i++)
            {
                if (i < keys.Length - 1)
                {
                    var twoKeys = keys.Skip(i).Take(2).ToArray();
                    if (twoKeys.Contains(Keys.A) && twoKeys.Contains(Keys.D) ||
                        twoKeys.Contains(Keys.W) && twoKeys.Contains(Keys.S))
                    {
                        i++;
                        continue;
                    }

                    if (twoKeys[0] == twoKeys[1])
                    {
                        i++;
                        continue;
                    }
                }

                newHead = keys[i] switch
                {
                    Keys.W when !fromTop => snake[0] - _gridSize,
                    Keys.S when !fromBottom => snake[0] + _gridSize,
                    Keys.A when !fromLeft => snake[0] - 1,
                    Keys.D when !fromRight => snake[0] + 1,
                    _ => newHead
                };

                unusedKeys = keys.Skip(i + 1).ToArray();
                break;
            }

            var lastPiece = snake.Last();
            // Complete movement
            for (var i = snake.Length - 1; i >= 1; i--)
            {
                snake[i] = snake[i - 1];
            }

            snake[0] = newHead;

            // TODO should you really limit trail to 10?
            return (snake, unusedKeys, new[] {lastPiece}.Concat(trail).Take(10).ToArray());
        }

        public int SpawnFood(int[] snake)
        {
            var stillEmpty = Enumerable.Range(0, _gridSize * _gridSize).ToList();
            while (stillEmpty.Count > 0)
            {
                var emptyIndex = _random.Next(0, stillEmpty.Count);
                if (snake.Contains(stillEmpty[emptyIndex]))
                    stillEmpty.RemoveAt(emptyIndex);
                else
                    return stillEmpty[emptyIndex];
            }

            return -2;
        }

        public int EatFood(int food, int[] snake)
        {
            if (food == snake[0]) return -1;
            return food;
        }

        public (int[] snake, int[] trail) LengthenSnake(int[] snake, int[] trail)
        {
            if (trail.Length < 1)
                return (snake, trail);
            return (snake.Append(trail.First()).ToArray(), trail.Skip(1).ToArray());
        }
    }
}