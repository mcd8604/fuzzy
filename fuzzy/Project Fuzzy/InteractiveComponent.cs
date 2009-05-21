using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project_Fuzzy.Items;
using Project_Fuzzy.Inventory.Items;
using Project_Fuzzy.Inventory;

namespace Project_Fuzzy
{
    public class InteractiveComponent : ModelComponent
    {

        private const float RANGE = 10;
        private bool isObtainable;
        private string modelName;
        private string textureName;
        private int id;

        private Item modelItem;

        public string TextureName
        {
            get { return textureName; }
        }

        public InteractiveComponent(Game game, int inID, string name, bool obtainable, string textureName, bool inRemoveable)
            : base(game, name)
        {
            modelName = name;
            isObtainable = obtainable;
            this.textureName = textureName;

            id = inID;

            if (isObtainable)
            {
                switch (name)
                {
                    case "sphere": modelItem = new Sphere(this.Game, textureName, name, inRemoveable);
                        break;
                    case "key": modelItem = new Key(this.Game, textureName, name, inRemoveable);
                        break;
                    default: modelItem = new Item(this.Game, textureName, name, inRemoveable);
                        break;
                }
            }
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

        public int ModelID
        {
            get { return id; }
            set { id = value; }

        }

        public Item ModelItem
        {
            get { return modelItem; }
            set { modelItem = value; }

        }

    }
}
