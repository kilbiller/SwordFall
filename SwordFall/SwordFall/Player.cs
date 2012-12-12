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
    public class Player : Sprite
    {
        private Viewport viewport;

        //Input
        KeyboardState keyboardState,previousKeyboardState;
        MouseState mouseState;

        //Animation
        private int frame;
        SpriteEffects effect;
        TimeSpan lastTime;
        private int animationSpeed; //en milliseconde;

        //Player
        private float playerScale; //Grossissement
        private float runningSpeed;
        public bool isAlive;
        public bool isTouching;

        //Jump
        bool isJumping;
        float gravity;
        int jumpNumber;
        int maxJump;

        public Player(Viewport _viewport)
        {
            this.viewport = _viewport;

            //Player
            width = 36;
            height = 48;
            playerScale = 1f;
            runningSpeed = 0.3f;
            isAlive = true;
            isTouching = false;

            //Animation
            frame = 1;
            effect = SpriteEffects.None;
            animationSpeed = 80;

            //Jump
            isJumping = true;
            gravity = 0.02f;
            jumpNumber = 0;
            maxJump = 1;
            
        }

        public override void LoadContent(ContentManager content, string assetName)
        {
            base.LoadContent(content, assetName);
            position = new Vector2(100, 400);
        }

        public void Animate(GameTime gameTime)
        {
                if (gameTime.TotalGameTime.Subtract(lastTime).Milliseconds >= animationSpeed && !isJumping)
                {
                    frame++;
                    if (frame > 9)
                        frame = 2;
                    lastTime = gameTime.TotalGameTime;
                }
        }

        public override void Update(GameTime gameTime)
        {
            //formule de position de la classe Sprite //Mettre avant pour pas avoir un saut qui rentre un peu dans la sol
            base.Update(gameTime);

            // On récupère les états du clavier et de la souris.
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            //Mouvement

            //Le else permet d'empecher le déplacement diagonal
            if (keyboardState.IsKeyDown(Keys.Q))
            {
                if (position.X > 0)
                {
                    velocity.X = -runningSpeed;
                }
                else
                    velocity.X = 0f;

                effect = SpriteEffects.FlipHorizontally;
                Animate(gameTime);
            }
            else if (keyboardState.IsKeyDown(Keys.D))
            {
                if ((position.X + width * playerScale) < viewport.Width)
                {
                    velocity.X = runningSpeed;
                }
                else
                    velocity.X = 0f;

                effect = SpriteEffects.None;
                Animate(gameTime);
            }
            else
                velocity.X = 0f;

            //Jump
            if (!isJumping)
            {
                if (keyboardState.IsKeyDown(Keys.Space) && !previousKeyboardState.IsKeyDown(Keys.Space))
                {
                    jumpNumber++;
                    if (jumpNumber == maxJump)
                        isJumping = true;
                    position.Y -= 1f; //Pour pas bloquer le saut si la gravité est constamment active
                    velocity.Y = -0.5f;
                }
            }

            //gravité
            //if (isJumping)
                velocity.Y += gravity;

            //Retombe au sol
            if (position.Y + height*playerScale > viewport.Height)
            {
                position.Y = viewport.Height - height * playerScale; //Replace bien le personnage sur le sol
                isJumping = false; //le personnage n'est plus en train de sauter
                velocity.Y = 0f; //donc pas de vitesse verticale
                jumpNumber = 0;
            }

            //Idle Animation
            if (keyboardState.IsKeyUp(Keys.Q) && keyboardState.IsKeyUp(Keys.D) && !isJumping)
            {
                effect = SpriteEffects.None;
                frame = 1;
            }

            previousKeyboardState = keyboardState;
                    
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, new Rectangle((frame - 1) * width, 0, width, height), Color.White, 0f, Vector2.Zero, playerScale, effect, 0f);
        }

    }
}
