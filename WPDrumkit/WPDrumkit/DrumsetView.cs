/*
 * Copyright (c) 2011-2014 Microsoft Mobile. All rights reserved.
 * See the license file delivered with this project for more information.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace WPDrumkit
{
    /// <summary>
    /// 3D like view presenting the full drumset. Extends DrummingView.
    /// </summary>
    class DrumsetView : DrummingView
    {
        Texture2D locks;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="content">Access to resources</param>
        /// <param name="percussions">List of percussion samples</param>
        /// <param name="recorder">Records drum strokes</param>        
        /// <param name="width">Width of the screen</param>
        /// <param name="height">Height of the screen</param>                
        public DrumsetView(ContentManager content, Dictionary<Percussion, SoundEffect> percussions, Recorder recorder, int width, int height)
            : base(content, recorder, content.Load<Texture2D>("Images/Drumset/background"), width, height)
        {
            locks = content.Load<Texture2D>("Images/Drumset/locks");

            pads = new List<Pad>();
            List<Rectangle> touchAreas = new List<Rectangle>();

            #region Initialize pads and their touch areas
            touchAreas = new List<Rectangle>();
            touchAreas.Add(new Rectangle(318, 60, 154, 69));
            Cymbal splash = new Cymbal(Percussion.SPLASH, percussions[Percussion.SPLASH], new Rectangle(309, 57, 168, 71), touchAreas, content.Load<Texture2D>("Images/Drumset/splash"));
            splash.Origin = new Vector2(84f, 8f);
            splash.MaxAngle = 0.2f;
            pads.Add(splash);

            touchAreas = new List<Rectangle>();
            touchAreas.Add(new Rectangle(221, 159, 132, 134));
            pads.Add(new Drum(Percussion.TOM1, percussions[Percussion.TOM1], new Rectangle(217, 155, 135, 168), touchAreas, content.Load<Texture2D>("Images/Drumset/tom1")));

            touchAreas = new List<Rectangle>();
            touchAreas.Add(new Rectangle(355, 149, 141, 132));
            pads.Add(new Drum(Percussion.TOM2, percussions[Percussion.TOM2], new Rectangle(356, 149, 150, 185), touchAreas, content.Load<Texture2D>("Images/Drumset/tom2")));

            touchAreas = new List<Rectangle>();
            touchAreas.Add(new Rectangle(475, 244, 169, 110));
            touchAreas.Add(new Rectangle(496, 358, 102, 35));
            pads.Add(new Drum(Percussion.TOM3, percussions[Percussion.TOM3], new Rectangle(471, 240, 179, 251), touchAreas, content.Load<Texture2D>("Images/Drumset/tom3")));

            touchAreas = new List<Rectangle>();
            touchAreas.Add(new Rectangle(0, 110, 144, 66));
            touchAreas.Add(new Rectangle(16, 154, 206, 85));
            Cymbal crash = new Cymbal(Percussion.CRASH, percussions[Percussion.CRASH], new Rectangle(-20, 92, 248, 145), touchAreas, content.Load<Texture2D>("Images/Drumset/crash"));
            crash.Origin = new Vector2(140f, 35f);
            crash.MaxAngle = 0.16f;
            pads.Add(crash);

            touchAreas = new List<Rectangle>();
            touchAreas.Add(new Rectangle(130, 40, 181, 76));
            touchAreas.Add(new Rectangle(185, 116, 136, 37));
            Cymbal china = new Cymbal(Percussion.CHINA, percussions[Percussion.CHINA], new Rectangle(114, 36, 213, 125), touchAreas, content.Load<Texture2D>("Images/Drumset/china"));
            china.Origin = new Vector2(110f, 46f);
            china.MaxAngle = 0.17f;
            pads.Add(china);

            touchAreas = new List<Rectangle>();
            touchAreas.Add(new Rectangle(467, 45, 274, 66));
            touchAreas.Add(new Rectangle(467, 112, 206, 32));
            Cymbal rideBell = new Cymbal(Percussion.RIDE_BELL, percussions[Percussion.RIDE_BELL], new Rectangle(464, 45, 279, 101), touchAreas, content.Load<Texture2D>("Images/Drumset/ride2"));
            rideBell.Origin = new Vector2(135f, 15f);
            rideBell.MaxAngle = -0.04f;
            pads.Add(rideBell);

            touchAreas = new List<Rectangle>();
            touchAreas.Add(new Rectangle(544, 144, 257, 92));
            touchAreas.Add(new Rectangle(678, 112, 122, 45));
            Cymbal ride = new Cymbal(Percussion.RIDE, percussions[Percussion.RIDE], new Rectangle(542, 108, 270, 125), touchAreas, content.Load<Texture2D>("Images/Drumset/ride1"));
            ride.Origin = new Vector2(125f, 27f);
            ride.MaxAngle = -0.08f;
            pads.Add(ride);

            touchAreas = new List<Rectangle>();
            touchAreas.Add(new Rectangle(642, 296, 0, 0));
            Cymbal hihatOpenL = new Cymbal(Percussion.HIHAT_OPEN, percussions[Percussion.HIHAT_OPEN], new Rectangle(634, 266, 207, 83), touchAreas, content.Load<Texture2D>("Images/Drumset/hihat_open_lower"));
            hihatOpenL.Origin = new Vector2(103f, 37f);
            hihatOpenL.MaxAngle = -0.06f;
            pads.Add(hihatOpenL); // lower cymbal

            touchAreas = new List<Rectangle>();
            touchAreas.Add(new Rectangle(641, 232, 159, 115));
            Cymbal hihatOpenU = new Cymbal(Percussion.HIHAT_OPEN, percussions[Percussion.HIHAT_OPEN], new Rectangle(634, 232, 216, 92), touchAreas, content.Load<Texture2D>("Images/Drumset/hihat_open_upper"));
            hihatOpenU.Origin = new Vector2(108f, 12f);
            hihatOpenU.MaxAngle = -0.14f;
            pads.Add(hihatOpenU); // upper cymbal

            touchAreas = new List<Rectangle>();
            touchAreas.Add(new Rectangle(643, 352, 157, 100));
            touchAreas.Add(new Rectangle(602, 364, 57, 69));
            Cymbal hihatClosed = new Cymbal(Percussion.HIHAT_CLOSED, percussions[Percussion.HIHAT_CLOSED], new Rectangle(591, 344, 229, 102), touchAreas, content.Load<Texture2D>("Images/Drumset/hihat_closed"));
            hihatClosed.Origin = new Vector2(115f, 13f);
            hihatClosed.MaxAngle = -0.01f;
            pads.Add(hihatClosed);

            touchAreas = new List<Rectangle>();
            touchAreas.Add(new Rectangle(16, 239, 196, 229));
            pads.Add(new Drum(Percussion.KICK, percussions[Percussion.KICK], new Rectangle(-10, 238, 250, 235), touchAreas, content.Load<Texture2D>("Images/Drumset/kick")));

            touchAreas = new List<Rectangle>();
            touchAreas.Add(new Rectangle(212, 292, 193, 122));
            pads.Add(new Drum(Percussion.SNARE, percussions[Percussion.SNARE], new Rectangle(212, 288, 197, 178), touchAreas, content.Load<Texture2D>("Images/Drumset/snare")));

            touchAreas = new List<Rectangle>();
            touchAreas.Add(new Rectangle(405, 354, 82, 108));
            touchAreas.Add(new Rectangle(463, 375, 50, 87));
            pads.Add(new Drum(Percussion.COWBELL, percussions[Percussion.COWBELL], new Rectangle(402, 353, 106, 107), touchAreas, content.Load<Texture2D>("Images/Drumset/cowbell")));
            #endregion

            infoButton.Location = new Point(-10, -10);
            viewButton.Location = new Point(66, -10);
            recordButton.Location = new Point(257, -10);
            playButton.Location = new Point(430, -10);
            stopButton.Location = new Point(430, -10);
            exitButton.Location = new Point(707, -10);

            playButton.Disable();
            stopButton.Visible = false;

            info.Message = "In this view you have actual drumset to play with.\n" +
                "Again, you may record your beats and play them afterwards.\n" +
                "All the percussions are available at your finger tips,\n" +
                "so there is no separate percussion palette in this view.\n\n" + 
                "Tap the screen to continue!";
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            foreach (Pad pad in pads)
            {
                pad.Draw(spriteBatch);
                pad.HasBeenHit = false;
            }
            spriteBatch.Draw(locks, new Vector2(0, 0), Color.White);
            foreach (Button button in buttons)
            {
                button.Draw(spriteBatch);
            }
            if (showInfo)
            {
                info.Draw(spriteBatch);
            }
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
            foreach (Button button in buttons)
            {
                button.TouchEvent(touch);
                if (button.Contains(touch.Position))
                    return;
            }
            if (touch.State == TouchLocationState.Pressed)
            {
                foreach (Pad pad in pads)
                {
                    if (pad.Contains(touch.Position))
                    {
                        pad.PlaySound();
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the gestures
        /// </summary>
        /// <param name="touch">Information concerning an individual gesture</param>
        public override void TouchEvent(GestureSample gesture)
        {
            // Do nothing
        }
    }
}
