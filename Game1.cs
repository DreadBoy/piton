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

        private const int TextureSize = 384;
        private const int TileSize = 32;
        private const int GridSize = 30;
        private const int UpdateInterval = 50;

        private int _timer;
        private readonly (int, int) _grass = (4, 0);
        private int[] _snake;
        private int _food;
        private readonly SnakeDraw _snakeDraw = new SnakeDraw(GridSize);
        private readonly ProcessInput _processInput = new ProcessInput();
        private readonly GameLogic _gameLogic = new GameLogic(GridSize);


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _snake = new[]
            {
                GridSize * 4 + 7,
                GridSize * 4 + 6,
                GridSize * 4 + 5,
                GridSize * 4 + 4,
                GridSize * 4 + 3,
                GridSize * 4 + 2,
                GridSize * 3 + 2,
                GridSize * 2 + 2,
                GridSize + 2,
                GridSize + 1,
                GridSize,
            };
            _food = _gameLogic.SpawnFood(_snake);
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

            _processInput.BatchInput();

            _timer += gameTime.ElapsedGameTime.Milliseconds;
            if (_timer > UpdateInterval)
            {
                _timer -= UpdateInterval;
                FixedUpdate();
            }

            base.Update(gameTime);
        }

        private void FixedUpdate()
        {
            var keys = _processInput.CollectInput();
            _snake = _gameLogic.MoveSnake(keys, _snake.ToArray(), out var unusedKeys);
            _processInput.ReturnUnusedKeys(unusedKeys);

            _food = _gameLogic.EatFood(_food, _snake);

            if (_food == -1)
                _food = _gameLogic.SpawnFood(_snake);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            for (var i = 0; i < GridSize * GridSize; i++)
                // ReSharper disable once PossibleLossOfFraction
                DrawTile(new Vector2(i % GridSize, i / GridSize), _grass);

            // ReSharper disable once PossibleLossOfFraction
            if (_food >= 0)
                DrawTile(new Vector2(_food % GridSize, _food / GridSize), _snakeDraw.food);

            for (var i = 0; i < _snake.Length; i++)
                // ReSharper disable once PossibleLossOfFraction
                DrawTile(new Vector2(_snake[i] % GridSize, _snake[i] / GridSize),
                    _snakeDraw.GetSprite(_snake, i), _snakeDraw.GetRotation(_snake, i) * (float) Math.PI / 2);
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawTile(Vector2 position, (int x, int y) source, float rotation = 0)
        {
            _spriteBatch.Draw(_spritesheet, position * TileSize + Vector2.One * TileSize * 0.5f,
                new Rectangle(TextureSize * source.x, 385 * source.y, TextureSize, TextureSize),
                Color.White, rotation, Vector2.One * 0.5f * TextureSize, Vector2.One / TextureSize * TileSize,
                SpriteEffects.None, 0);
        }
    }
}