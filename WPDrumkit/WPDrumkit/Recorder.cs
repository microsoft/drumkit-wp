/*
 * Copyright (c) 2011-2014 Microsoft Mobile. All rights reserved.
 * See the license file delivered with this project for more information.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework.Audio;

namespace WPDrumkit
{
    /// <summary>
    /// Recorder saves a drum sequence played by user
    /// </summary>
    class Recorder
    {
        /// <summary>
        /// Struct for saving an individual drum stroke
        /// </summary>
        struct Sample
        {
            public long timestamp;
            public Pad pad;
            public SoundEffect sound;

            public Sample(long timestamp, Pad pad)
            {
                this.timestamp = timestamp;
                this.pad = pad;
                this.sound = pad.Sound;
            }
        }

        public event EventHandler TapeEnded = delegate { };
        private List<Sample> tape = new List<Sample>();
        private long started = 0;
        private int position = 0;

        public bool Recording { get; private set; }
        public bool Playing { get; private set; }

        public bool Empty
        {
            get { return tape.Count == 0; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Recorder()
        {
            Playing = false;
            Recording = false;
        }

        /// <summary>
        /// Starts playback
        /// </summary>
        public void Play()
        {
            Playing = true;
            started = GetTimestamp();
        }

        /// <summary>
        /// Stops both recording and playback
        /// </summary>
        public void Stop()
        {
            Recording = false;
            Playing = false;
            position = 0;
        }

        /// <summary>
        /// Starts recording
        /// </summary>
        public void Record()
        {
            tape.Clear();
            Recording = true;
        }

        /// <summary>
        /// Saves a drum stroke
        /// </summary>        
        public void RecordSample(Pad pad)
        {
            if (Recording)
                tape.Add(new Sample(GetTimestamp(), pad));
        }

        /// <summary>
        /// Plays the current strokes
        /// </summary>
        public void Update()
        {
            while (position < tape.Count && GetTimestamp() >= started + tape[position].timestamp - tape[0].timestamp)
            {
                tape[position].pad.HasBeenHit = true;
                tape[position].sound.Play();
                position++;
            }

            if (position >= tape.Count)
            {
                Stop();
                this.TapeEnded(this, new EventArgs());
            }
        }

        /// <summary>
        /// Returns the current timestamp
        /// </summary>        
        private long GetTimestamp()
        {
            return DateTime.Now.Ticks;
        }
    }
}
