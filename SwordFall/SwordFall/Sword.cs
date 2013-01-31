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

    class Sword : Sprite
    {
        float gravity;

        public bool isTouching;
        public bool isVisible;
        public double timer;

        int posX;

        public Sword(int _posX)
        {
            width = 31;
            height = 124;
            gravity = 0.008f;
            isTouching = false;
            isVisible = false;
            timer = 0;
            posX = _posX;
        }

        public override void LoadContent(ContentManager content, string assetName)
        {
            base.LoadContent(content, assetName);
            position = new Vector2(0, 100000); // Pour ne pas reset au début
        }

        public void Update(GameTime gameTime, int randTimer)
        {
            //Formule de position de la classe Sprite 
            position += velocity * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            velocity.Y += gravity;

            if(!isVisible)
            {
                timer += gameTime.ElapsedGameTime.TotalMilliseconds;

                if (timer > randTimer)
                {
                    position = new Vector2(posX, -(height + 10));
                    velocity.Y = 0;
                    isVisible = true;
                    timer = 0;
                }
            }

        }

        public void Collision(int viewportWidth, int viewportHeight)
        {
            //Si traverse le sol
            if (position.Y - 10 > viewportHeight)
                isVisible = false;
        }

    }
}
