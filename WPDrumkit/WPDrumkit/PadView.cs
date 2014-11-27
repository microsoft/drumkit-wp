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
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace WPDrumkit
{
    /// <summary>
    /// View with pads. Extends DrummingView.
    /// </summary>
    class PadView : DrummingView
    {
        protected Dictionary<Percussion, SoundEffect> percussions;
        private Texture2D splash;
        private PercussionMenu palette;
        private Pad activePad;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="content">Access to resources</param>
        /// <param name="percussions">List of percussion samples</param>
        /// <param name="recorder">Records drum strokes</param>        
        /// <param name="width">Width of the screen</param>
        /// <param name="height">Height of the screen</param>
        public PadView(ContentManager content, Dictionary<Percussion, SoundEffect> percussions, Recorder recorder, int width, int height)
            : base(content, recorder, content.Load<Texture2D>("Images/bg_pads"), width, height)
        {
            this.percussions = percussions;
            splash = content.Load<Texture2D>("Images/splash");

            #region Initialize percussion menu and pads

            palette = new PercussionMenu(content.Load<Texture2D>("Images/Menu/close"));
            palette.Destination = new Point(400, 240);
            palette.Add(new PercussionItem(content.Load<Texture2D>("Images/Menu/splash"), content.Load<Texture2D>("Images/Menu/splash_highlight"), Percussion.SPLASH));
            palette.Add(new PercussionItem(content.Load<Texture2D>("Images/Menu/ride1"), content.Load<Texture2D>("Images/Menu/ride1_highlight"), Percussion.RIDE));
            palette.Add(new PercussionItem(content.Load<Texture2D>("Images/Menu/ride2"), content.Load<Texture2D>("Images/Menu/ride2_highlight"), Percussion.RIDE_BELL));
            palette.Add(new PercussionItem(content.Load<Texture2D>("Images/Menu/tom1"), content.Load<Texture2D>("Images/Menu/tom1_highlight"), Percussion.TOM1));
            palette.Add(new PercussionItem(content.Load<Texture2D>("Images/Menu/tom2"), content.Load<Texture2D>("Images/Menu/tom2_highlight"), Percussion.TOM2));
            palette.Add(new PercussionItem(content.Load<Texture2D>("Images/Menu/tom3"), content.Load<Texture2D>("Images/Menu/tom3_highlight"), Percussion.TOM3));
            palette.Add(new PercussionItem(content.Load<Texture2D>("Images/Menu/kick"), content.Load<Texture2D>("Images/Menu/kick_highlight"), Percussion.KICK));
            palette.Add(new PercussionItem(content.Load<Texture2D>("Images/Menu/snare"), content.Load<Texture2D>("Images/Menu/snare_highlight"), Percussion.SNARE));
            palette.Add(new PercussionItem(content.Load<Texture2D>("Images/Menu/cowbell"), content.Load<Texture2D>("Images/Menu/cowbell_highlight"), Percussion.COWBELL));
            palette.Add(new PercussionItem(content.Load<Texture2D>("Images/Menu/hihat2"), content.Load<Texture2D>("Images/Menu/hihat2_highlight"), Percussion.HIHAT_CLOSED));
            palette.Add(new PercussionItem(content.Load<Texture2D>("Images/Menu/hihat1"), content.Load<Texture2D>("Images/Menu/hihat1_highlight"), Percussion.HIHAT_OPEN));
            palette.Add(new PercussionItem(content.Load<Texture2D>("Images/Menu/crash"), content.Load<Texture2D>("Images/Menu/crash_highlight"), Percussion.CRASH));
            palette.Add(new PercussionItem(content.Load<Texture2D>("Images/Menu/china"), content.Load<Texture2D>("Images/Menu/china_highlight"), Percussion.CHINA));

            pads = new List<Pad>();
            pads.Add(new Pad(Percussion.CRASH, percussions[Percussion.CRASH], new Rectangle(130, 76, 173, 172)));
            pads.Add(new Pad(Percussion.RIDE, percussions[Percussion.RIDE], new Rectangle(515, 71, 180, 170)));
            pads.Add(new Pad(Percussion.SNARE, percussions[Percussion.SNARE], new Rectangle(254, 322, 148, 148)));
            pads.Add(new Pad(Percussion.KICK, percussions[Percussion.KICK], new Rectangle(13, 252, 219, 211)));
            pads.Add(new Pad(Percussion.HIHAT_CLOSED, percussions[Percussion.HIHAT_CLOSED], new Rectangle(589, 247, 205, 192)));
            pads.Add(new Pad(Percussion.TOM1, percussions[Percussion.TOM1], new Rectangle(317, 132, 188, 188)));
            pads.Add(new Pad(Percussion.TOM3, percussions[Percussion.TOM3], new Rectangle(435, 325, 146, 145)));

            #endregion

            PercussionItem.Selected += new EventHandler(PercussionItem_Selected);

            infoButton.Location = new Point(0, 0);
            viewButton.Location = new Point(0, 103);
            recordButton.Location = new Point(297, 16);
            playButton.Location = new Point(400, 16);
            stopButton.Location = new Point(400, 16);
            exitButton.Location = new Point(697, 0);

            info.Message = "Thanks for using Drumkit!\n\n" + "Let your fills fly by tapping the pads.\n" +
                "You may also record your beats and play them afterwards.\n" +
                "Long tap on a pad lets you select different instruments.\n\n" +
                "Tap the screen to continue!";
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            foreach (Pad pad in pads)
            {
                if (pad.HasBeenHit)
                    spriteBatch.Draw(splash, pad.Boundaries, Color.White);
                pad.HasBeenHit = false;
            }
            foreach (Button button in buttons)
            {
                button.Draw(spriteBatch);
            }
            if (palette.Visible)
            {
                palette.Draw(spriteBatch);
            }
            if (showInfo)
                info.Draw(spriteBatch);
        }

        /// <summary>
        /// Handles the touch events
        /// </summary>
        /// <param name="touch">Touch information concerning an individual touch point</param>
        public override void TouchEvent(TouchLocation touch)
        {
            if (showInfo)
            {
                if (touch.State == TouchLocationState.Released) showInfo = false;
                return;
            }
            if (palette.Visible)
            {
                palette.TouchEvent(touch);
                return;
            }
            if (touch.State == TouchLocationState.Pressed)
            {
                foreach (Pad pad in pads)
                {
                    if (pad.Contains(touch.Position))
                    {
                        pad.PlaySound();
                    }
                }
            }
            foreach (Button button in buttons)
            {
                button.TouchEvent(touch);
            }
        }

        /// <summary>
        /// Handles gestures
        /// </summary>
        /// <param name="gesture">Information concerning an individual gesture</param>
        public override void TouchEvent(GestureSample gesture)
        {
            if (gesture.GestureType == GestureType.Hold)
            {
                foreach (Pad pad in pads)
                {
                    if (pad.Contains(gesture.Position) && !palette.Visible)
                    {
                        activePad = pad;
                        palette.Location = new Point((int)gesture.Position.X, (int)gesture.Position.Y);
                        Point displayCenter = new Point(width / 2, height / 2);
                        int destX = displayCenter.X - (int)(0.5 * (displayCenter.X - pad.Boundaries.Center.X));
                        int destY = displayCenter.Y - (int)(0.3 * (displayCenter.Y - pad.Boundaries.Center.Y));
                        palette.Destination = new Point(destX, destY);
                        // Set the preselected percussion
                        foreach (PercussionItem item in palette.Buttons)
                        {
                            item.On = item.Percussion == pad.Percussion ? true : false;
                        }
                        palette.Show();
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Sets the pad sound according to the menu selection
        /// </summary>
        private void PercussionItem_Selected(object sender, EventArgs e)
        {
            activePad.Percussion = ((PercussionItem)sender).Percussion;
            activePad.Sound = percussions[activePad.Percussion];
            activePad.PlaySound();
        }
    }
}
