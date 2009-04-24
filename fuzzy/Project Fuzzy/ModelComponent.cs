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
        protected CustomModel model;

        protected Effect effect;
        public Effect ModelEffect
        {
            set
            {
                effect = value;
                if (model != null)
                {
                    foreach (ModelPart part in model.ModelParts)
                        part.Effect = effect;
                    //foreach (ModelMesh mesh in model.Meshes)
                    //    foreach (ModelMeshPart part in mesh.MeshParts)
                    //        part.Effect = effect;
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

        public ModelComponent(Game game, string modelName)
            : base(game)
        {
            this.modelName = modelName;
        }

        protected override void LoadContent()
        {
            model = Game.Content.Load<CustomModel>(modelName);
            
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

        public List<Triangle> GetPlanes()
        {
            List<Triangle> planes = new List<Triangle>();

            foreach (ModelPart part in model.ModelParts)
            {
                VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[part.VertexBuffer.SizeInBytes / part.VertexStride];
                part.VertexBuffer.GetData<VertexPositionNormalTexture>(vertices);

                if (modelName == "courtyard")
                { }

                if (part.IndexBuffer.IndexElementSize == IndexElementSize.SixteenBits) 
                {
                    Int16[] indices = new Int16[part.IndexBuffer.SizeInBytes >> 1];
                    
                    part.IndexBuffer.GetData<Int16>(indices);

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
                            //Console.WriteLine("\tMesh: " + part.Name);
                            Console.WriteLine("\tVertex 1: " + vertices[indices[i]].Position);
                            Console.WriteLine("\tVertex 2: " + vertices[indices[i + 1]].Position);
                            Console.WriteLine("\tVertex 3: " + vertices[indices[i + 2]].Position);
                        }
                    }
                }
                else if (part.IndexBuffer.IndexElementSize == IndexElementSize.ThirtyTwoBits)
                {
                    Int32[] indices = new Int32[part.IndexBuffer.SizeInBytes >> 2];
                    part.IndexBuffer.GetData<Int32>(indices);

                    for(int i = 0; i < indices.Length; i += 3)
                    {
                        planes.Add(new Triangle(vertices[indices[i]].Position, vertices[indices[i + 1]].Position, vertices[indices[i + 2]].Position));
                    }
                }


            }

            return planes;
        }

        public override void Draw(GameTime gameTime)
        {
            model.Draw(getTransform());
            //effect.Parameters["World"].SetValue(getTransform());
            //foreach (ModelPart part in model.ModelParts)
            //{
            //    GraphicsDevice.Vertices[0].SetSource(part.VertexBuffer, 0, part.VertexStride);
            //    GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, part.VertexCount, 0, part.TriangleCount);
            //}
            //base.Draw(gameTime);

            //effect.Parameters["World"].SetValue(Matrix.Identity);
        }

        private Matrix getTransform()
        {
            return Matrix.CreateTranslation(position);
        }
    }
}
