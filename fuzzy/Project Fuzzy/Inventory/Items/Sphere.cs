using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project_Fuzzy.Items
{
    public class Sphere : Inventory.Item
    {
        public Sphere(Game game, string imageName, string itemName, bool inRemoveable)
            : base(game, imageName, itemName, inRemoveable)
        {

        }

        public override bool Use(Avatar player)
        {
            Console.WriteLine("SPEHERE USE");

            base.Use(player);

            return true;
        }

    }
}
