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

        public Random random = new Random();

        // At the top of your class:
        Texture2D pixel;

        Song bloody_tears;
        bool songstart = false;

        bool showRule = true;
        Color ruleColor = new Color(245, 245, 255, 255);

        Texture2D background_corridor;

        //Constantes
        int swordNumber = 16;
        int maxRespawnTime = 20000; //Delais max pour le random du respawn des épée (en s)
        //Debug
        bool Debug = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            viewport = graphics.GraphicsDevice.Viewport;

            player = new Player();

            for (int i = 0; i < swordNumber; i++)
                swords.Add(new Sword(i*50));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            player.LoadContent(Content, "hero");

            foreach (Sword sword in swords)
                sword.LoadContent(Content, "sword");

            font = Content.Load<SpriteFont>("SpriteFont1");

            //Debug boxes
            pixel = new Texture2D(graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White }); // so that we can draw whatever color we want on top of it

            //Background music
            bloody_tears = Content.Load<Song>("Bloody tears");
            MediaPlayer.IsRepeating = true;

            //Castle corridor background
            background_corridor = Content.Load<Texture2D>("background_corridor");

        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Escape))
                this.Exit();

            //Player Logic
            player.Update(gameTime);
            player.Collision(viewport.Width, viewport.Height);

            //Sword Logic
            foreach (Sword sword in swords)
            {
                sword.Update(gameTime, random.Next(0, maxRespawnTime));
                sword.Collision(viewport.Width, viewport.Height);
            }

            //Per Pixel collision with swords
            foreach (Sword sword in swords)
            {
                if (player.CollidesWith(sword))
                {
                    player.isTouching = true;
                    sword.isTouching = true;
                    player.isAlive = false;
                }
                else
                    sword.isTouching = false;
            }

            //Background Music
            if (!songstart)
            {
                MediaPlayer.Play(bloody_tears);
                songstart = true;
            }

            //Rules
            if (gameTime.TotalGameTime.Seconds > 1)
            {
                ruleColor *= 0.98f;
                if(ruleColor.A == 0)
                    showRule = false;
            }
            
            base.Update(gameTime);
        }

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

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.Clear(Color.Gray);

            //spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            spriteBatch.Begin();

            //Background
            spriteBatch.Draw(background_corridor, background_corridor.Bounds, Color.White);

            //Texts
            if (showRule)
                spriteBatch.DrawString(font, "Stay away from the swords !", new Vector2(viewport.Width / 2, viewport.Height / 2 - 20), ruleColor);
            if (!player.isAlive)
                spriteBatch.DrawString(font, "You are dead... Retry?", new Vector2(viewport.Width / 2, viewport.Height / 2), Color.WhiteSmoke);
            
            //Player
            player.Draw(spriteBatch);
            if (Debug)
            {
                if (!player.isTouching)
                    DrawBorder(player.positionRectangle, 1, Color.LightGreen);
                else
                    DrawBorder(player.positionRectangle, 1, Color.Red);
            }

            int i = 1;
            //Swords
            foreach (Sword sword in swords)
            {
                sword.Draw(spriteBatch);

                if (Debug)
                {
                    if (!sword.isTouching)
                        DrawBorder(sword.positionRectangle, 1, Color.LightGreen);
                    else
                        DrawBorder(sword.positionRectangle, 1, Color.Red);

                    string swordDebugText = "Sword " + i + " : " + sword.timer.ToString();
                    spriteBatch.DrawString(font, swordDebugText, new Vector2(2, 1 + (i * 15)), Color.WhiteSmoke);
                    i++;
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
