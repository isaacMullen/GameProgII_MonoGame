using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Reflection.Metadata;


namespace GameProgII_MonoGame
{
    public class Player
    {
        public Texture2D Texture { get; private set; }
        public Vector2 Position { get; private set; }
        private float spriteScale = 1f;

        private float jumpTime = 0;

        private float gravityMod = 175f;
        private float jumpForce = 400f;

        private bool gravity = true;

        public void LoadContent(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState keyBoardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (keyBoardState.IsKeyDown(Keys.Space))
            {
                gravity = false;
                jumpTime = .25f;
            }
            if (keyBoardState.IsKeyDown(Keys.D))
            {
                Position += new Vector2(1, 0);
            }
            if (keyBoardState.IsKeyDown(Keys.A))
            {
                Position += new Vector2(-1, 0);
            }
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                spriteScale += 0.01f;
            }

            JumpAndGravity(gameTime);
        }

        public void JumpAndGravity(GameTime gameTime)
        {
            double deltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            if (gravity)
            {
                Position += new Vector2(0, gravityMod * (float)deltaTime);
            }
            else if (!gravity)
            {
                Position -= new Vector2 (0, jumpForce * (float)deltaTime);
            }

            if (jumpTime <= 0)
            {
                gravity = true;
            }

            if (jumpTime > 0)
            {
                jumpTime -= .1f;
            }
            //Console.Clear();
            Console.WriteLine(jumpTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Vector2.Zero, spriteScale, SpriteEffects.None, 0f);
        }
    }
}
