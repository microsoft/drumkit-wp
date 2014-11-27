/*
 * Copyright (c) 2011-2014 Microsoft Mobile. All rights reserved.
 * See the license file delivered with this project for more information.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;

namespace WPDrumkit
{
    /// <summary>
    /// Extends Pad with drum vibration animation
    /// </summary>
    class Drum : Pad
    {
        private Timer shaker;
        private TimerCallback shake;
        private float dampening = 0.8f;
        private float magnitude = 0f;

        public float MaxMagnitude { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="percussion">Percussion enumeration</param>
        /// <param name="sound">Cymbal sound</param>
        /// <param name="boundaries">Bounding box</param>
        /// <param name="touchAreas">List of rectangular touch areas</param>
        /// <param name="image">Cymbal image</param>
        public Drum(Percussion percussion, SoundEffect sound, Rectangle boundaries, List<Rectangle> touchAreas, Texture2D image) :
            base(percussion, sound, boundaries, touchAreas, image)
        {
            MaxMagnitude = -0.04f;
            Origin = new Vector2(boundaries.Width / 2, boundaries.Height / 2);
            shake = new TimerCallback(ShakeCallback);
            Played += new EventHandler(Drum_Played);
        }

        override public void Draw(SpriteBatch spriteBatch)
        {
            if (image != null)
                spriteBatch.Draw(image, Position, null, Color.White, 0.0f, Origin, 1.0f + magnitude, SpriteEffects.None, 0f);

        }

        /// <summary>
        /// Triggers the animation when drum gets played
        /// </summary>
        private void Drum_Played(object sender, EventArgs e)
        {
            if (sender == this)
            {
                magnitude = MaxMagnitude;
                if (shaker != null) shaker.Dispose();
                AutoResetEvent reset = new AutoResetEvent(false);
                shaker = new Timer(shake, reset, 0L, 30L);
            }
        }

        /// <summary>
        /// Scales the drum up and down creating a vibration effect
        /// </summary>        
        private void ShakeCallback(Object stateInfo)
        {
            magnitude *= -dampening;
            if (Math.Abs(magnitude) < 0.01)
                shaker.Dispose();
        }
    }
}
