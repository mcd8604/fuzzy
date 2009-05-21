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

        public override bool Use(Avatar player)
        {
            Console.WriteLine("KEY USE");
            bool retVal = false;
            List<InteractiveComponent> itemsToRemove = new List<InteractiveComponent>();

            foreach (InteractiveComponent model in interactives)
            {
                if (model.inRange(player.Position))
                {
                    model.Model.SetCollidable(false);
                    itemsToRemove.Add(model);
                    retVal = true;
                }
            }

            while (itemsToRemove.Count != 0)
            {
                Game.Components.Remove(itemsToRemove[0]);
                Game1.interactiveModelList.Remove(itemsToRemove[0]);
                itemsToRemove.Remove(itemsToRemove[0]);
            }

            base.Use(player);

            return retVal;
        }

    }
}
