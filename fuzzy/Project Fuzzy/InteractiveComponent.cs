using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project_Fuzzy
{
    class InteractiveComponent : ModelComponent
    {

        private const float RANGE = 10;
        private bool isObtainable;
        private string modelName;
        private string textureName;
        private Texture2D modelTextureForInventory;

        public InteractiveComponent(Game game, string name, bool obtainable, string textureName)
            : base(game, name)
        {
            modelName = name;
            isObtainable = obtainable;
            this.textureName = textureName;
        }

        protected override void LoadContent()
        {
            if (!String.IsNullOrEmpty(textureName))
                modelTextureForInventory = Game.Content.Load<Texture2D>(textureName);

            base.LoadContent();
        }

        public bool inRange(Vector3 ballPosition)
        {
            bool retVal = false;

            if (Vector3.Distance(ballPosition, position) <= RANGE)
            {
                retVal = true;
            }

            return retVal;
        }

        public bool Obtainable
        {
            get { return isObtainable; }
            set { isObtainable = value; }

        }

        public string ModelName
        {
            get { return modelName; }
            set { modelName = value; }

        }

        public Texture2D ModelTextureForInventory
        {
            get { return modelTextureForInventory; }
            set { modelTextureForInventory = value; }

        }


    }
}
