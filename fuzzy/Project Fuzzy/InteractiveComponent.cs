using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project_Fuzzy
{
    public class InteractiveComponent : ModelComponent
    {

        private const float RANGE = 10;
        private bool isObtainable;
        private string modelName;
        private string textureName;

        public string TextureName
        {
            get { return textureName; }
        }

        public InteractiveComponent(Game game, string name, bool obtainable, string textureName)
            : base(game, name)
        {
            modelName = name;
            isObtainable = obtainable;
            this.textureName = textureName;
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


    }
}
