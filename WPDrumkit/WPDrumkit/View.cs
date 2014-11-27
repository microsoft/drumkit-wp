/*
 * Copyright (c) 2011-2014 Microsoft Mobile. All rights reserved.
 * See the license file delivered with this project for more information.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WPDrumkit
{
    /// <summary>
    /// View base class
    /// </summary>
    abstract class View
    {
        public event EventHandler Exit = delegate { };
        protected bool visible = true;
        protected int width;
        protected int height;

        public bool Visible { get; set; }

        /// <summary>
        /// Costructor
        /// </summary>
        /// <param name="width">Width of the screen</param>
        /// <param name="height">Height of the screen</param>
        public View(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Requests exit
        /// </summary>
        public void exit()
        {
            Exit(this, new EventArgs());
        }

        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);
        public abstract void TouchEvent(TouchLocation touch);
        public abstract void TouchEvent(GestureSample gesture);
    }
}