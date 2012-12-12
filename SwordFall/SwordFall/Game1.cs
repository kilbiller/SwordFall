using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SwordFall
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Viewport viewport;

        SpriteFont font;
        Player player;

        List<Sword> swords = new List<Sword>();

        Random random = new Random();
        int randX;

        // At the top of your class:
        Texture2D pixel;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //Jeu en Fullscreen
            //graphics.IsFullScreen = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            viewport = graphics.GraphicsDevice.Viewport;

            player = new Player(viewport);
            player.Initialize();

            /*sword1 = new Sword();
            sword1.Initialize();*/


            for (int i = 0; i < 10; i++)
            {
                randX = random.Next(1, 500);
                swords.Add(new Sword(randX));
            }

            foreach(Sword sword in swords)
                sword.Initialize();

           
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            player.LoadContent(Content, "hero");

            foreach (Sword sword in swords)
                sword.LoadContent(Content, "sword");
            //sword1.LoadContent(Content, "sword");

            font = Content.Load<SpriteFont>("SpriteFont1");

            // Somewhere in your LoadContent() method:
            pixel = new Texture2D(graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White }); // so that we can draw whatever color we want on top of it

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Escape))
                this.Exit();

            player.Update(gameTime);
            //sword1.Update(gameTime);

            foreach (Sword sword in swords)
                sword.Update(gameTime);


            foreach (Sword sword in swords)
            {
                if(player.positionRectangle.Intersects(sword.positionRectangle))
                {
                    player.isTouching = true;
                    sword.isTouching = true;
                    player.isAlive = false;
                }
                else
                {
                    sword.isTouching = false;
                }

            }

            base.Update(gameTime);
        }


        /// <summary>
        /// Will draw a border (hollow rectangle) of the given 'thicknessOfBorder' (in pixels)
        /// of the specified color.
        ///
        /// By Sean Colombo, from http://bluelinegamestudios.com/blog
        /// </summary>
        /// <param name="rectangleToDraw"></param>
        /// <param name="thicknessOfBorder"></param>
        private void DrawBorder(Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor)
        {
            // Draw top line
            spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), borderColor);

            // Draw left line
            spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);

            // Draw right line
            spriteBatch.Draw(pixel, new Rectangle((rectangleToDraw.X + rectangleToDraw.Width - thicknessOfBorder),
                                            rectangleToDraw.Y,
                                            thicknessOfBorder,
                                            rectangleToDraw.Height), borderColor);
            // Draw bottom line
            spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X,
                                            rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder,
                                            rectangleToDraw.Width,
                                            thicknessOfBorder), borderColor);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.Clear(Color.Gray);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            //Texts
            spriteBatch.DrawString(font, "Hit the sword.", new Vector2(viewport.Width / 2, viewport.Height / 2 - 20), Color.WhiteSmoke);
            if (!player.isAlive)
                spriteBatch.DrawString(font, "You are dead... Retry?", new Vector2(viewport.Width / 2, viewport.Height / 2), Color.WhiteSmoke);
            
            //Player

            player.Draw(spriteBatch);
            if (!player.isTouching)
                DrawBorder(player.positionRectangle, 1, Color.LightGreen);
            else
                DrawBorder(player.positionRectangle, 1, Color.Red);

            player.isTouching = false;

            //Sword
            foreach (Sword sword in swords)
            {
                sword.Draw(spriteBatch);
                if(!sword.isTouching)
                    DrawBorder(sword.positionRectangle, 1, Color.LightGreen);
                else
                    DrawBorder(sword.positionRectangle, 1, Color.Red);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
