/*
 * Copyright (c) 2011-2014 Microsoft Mobile. All rights reserved.
 * See the license file delivered with this project for more information.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace WPDrumkit
{ 
    /// <summary>
    /// A round menu for changing drum samples
    /// </summary>
    class PercussionMenu
    {
        private Timer animator;
        private TimerCallback show;
        private TimerCallback hide;
        private Button close;
        private Point destination; //Location for where the menu winds up to
        private int maxRadius = 160;
        private int radius = 0;
        private double angle = 0;        
        private bool open = false;

        public List<Button> Buttons { get; set; }
        public Point Location { get; set; }
        public bool Visible { get; set; }

        public Point Destination
        {
            get { return destination; }
            set
            {
                destination = value;
                close.Center = destination;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="closeImg">Image for closing button</param>
        public PercussionMenu(Texture2D closeImg)
        {
            Buttons = new List<Button>();
            show = new TimerCallback(ShowCallback);
            hide = new TimerCallback(HideCallback);            
            close = new Button(closeImg, closeImg);
            close.Clicked += new EventHandler(Close_Clicked);
            Visible = false;
        }       

        /// <summary>
        /// Adds a button to the menu
        /// </summary>        
        public void Add(Button button)
        {
            button.Opacity = 0.0f;
            Buttons.Add(button);
            angle = 360 / Buttons.Count * Math.PI / 180;
        }

        /// <summary>
        /// Opens the menu
        /// </summary>
        public void Show()
        {
            Visible = true;
            if (animator != null) animator.Dispose();
            AutoResetEvent reset = new AutoResetEvent(false);
            animator = new Timer(show, reset, 0L, 30L);
        }

        /// <summary>
        /// Closes the menu
        /// </summary>
        public void Hide()
        {
            open = false;
            if (animator != null) animator.Dispose();
            AutoResetEvent reset = new AutoResetEvent(false);
            animator = new Timer(hide, reset, 0L, 30L);
        }

        /// <summary>
        /// Calculates new locations for buttons
        /// </summary>
        public void Update()
        {
            float ratio = (float)radius / maxRadius;
            foreach (Button button in Buttons)
                button.Opacity = ratio;

            int newX = Location.X + (int)((destination.X - Location.X) * ratio);
            int newY = Location.Y + (int)((double)(destination.Y - Location.Y) * ratio);
            int btnX = 0;
            int btnY = 0;

            // AngleExtra is used to create the spinning effect
            double angleExtra = 8 * ratio;

            for (int i = 0; i < Buttons.Count; i++)
            {
                btnX = (int)(Math.Cos(i * angle + angleExtra) * radius);
                btnY = (int)(Math.Sin(i * angle + angleExtra) * radius);
                Buttons[i].Center = new Point(btnX + newX, btnY + newY);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                if (open)
                    close.Draw(spriteBatch);

                foreach (Button button in Buttons)
                    button.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Handles the touch events
        /// </summary>
        /// <param name="touch">Touch information concerning an individual touch point</param>
        public void TouchEvent(TouchLocation touch)
        {
            if (open)
            {
                close.TouchEvent(touch);
                foreach (Button button in Buttons)
                    button.TouchEvent(touch);
            }
        }
        
        /// <summary>
        /// Animates opening the menu
        /// </summary>
        private void ShowCallback(Object stateInfo)
        {
            if (radius < maxRadius)
            {
                radius += 8;
                Update();
            }
            else
            {
                animator.Dispose();
                open = true;
            }
        }

        /// <summary>
        /// Animates closing the menu
        /// </summary>
        private void HideCallback(Object stateInfo)
        {
            if (radius > 0)
            {
                radius -= 8;
                Update();
            }
            else
            {
                radius = 0;
                Visible = false;
                animator.Dispose();
            }
        }

        /// <summary>
        /// Closes the menu
        /// </summary>        
        private void Close_Clicked(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
