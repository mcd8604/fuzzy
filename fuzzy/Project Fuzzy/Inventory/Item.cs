using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Project_Fuzzy.Inventory
{
    class Item
    {
        //protected ModelComponent model;

        protected Texture2D uiImage;

        protected string name;

        // If length == 0, useable anywhere
        protected List<InteractiveComponent> interactives;

        public virtual void Use()
        {
            // Do Stuff
        }
    }
}
