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
                
            addItem(@"inventory", "inventory");

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
                    spriteBatch.Draw(image.UIImage, new Vector2(500, 300), Color.White);
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
            Item item = new Item(this.Game, name2D, name);

            int yOffset = itemList.Count / 4;
            int xOffset = itemList.Count % 4;

            item.Position = new Vector2((xOffset * 60), yOffset * 60);
            item.DrawOrder = this.DrawOrder + itemList.Count;

            itemList.Add(item);
            this.Game.Components.Add(item);

        }

        /// <summary>
        /// Removes the item from the array with a certain name
        /// </summary>
        /// <param name="name"></param>
        public void removeItem(string name)
        {
            int index = 0;

            foreach(Item image in itemList)
            {
                if (image.Name == name)
                {
                    itemList.RemoveAt(index);
                    break;
                }
                
            }

        }
    }
}