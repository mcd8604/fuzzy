using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WorldDemo
{
    class ModelComponent : DrawableGameComponent
    {

        private string modelName;
        protected Model model;

        protected Effect effect;
        public Effect ModelEffect
        {
            set
            {
                effect = value;
                if (model != null)
                {
                    foreach (ModelMesh mesh in model.Meshes)
                        foreach (ModelMeshPart part in mesh.MeshParts)
                            part.Effect = effect;
                }
            }

            get
            {
                return effect;
            }
        }

        protected Vector4 color = Color.White.ToVector4();
        public Vector4 ModelColor
        {
            get { return color; }
            set { color = value; }
        }

        protected Vector3 position = Vector3.Zero;
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public ModelComponent(Game game, String modelName)
            : base(game)
        {
            this.modelName = modelName;
        }

        protected override void LoadContent()
        {
            model = Game.Content.Load<Model>(modelName);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            effect.Parameters["World"].SetValue(getTransform());
            effect.Parameters["materialColor"].SetValue(this.color);

            foreach (ModelMesh mesh in model.Meshes)
            {
                mesh.Draw();
            }
            base.Draw(gameTime);

            effect.Parameters["World"].SetValue(Matrix.Identity);
        }

        private Matrix getTransform()
        {
            return Matrix.CreateTranslation(position);
        }
    }
}
