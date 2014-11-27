/*
 * Copyright (c) 2011-2014 Microsoft Mobile. All rights reserved.
 * See the license file delivered with this project for more information.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WPDrumkit
{
    /// <summary>
    /// A layer containing information
    /// </summary>
    class InfoOverlay
    {
        Rectangle overlayRect;
        Texture2D overlayTexture = Visual.defaultTexture;
        SpriteFont infoFont = Visual.defaultFont;

        public String Message { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rect">Rectangle defining overlay position and measures</param>
        public InfoOverlay(Rectangle rect)
        {
            overlayRect = rect;
            Message = String.Empty;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(overlayTexture, overlayRect, Visual.defaultBackgroundColor * 0.4f);
            Vector2 textSize = infoFont.MeasureString(Message);
            float x = (overlayRect.Width - textSize.X) / 2;
            float y = (overlayRect.Height - textSize.Y) / 2;
            spriteBatch.DrawString(infoFont, Message, new Vector2(x, y), Visual.defaultFontColor);
        }
    }
}
