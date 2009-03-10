using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WorldDemo
{
    class Avatar : ModelComponent
    {
        protected Vector3 facing = -Vector3.UnitZ;

        public Avatar(Game game, String modelName, Effect effect)
            : base(game, modelName, effect) { }

        public void StrafeRight() 
        {

        }

        public void StrafeLeft()
        {

        }

        public void MoveForward()
        {
            position += facing;
        }

        public void MoveBackward()
        {
            position -= facing;
        }
    }
}
