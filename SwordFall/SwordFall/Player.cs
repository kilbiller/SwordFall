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
        private SoundEffect jumping;
        SoundEffectInstance jumpingInstance;

        public Player()
        {
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

            //Son du saut
            jumping = content.Load<SoundEffect>("jumping");
            jumpingInstance = jumping.CreateInstance();
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

        public void Jump()
        {
            if (!isJumping)
            {
                jumpNumber++;
                if (jumpNumber == maxJump)
                    isJumping = true;
                position.Y -= 1f; //Pour pas bloquer le saut si la gravité est constamment active
                velocity.Y = -0.5f;
                //SoundEffect
                jumpingInstance.Volume = 0.7f;
                jumpingInstance.Play();
            }
        }

        public override void Update(GameTime gameTime)
        {
            //Formule de position de la classe Sprite //Mettre avant pour pas avoir un saut qui rentre un peu dans la sol
            position += velocity * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // On récupère les états du clavier et de la souris.
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            //Mouvement
            //Le else permet d'empecher le déplacement diagonal
            if (keyboardState.IsKeyDown(Keys.Q))
            {
                velocity.X = -runningSpeed;

                effect = SpriteEffects.FlipHorizontally;
                Animate(gameTime);
            }
            else if (keyboardState.IsKeyDown(Keys.D))
            {
                velocity.X = runningSpeed;

                effect = SpriteEffects.None;
                Animate(gameTime);
            }
            else
                velocity.X = 0f;

            //Jump
            if (keyboardState.IsKeyDown(Keys.Space) && !previousKeyboardState.IsKeyDown(Keys.Space))
                Jump();

            //Gravity Pull
                velocity.Y += gravity;

            //Idle Animation
            if (keyboardState.IsKeyUp(Keys.Q) && keyboardState.IsKeyUp(Keys.D) && !isJumping)
            {
                effect = SpriteEffects.None;
                frame = 1;
            }

            previousKeyboardState = keyboardState;     
        }

        //Collisions
        public void Collision(int viewportWidth, int viewportHeight)
        {
            if (position.X < 0) position.X = 0; //Gauche
            if ((position.X + width * playerScale) > viewportWidth) position.X = viewportWidth - (width * playerScale); // Droite
            if (position.Y + height * playerScale > viewportHeight) //Sol
            {
                position.Y = viewportHeight - (height * playerScale);
                isJumping = false; //le personnage n'est plus en train de sauter
                velocity.Y = 0f; //donc pas de vitesse verticale
                jumpNumber = 0;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, new Rectangle((frame - 1) * width, 0, width, height), Color.White, 0f, Vector2.Zero, playerScale, effect, 0f);
        }

    }
}
