using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace piton
{
    public class Game1 : Game
    {
        private Texture2D _spritesheet;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private const int TileSize = 32;
        private const int GridSize = 30;

        private List<Tile> _grid;
        private readonly Grass _grass = new Grass();


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _grid = Enumerable.Repeat<Tile>(null, GridSize * GridSize).ToList();
            _grid[GridSize] = new SnakeTail();
            _grid[GridSize + 1] = new SnakeBody();
            _grid[GridSize + 2] = new SnakeHead();
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = TileSize * GridSize;
            _graphics.PreferredBackBufferWidth = TileSize * GridSize;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _spritesheet = Content.Load<Texture2D>("Textures");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            for (int i = 0; i < _grid.Count; i++)
            {
                DrawTile(new Vector2(i % GridSize, (int) Math.Floor((double) i / GridSize)), _grass.sprite);
                if (_grid[i] != null)
                    DrawTile(new Vector2(i % GridSize, (int) Math.Floor((double) i / GridSize)), _grid[i].sprite);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawTile(Vector2 position, Vector2 source)
        {
            _spriteBatch.Draw(_spritesheet, position * TileSize,
                new Rectangle(384 * (int) source.X, 385 * (int) source.Y, 384, 384),
                Color.White, 0, Vector2.Zero, Vector2.One / 384f * TileSize,
                SpriteEffects.None, 0);
        }
    }
}