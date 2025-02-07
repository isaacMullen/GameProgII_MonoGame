using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GameProgII_MonoGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Player _player;
        Pipe _pipe;
        SpawnManager _spawnManager;

        public List<Pipe> pipes;

        public Game1()
        {
            pipes = new List<Pipe>();

            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();

            _player = new Player();            
            _spawnManager = new SpawnManager();                                
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
            
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Loading in the player texture and its starting position            
            Vector2 startingPosition = new Vector2(400, 200);

            // Loading the pipe texture            
            Vector2 startingPipePosition = new Vector2(_graphics.PreferredBackBufferWidth, 300);

            // Initial load of the player with the correct texture
            _player.LoadContent(startingPosition, Content);
           // _player.LoadFont(Content);

            // Loading content for each pipe in the list
            foreach (Pipe pipe in pipes)
            {
                pipe.LoadContent(startingPipePosition, Content);  // Ensure the texture is loaded
            }
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _spawnManager.Update(gameTime, pipes, Content); // Pass the ContentManager here

            _player.Update(gameTime, pipes);

            foreach (var pipe in pipes)
            {
                if (_player.CheckCollision(pipe.BottomBoundingBox, pipe.TopBoundingBox))
                {
                    Console.WriteLine("Collision Detected");
                    Exit();
                }
            }

            for (int i = 0; i < pipes.Count; i++)
            {
                _spawnManager.Despawn(pipes[i], pipes);
                pipes[i].Update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            foreach (var pipe in pipes)
            {
                pipe.Draw(_spriteBatch);
            }
            _player.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }        
    }
}
