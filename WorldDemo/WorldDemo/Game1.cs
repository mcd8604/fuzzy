using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace WorldDemo
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Matrix worldMatrix;
        Matrix viewMatrix;
        Matrix projectionMatrix;

        Vector3 cameraPos = new Vector3(40, 5, 20);

        Effect effect;
        Model model;

        VertexPositionNormalTexture[] floorVertices;
        VertexDeclaration vpntDeclaration;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            InitializeFloor();
            base.Initialize();
        }

        private void InitializeFloor()
        {
            floorVertices = new VertexPositionNormalTexture[6];

            floorVertices[0] = new VertexPositionNormalTexture(new Vector3(1000, 0, 1000), Vector3.Up, Vector2.Zero);
            floorVertices[1] = new VertexPositionNormalTexture(new Vector3(-1000, 0, 1000), Vector3.Up, Vector2.Zero);
            floorVertices[2] = new VertexPositionNormalTexture(new Vector3(-1000, 0, -1000), Vector3.Up, Vector2.Zero);

            floorVertices[3] = new VertexPositionNormalTexture(new Vector3(1000, 0, 1000), Vector3.Up, Vector2.Zero);
            floorVertices[4] = new VertexPositionNormalTexture(new Vector3(-1000, 0, -1000), Vector3.Up, Vector2.Zero);
            floorVertices[5] = new VertexPositionNormalTexture(new Vector3(1000, 0, -1000), Vector3.Up, Vector2.Zero);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            vpntDeclaration = new VertexDeclaration(GraphicsDevice, VertexPositionNormalTexture.VertexElements);
            GraphicsDevice.VertexDeclaration = vpntDeclaration;

            InitializeTransform();
            InitializeEffect();

            LoadModel();
        }

        /// <summary>
        /// Initializes the transforms used for the 3D model.
        /// </summary>
        private void InitializeTransform()
        {
            worldMatrix = Matrix.CreateRotationX(0);

            viewMatrix = Matrix.CreateLookAt(cameraPos, Vector3.Zero, Vector3.Up);

            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45),
                GraphicsDevice.Viewport.AspectRatio,
                1.0f, 1000.0f);
        }

		/// <summary>
		/// Initializes the basic effect (parameter setting and technique selection)
		/// used for the 3D model.
		/// </summary>
        private void InitializeEffect()
        {
            effect = Content.Load<Effect>("Basic");

            effect.Parameters["World"].SetValue(worldMatrix);
            effect.Parameters["View"].SetValue(viewMatrix);
            effect.Parameters["Projection"].SetValue(projectionMatrix);

            effect.Parameters["lightPos"].SetValue(new Vector4(20f, 20f, 20f, 1f));
            effect.Parameters["lightColor"].SetValue(Vector4.One);
            effect.Parameters["cameraPos"].SetValue(new Vector4(cameraPos, 1f));

            effect.Parameters["ambientColor"].SetValue(new Vector4(.2f, .2f, .2f, 1f));
            effect.Parameters["diffusePower"].SetValue(1f);
            effect.Parameters["specularPower"].SetValue(1);
            effect.Parameters["exponent"].SetValue(8);
        }

        private void LoadModel()
        {
            model = Content.Load<Model>("sphere");

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;
                }
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            
            DrawFloor();
            DrawModel();
            
            base.Draw(gameTime);
        }

        private void DrawFloor()
        {
            effect.Parameters["materialColor"].SetValue(Color.Green.ToVector4());
            effect.Begin();
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Begin();
                GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, floorVertices, 0, 2);
                pass.End();
            }
            effect.End();
        }

        private void DrawModel()
        {
            //effect.Parameters["World"].SetValue(Matrix.CreateTranslation(Vector3.Zero));
            effect.Parameters["materialColor"].SetValue(new Vector4(.0f, .5f, .8f, 1f));
            foreach (ModelMesh mesh in model.Meshes)
            {
                mesh.Draw();
            }
        }
    }
}
