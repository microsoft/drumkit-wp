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
    /// Extends a toggle button with animated ON state.
    /// </summary>
    public class AnimatedToggleButton : ToggleButton
    {
        private float timePerFrame;
        private float totalElapsed;
        private int frameCount;
        private int frame;
        private int frameWidth;
        private int frameHeight;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="off">Image shown while toggle button is up</param>
        /// <param name="pressed">Image shown while toggle button is pressed</param>
        /// <param name="on">Sprite image shown while toggle button is down and animated</param>
        /// <param name="frames">Number of frames in sprite image</param>
        /// <param name="fps">Animation speed (frames per second)</param>
        public AnimatedToggleButton(Texture2D off, Texture2D pressed, Texture2D on, int frames, int fps)
            : base(off, pressed, on)
        {
            frameCount = frames;
            timePerFrame = (float)1 / fps;
            // Animation works only with vertically lined up sprite image
            frameWidth = on.Width;
            frameHeight = on.Height / frameCount;
        }

        override public void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                if (On && !pressed)
                {
                    // Select current frame from sprite image
                    Rectangle sourceRect = new Rectangle(0, frameHeight * frame, frameWidth, frameHeight);
                    spriteBatch.Draw(onImg, rect, sourceRect, Color.White * opacity);
                }
                else
                {
                    base.Draw(spriteBatch);
                }
            }
        }

        /// <summary>
        /// Updates the frame position according to the elapsed time.
        /// </summary>
        /// <param name="elapsed">The time elapsed since last update</param>
        public void UpdateFrame(float elapsed)
        {
            if (On)
            {
                totalElapsed += elapsed;
                if (totalElapsed > timePerFrame)
                {
                    frame++;
                    frame = frame % frameCount;
                    totalElapsed -= timePerFrame;
                }
            }
        }
    }
}
