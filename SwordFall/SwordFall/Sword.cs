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
        int posX;

        public bool isTouching;

        public Sword(int _posX)
        {
            width = 31;
            height = 124;
            gravity = 0.008f;
            this.posX = _posX;
            isTouching = false;
        }

        public override void LoadContent(ContentManager content, string assetName)
        {
            base.LoadContent(content, assetName);
            position = new Vector2(posX, 0);
        }

        public override void Update(GameTime gameTime)
        {
            //Formule de position de la classe Sprite //Mettre avant pour pas avoir un saut qui rentre un peu dans la sol
            base.Update(gameTime);

            velocity.Y += gravity;
        }

    }
}
