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
        public Vector2 position;// { get; set; }

        public int width { get; protected set; } //Protected car ne doit
        public int height { get; protected set; } //pas être changer par le jeu

        // Récupère ou définit la vitesse de déplacement du sprite.
        public Vector2 velocity;// { get; set; }

        public Rectangle positionRectangle
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, width, height);
            }
        }

        // Initialise les variables du Sprite
        public virtual void Initialize()
        {
            position = Vector2.Zero;
            velocity = Vector2.Zero;
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
        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

    }


}
