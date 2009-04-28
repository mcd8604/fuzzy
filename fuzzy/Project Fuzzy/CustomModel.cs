#region File Description
//-----------------------------------------------------------------------------
// CustomModel.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
#endregion

namespace Project_Fuzzy
{

    // Each model part represents a piece of geometry that uses one
    // single effect. Multiple parts are needed for models that use
    // more than one effect.
    public class ModelPart
    {
        public int TriangleCount;
        public int VertexCount;
        public int VertexStride;

        public VertexDeclaration VertexDeclaration;
        public VertexBuffer VertexBuffer;
        public IndexBuffer IndexBuffer;

        public Effect Effect;

        public BoundingSphere BoundingSphere;

        public List<Triangle> CollisionTriangles;

        public bool Visible = true;
        public bool Collidable = true;
    }

    /// <summary>
    /// Custom class that can be used as a replacement for the built-in Model type.
    /// This provides functionality roughly similar to Model, but simplified as far
    /// as possible while still being able to correctly render data from arbitrary
    /// X or FBX files. This can be used as a starting point for building up your
    /// own more sophisticated Model replacements.
    /// </summary>
    public class CustomModel
    {
        // Internally our custom model is made up from a list of model parts.
        List<ModelPart> modelParts = new List<ModelPart>();

        string modelName;

        public List<ModelPart> ModelParts
        {
            get { return modelParts; }
        }

        /// <summary>
        /// The constructor reads model data from our custom XNB format.
        /// This is called by the CustomModelReader class, which is invoked
        /// whenever you ask the ContentManager to read a CustomModel object.
        /// </summary>
        internal CustomModel(ContentReader input)
        {
            int partCount = input.ReadInt32();

            for (int i = 0; i < partCount; i++)
            {
                ModelPart modelPart = new ModelPart();

                // Simple data types like integers can be read directly.
                modelPart.TriangleCount = input.ReadInt32();
                modelPart.VertexCount = input.ReadInt32();
                modelPart.VertexStride = input.ReadInt32();

                // These XNA Framework types can be read using the ReadObject method,
                // which calls into the appropriate ContentTypeReader for each type.
                // The framework provides built-in ContentTypeReader implementations
                // for important types such as vertex declarations, vertex buffers,
                // index buffers, effects, and textures.
                modelPart.VertexDeclaration = input.ReadObject<VertexDeclaration>();
                modelPart.VertexBuffer = input.ReadObject<VertexBuffer>();
                modelPart.IndexBuffer = input.ReadObject<IndexBuffer>();
                modelPart.BoundingSphere = input.ReadObject<BoundingSphere>();

                // Shared resources have to be read in a special way. Because the same
                // object can be referenced from many different parts of the file, the
                // actual object data is stored at the end of the XNB binary. When we
                // call ReadSharedResource we are just reading an ID that will later be
                // used to locate the actual data, so ReadSharedResource is unable to
                // directly return the shared instance. Instead, it takes in a delegate
                // parameter, and will call us back as soon as the shared value becomes
                // available. We use C# anonymous delegate syntax to store the value
                // into its final location.
                input.ReadSharedResource<Effect>(delegate(Effect value)
                {
                    modelPart.Effect = value;
                });

                modelParts.Add(modelPart);
            }
        }


        /// <summary>
        /// Draws the model using the specified camera matrices.
        /// </summary>
        public void Draw(Matrix world)
        {
            foreach (ModelPart modelPart in modelParts)
            {
                if (modelPart.Visible)
                {
                    // Look up the effect, and set effect parameters on it. This sample
                    // assumes the model will only be using BasicEffect, but a more robust
                    // implementation would probably want to handle custom effects as well.
                    BasicEffect effect = (BasicEffect)modelPart.Effect;

                    effect.EnableDefaultLighting();

                    effect.World = world;

                    // Set the graphics device to use our vertex declaration,
                    // vertex buffer, and index buffer.
                    GraphicsDevice device = effect.GraphicsDevice;

                    device.VertexDeclaration = modelPart.VertexDeclaration;

                    device.Vertices[0].SetSource(modelPart.VertexBuffer, 0,
                                                 modelPart.VertexStride);

                    device.Indices = modelPart.IndexBuffer;

                    // Begin the effect, and loop over all the effect passes.
                    effect.Begin();

                    foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Begin();

                        // Draw the geometry.
                        device.DrawIndexedPrimitives(PrimitiveType.TriangleList,
                                                     0, 0, modelPart.VertexCount,
                                                     0, modelPart.TriangleCount);

                        pass.End();
                    }

                    effect.End();
                }
            }
        }

        internal void GenerateCollisionTriangles()
        {
            foreach (ModelPart part in modelParts)
            {
                part.CollisionTriangles = new List<Triangle>();

                VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[part.VertexBuffer.SizeInBytes / part.VertexStride];
                part.VertexBuffer.GetData<VertexPositionNormalTexture>(vertices);

                if (modelName == "courtyard")
                { }

                if (part.IndexBuffer.IndexElementSize == IndexElementSize.SixteenBits)
                {
                    Int16[] indices = new Int16[part.IndexBuffer.SizeInBytes >> 1];

                    part.IndexBuffer.GetData<Int16>(indices);

                    for (int i = 0; i < indices.Length; i += 3)
                    {
                        if (vertices[indices[i]].Position != vertices[indices[i + 1]].Position &&
                            vertices[indices[i + 1]].Position != vertices[indices[i + 2]].Position &&
                            vertices[indices[i + 2]].Position != vertices[indices[i]].Position)
                        {
                            part.CollisionTriangles.Add(new Triangle(vertices[indices[i]].Position, vertices[indices[i + 1]].Position, vertices[indices[i + 2]].Position));
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

                    for (int i = 0; i < indices.Length; i += 3)
                    {
                        part.CollisionTriangles.Add(new Triangle(vertices[indices[i]].Position, vertices[indices[i + 1]].Position, vertices[indices[i + 2]].Position));
                    }
                }
            }
        }
    }
}
