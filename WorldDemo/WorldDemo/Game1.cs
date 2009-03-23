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

        Matrix projectionMatrix;

        Camera camera;
        Vector3 cameraOffset = new Vector3(0, 1, 10);

        BasicEffect effect;
        Avatar avatar;
        ModelComponent sphere1;
        ModelComponent sphere2;
        ModelComponent sphere3;
        ModelComponent sphere4;
        ModelComponent courtyard;
        Texture2D texture;

        VertexPositionNormalTexture[] floorVertices;
        VertexDeclaration vpntDeclaration;

        Physics physics;

        PhysicsBody testBody;

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
            physics = new Physics(this);
            Components.Add(physics);

            testBody = new PhysicsBody();

            physics.AddBody(testBody);
            
            avatar = new Avatar(this, "sphere", testBody);
            avatar.DrawOrder = 1;
            avatar.Position = new Vector3(-75, 50, 0);
            Components.Add(avatar);

            sphere1 = new ModelComponent(this, "sphere", "Checker");
            sphere1.DrawOrder = 1;
            sphere1.Position = new Vector3(10, 5, 10);
            Components.Add(sphere1);

            sphere2 = new ModelComponent(this, "sphere", "Checker");
            sphere2.DrawOrder = 1;
            sphere2.Position = new Vector3(-10, 5, 10);
            Components.Add(sphere2);

            sphere3 = new ModelComponent(this, "sphere", "Checker");
            sphere3.DrawOrder = 1;
            sphere3.Position = new Vector3(10, 5, -10);
            Components.Add(sphere3);

            sphere4 = new ModelComponent(this, "sphere", "Checker");
            sphere4.DrawOrder = 1;
            sphere4.Position = new Vector3(-10, 5, -10);
            Components.Add(sphere4);

            courtyard = new ModelComponent(this, "courtyard", "Checker");
            courtyard.DrawOrder = 1;
            Components.Add(courtyard);

            camera = new Camera(this);
            camera.Position = cameraOffset;
            camera.DrawOrder = 0;
            Components.Add(camera);

            //InitializeFloor();
            
            base.Initialize();
        }

        //private void InitializeFloor()
        //{
        //    floorVertices = new VertexPositionNormalTexture[6];

        //    floorVertices[0] = new VertexPositionNormalTexture(new Vector3(1000, 0, 1000), Vector3.Up, Vector2.One);
        //    floorVertices[1] = new VertexPositionNormalTexture(new Vector3(-1000, 0, 1000), Vector3.Up, new Vector2(0f, 1f));
        //    floorVertices[2] = new VertexPositionNormalTexture(new Vector3(-1000, 0, -1000), Vector3.Up, Vector2.Zero);

        //    floorVertices[3] = new VertexPositionNormalTexture(new Vector3(1000, 0, 1000), Vector3.Up, Vector2.One);
        //    floorVertices[4] = new VertexPositionNormalTexture(new Vector3(-1000, 0, -1000), Vector3.Up, Vector2.Zero);
        //    floorVertices[5] = new VertexPositionNormalTexture(new Vector3(1000, 0, -1000), Vector3.Up, new Vector2(1f, 0f));

        //    physics.AddCollidable(new Triangle(floorVertices[1].Position, floorVertices[0].Position, floorVertices[2].Position));
        //    physics.AddCollidable(new Triangle(floorVertices[4].Position, floorVertices[3].Position, floorVertices[5].Position));
        //}

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

            texture = Content.Load<Texture2D>("Checker");

            InitializeEffect();

            avatar.ModelEffect = effect;
            sphere1.ModelEffect = effect;
            sphere2.ModelEffect = effect;
            sphere3.ModelEffect = effect;
            sphere4.ModelEffect = effect;
            courtyard.ModelEffect = effect;
            camera.Effect = effect;

            physics.AddCollidables(courtyard.GetPlanes());

            InitializeTransform();
        }

        /// <summary>
        /// Initializes the transforms used for the 3D model.
        /// </summary>
        private void InitializeTransform()
        {
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45),
                GraphicsDevice.Viewport.AspectRatio,
                1.0f, 1000.0f);

            effect.Parameters["Projection"].SetValue(projectionMatrix);
            effect.Parameters["World"].SetValue(Matrix.Identity);
        }

		/// <summary>
		/// Initializes the basic effect (parameter setting and technique selection)
		/// used for the 3D model.
		/// </summary>
        private void InitializeEffect()
        {
            //effect = Content.Load<Effect>("Basic");

            //effect.Parameters["lightPos"].SetValue(new Vector4(20f, 20f, 20f, 1f));
            //effect.Parameters["lightColor"].SetValue(Vector4.One);

            //effect.Parameters["AmbientLightColor"].SetValue(new Vector4(.2f, .2f, .2f, 1f));
            //effect.Parameters["DiffusePower"].SetValue(1f);
            //effect.Parameters["SpecularPower"].SetValue(1);
            //effect.Parameters["exponent"].SetValue(8);

            effect = new BasicEffect(GraphicsDevice, new EffectPool());

            effect.LightingEnabled = true;
            effect.TextureEnabled = true;

            effect.AmbientLightColor = new Vector3(.2f, .2f, .2f);

            effect.DirectionalLight0.Enabled = true;
            effect.DirectionalLight0.Direction = Vector3.Normalize(Vector3.One * -1);
            effect.DirectionalLight0.SpecularColor = Vector3.One;
            effect.DirectionalLight0.DiffuseColor = Vector3.One;
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
            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            if (keyboard.IsKeyDown(Keys.W))
            {
                avatar.MoveForward();
            }

            if (keyboard.IsKeyDown(Keys.A))
            {
                avatar.TurnLeft();
            }

            if (keyboard.IsKeyDown(Keys.S))
            {
                avatar.MoveBackward();
            }

            if (keyboard.IsKeyDown(Keys.D))
            {
                avatar.TurnRight();
            }

            if (keyboard.IsKeyDown(Keys.Up))
            {
                avatar.MoveForward();
            }

            if (keyboard.IsKeyDown(Keys.Down))
            {
                avatar.MoveBackward();
            }

            if (keyboard.IsKeyDown(Keys.Right))
            {
                avatar.TurnRight();
            }

            if (keyboard.IsKeyDown(Keys.Left))
            {
                avatar.TurnLeft();
            }

            if (keyboard.IsKeyDown(Keys.Space))
            {
                avatar.Jump();
            }

            camera.Target = avatar.Position;
            camera.Position = avatar.Position + Vector3.Transform(cameraOffset, avatar.Rotation);

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
            
            base.Draw(gameTime);

            //DrawFloor();
        }

        private void DrawFloor()
        {
            effect.Parameters["BasicTexture"].SetValue(texture);
            effect.Begin();
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Begin();
                GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, floorVertices, 0, 2);
                pass.End();
            }
            effect.End();
        }

    }
}
