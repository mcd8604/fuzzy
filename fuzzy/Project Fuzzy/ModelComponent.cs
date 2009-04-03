using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project_Fuzzy
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
                VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[mesh.VertexBuffer.SizeInBytes / mesh.MeshParts[0].VertexStride];
                mesh.VertexBuffer.GetData<VertexPositionNormalTexture>(vertices);

                if (mesh.IndexBuffer.IndexElementSize == IndexElementSize.SixteenBits) 
                {
                    Int16[] indices = new Int16[mesh.IndexBuffer.SizeInBytes >> 1];
                    
                    mesh.IndexBuffer.GetData<Int16>(indices);

                    for(int i = 0; i < indices.Length; i += 3)
                    {
                        if (vertices[indices[i]].Position != vertices[indices[i + 1]].Position &&
                            vertices[indices[i + 1]].Position != vertices[indices[i + 2]].Position &&
                            vertices[indices[i + 2]].Position != vertices[indices[i]].Position)
                        {
                            planes.Add(new Triangle(vertices[indices[i]].Position, vertices[indices[i + 1]].Position, vertices[indices[i + 2]].Position));
                        }
                        else
                        {
                            Console.WriteLine("Redundant vertices found!");
                            Console.WriteLine("\tModel: " + modelName);
                            Console.WriteLine("\tMesh: " + mesh.Name);
                            Console.WriteLine("\tVertex 1: " + vertices[indices[i]].Position);
                            Console.WriteLine("\tVertex 2: " + vertices[indices[i + 1]].Position);
                            Console.WriteLine("\tVertex 3: " + vertices[indices[i + 2]].Position);
                        }
                    }
                }
                else if (mesh.IndexBuffer.IndexElementSize == IndexElementSize.ThirtyTwoBits)
                {
                    Int32[] indices = new Int32[mesh.IndexBuffer.SizeInBytes >> 2];
                    mesh.IndexBuffer.GetData<Int32>(indices);

                    for(int i = 0; i < indices.Length; i += 3)
                    {
                        planes.Add(new Triangle(vertices[indices[i]].Position, vertices[indices[i + 1]].Position, vertices[indices[i + 2]].Position));
                    }
                }


            }

            return planes;
        }

        public List<BoundingBox> GetBoundingBoxes()
        {
            List<BoundingBox> boxes = new List<BoundingBox>();

            foreach (ModelMesh mesh in model.Meshes)
            {
                boxes.Add(BoundingBox.CreateFromSphere(mesh.BoundingSphere));
            }

            return boxes;
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
