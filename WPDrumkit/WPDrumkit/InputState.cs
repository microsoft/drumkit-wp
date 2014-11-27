/*
 * Copyright (c) 2011-2014 Microsoft Mobile. All rights reserved.
 * See the license file delivered with this project for more information.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input.Touch;

namespace WPDrumkit
{
    /// <summary>
    /// Class that contains the latest touch input information
    /// </summary>
    public class InputState
    {
        public TouchCollection touchState;
        public readonly List<GestureSample> Gestures = new List<GestureSample>();

        public void Update()
        {
            touchState = TouchPanel.GetState();
            Gestures.Clear();
            while (TouchPanel.IsGestureAvailable)
                Gestures.Add(TouchPanel.ReadGesture());
        }
    }
}
