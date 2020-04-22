using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using AsteroidTools;
using LineDraw; 

//Asteroids Clone
//Lab 05

namespace DrawingExample
{
    class GameMode : GameApp
    {
        float offset = 0;
        
        //Font for score info
        SpriteFont gameFont;
        BaseGameObject playerShip;

        //Player info
        int playerScore = 0000;
        int playerLives = 4;
        List<BaseGameObject> playerLifeIcons;

        /// <summary>
        /// Public contstructor... Does need to do anything at all. Those are the best constructors. 
        /// </summary>
        public GameMode() : base() { }

        protected override void Initialize()
        {
            base.Initialize();

            // Setting up Screen Resolution
            // Read more here: http://rbwhitaker.wikidot.com/changing-the-window-size
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

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

            //Player object initial set up
            playerShip = new BaseGameObject("WhiteTriShip");
            playerShip.objectSprite.origin.X = playerShip.objectSprite.texture.Width / 2;
            playerShip.objectSprite.origin.Y = playerShip.objectSprite.texture.Height / 2;
            playerShip.Scale = 0.07f;
            playerShip.Position = new Vector2((graphics.PreferredBackBufferWidth / 2), (graphics.PreferredBackBufferHeight / 2));

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

            offset += gameTime.ElapsedGameTime.Milliseconds;

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            int deltaScrollWheel = mousePrevious.ScrollWheelValue - mouseCurrent.ScrollWheelValue; 
            if (deltaScrollWheel != 0)
            {
                //theGrid.GridSize += (Math.Abs(deltaScrollWheel) / deltaScrollWheel) * 2; 
            }

            //mouseCurrent.MiddleButton
            if (IsKeyHeld(Keys.A))
            {
                playerShip.Rotation -= 5 * (MathHelper.Pi / 180); ;
            }

            //mouseCurrent.MiddleButton
            if (IsKeyHeld(Keys.D))
            {
                playerShip.Rotation += 5 * (MathHelper.Pi / 180); ;
            }

            //Activate ships thruster
            if (IsKeyHeld(Keys.W))
            {
                //Get forward direction of ship
                float rotAngle = MathHelper.ToRadians(-90);
                Vector2 direction = new Vector2((float)Math.Cos(playerShip.Rotation + rotAngle), (float)Math.Sin(playerShip.Rotation + rotAngle));
                direction.Normalize();

                //Increase velocity in forward direction of ship
                playerShip.Velocity += direction * 5;
            }
            else
            {
               //Key not pressed, no thrusters so decrease velocity to 0
               if (playerShip.Velocity.X != 0)
               {
                   if (playerShip.Velocity.X > 0)
                   {
                       playerShip.Velocity.X -= 1;
                   }
                   else
                   {
                       playerShip.Velocity.X += 1;
                   }
               }

               if (playerShip.Velocity.Y != 0)
               {
                   if (playerShip.Velocity.Y > 0)
                   {
                       playerShip.Velocity.Y -= 1;
                   }
                   else
                   {
                       playerShip.Velocity.Y += 1;
                   }
               }              
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

        void SetUpPlayerLives()
        {
            //Init list of game objects
            playerLifeIcons = new List<BaseGameObject>();

            //Set up row of extra life icons in upper left of screen
            for (int i = 0; i < playerLives; i++)
            {
                playerLifeIcons.Add(new BaseGameObject("WhiteTriShip"));
                playerLifeIcons[i].objectSprite.origin.X = playerShip.objectSprite.texture.Width / 2;
                playerLifeIcons[i].objectSprite.origin.Y = playerShip.objectSprite.texture.Height / 2;
                playerLifeIcons[i].Scale = 0.05f;

                if (i == 0)
                {
                    //Where the row of lives icons should start from
                    playerLifeIcons[i].Position = new Vector2(35, 55);
                }
                else
                {
                    //Offset new sprite relative to previous index in list for X value only
                    playerLifeIcons[i].Position = new Vector2((playerLifeIcons[i - 1].Position.X + 25), 55);
                }
            }
        }
    }
}
