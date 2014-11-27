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
using Microsoft.Xna.Framework.Input.Touch;

namespace WPDrumkit
{
    /// <summary>
    /// ToggleButton with two states. Extends Button.
    /// </summary>
    public class ToggleButton : Button
    {
        protected Texture2D onImg;

        public bool On { get; set; }

        public bool Off
        {
            get { return !On; }
            set { On = !value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="off">Image shown while toggle button is up</param>
        /// <param name="pressed">Image shown while toggle button is pressed</param>
        /// <param name="on">Sprite image shown while toggle button is down</param>
        public ToggleButton(Texture2D off, Texture2D pressed, Texture2D on)
            : base(off, pressed)
        {
            onImg = on;
            Clicked += new EventHandler(ToggleButton_Clicked);
            On = false;
        }

        /// <summary>
        /// Toggles the button state
        /// </summary>
        public void Toggle()
        {
            On = !On;
        }

        override public void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
                spriteBatch.Draw(pressed ? pressedImg : (On ? onImg : releasedImg), rect, Color.White * opacity);
        }

        /// <summary>
        /// Toggles the button state on button clicked
        /// </summary>        
        private void ToggleButton_Clicked(object sender, EventArgs e)
        {
            Toggle();
        }
    }
}
