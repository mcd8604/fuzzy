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
        private string textureName;
        protected Model model;
        protected Texture2D texture;

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

        public ModelComponent(Game game, String modelName)
            : base(game)
        {
            this.modelName = modelName;
        }

        public ModelComponent(Game game, String modelName, String textureName)
            : base(game)
        {
            this.modelName = modelName;
            this.textureName = textureName;
        }

        protected override void LoadContent()
        {
            model = Game.Content.Load<Model>(modelName);
            
            GenerateBoundingSphere();

            if(!string.IsNullOrEmpty(textureName))
                texture = Game.Content.Load<Texture2D>(textureName);
            
            base.LoadContent();
        }

        private void GenerateBoundingSphere()
        {
            bounds = new BoundingSphere();

            foreach (ModelMesh mesh in model.Meshes)
            {
                bounds = BoundingSphere.CreateMerged(bounds, mesh.BoundingSphere);
            }
        }

        public List<Triangle> GetPlanes()
        {
            List<Triangle> planes = new List<Triangle>();

            foreach (ModelMesh mesh in model.Meshes)
            {
                int numVertices = 0;
                int numPrimitives = 0;

                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    numVertices += part.NumVertices;
                    numPrimitives += part.PrimitiveCount;
                }

                Vector3[] vertices = new Vector3[numVertices];
                mesh.VertexBuffer.GetData<Vector3>(vertices);

                short[] indices = new short[numPrimitives];
                mesh.IndexBuffer.GetData<short>(indices);

                for(int i = 0; i < indices.Length; i += 3)
                {
                    planes.Add(new Triangle(vertices[i], vertices[i + 1], vertices[i + 2]));
                }

            }

            return planes;
        }

        public override void Draw(GameTime gameTime)
        {
            effect.Parameters["World"].SetValue(getTransform());
            effect.Parameters["BasicTexture"].SetValue(texture);

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
