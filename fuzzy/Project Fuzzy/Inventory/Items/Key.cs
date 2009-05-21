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
        public Key(Game game, string imageName, string itemName)
            :base(game, imageName, itemName)
        {

        }

        public override void Use()
        {
            

            base.Use();
        }

    }
}
