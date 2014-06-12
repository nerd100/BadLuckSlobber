#region File Description
//-----------------------------------------------------------------------------
// Game.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
#endregion

namespace ChaseCameraSample
{
    /// <summary>
    /// Sample showing how to implement a simple chase camera.
    /// </summary>
    public class ChaseCameraGame : Microsoft.Xna.Framework.Game
    {
        HUD hud = new HUD();
        private HealthBar healthBar;

        #region Fields
        
        GraphicsDeviceManager graphics;
        

        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        
        KeyboardState lastKeyboardState = new KeyboardState();
        GamePadState lastGamePadState = new GamePadState();
        MouseState lastMousState = new MouseState();
        KeyboardState currentKeyboardState = new KeyboardState();
        GamePadState currentGamePadState = new GamePadState();
        MouseState currentMouseState = new MouseState();

        Ship ship;
        ChaseCamera camera;

        Model shipModel;
        Model groundModel;

        Model skyDome;
        Texture2D skydomeTex;
        Vector3 PlayerPosition = Vector3.Zero;

         
        

        bool cameraSpringEnabled = true;

        #endregion
        
        #region Initialization

      
        public ChaseCameraGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.SupportedOrientations = DisplayOrientation.Portrait;
            
            
            Content.RootDirectory = "Content";
            IsMouseVisible = true;


            graphics.PreferredBackBufferWidth = 1400;
            graphics.PreferredBackBufferHeight = 800;


            // Create the chase camera
            camera = new ChaseCamera();

            // Set the camera offsets
            camera.DesiredPositionOffset = new Vector3(0.0f, 2000.0f, 6000.0f);
            camera.LookAtOffset = new Vector3(0.0f, 150.0f, 0.0f);

            // Set camera perspective
            camera.NearPlaneDistance = 10.0f;
            camera.FarPlaneDistance = 100000.0f;

            //TODO: Set any other camera invariants here such as field of view
        }


        /// <summary>
        /// Initalize the game
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            

            ship = new Ship(GraphicsDevice);

            // Set the camera aspect ratio
            // This must be done after the class to base.Initalize() which will
            // initialize the graphics device.
            camera.AspectRatio = (float)graphics.GraphicsDevice.Viewport.Width /
                graphics.GraphicsDevice.Viewport.Height;


            // Perform an inital reset on the camera so that it starts at the resting
            // position. If we don't do this, the camera will start at the origin and
            // race across the world to get behind the chased object.
            // This is performed here because the aspect ratio is needed by Reset.
            UpdateCameraChaseTarget();
            camera.Reset();
        }


        /// <summary>
        /// Load graphics content.
        /// </summary>
        protected override void LoadContent()
        {
            

            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>("gameFont");

            shipModel = Content.Load<Model>("Slobber");
            groundModel = Content.Load<Model>("Ground");
            hud.LoadContent(Content);
            healthBar = new HealthBar(Content);

            skyDome = Content.Load<Model>("dome");
            skydomeTex = Content.Load<Texture2D>("cloudMap"); 
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Allows the game to run logic.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {

            healthBar.Update();

            lastKeyboardState = currentKeyboardState;
            lastMousState = currentMouseState;


            currentKeyboardState = Keyboard.GetState();



            // Exit when the Escape key or Back button is pressed
            if (currentKeyboardState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // Pressing the A button or key toggles the spring behavior on and off
            if (lastKeyboardState.IsKeyUp(Keys.Space) &&
                (currentKeyboardState.IsKeyDown(Keys.Space)))
            {
                cameraSpringEnabled =! cameraSpringEnabled;
            }

            // Reset the ship on R key or right thumb stick clicked
            if (currentKeyboardState.IsKeyDown(Keys.R) ||
                currentGamePadState.Buttons.RightStick == ButtonState.Pressed)
            {
                ship.Reset();
                camera.Reset();
            }

            // Update the ship
            ship.Update(gameTime);

            // Update the camera to chase the new target
            UpdateCameraChaseTarget();

            // The chase camera's update behavior is the springs, but we can
            // use the Reset method to have a locked, spring-less camera
            if (cameraSpringEnabled)
                camera.Update(gameTime);
            else
                camera.Reset();


            base.Update(gameTime);
        }

        /// <summary>
        /// Update the values to be chased by the camera
        /// </summary>
        private void UpdateCameraChaseTarget()
        {
            camera.ChasePosition = ship.Position + new Vector3(0,3000,0);
            camera.ChaseDirection = ship.Direction;
            camera.Up = ship.Up;
        }


        /// <summary>
        /// Draws the ship and ground.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice device = graphics.GraphicsDevice;
              
            device.Clear(Color.CornflowerBlue);

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            DrawModel(shipModel, ship.World);
            DrawModel(groundModel, Matrix.Identity);

            DrawOverlayText();

            base.Draw(gameTime);


        }


        /// <summary>
        /// Simple model drawing method. The interesting part here is that
        /// the view and projection matrices are taken from the camera object.
        /// </summary>        
        private void DrawModel(Model model, Matrix world)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] * world;

                    // Use the matrices provided by the chase camera
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                }
                mesh.Draw();
            }
        }


        /// <summary>
        /// Displays an overlay showing what the controls are,
        /// and which settings are currently selected.
        /// </summary>
        private void DrawOverlayText()
        {
            hud.Draw(spriteBatch);        
            spriteBatch.Begin();
            healthBar.Draw(spriteBatch); 
            string text = "Bewegen mit W,A,S,D\n"
                           + "Springen mit Pfeiltaste oben\n"
                           +"Reset mit R\n"
                           +"toggle Camera mit Space";
           
            spriteBatch.DrawString(spriteFont, text, new Vector2(60, 100), Color.White);
             
            spriteBatch.End();


        }


        #endregion
    }


    #region Entry Point

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    static class Program
    {
        static void Main()
        {
            using (ChaseCameraGame game = new ChaseCameraGame())
            {
                game.Run();
            }
        }
    }

    #endregion
}
