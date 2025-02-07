using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GameProgII_MonoGame
{
    public class Pipe
    {
        public bool hasBeenGoneThrough = false;

        public Texture2D Texture { get; private set; }
        public Vector2 Position { get; private set; }
        public Rectangle BottomBoundingBox { get; private set; }
        public Rectangle TopBoundingBox { get; private set; }

        //used to determine the bounding boxes of each pipe segment
        private float pipeHeight;
        private float pipeWidth;
        
        private float gapSize = 325f;               

        //handling it's own loading of texture
        public void LoadContent(Vector2 startingPosition, ContentManager content)
        {
            Texture = content.Load<Texture2D>("Sprites/pipe");
            Position = startingPosition;

            pipeWidth = Texture.Width;
            pipeHeight = Texture.Height;
        }
        
        public void Update(GameTime gameTime)
        {
            //getting "colliders" for the top and bottom segement of the pipe
            BottomBoundingBox = GetPipeBoundingBox(Position, pipeWidth, pipeHeight);
            TopBoundingBox = GetPipeBoundingBox(Position - new Vector2(0, 1080 + gapSize), pipeWidth, pipeHeight);
            //Console.WriteLine(BottomBoundingBox);
            //Console.WriteLine(TopBoundingBox);
            
            //setting deltatime for consistent performance
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Move(gameTime);          
        }
               
        //very simple right to left movement
        public void Move(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Position -= new Vector2(170f * deltaTime, 0);
        }

        //abstracting the Draw() method
        public void Draw(SpriteBatch spriteBatch)
        {
            //drawing pipes
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f); //bottom pipe
            spriteBatch.Draw(Texture, Position - new Vector2(0, 1080 + gapSize), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f); //top pipe
        }
        
        //called in Update() which is then called in Game1.Update()
        public Rectangle GetPipeBoundingBox(Vector2 pipePosition, float pipeWidth, float pipeHeight)
        {
            //simple formula for determining the bounding box of a perfectly cropped image
            return new Rectangle((int)pipePosition.X, (int)pipePosition.Y, (int)pipeWidth, (int)pipeHeight);
        }                
    }
}
