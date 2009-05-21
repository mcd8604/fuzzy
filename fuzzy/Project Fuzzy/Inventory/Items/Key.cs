using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project_Fuzzy.Inventory.Items
{
    public class Key : Inventory.Item
    {
        public Key(Game game, string imageName, string itemName, bool inRemoveable)
            : base(game, imageName, itemName, inRemoveable)
        {

        }

        public override bool Use()
        {
            Console.WriteLine("KEY USE");

            foreach (InteractiveComponent model in interactives)
            {
                Console.WriteLine(model.ModelName);
            }
        

            base.Use();

            return true;
        }

    }
}
