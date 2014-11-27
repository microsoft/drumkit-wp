/*
 * Copyright (c) 2011-2014 Microsoft Mobile. All rights reserved.
 * See the license file delivered with this project for more information.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace WPDrumkit
{
    public enum Percussion { KICK, SNARE, TOM1, TOM2, TOM3, HIHAT_CLOSED, HIHAT_OPEN, CRASH, CHINA, SPLASH, RIDE, RIDE_BELL, COWBELL }

    /// <summary>
    /// Main class - does the app initialization procedures and implements the game loop
    /// </summary>
    public class Main : Microsoft.Xna.Framework.Game
    {
        #region Fields

        int displayWidth;
        int displayHeight;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        InputState inputState;
        PadView padView;
        DrumsetView drumsetView;
        View activeView;
        Dictionary<Percussion, SoundEffect> percussions;
        Recorder recorder;

#if DEBUG // Define FPS variables
        String fpsInfo = "";
        float deltaFPSTime = 0;
        float fps = 0;
#endif
        #endregion

        #region Initialization

        /// <summary>
        /// Constructor
        /// </summary>
        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            inputState = new InputState();
            recorder = new Recorder();
            Pad.Played += new EventHandler(Pad_Played);

            // Frame rate is 30 fps (=333333 ticks) by default for Windows Phone 7. However later
            // Windows Phone releases should support 60 fps, so we'll try that
            TargetElapsedTime = TimeSpan.FromTicks(166666);
            System.Diagnostics.Debug.WriteLine("Target elapsed time " + TargetElapsedTime.ToString());
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            displayWidth = GraphicsDevice.Viewport.Width;
            displayHeight = GraphicsDevice.Viewport.Height;

            TouchPanel.EnabledGestures = GestureType.Hold;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Visual.LoadContent(Content, GraphicsDevice);

            // Load drum samples
            percussions = new Dictionary<Percussion, SoundEffect>();
            percussions.Add(Percussion.KICK, Content.Load<SoundEffect>("Audio/kick"));
            percussions.Add(Percussion.SNARE, Content.Load<SoundEffect>("Audio/snare"));
            percussions.Add(Percussion.TOM1, Content.Load<SoundEffect>("Audio/tom1"));
            percussions.Add(Percussion.TOM2, Content.Load<SoundEffect>("Audio/tom2"));
            percussions.Add(Percussion.TOM3, Content.Load<SoundEffect>("Audio/tom3"));
            percussions.Add(Percussion.HIHAT_CLOSED, Content.Load<SoundEffect>("Audio/hihat1"));
            percussions.Add(Percussion.HIHAT_OPEN, Content.Load<SoundEffect>("Audio/hihat2"));
            percussions.Add(Percussion.CRASH, Content.Load<SoundEffect>("Audio/crash"));
            percussions.Add(Percussion.CHINA, Content.Load<SoundEffect>("Audio/china"));
            percussions.Add(Percussion.SPLASH, Content.Load<SoundEffect>("Audio/splash"));
            percussions.Add(Percussion.RIDE, Content.Load<SoundEffect>("Audio/ride1"));
            percussions.Add(Percussion.RIDE_BELL, Content.Load<SoundEffect>("Audio/ride2"));
            percussions.Add(Percussion.COWBELL, Content.Load<SoundEffect>("Audio/cowbell"));

            // Create views
            padView = new PadView(Content, percussions, recorder, displayWidth, displayHeight);
            padView.Exit += new EventHandler(Exit);
            padView.ViewButtonClicked += new EventHandler(padView_ViewButtonClicked);

            drumsetView = new DrumsetView(Content, percussions, recorder, displayWidth, displayHeight);
            drumsetView.Exit += new EventHandler(Exit);
            drumsetView.ViewButtonClicked += new EventHandler(drumsetView_ViewButtonClicked);

            activeView = padView;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // ContentManager will handle unloading the content
        }

        #endregion

        #region Loop

        /// <summary>
        /// Updates the game state
        /// </summary>        
        protected override void Update(GameTime gameTime)
        {
            inputState.Update();
            HandleInput(inputState);

#if DEBUG // Calculate FPS
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            fps = 1 / elapsed;
            deltaFPSTime += elapsed;

            if (deltaFPSTime > 1)
            {
                fpsInfo = "Running at <" + fps.ToString() + "> FPS." + (gameTime.IsRunningSlowly ? " Running slowly." : "");
                deltaFPSTime -= 1;
            }
#endif
            if (recorder.Playing)
                recorder.Update();

            activeView.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            activeView.Draw(spriteBatch);
#if DEBUG // Print FPS info
            spriteBatch.DrawString(Visual.defaultFont, fpsInfo, new Vector2(20, 450), Visual.defaultFontColor);
#endif
            spriteBatch.End();
            base.Draw(gameTime);
        }

        #endregion

        #region EventHandlers

        private void Pad_Played(object sender, EventArgs e)
        {
            if (sender is Pad)
                recorder.RecordSample((Pad)sender);
        }

        /// <summary>
        /// Changes view to drumset view
        /// </summary>        
        private void padView_ViewButtonClicked(object sender, EventArgs e)
        {
            activeView = drumsetView;
        }

        /// <summary>
        /// Changes view to pad view
        /// </summary>        
        private void drumsetView_ViewButtonClicked(object sender, EventArgs e)
        {
            activeView = padView;
        }

        /// <summary>
        /// Exits the application
        /// </summary>        
        private void Exit(object sender, EventArgs e)
        {
            this.Exit();
        }

        #endregion

        /// <summary>
        /// Delivers touch input to the active views
        /// </summary>
        /// <param name="input"></param>
        private void HandleInput(InputState input)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            foreach (TouchLocation touch in input.touchState)
            {
                activeView.TouchEvent(touch);
            }
            foreach (GestureSample gesture in input.Gestures)
            {
                activeView.TouchEvent(gesture);
            }
        }
    }
}
