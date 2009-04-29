using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project_Fuzzy
{
    public class Camera : DrawableGameComponent
    {

        protected Vector3 position = Vector3.Zero;
        public Vector3 Position
        {
            get { return position; }
            set 
            {
                position = value;
                viewMatrix = Matrix.CreateLookAt(position, target, Vector3.Up);

                //if (effect != null)
                //    effect.Parameters["EyePosition"].SetValue(position);
            }
        }

        protected Vector3 target = Vector3.Zero;
        public Vector3 Target
        {
            get { return target; }
            set
            {
                target = value;
                viewMatrix = Matrix.CreateLookAt(position, target, Vector3.Up); 
            }
        }

        //protected Effect effect;
        //public Effect Effect
        //{
        //    get { return effect; }
        //    set { effect = value; }
        //}

        protected Matrix viewMatrix;
        public Matrix View
        {
            get { return viewMatrix; }
        }

        protected Matrix projection;
        public Matrix Projection
        {
            set { projection = value; }
            get { return projection; }
        }

        public Camera(Game game)
            : base(game) { }

        public override void Update(GameTime gameTime)
        {
 	        base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //effect.Parameters["View"].SetValue(viewMatrix);

            base.Draw(gameTime);
        }
    }
}
