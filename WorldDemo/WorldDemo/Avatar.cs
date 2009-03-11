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
        protected Vector3 leftFacing = Vector3.Left;

        public Avatar(Game game, String modelName)
            : base(game, modelName) { }

        public void StrafeRight() 
        {
            position -= leftFacing;
        }

        public void StrafeLeft()
        {
            position += leftFacing;
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
