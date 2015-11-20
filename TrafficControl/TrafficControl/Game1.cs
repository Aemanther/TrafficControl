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

namespace TrafficControl
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MouseState prevMouseState;

        //Graphics to render
        Texture2D Map;
        Texture2D LightGreen;
        Texture2D LightRed;
        Texture2D CarBlue;

        //Static Coordinates
        Vector2 PositionOrigo = Vector2.Zero;

        //Local variables
        int NumberofStreets = 2;

        //Collections
        List<Light> TrafficLights = new List<Light>();
        List<Car> Cars = new List<Car>();
        List<Street> Streets = new List<Street>();

        Intersection Intersection1 = new Intersection();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 400;
            graphics.PreferredBackBufferWidth = 400;

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
            base.Initialize();

            for (int i = 0; i < NumberofStreets; i++)
            {
                Street newStreet = new Street();

                newStreet.Initialize(i);

                Streets.Add(newStreet);
            }

            Intersection1.Initialize(new Vector2(100f, 100f));
            CreateLights(Intersection1.Position);

            this.IsMouseVisible = true;
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
            Map = this.Content.Load<Texture2D>("EasyMap");
            LightGreen = this.Content.Load<Texture2D>("LightGreen");
            LightRed = this.Content.Load<Texture2D>("LightRed");
            CarBlue = this.Content.Load<Texture2D>("Car_Blue");

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

            // Mouse Controls
            MouseState mouseState = Mouse.GetState();

            Point mousePosition = new Point(mouseState.X, mouseState.Y);

            if (prevMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed)
            {
                for (int i = 0; i < TrafficLights.Count; i++)
                {
                    if (TrafficLights[i].Area.Contains(mousePosition))
                    {
                        TrafficLights[i].RedGreen = true;
                        Intersection1.Lights[i] = true;
                    }
                    else
                    {
                        TrafficLights[i].RedGreen = false;
                        Intersection1.Lights[i] = false;
                    }
                }
            }

            prevMouseState = mouseState;

            // TODO: Add your update logic here
            if (Cars.Count < 1)
            {
                CreateCar();
            }

            foreach (Car car in Cars)
            {
                if (car.Active)
                {
                    car.Update(gameTime.ElapsedGameTime);
                    if (!car.Moving)
                    {
                        if (Streets[0].IsSlotFree())
                        {
                            if (car.CurrentSlot == 2)
                            {
                                if (Intersection1.Lights[0])
                                {
                                    car.Go();
                                }
                            }
                            else
                            {
                                car.Go();
                            }                            
                        }
                    }
                }
            }

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
            spriteBatch.Begin();

            spriteBatch.Draw(Map, PositionOrigo, Color.White);

            foreach (Light light in TrafficLights)
            {
                light.Draw(spriteBatch);
            }

            for (int i = 0; i < Cars.Count(); i++)
            {
                if (Cars[i].Active)
                {
                    Cars[i].Draw(spriteBatch);
                }
                else
                {
                    Cars.RemoveAt(i);
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        #region Game Functions

        private void CreateCar()
        {
            Car newCar = new Car();

            newCar.Initialize(CarBlue, 0, 0, 0, 6, Streets[0].IsVertical, Streets[0].StreetStart);

            Cars.Add(newCar);
        }

        private void CreateLights(Vector2 IntersectionPosition)
        {
            for (int i = 0; i < 4; i++)
            {
                Light light = new Light();
                Vector2 pos = new Vector2();

                //Lights are ordered as follows 0, 1, 2, 3 = EB, WB, SB, NB
                switch (i)
                {
                    case 0:
                        pos = new Vector2(IntersectionPosition.X + 30f, IntersectionPosition.Y + 100f);
                        light.Initialize(LightRed, LightGreen, pos, true);
                        break;
                    case 1:
                        pos = new Vector2(IntersectionPosition.X + 140f, IntersectionPosition.Y + 50f);
                        light.Initialize(LightRed, LightGreen, pos, true);
                        break;
                    case 2:
                        pos = new Vector2(IntersectionPosition.X + 50f, IntersectionPosition.Y + 30f);
                        light.Initialize(LightRed, LightGreen, pos, false);
                        break;
                    case 3:
                        pos = new Vector2(IntersectionPosition.X + 100f, IntersectionPosition.Y + 140f);
                        light.Initialize(LightRed, LightGreen, pos, false);
                        break;
                }
                TrafficLights.Add(light);
            }
        }

        #endregion
    }
}
