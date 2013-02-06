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
    public class Sprite
    {
        // Récupère ou définit l'image du sprite
        public Texture2D texture { get; set; }

        // Récupère ou définit la position du Sprite
        public Vector2 position = Vector2.Zero;// { get; set; }

        public int width { get; protected set; } //Protected car ne doit
        public int height { get; protected set; } //pas être changer par le jeu

        // Récupère ou définit la vitesse de déplacement du sprite.
        public Vector2 velocity = Vector2.Zero;// { get; set; }

        public Rectangle bounds
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, width, height);
            }
        }

        // Charge l'image voulue grâce au ContentManager donné
        //Le ContentManager qui chargera l'image
        //L'asset name de l'image à charger pour ce Sprite
        public virtual void LoadContent(ContentManager content, string assetName)
        {
            texture = content.Load<Texture2D>(assetName);
        }

        // Met à jour les variables du sprite
        //Le GameTime associé à la frame
        public virtual void Update(GameTime gameTime)
        {
            position += velocity * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        /// <summary>
        /// Dessine le sprite en utilisant ses attributs et le spritebatch donné
        /// </summary>
        /// <param name="spriteBatch">Le spritebatch avec lequel dessiner</param>
        /// <param name="gameTime">Le GameTime de la frame</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

        public bool CollidesWith(Sprite other, bool calcPerPixel = true)
        {
            // Get dimensions of texture
            int widthOther = other.texture.Width;
            int heightOther = other.texture.Height;
            int widthMe = texture.Width;
            int heightMe = texture.Height;

            // if we need per pixel // at least avoid doing it // for small sizes (nobody will notice :P)
            if (calcPerPixel && ((Math.Min(widthOther, heightOther) > 10) || (Math.Min(widthMe, heightMe) > 10)))
            {
                // If simple intersection fails, don't even bother with per-pixel
                return bounds.Intersects(other.bounds) && PerPixelCollision(this, other);
            }

            return bounds.Intersects(other.bounds);
        }

        static bool PerPixelCollision(Sprite a, Sprite b)
        {
            // Get Color data of each Texture
            Color[] bitsA = new Color[a.texture.Width * a.texture.Height];
            a.texture.GetData(bitsA);
            Color[] bitsB = new Color[b.texture.Width * b.texture.Height];
            b.texture.GetData(bitsB);

            // Calculate the intersecting rectangle
            int x1 = Math.Max(a.bounds.X, b.bounds.X);
            int x2 = Math.Min(a.bounds.X + a.bounds.Width, b.bounds.X + b.bounds.Width);

            int y1 = Math.Max(a.bounds.Y, b.bounds.Y);
            int y2 = Math.Min(a.bounds.Y + a.bounds.Height, b.bounds.Y + b.bounds.Height);

            // For each single pixel in the intersecting rectangle
            for (int y = y1; y < y2; ++y)
            {
                for (int x = x1; x < x2; ++x)
                {
                    // Get the color from each texture
                    Color ac = bitsA[(x - a.bounds.X) + (y - a.bounds.Y) * a.texture.Width];
                    Color bc = bitsB[(x - b.bounds.X) + (y - b.bounds.Y) * b.texture.Width];

                    if (ac.A != 0 && bc.A != 0) // If both colors are not transparent (the alpha channel is not 0), then there is a collision
                        return true;
                }
            }
            // If no collision occurred by now, we're clear.
            return false;
        }

    }


}
