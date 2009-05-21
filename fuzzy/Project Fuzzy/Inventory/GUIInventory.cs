using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using Project_Fuzzy.Items;
using Project_Fuzzy.Inventory.Items;


namespace Project_Fuzzy.Inventory
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class GUIInventory : DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        List<Item> itemList;

        public GUIInventory(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            itemList = new List<Item>();

           addItem(@"inventory", "inventory", new Vector2(500, 300));
            

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(this.Game.GraphicsDevice); 

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            foreach (Item image in itemList)
            {
                if(image.UIImage != null)
                    spriteBatch.Draw(image.UIImage, image.Position, Color.White);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        /// <summary>
        /// Adds the item to the inventory list
        /// </summary>
        /// <param name="name2D"></param>
        /// <param name="name"></param>
        public void addItem(string name2D, string name)
        {
            Item item;
            switch (name)
            {
                case "sphere": item = new Sphere(this.Game, name2D, name);
                    break;
                case "key": item = new Key(this.Game, name2D, name);
                    break;
                default: item = new Item(this.Game, name2D, name);
                    break;
            }
            

            int yOffset = ((itemList.Count-1) / 5);
            int xOffset = ((itemList.Count-1) % 5);

            item.Position = new Vector2((xOffset * 50) + getItemByName("inventory").Position.X, (yOffset * 50) + getItemByName("inventory").Position.Y);
            item.DrawOrder = this.DrawOrder + itemList.Count;

            itemList.Add(item);
            this.Game.Components.Add(item);

        }

        public void rePositionItems()
        {
            int yOffset = 0;
            int xOffset = 0;
            int count = 0;
            foreach (Item item in itemList)
            {
                if(count > 0){ //Skips the inventory item
                    item.Position = new Vector2((xOffset * 50) + getItemByName("inventory").Position.X, (yOffset * 50) + getItemByName("inventory").Position.Y);
                    item.DrawOrder = this.DrawOrder + itemList.Count;

                    yOffset = ((count) / 5);
                    xOffset = ((count) % 5);
                }
                count++;
            }

        }

        /// <summary>
        /// Adds the item to the inventory list
        /// </summary>
        /// <param name="name2D"></param>
        /// <param name="name"></param>
        public void addItem(string name2D, string name, Vector2 pos)
        {
            Item item = new Item(this.Game, name2D, name, pos);

            item.DrawOrder = this.DrawOrder + itemList.Count;

            itemList.Add(item);
            this.Game.Components.Add(item);

        }

        /// <summary>
        /// Removes the item from the array with a certain name
        /// </summary>
        /// <param name="name"></param>
        public bool removeItem(int index)
        {
            if (itemList.Count > (index + 1))
            {
                this.Game.Components.Remove(itemList[index+1]);
                itemList.RemoveAt(index+1);
                return true;
            }
            return false;

        }

        public bool checkIfItemClicked(int xVal, int yVal)
        {
            bool retVal = false;

            Item tempInventory = getItemByName("inventory");

            float curXVal = xVal - tempInventory.Position.X;
            float curYVal = yVal - tempInventory.Position.Y;

            if (xVal - tempInventory.Position.X >= 0 && yVal - tempInventory.Position.Y >= 0)
            {
                Console.WriteLine(xVal + "::" + yVal);
                Console.WriteLine("Clicked in Inventory at X: " + (xVal - tempInventory.Position.X) + " Y: " + (yVal - tempInventory.Position.Y));
                Console.WriteLine("Inventory Item clicked : " + (((int)curXVal) / 50 + (((int)curYVal) / 50) * 4));
                removeItem(((int)curXVal) / 50 + (((int)curYVal) / 50) * 4);
                rePositionItems();

            }
            

            return retVal;
        }

        public Item getItemByName(string name)
        {
            Item retVal = null;
            foreach (Item image in itemList)
            {
                if (image.Name == name)
                {
                    retVal = image;
                    break;
                }
            }

            return retVal;

        }

    }
}