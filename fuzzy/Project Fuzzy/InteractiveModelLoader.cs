﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project_Fuzzy
{
    public class InteractiveModelLoader
    {
        private StreamReader reader;
        
        private Game myGame;
        private Camera myCamera;

        public InteractiveModelLoader(Game game, Camera camera, string fileName)
        {
            myGame = game;
            myCamera = camera;
            reader = new StreamReader(fileName);
            
        }

        public List<InteractiveComponent> readFile()
        {
            string line = reader.ReadLine();
            string[] lineContext;
            List<InteractiveComponent> interactiveModelList = interactiveModelList = new List<InteractiveComponent>();
            InteractiveComponent tempModel;

            while (line != null)
            {
                lineContext = line.Split(';');

                tempModel = new InteractiveComponent(myGame, Int32.Parse(lineContext[0]) ,lineContext[1], Boolean.Parse(lineContext[2]), @lineContext[3], Boolean.Parse(lineContext[4]));
                tempModel.Position = new Vector3(float.Parse(lineContext[5]), float.Parse(lineContext[6]), float.Parse(lineContext[7]));
                tempModel.Camera = myCamera;
                interactiveModelList.Add(tempModel);
                myGame.Components.Add(tempModel);

                line = reader.ReadLine();
            }

            


            return interactiveModelList;

        }

        public void linkItems(List<InteractiveComponent> items)
        {
            StreamReader linkageReader = new StreamReader("Content/ItemLinkage.txt");
            string lineLinkage = linkageReader.ReadLine();
            string[] lineLinkageContext;

            while (lineLinkage != null)
            {
                lineLinkageContext = lineLinkage.Split(';');

                items[findIndexFromID(Int32.Parse(lineLinkageContext[0]), items)].ModelItem.addLinkedItem(items[findIndexFromID(Int32.Parse(lineLinkageContext[1]), items)]);


                lineLinkage = linkageReader.ReadLine();
            }
        }

        public int findIndexFromID(int id, List<InteractiveComponent> items )
        {
            int count = 0;

            foreach (InteractiveComponent model in items)
            {
                if (model.ModelID == id)
                {
                    return count;
                }

                count++;
            }

            return count;
        }

    }
}
