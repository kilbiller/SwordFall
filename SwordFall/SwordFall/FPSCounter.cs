using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SwordFall
{
    public class FPSCounter
    {
        SpriteFont spriteFont;

        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;

        public bool show = true;




        public virtual void LoadContent(ContentManager Content)
        {
            spriteFont = Content.Load<SpriteFont>("SpriteFont1");
        }

        public virtual void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
        }


        public virtual void Draw(SpriteBatch spriteBatch)
        {
            frameCounter++;
            if (!show) return;

            string fps = string.Format("FPS : {0}", frameRate);

            spriteBatch.DrawString(spriteFont, fps, new Vector2(3, 3), Color.Black);
            spriteBatch.DrawString(spriteFont, fps, new Vector2(2, 2), Color.White);
        }
    }
}
