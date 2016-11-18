using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;

namespace UniverseV2
{
    public class Ressources
    {
        //FIELDS
        public static Dictionary<string, Texture2D> sprites;

        //METHODS
        public static void LoadSprites(ContentManager content)
        {
            sprites = new Dictionary<string, Texture2D>();

            List<string> spr = new List<string>()
            {
                "planet1",
                "Dot"
            };

            foreach (string sprite in spr)
            {
                sprites.Add(sprite, content.Load<Texture2D>($"spr/{sprite}"));
            }

        }

    }
}
