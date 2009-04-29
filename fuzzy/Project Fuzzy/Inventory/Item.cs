using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Project_Fuzzy.Inventory
{
    public class Item : DrawableGameComponent
    {
        //protected ModelComponent model;

        protected string imageName;
        protected Texture2D uiImage;

        protected string name;

        protected Vector2 position;

        // If length == 0, useable anywhere
        protected List<InteractiveComponent> interactives;

        public Item(Game game, string imageName, string itemName)
            : base(game)
        {
            this.imageName = imageName;
            name = itemName;
        }

        protected override void LoadContent()
        {
            if(!string.IsNullOrEmpty(imageName))
                uiImage = this.Game.Content.Load<Texture2D>(imageName);

            base.LoadContent();
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
