/*
 * Copyright (c) 2011-2014 Microsoft Mobile. All rights reserved.
 * See the license file delivered with this project for more information.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace WPDrumkit
{
    /// <summary>
    /// Implements a basic button
    /// </summary>
    public class Button
    {
        public event EventHandler Clicked = delegate { };

        protected Texture2D releasedImg;
        protected Texture2D pressedImg;
        protected Rectangle rect;
        // A big button can be pressed with multiple fingers at the same time
        // so we need to keep track of every individual touch point
        protected List<int> touchIds;
        protected bool pressed = false;
        protected bool animated = false;
        protected float opacity = 1.0f;

        public bool Enabled { get; set; }
        public bool Visible { get; set; }

        /// <summary>
        /// Returns the 
        /// </summary>
        public Point Location
        {
            get { return rect.Location; }
            set { rect.Location = value; }
        }

        public Point Center
        {
            get { return rect.Center; }
            set
            {
                rect.X = value.X - rect.Width / 2;
                rect.Y = value.Y - rect.Height / 2;
            }
        }

        /// <summary>
        /// Gets or sets the opacity of the button.
        /// </summary>
        public float Opacity
        {
            get { return opacity; }
            set
            {
                opacity = value;
                Visible = opacity > 0.0f;
            }
        }

        /// <summary>
        /// Initializes a new instance of Button
        /// </summary>
        /// <param name="released">Image shown when the button is up</param>
        /// <param name="pressed">Image shown when the button is down</param>
        public Button(Texture2D released, Texture2D pressed)
        {
            releasedImg = released;
            pressedImg = pressed;
            rect = released.Bounds;
            Location = new Point(0, 0);
            touchIds = new List<int>();
            Enabled = true;
            Visible = true;
        }

        /// <summary>
        /// Enables the button
        /// </summary>
        public void Enable()
        {
            Enabled = true;
            opacity = 1.0f;
        }

        /// <summary>
        /// Disables the button
        /// </summary>
        public void Disable()
        {
            Enabled = false;
            opacity = 0.5f;
        }

        /// <summary>
        /// Determines whether a point is inside the button area
        /// </summary>
        /// <param name="position">Coordinates</param>
        public bool Contains(Point position)
        {
            return rect.Contains(position);
        }

        /// <summary>
        /// Determines whether a point is inside the button area
        /// </summary>
        /// <param name="position">Coordinates</param>
        public bool Contains(Vector2 position)
        {
            return Contains(new Point((int)position.X, (int)position.Y));
        }

        /// <summary>
        /// Draws an image representing the current state of the button.
        /// </summary>
        /// <param name="spriteBatch">Batch to add the sprite into</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
                // Choose and draw image accoriding to the state of the button and visibility
                spriteBatch.Draw(pressed ? pressedImg : releasedImg, rect, Color.White * opacity);
        }

        /// <summary>
        /// Handles the touch events
        /// </summary>
        /// <param name="touch">Touch information concerning an individual touch point</param>
        public void TouchEvent(TouchLocation touch)
        {
            switch (touch.State)
            {
                case TouchLocationState.Pressed:
                    Pressed(touch);
                    break;
                case TouchLocationState.Moved:
                    Moved(touch);
                    break;
                case TouchLocationState.Released:
                    Released(touch);
                    break;
            }
        }

        /// <summary>
        /// Handles the pressed (down) event if the touch postition is inside the button
        /// </summary>
        /// <param name="touch">Contains the information about the touch event</param>
        public void Pressed(TouchLocation touch)
        {
            if (!Enabled || !Visible)
                return;
            if (Contains(touch.Position))
            {
                if (!touchIds.Contains(touch.Id))
                    touchIds.Add(touch.Id);
                pressed = true;
            }
        }

        /// <summary>
        /// Handles the moved (drag) event. Checks whether the touch position enters or leaves the button area
        /// </summary>
        /// <param name="touch">Contains the information about the touch event</param>
        public void Moved(TouchLocation touch)
        {
            if (!Enabled || !Visible)
                return;
            if (Contains(touch.Position))
            {
                pressed = true;
                if (!touchIds.Contains(touch.Id))
                    touchIds.Add(touch.Id);
            }
            else if (touchIds.Contains(touch.Id))
            {
                pressed = false;
                touchIds.Remove(touch.Id);
            }
        }

        /// <summary>
        /// Handles the released (up) event.
        /// Triggers Clicked event if released event happens inside the button and
        /// none of the other existing pointer is still holding the button down
        /// </summary>
        /// <param name="touch">Contains the information about the touch event</param>
        public void Released(TouchLocation touch)
        {
            if (!Enabled || !Visible)
                return;
            if (touchIds.Contains(touch.Id))
            {
                pressed = false;
                touchIds.Remove(touch.Id);
                if (Contains(touch.Position) && touchIds.Count == 0)
                {
                    Clicked(this, new EventArgs());
                }
            }
        }
    }
}
