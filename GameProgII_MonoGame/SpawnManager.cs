using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GameProgII_MonoGame
{
    internal class SpawnManager
    {
        //arbitrary timer value
        public float timer = 2f;

        public void Despawn(Pipe pipe, List<Pipe> pipes)
        {
            //if pipe is off the left side of the screen
            if (pipe.Position.X < 0 - pipe.Texture.Width)
            {
                pipes.Remove(pipe); 
                //debugging
                Console.WriteLine("OFFSCREEN");
            }
        }

        //abstracting the update functionality away from Game1
        public void Update(GameTime gameTime, List<Pipe> pipes, ContentManager content)
        {
            //spawn takes the same parameters and update, will get these references in Game1
            Spawn(gameTime, pipes, content);
        }

        
        public void Spawn(GameTime gameTime, List<Pipe> pipes, ContentManager content)
        {
            //timer in ~real world seconds
            timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (timer <= 0)
            {
                //loading the pipes into the right side of the screen at a random y position that fits the screen
                Random random = new Random();
                Pipe pipe = new Pipe();
                pipe.LoadContent(new Vector2(1200, random.Next(375, 720)), content);  
                //adding the pipe to the list that lives in Game1 so we can update all the pipes in a foreach loop
                pipes.Add(pipe);

                //resetting timer
                timer = 2f;
            }
        }
    }
}
