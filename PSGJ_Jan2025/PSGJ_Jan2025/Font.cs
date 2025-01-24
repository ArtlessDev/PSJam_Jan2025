using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Numerics;

namespace PSGJ_Jan2025
{
    public class Font
    {
        string fontText;
        Color fontColor;
        Microsoft.Xna.Framework.Vector2 fontPosition;
        SpriteFont fontSprite;
        public Font() 
        {
            fontColor = Color.White;
            fontText = "default test";
            fontPosition = new(8, 8);
            //fontSprite = GameMaster.CustomContent.Load<SpriteFont>("NotChunky");

        }

        public string FontText
        {
            get { return fontText; }
            set { fontText = value; }
        }
        public Color FontColor
        {
            get { return fontColor; }
            set { fontColor = value; }
        }
        public Microsoft.Xna.Framework.Vector2 FontPosition
        {
            get { return fontPosition; }
            set { fontPosition = value; }
        }
        public SpriteFont FontSprite
        {
            get { return fontSprite; }
            set { fontSprite = value; }
        }
    }
}
