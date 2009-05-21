using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project_Fuzzy
{
    public class ModelComponent : DrawableGameComponent
    {
        private Camera camera;
        public Camera Camera
        {
            get { return camera; }
            set { camera = value; }
        }

        private string modelName;
        protected CustomModel model;
        public CustomModel Model
        {
            get { return model; }
        }

        //protected Effect effect;
        //public Effect ModelEffect
        //{
        //    set
        //    {
        //        effect = value;
        //        if (model != null)
        //        {
        //            foreach (ModelPart part in model.ModelParts)
        //                part.Effect = effect;
        //        }
        //    }

        //    get
        //    {
        //        return effect;
        //    }
        //}

        protected Vector4 color = Color.White.ToVector4();
        public Vector4 ModelColor
        {
            get { return color; }
            set { color = value; }
        }

        protected Vector3 position = Vector3.Zero;
        public virtual Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        protected BoundingSphere bounds;
        public BoundingSphere Bounds
        {
            get { return bounds; }
        }

        public ModelComponent(Game game, string modelName)
            : base(game)
        {
            this.modelName = modelName;
        }

        protected override void LoadContent()
        {
            model = Game.Content.Load<CustomModel>(modelName);
            model.GenerateCollisionTriangles(position);
            
            GenerateBoundingSphere();
            
            base.LoadContent();
        }

        private void GenerateBoundingSphere()
        {
            bounds = new BoundingSphere();
      
            foreach (ModelPart part in model.ModelParts)
            {
                bounds = BoundingSphere.CreateMerged(bounds, part.BoundingSphere);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            model.Draw(getTransform(), camera.View, camera.Projection);
        }

        private Matrix getTransform()
        {
            return Matrix.CreateTranslation(position);
        }
    }
}
