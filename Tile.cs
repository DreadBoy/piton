using Microsoft.Xna.Framework;

namespace piton
{
    public class Tile
    {
        public Vector2 sprite;

        public Tile(Vector2 sprite)
        {
            this.sprite = sprite;
        }
    }

    public class SnakeHead : Tile
    {
        public SnakeHead() : base(new Vector2(2, 0))
        {
        }
    }

    public class SnakeTail : Tile
    {
        public SnakeTail() : base(new Vector2(0, 0))
        {
        }
    }

    public class SnakeBody : Tile
    {
        public SnakeBody() : base(new Vector2(1, 0))
        {
        }
    }

    public class Grass : Tile
    {
        public Grass() : base(new Vector2(4, 0))
        {
        }
    }
}