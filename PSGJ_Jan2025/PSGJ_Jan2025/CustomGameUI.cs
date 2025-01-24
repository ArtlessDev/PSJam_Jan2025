﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace PSGJ_Jan2025
{
    public class CustomGameUI
    {
        Rectangle rect;
        Texture2D texture;
        Vector2 position;
        Point size;
        Color textureColor;

        public CustomGameUI() 
        {
            position = new Vector2(128, 416);
            size = new Point(320, 96);
            rect = new Rectangle(new Point((int)position.X, (int)position.Y), size);
            textureColor = Color.White;
            texture = GameMaster.CustomContent.Load<Texture2D>("gray-export");
        }

        public CustomGameUI(Rectangle customRect)
        {
            texture = GameMaster.CustomContent.Load<Texture2D>("zone");
            textureColor = Color.White;
            rect = customRect;
        }

        public Rectangle Rect
        {
            get { return rect; }
            set { rect = value; }
        }
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        public Point Size
        {
            get { return size; }
            set { size = value; }
        }
        public Color TextureColor
        {
            get { return textureColor; }
            set{ textureColor = value; }
        }

        public void changeColor(Rectangle mouseRect)
        {
            if (mouseRect.Intersects(this.Rect))
            {
                //this.Texture = Content.Load<Texture2D>("zilla");
                this.TextureColor = Color.DarkGray;
            }
            else
            {
                //this.Texture = Content.Load<Texture2D>("blank-button");
                this.TextureColor = Color.White;
            }
        }
        public void changeColors(Rectangle mouseRect, CustomGameUI zones)
        {
            if (mouseRect.Intersects(zones.Rect))
            {
                //this.Texture = Content.Load<Texture2D>("zilla");
                this.TextureColor = Color.DarkGray;
            }
            else
            {
                //this.Texture = Content.Load<Texture2D>("blank-button");
                this.TextureColor = Color.White;
            }   
        }
    }
}
