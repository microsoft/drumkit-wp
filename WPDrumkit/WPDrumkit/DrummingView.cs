/*
 * Copyright (c) 2011-2014 Microsoft Mobile. All rights reserved.
 * See the license file delivered with this project for more information.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace WPDrumkit
{
    /// <summary>
    /// Drumming base view. Contains things that are common to PadView and DrumsetView such as recording etc.
    /// </summary>
    abstract class DrummingView : View
    {
        protected Texture2D background;
        protected List<Button> buttons;
        public List<Pad> pads;
        protected Button infoButton;
        protected Button viewButton;
        protected Button playButton;
        protected Button stopButton;
        protected Button exitButton;
        protected AnimatedToggleButton recordButton;
        protected Recorder recorder;
        protected InfoOverlay info;

        protected bool showInfo;
        public event EventHandler ViewButtonClicked = delegate { };

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="content">Access to resources</param>
        /// <param name="recorder">Records drum strokes</param>
        /// <param name="background">Background image</param>
        /// <param name="width">Width of the screen</param>
        /// <param name="height">Height of the screen</param>
        public DrummingView(ContentManager content, Recorder recorder, Texture2D background, int width, int height)
            : base(width, height)
        {
            this.background = background;
            this.recorder = recorder;

            recorder.TapeEnded += new EventHandler(Recorder_TapeEnded);

            // Create buttons
            buttons = new List<Button>();
            infoButton = new Button(content.Load<Texture2D>("Images/Buttons/info"), content.Load<Texture2D>("Images/Buttons/info_pressed"));
            viewButton = new Button(content.Load<Texture2D>("Images/Buttons/pads"), content.Load<Texture2D>("Images/Buttons/pads_pressed"));
            recordButton = new AnimatedToggleButton(content.Load<Texture2D>("Images/Buttons/record"), content.Load<Texture2D>("Images/Buttons/record_pressed"),
                content.Load<Texture2D>("Images/Buttons/recording"), 18, 18);
            playButton = new Button(content.Load<Texture2D>("Images/Buttons/play"), content.Load<Texture2D>("Images/Buttons/play_pressed"));
            stopButton = new Button(content.Load<Texture2D>("Images/Buttons/stop"), content.Load<Texture2D>("Images/Buttons/stop_pressed"));
            exitButton = new Button(content.Load<Texture2D>("Images/Buttons/exit"), content.Load<Texture2D>("Images/Buttons/exit_pressed"));

            // Attach event handlers
            infoButton.Clicked += new EventHandler(InfoButton_Clicked);
            viewButton.Clicked += new EventHandler(viewButton_Clicked);
            recordButton.Clicked += new EventHandler(RecordButton_Clicked);
            playButton.Clicked += new EventHandler(PlayButton_Clicked);
            stopButton.Clicked += new EventHandler(StopButton_Clicked);
            exitButton.Clicked += new EventHandler(ExitButton_Clicked);

            playButton.Disable();
            stopButton.Visible = false;

            // Add buttons to a list for easy looping
            buttons.Add(infoButton);
            buttons.Add(viewButton);
            buttons.Add(recordButton);
            buttons.Add(playButton);
            buttons.Add(stopButton);
            buttons.Add(exitButton);

            info = new InfoOverlay(new Rectangle(0, 0, width, height));
        }

        public override void Update(GameTime gameTime)
        {
            if (recordButton.On)
            {
                // Update recording button animation
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                recordButton.UpdateFrame(elapsed);
            }
        }

        /// <summary>
        /// Shows info overlay
        /// </summary>        
        private void InfoButton_Clicked(object sender, EventArgs e)
        {
            showInfo = true;
        }

        /// <summary>
        /// Prepares the viewChange
        /// </summary>        
        private void viewButton_Clicked(object sender, EventArgs e)
        {
            if (!recorder.Empty)
                playButton.Enable();
            recorder.Stop();
            recordButton.Off = true;
            ViewButtonClicked(this, new EventArgs());
        }

        /// <summary>
        /// Starts recording
        /// </summary>        
        private void RecordButton_Clicked(object sender, EventArgs e)
        {
            if (recordButton.On)
            {
                playButton.Disable();
                recorder.Record();
            }
            else
            {
                if (!recorder.Empty) playButton.Enable();
                recorder.Stop();
            }
        }

        /// <summary>
        /// Starts playback
        /// </summary>        
        private void PlayButton_Clicked(object sender, EventArgs e)
        {
            recordButton.Disable();
            playButton.Visible = false;
            stopButton.Visible = true;
            recorder.Play();
        }

        /// <summary>
        /// Stops playback
        /// </summary>        
        private void StopButton_Clicked(object sender, EventArgs e)
        {
            recorder.Stop();
            EnableButtons();
        }

        /// <summary>
        /// Exits the app
        /// </summary>        
        private void ExitButton_Clicked(object sender, EventArgs e)
        {
            exit();
        }

        private void Recorder_TapeEnded(object sender, EventArgs e)
        {
            EnableButtons();
        }

        /// <summary>
        /// Enables recording and playback buttons
        /// </summary>
        private void EnableButtons()
        {
            recordButton.Enable();
            playButton.Visible = true;
            stopButton.Visible = false;
        }
    }
}
