using System;
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

                tempModel = new InteractiveComponent(myGame, lineContext[0], Boolean.Parse(lineContext[1]), @lineContext[2]);
                tempModel.Position = new Vector3(float.Parse(lineContext[3]), float.Parse(lineContext[4]), float.Parse(lineContext[5]));
                tempModel.Camera = myCamera;
                interactiveModelList.Add(tempModel);
                myGame.Components.Add(tempModel);

                line = reader.ReadLine();
            }

            return interactiveModelList;

        }
    }
}
