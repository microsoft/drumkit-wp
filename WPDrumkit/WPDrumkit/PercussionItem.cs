/*
 * Copyright (c) 2011-2014 Microsoft Mobile. All rights reserved.
 * See the license file delivered with this project for more information.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace WPDrumkit
{
    /// <summary>
    /// Percussion menu item
    /// </summary>
    class PercussionItem : ToggleButton
    {
        public static event EventHandler Selected = delegate { };

        public Percussion Percussion { get; private set; }

        public PercussionItem(Texture2D released, Texture2D pressed,
            Percussion percussion)
            : base(released, pressed, pressed)
        {
            Percussion = percussion;
            Clicked += new EventHandler(PercussionItem_Clicked);
            // Since only one percussion item can be selected at the time, a static Selected event
            // is used to notify other Percussion items to set themselves to unselected state
            Selected += new EventHandler(PercussionItem_Selected);
        }

        /// <summary>
        /// Selects the percussion item
        /// </summary>        
        public void PercussionItem_Clicked(object sender, EventArgs e)
        {            
            On = true;
            Selected(this, new EventArgs());
        }

        /// <summary>
        /// Unselects all other percussion items
        /// </summary>        
        private void PercussionItem_Selected(object sender, EventArgs e)
        {
            if (sender != this)
                On = false;
        }
    }
}
