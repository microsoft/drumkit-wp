/*
 * Copyright (c) 2011-2014 Microsoft Mobile. All rights reserved.
 * See the license file delivered with this project for more information.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace WPDrumkit
{
    /// <summary> 
    /// Drum pad plays a sound when touched
    /// </summary>
    public class Pad
    {
        protected Texture2D image;
        protected Rectangle boundaries;
        private List<Rectangle> touchAreas = new List<Rectangle>();
        private Vector2 origin;
        private bool hit = false;

        public static event EventHandler Played = delegate { };

        public SoundEffect Sound { get; set; }
        public Percussion Percussion { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Origin
        {
            get { return origin; }
            set
            {
                origin = value;
                Position = new Vector2(boundaries.Left + Origin.X, boundaries.Top + Origin.Y);
            }
        }

        /// <summary>
        /// A flag indicating the pad was hit
        /// </summary>
        public bool HasBeenHit
        {
            get { return hit; }
            set { hit = value; }
        }

        public Rectangle Boundaries
        {
            get { return boundaries; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="percussion">Percussion enumeration</param>
        /// <param name="sound">Percussion sound</param>
        /// <param name="boundaries">Bounding box</param>
        public Pad(Percussion percussion, SoundEffect sound, Rectangle boundaries)
        {
            Origin = new Vector2(0f, 0f);
            Percussion = percussion;
            Sound = sound;
            this.boundaries = boundaries;
            this.touchAreas.Add(boundaries);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="percussion">Percussion enumeration</param>
        /// <param name="sound">Percussion sound</param>
        /// <param name="boundaries">Bounding box</param>
        /// <param name="touchAreas">List of rectangular touch areas</param>
        public Pad(Percussion percussion, SoundEffect sound, Rectangle boundaries, List<Rectangle> touchAreas)
        {
            Origin = new Vector2(0f, 0f);
            Percussion = percussion;
            Sound = sound;
            this.boundaries = boundaries;
            this.touchAreas = touchAreas;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="percussion">Percussion enumeration</param>
        /// <param name="sound">Percussion sound</param>
        /// <param name="boundaries">Bounding box</param>
        /// <param name="touchAreas">List of rectangular touch areas</param>
        /// <param name="image">Cymbal image</param>
        public Pad(Percussion percussion, SoundEffect sound, Rectangle boundaries, List<Rectangle> touchAreas, Texture2D image)
            : this(percussion, sound, boundaries, touchAreas)
        {
            this.image = image;
        }

        /// <summary>
        /// Plays the sound attached to the pad. Triggers also the static Played event.
        /// </summary>
        public void PlaySound()
        {
            hit = true;
            Sound.Play();
            Played(this, new EventArgs());
        }

        /// <summary>
        /// Determines whether a point is inside any touch area
        /// </summary>
        /// <param name="position">Coordinates</param>
        public bool Contains(Point point)
        {
            foreach (Rectangle rect in touchAreas)
            {
                if (rect.Contains(point))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether a point is inside any touch area
        /// </summary>
        /// <param name="position">Coordinates</param>
        public bool Contains(Vector2 position)
        {
            return Contains(new Point((int)position.X, (int)position.Y));
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (image != null)
                spriteBatch.Draw(image, Position, null, Color.White, 0f, Origin, 1.0f, SpriteEffects.None, 0f);
        }
    }
}