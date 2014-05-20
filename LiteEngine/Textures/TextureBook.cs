using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiteEngine.Math;

namespace LiteEngine.Textures
{
    /// <summary>
    /// Stores textures for the game. Textures used in the game should be registered through this class.
    /// </summary>
    public class TextureBook
    {
        Dictionary<string, Texture> _textures = new Dictionary<string, Texture>();

        /// <summary>
        /// Adds all the textures in a spritesheet. Loads the spritesheet's corresponding XML file
        /// (which should be in the Content\SpriteSheets folder)
        /// </summary>
        /// <param name="spriteSheetName">name of the spritesheet. This is the name of the texture marked as 
        /// build action Content (and copied to output folder) without the file extension.</param>
        /// <param name="spriteSheetXmlPath">resource path to the sprite sheet xml file. If omitted the xml file
        /// is assumed to be Content\SpriteSheets\spriteSheetName.xml</param>
        public void AddSpriteSheetTextures(string spriteSheetName, string spriteSheetXmlPath="")
        {
            if (spriteSheetXmlPath == "")
                spriteSheetXmlPath = @"Content\" + spriteSheetName + ".xml";
            SpriteSheet sh = new SpriteSheet();
            sh.Load(spriteSheetXmlPath);
            foreach (SpriteSheetTexture texture in sh.Images)
            {
                _textures.Add(spriteSheetName + "." + texture.Name, new Texture(spriteSheetName, new RectangleI(texture.X, texture.Y, sh.ImageSize, sh.ImageSize)));
            }
        }

        /// <summary>
        /// Obtains a texture by name
        /// </summary>
        public Texture GetTexture(string name)
        {
            return _textures[name];
        }
    }


}
