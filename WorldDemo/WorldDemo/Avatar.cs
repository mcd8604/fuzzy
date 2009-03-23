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

        PhysicsBody testBody;

        public override Vector3 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                base.Position = value;
                testBody.Position = value;
            }
        }

        public Avatar(Game game, String modelName, PhysicsBody body)
            : base(game, modelName) 
        {
            testBody = body;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            testBody.Radius = bounds.Radius;
        }

        public override void Update(GameTime gameTime)
        {
            position = testBody.Position;
            base.Update(gameTime);
        }

        public void StrafeRight() 
        {
            //position -= leftFacing;
        }

        public void StrafeLeft()
        {
            //position += leftFacing;
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
            //position += facing;
            testBody.ApplyForce(facing * 100);
        }

        public void MoveBackward()
        {
            //position -= facing;
            testBody.ApplyForce(Vector3.Negate(facing * 100));
        }

        internal void Jump()
        {
            testBody.ApplyForce(Vector3.Up * 19.6f * testBody.Normal);
        }
    }
}
