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
        protected Quaternion rotation = Quaternion.Identity;
        public Quaternion Rotation
        {
            get { return rotation; }
        }
        protected Vector3 leftFacing = Vector3.Left;
        protected readonly Quaternion leftRotation = Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.ToRadians(5));
        protected readonly Quaternion rightRotation = Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.ToRadians(-5));

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

        public void TurnRight()
        {
            facing = Vector3.Transform(facing, rightRotation);
            rotation = Quaternion.CreateFromAxisAngle(Vector3.Down, (float)Math.Atan2(facing.Z, facing.X) + MathHelper.PiOver2);
            leftFacing = Vector3.Transform(facing, rightRotation);
        }

        public void TurnLeft()
        {
            facing = Vector3.Transform(facing, leftRotation);
            rotation = Quaternion.CreateFromAxisAngle(Vector3.Down, (float)Math.Atan2(facing.Z, facing.X) + MathHelper.PiOver2);
            leftFacing = Vector3.Transform(facing, leftRotation);
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
