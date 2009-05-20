#undef FLOOR_TEST
#define DRAW_COLLIDABLES
#undef WIREFRAME

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
using Project_Fuzzy.Inventory;

namespace Project_Fuzzy
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GUIInventory inventory;
        KeyboardState lastState;
        MouseState lastMouseState;

        List<InteractiveComponent> interactiveModelList;

        Matrix projectionMatrix;

        Camera camera;
        Vector3 cameraOffset = new Vector3(0, 1, 10);

        BasicEffect effect;
        Avatar avatar;
        InteractiveComponent sphere1;
        ModelComponent sphere2;
        ModelComponent sphere3;
        ModelComponent sphere4;        
        Texture2D texture;

        InteractiveModelLoader modelLoader;


#if FLOOR_TEST
        VertexPositionNormalTexture[] floorVertices;
#else 
        ModelComponent courtyard;
#endif

        VertexDeclaration vpntDeclaration;

        Physics physics;

        PhysicsBody testBody;

        MouseCursor mouse;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            lastMouseState = Mouse.GetState();
            lastState = Keyboard.GetState();
            
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

            mouse = new MouseCursor(this, "cursor");
            mouse.DrawOrder = 99999;
            Components.Add(mouse);

            avatar = new Avatar(this, "sphere", testBody);
            avatar.DrawOrder = 1;
            avatar.Position = new Vector3(-0, 3, 0);
            Components.Add(avatar);
            /*
            sphere1 = new InteractiveComponent(this, "sphere", true, @"ball");
            sphere1.DrawOrder = 1;
            sphere1.Position = new Vector3(10, 2, 10);
            Components.Add(sphere1);

            sphere2 = new ModelComponent(this, "sphere");
            sphere2.DrawOrder = 1;
            sphere2.Position = new Vector3(-10, 5, 10);
            Components.Add(sphere2);

            sphere3 = new ModelComponent(this, "sphere");
            sphere3.DrawOrder = 1;
            sphere3.Position = new Vector3(10, 5, -10);
            Components.Add(sphere3);

            sphere4 = new ModelComponent(this, "sphere");
            sphere4.DrawOrder = 1;
            sphere4.Position = new Vector3(-10, 5, -10);
            Components.Add(sphere4);*/
#if !FLOOR_TEST
            courtyard = new ModelComponent(this, "courtyard");
            courtyard.DrawOrder = 1;
            Components.Add(courtyard);
#endif
            camera = new Camera(this);
            camera.Position = cameraOffset;
            camera.DrawOrder = 0;
            Components.Add(camera);

            modelLoader = new InteractiveModelLoader(this, camera, "Content/interactiveComponents.txt");
            interactiveModelList = modelLoader.readFile();

#if FLOOR_TEST
            InitializeFloor();
#endif

            inventory = new GUIInventory(this);
            inventory.Visible = false;
            inventory.Enabled = false;
            inventory.DrawOrder = 2;
            Components.Add(inventory);

            base.Initialize();
        }

#if FLOOR_TEST
        private void InitializeFloor()
        {
            floorVertices = new VertexPositionNormalTexture[6];

            floorVertices[0] = new VertexPositionNormalTexture(new Vector3(50, 0, 50), Vector3.Up, new Vector2(1f, 1f));
            floorVertices[1] = new VertexPositionNormalTexture(new Vector3(-50, 0, 50), Vector3.Up, new Vector2(0f, 1f));
            floorVertices[2] = new VertexPositionNormalTexture(new Vector3(-50, 0, -50), Vector3.Up, Vector2.Zero);

            floorVertices[3] = new VertexPositionNormalTexture(new Vector3(50, 0, 50), Vector3.Up, new Vector2(1f, 1f));
            floorVertices[4] = new VertexPositionNormalTexture(new Vector3(-50, 0, -50), Vector3.Up, Vector2.Zero);
            floorVertices[5] = new VertexPositionNormalTexture(new Vector3(50, 0, -50), Vector3.Up, new Vector2(1f, 0f));

            //physics.AddCollidable(new Triangle(floorVertices[0].Position, floorVertices[1].Position, floorVertices[2].Position));
            //physics.AddCollidable(new Triangle(floorVertices[3].Position, floorVertices[4].Position, floorVertices[5].Position));
        }
#endif

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

#if WIREFRAME      
            GraphicsDevice.RenderState.FillMode = FillMode.WireFrame;
#endif

            vpntDeclaration = new VertexDeclaration(GraphicsDevice, VertexPositionNormalTexture.VertexElements);
            //GraphicsDevice.VertexDeclaration = vpntDeclaration;

            texture = Content.Load<Texture2D>("Checker");

            InitializeEffect();

            avatar.Camera = camera;
           // sphere1.Camera = camera;
            //sphere2.Camera = camera;
            //sphere3.Camera = camera;
           // sphere4.Camera = camera;
#if !FLOOR_TEST
            courtyard.Camera = camera;
#if DRAW_COLLIDABLES
            courtyard.Visible = false;
#endif
            physics.AddModel(courtyard.Model);
#endif
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

            camera.Projection = projectionMatrix;

            //effect.Parameters["Projection"].SetValue(projectionMatrix);
            //effect.Parameters["World"].SetValue(Matrix.Identity);
        }

		/// <summary>
		/// Initializes the basic effect (parameter setting and technique selection)
		/// used for the 3D model.
		/// </summary>
        private void InitializeEffect()
        {
            effect = new BasicEffect(GraphicsDevice, new EffectPool());

            //effect.LightingEnabled = true;
            //effect.TextureEnabled = true;

            //effect.AmbientLightColor = new Vector3(.2f, .2f, .2f);

            //effect.DirectionalLight0.Enabled = true;
            //effect.DirectionalLight0.Direction = Vector3.Normalize(Vector3.One * -1);
            //effect.DirectionalLight0.SpecularColor = Vector3.One;
            //effect.DirectionalLight0.DiffuseColor = Vector3.One;
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
            KeyboardState currentState = Keyboard.GetState();
            MouseState currentMouseState = Mouse.GetState();

            if (currentMouseState.LeftButton == ButtonState.Pressed)
            {
                if (lastMouseState.LeftButton == ButtonState.Released)
                {
                    if (inventory.Enabled)
                    {
                        //Console.WriteLine(currentMouseState.X + ":" + currentMouseState.Y);
                        inventory.checkIfItemClicked(currentMouseState.X, currentMouseState.Y);
                    }
                }
            }

            if (currentState.IsKeyDown(Keys.Enter))
            {
                if (lastState != currentState)
                {
                    foreach (InteractiveComponent model in interactiveModelList)
                    {
                        if (model.inRange(avatar.Position))
                        {
                            inventory.addItem(model.TextureName, model.ModelName);
                            model.Visible = false;
                            Console.WriteLine("ADDED");

                        }
                    }

                   // if (sphere1.inRange(avatar.Position))
                   // {
                    //    inventory.addItem(sphere1.TextureName, sphere1.ModelName);
                   //     sphere1.Visible = false;
                    //    Console.WriteLine("ADDED");
                   // }
                }
            }

            if (currentState.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            if (currentState.IsKeyDown(Keys.W))
            {
                avatar.MoveForward();
            }

            if (currentState.IsKeyDown(Keys.A))
            {
                avatar.TurnLeft();
            }

            if (currentState.IsKeyDown(Keys.S))
            {
                avatar.MoveBackward();
            }

            if (currentState.IsKeyDown(Keys.D))
            {
                avatar.TurnRight();
            }

            if (currentState.IsKeyDown(Keys.Up))
            {
                avatar.MoveForward();
            }

            if (currentState.IsKeyDown(Keys.Down))
            {
                avatar.MoveBackward();
            }

            if (currentState.IsKeyDown(Keys.Right))
            {
                avatar.TurnRight();
            }

            if (currentState.IsKeyDown(Keys.Left))
            {
                avatar.TurnLeft();
            }

            if (currentState.IsKeyDown(Keys.Space))
            {
                avatar.Jump();
            }

            if (currentState.IsKeyDown(Keys.I))
            {
                if (lastState != currentState)
                {
                    if (!inventory.Enabled)
                    {
                        inventory.Enabled = true;
                        inventory.Visible = true;
                    }
                    else
                    {
                        inventory.Enabled = false;
                        inventory.Visible = false;
                    }
                }
            }

            camera.Target = avatar.Position;
            camera.Position = avatar.Position + Vector3.Transform(cameraOffset, avatar.Rotation);

            lastState = currentState;
            lastMouseState = currentMouseState;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.RenderState.DepthBufferEnable = true;
            //Object temp = GraphicsDevice.PresentationParameters;
            //GraphicsDevice.Reset();
            // TODO: Add your drawing code here      
            

#if FLOOR_TEST
            DrawFloor();
#endif

#if DRAW_COLLIDABLES
            DrawCollidables();
#endif
            base.Draw(gameTime);
        }

#if FLOOR_TEST
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
#endif

#if DRAW_COLLIDABLES
        private void DrawCollidables()
        {
            foreach (CustomModel model in physics.Collidables)
            {
                model.Draw(Matrix.Identity, camera.View, projectionMatrix);
                //effect.Parameters["BasicTexture"].SetValue(texture);
                //effect.Begin();
                //foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                //{
                //    pass.Begin();
                //    GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, physics.Vertices, 0, physics.Vertices.Length / 3);
                //    pass.End();
                //}
                //effect.End();
            }
        }
#endif
    }
}
