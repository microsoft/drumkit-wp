/*
 * Copyright © 2011-2012 Nokia Corporation. All rights reserved.
 * Nokia and Nokia Connecting People are registered trademarks of Nokia Corporation. 
 * Other product and company names mentioned herein may be trademarks
 * or trade names of their respective owners. 
 * See LICENSE.TXT for license information.
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
