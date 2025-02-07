using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;


namespace GameProgII_MonoGame
{
    public class Player
    {
        public int score;
        
        //properties for texture and position
        public Texture2D Texture { get; private set; }
        public SoundEffect SoundEffect { get; private set; }
        public SpriteFont Font { get; private set; }
        public Vector2 Position { get; private set; }
        public Rectangle BoundingBox { get; private set; }

        //getting the dimensions of the player
        int playerWidth;
        int playerHeight;

        //scale variable for potential animations in the future
        private float spriteScale = 1f;

        //timer variable for handling length of jump
        private float jumpTime = 0;

        //simple floats for physics emulation, (gravity, jumpForce)
        private float gravityMod = 350f;
        private float jumpForce = 625f;

        //gravity toggle
        private bool gravity = true;

        //used to isolate input to a single frame by tracking state of buttons on previous frames
        KeyboardState previousKeyboardState = Keyboard.GetState();

        //loading the player within the class so i can keep the setters of position and texture private (also handles loading of content)
        public void LoadContent(Vector2 position, ContentManager content)
        {
            //loading dependencies
            Texture = content.Load<Texture2D>("Sprites/flapper");
            Font = content.Load<SpriteFont>("Fonts/galleryFont");
            SoundEffect = content.Load<SoundEffect>("Sounds/point-smooth-beep-230573");
            
            //will be initial position
            Position = position;
            
            //for bounding box
            playerWidth = Texture.Width;
            playerHeight = Texture.Height;            
        }

        //abstracting away the input management into the players own Update() method, will be called from Game1.Update()
        public void Update(GameTime gameTime, List<Pipe> pipes)
        {
            BoundingBox = GetPlayerBoundingBox(Position, playerWidth, playerHeight);
            //Console.WriteLine(BoundingBox);
            
            //getting the states of the peripherals to detect input
            KeyboardState keyBoardState = Keyboard.GetState();
            //previousKeyBoardState;

            MouseState mouseState = Mouse.GetState();

            //setting deltatime for decoupling performance from framerate
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //if button hasn't been pressed the previous frame and is pressed this frame
            if (keyBoardState.IsKeyDown(Keys.Space) && previousKeyboardState.IsKeyUp(Keys.Space))
            {
                //SoundEffect.Play();
                gravity = false;
                jumpTime = .3f;
            }
            previousKeyboardState = keyBoardState;

            //CheckCollision();
            JumpAndGravity(gameTime);
            ScoreIncrease(pipes);
        }

        //handles grravity and jump logic
        public void JumpAndGravity(GameTime gameTime)
        {
            double deltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            //appleis gravity
            if (gravity)
            {
                Position += new Vector2(0, gravityMod * (float)deltaTime);
            }
            //jump
            else if (!gravity)
            {
                Position -= new Vector2 (0, jumpForce * (float)deltaTime);
            }

            //ensuring jump only happens for a split second
            if (jumpTime <= 0)
            {
                gravity = true;
            }

            if (jumpTime > 0)
            {
                jumpTime -= .03f;
            }
            //Console.Clear();
            //Console.WriteLine(jumpTime); 
        }

        //abstracting Draw() method, will be called in Game1.Draw()
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Vector2.Zero, spriteScale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(Font, $"Score: {score}", new Vector2(0, 0), Color.White);
        }

        //getting bounding box to compare with other objects for collisions
        public Rectangle GetPlayerBoundingBox(Vector2 playerPosition, float playerWidth, float playerHeight)
        {           
            return new Rectangle((int)playerPosition.X, (int)playerPosition.Y, (int)playerWidth, (int)playerHeight);
        }

        public bool CheckCollision(Rectangle other)
        {
            if (BoundingBox.Intersects(other))
            {
                Console.WriteLine("Collision Detected");
                return true;
            }
            return false;            
        }
        //method overloading to be able to track 2 objects at once
        public bool CheckCollision(Rectangle other, Rectangle otherTwo)
        {
            if (BoundingBox.Intersects(other) || BoundingBox.Intersects(otherTwo))
            {                
                return true;
            }
            return false;
        }

        public void ScoreIncrease(List<Pipe> pipes)
        {
            foreach(Pipe pipe in pipes)
            {
                if (Position.X > pipe.Position.X && !pipe.hasBeenGoneThrough)
                {
                    SoundEffect.Play();
                    score += 1;
                    pipe.hasBeenGoneThrough = true;
                }
            }
            

        }

    }
}
