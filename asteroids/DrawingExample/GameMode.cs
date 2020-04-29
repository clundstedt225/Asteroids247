using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using AsteroidTools;
using LineDraw; 

//Asteroids Clone
//Lab 05

namespace DrawingExample
{
    class GameMode : GameApp
    {
        public static int screenWidth = 960;
        public static int screenHeight = 720;

        //Font for score info
        SpriteFont gameFont;
        public static SoundEffect shootSound, explosionAsteroidSound, explosionShipSound;

        //Player info
        public static int playerScore = 0;
        public static int playerLives = 4;
        public static List<Sprite> playerLifeIcons;

        /// <summary>
        /// Public contstructor... Does need to do anything at all. Those are the best constructors. 
        /// </summary>
        public GameMode() : base() { }

        protected override void Initialize()
        {
            base.Initialize();

            // Setting up Screen Resolution
            // Read more here: http://rbwhitaker.wikidot.com/changing-the-window-size
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            Window.Title = "C# Asteroids";

            IsMouseVisible = true;    
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {           
            // TODO: use this.Content to load your game content here
            gameFont = Content.Load<SpriteFont>("MyFont");

            shootSound = Content.Load<SoundEffect>("asteroidFire");
            explosionShipSound = Content.Load<SoundEffect>("shipDeath");
            explosionAsteroidSound = Content.Load<SoundEffect>("asteroidDestroyed");

            SpawnPlayer();

            SpawnAsteroid(4);

            //Populate upper left with player lives
            SetUpPlayerLives();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            // If you create textures on the fly, you need to unload them. 
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void GameUpdate(GameTime gameTime)
        {

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(clearColor);

            spriteBatch.Begin();

            foreach (Sprite icon in playerLifeIcons)
            {
                icon.Draw(spriteBatch);
            }

            // Draw items in our Scene List
            foreach (BaseGameObject go in SceneList)
            {
                //Call each objects draw method
                go.ObjectDraw(spriteBatch);
            }

            //Draw score related text on screen
            //spriteBatch.DrawString(gameFont, "Score ", new Vector2(75, 15), Color.White);
            spriteBatch.DrawString(gameFont, playerScore.ToString(), new Vector2(28, 8), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public static void SetUpPlayerLives()
        {
            //Init list of game objects
            playerLifeIcons = new List<Sprite>();

            //Set up row of extra life icons in upper left of screen
            for (int i = 0; i < playerLives; i++)
            {
                playerLifeIcons.Add(new Sprite("WhiteTriShip"));
                playerLifeIcons[i].origin.X = playerLifeIcons[i].texture.Width / 2;
                playerLifeIcons[i].origin.Y = playerLifeIcons[i].texture.Height / 2;
                playerLifeIcons[i].scale = 0.05f;

                if (i == 0) {
                    //Where the row of lives icons should start from
                    playerLifeIcons[i].position = new Vector2(35, 55);
                } else {
                    //Offset new sprite relative to previous index in list for X value only
                    playerLifeIcons[i].position = new Vector2((playerLifeIcons[i - 1].position.X + 25), 55);
                }
            }
        }

        public static void SpawnPlayer()
        {
            //Player object initial set up
            PlayerShip playerShip = new PlayerShip("WhiteTriShip");
            playerShip.objectSprite.origin.X = playerShip.objectSprite.texture.Width / 2;
            playerShip.objectSprite.origin.Y = playerShip.objectSprite.texture.Height / 2;
            playerShip.Scale = 0.06f;
            playerShip.Position = new Vector2((screenWidth / 2), (screenHeight / 2));
        }

        public static void SpawnAsteroid(int count)
        {
            Random rand = new Random();

            //How many asteroids?
            for (int i = 0; i < count; i++)
            {
                //Generate new random direction
                float rotAngle = MathHelper.ToRadians(rand.Next(360));
                Vector2 direction = new Vector2((float)Math.Cos(rotAngle), (float)Math.Sin(rotAngle));
                direction.Normalize();
               
                //Spawn asteroid with randomized values
                Asteroid asteroid = new Asteroid();
                asteroid.objectCircle = new Circle();
                asteroid.circleSides = 24;
                asteroid.circleRadius = 50f;

                int randHeight;
                int randWidth;

                //Alternate upper and lower areas
                if (i % 2 == 0)
                {
                    //from middle of (screen + 100), to edge
                    randHeight = rand.Next((screenHeight / 2) + 50, screenHeight);
                    randWidth = rand.Next((screenWidth / 2) + 50, screenWidth);
                } else
                {
                    randHeight = rand.Next(0, (screenHeight / 2) - 50);
                    randWidth = rand.Next(0, (screenWidth / 2) - 50);
                }

                asteroid.Position = new Vector2(randWidth, randHeight);

                //Apply constant velocity in randomized direction
                asteroid.Velocity = direction * 12;               
            }
        }

        //Overloaded variant
        public static void SpawnAsteroid(int count, Vector2 pos, float radius, int points)
        {
            Random rand = new Random();

            //How many asteroids?
            for (int i = 0; i < count; i++)
            {
                //Generate new random direction
                float rotAngle = MathHelper.ToRadians(rand.Next(360));
                Vector2 direction = new Vector2((float)Math.Cos(rotAngle), (float)Math.Sin(rotAngle));
                direction.Normalize();

                //Spawn asteroid with randomized values
                Asteroid asteroid = new Asteroid();
                asteroid.objectCircle = new Circle();
                asteroid.circleSides = 24;
                asteroid.circleRadius = radius;
                asteroid.pointValue = points;

                asteroid.Position = pos;

                //Apply constant velocity in randomized direction
                asteroid.Velocity = direction * 12;
            }
        }
    }
}
