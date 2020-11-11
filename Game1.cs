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
        private Effect _allWhiteEffect;

        private const int TextureSize = 384;
        private const int TileSize = 32;
        private const int GridSize = 30;
        private const int UpdateInterval = 50;

        private int _timer;
        private readonly (int, int) _grass = (4, 0);
        private int[] _trail;
        private int[] _snake;
        private int _food;
        // TODO food should disappear after timeout
        // TODO what about multiple foods?
        private Draw _draw;
        private readonly ProcessInput _processInput = new ProcessInput();
        private readonly GameLogic _gameLogic = new GameLogic(GridSize);


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _trail = new int[0];
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

            _spritesheet = Content.Load<Texture2D>("Textures");
            _allWhiteEffect = Content.Load<Effect>("AllWhite");

            _draw = new Draw(_spriteBatch, _spritesheet, GridSize, TextureSize, TileSize);
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
            var (snake, unusedKeys, trail) = _gameLogic.MoveSnake(keys, _snake, _trail);
            _snake = snake;
            _trail = trail;
            _processInput.ReturnUnusedKeys(unusedKeys);

            _food = _gameLogic.EatFood(_food, _snake);

            if (_food == -1)
            {
                (_snake, _trail) = _gameLogic.LengthenSnake(_snake, _trail);
                _food = _gameLogic.SpawnFood(_snake);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _draw.Begin();
            for (var i = 0; i < GridSize * GridSize; i++)
                // ReSharper disable once PossibleLossOfFraction
                _draw.DrawTile(new Vector2(i % GridSize, i / GridSize), _grass);
            
            // ReSharper disable once PossibleLossOfFraction
            if (_food >= 0)
                _draw.DrawTile(new Vector2(_food % GridSize, _food / GridSize), _draw.food);
            
            for (var i = 0; i < _snake.Length; i++)
                // ReSharper disable once PossibleLossOfFraction
                _draw.DrawTile(new Vector2(_snake[i] % GridSize, _snake[i] / GridSize),
                    _draw.GetSprite(_snake, i), _draw.GetRotation(_snake, i) * (float) Math.PI / 2);

            // _draw.DrawTile(new Vector2(2, 2), _draw.food, effect: _allWhiteEffect);
            
            _draw.End();
            base.Draw(gameTime);
        }
    }
}