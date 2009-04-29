using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project_Fuzzy
{
    class Physics : GameComponent
    {
        protected Vector3 gravity = new Vector3(0, -9.8f, 0);

        /// <summary>
        /// Arbitrary frictional coefficient for two sliding surfaces
        /// </summary>
        protected const float friction = 0.2f;

        protected List<PhysicsBody> bodies = new List<PhysicsBody>();

        protected List<CustomModel> collidables = new List<CustomModel>();
        public List<CustomModel> Collidables
        {
            get { return collidables; }
        }

        public Physics(Game game)
            : base(game) { }

        public void AddBody(PhysicsBody body)
        {
            bodies.Add(body);            
        }

        public void AddModel(CustomModel model)
        {
            collidables.Add(model);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (PhysicsBody body in bodies)
            {
                body.Update((float)gameTime.ElapsedGameTime.TotalSeconds, collidables, gravity);
            }

            base.Update(gameTime);
        }

    }
}
