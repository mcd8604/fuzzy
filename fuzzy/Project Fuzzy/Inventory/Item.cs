using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Project_Fuzzy.Inventory
{
    class Item
    {
        //protected ModelComponent model;

        protected Texture2D uiImage;

        protected string name;

        protected Vector2 position;

        // If length == 0, useable anywhere
        protected List<InteractiveComponent> interactives;

        public Item(Texture2D image, string itemName)
        {
            uiImage = image;
            name = itemName;
        }

        public virtual void Use()
        {
            // Do Stuff
        }

        public Texture2D UIImage
        {
            get { return uiImage; }
            set { uiImage = value; }

        }

        public string Name
        {
            get { return name; }
            set { name = value; }

        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }

        }
    }
}
