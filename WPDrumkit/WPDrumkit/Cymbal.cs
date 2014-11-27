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
    /// Extends Pad with cymbal swinging animation
    /// </summary>
    class Cymbal : Pad
    {
        private Timer animator;
        private TimerCallback rotate;
        private const float dt = 0.1f;
        private float dampening = 0.96f;
        private float angle = 0f;
        private float acceleration = 0f;
        private float velocity = 0f;

        /// <summary>
        /// Maximum swinging angle
        /// </summary>
        public float MaxAngle { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="percussion">Percussion enumeration</param>
        /// <param name="sound">Cymbal sound</param>
        /// <param name="boundaries">Bounding box</param>
        /// <param name="touchAreas">List of rectangular touch areas</param>
        /// <param name="image">Cymbal image</param>
        public Cymbal(Percussion percussion, SoundEffect sound, Rectangle boundaries, List<Rectangle> touchAreas, Texture2D image) :
            base(percussion, sound, boundaries, touchAreas, image)
        {
            MaxAngle = 0.26f; // Default for max angle (~15 degrees)            
            rotate = new TimerCallback(RotateCallback);
            Played += new EventHandler(Cymbal_Played);
        }

        override public void Draw(SpriteBatch spriteBatch)
        {
            if (image != null)
                spriteBatch.Draw(image, Position, null, Color.White, angle, Origin, 1.0f, SpriteEffects.None, 0f);

        }

        /// <summary>
        /// Triggers the animation when cymbal gets played
        /// </summary>
        private void Cymbal_Played(object sender, EventArgs e)
        {
            if (sender == this)
            {
                angle = MaxAngle;
                if (animator != null) animator.Dispose();
                AutoResetEvent reset = new AutoResetEvent(false);
                animator = new Timer(rotate, reset, 0L, 30L);
            }
        }

        /// <summary>
        /// Rotates the cymbal creating a swinging effect
        /// </summary>        
        private void RotateCallback(Object stateInfo)
        {
            angle *= dampening;
            acceleration = -9.81f / (0.04f * Origin.X) * (float)Math.Sin(angle);
            velocity += acceleration * dt;
            angle += velocity * dt;

            if (Math.Abs(angle) < 0.01 && Math.Abs(velocity) < 0.01)
                animator.Dispose();
        }
    }
}
