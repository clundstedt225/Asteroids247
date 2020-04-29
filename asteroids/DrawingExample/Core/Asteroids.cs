using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DrawingExample;
using LineDraw;

namespace AsteroidTools
{
    
    class Asteroid : BaseGameObject
    {
        // Large = 20 pts, Medium = 50 pts, Small = 100 pts...
        public int pointValue = 20;
        public Size asteroidSize = Size.Large;
        
        //Asteroid sizes
        public enum Size {
            Large,
            Medium,
            Small
        }

        public Asteroid()
        {
            //Set collision checks to TRUE
            collisionChecks = true;

        }

        public override void OnDestroy()
        {
            isActive = false;

            //Award Point value
            GameMode.playerScore += pointValue;

            //Play explosion sound
            GameMode.explosionAsteroidSound.Play();

            // - Spawn 2 smaller level asteroids (if not already small)         
        }
    }

    class PlayerShip : BaseGameObject
    {

        public PlayerShip(string spriteName) : base(spriteName)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            //Call base so this only extends update for the player ship
            base.Update(gameTime);

            //Get forward direction of ship
            float rotAngle = MathHelper.ToRadians(-90);
            Vector2 direction = new Vector2((float)Math.Cos(Rotation + rotAngle), (float)Math.Sin(Rotation + rotAngle));
            direction.Normalize();

            //Shoot torpedo
            if (GameApp.instance.IsKeyPressed(Keys.Space))
            {
                //Create circle game object
                BaseGameObject torpedo = new BaseGameObject();
                torpedo.objectCircle = new Circle();
                torpedo.circleRadius = 3.5f;
                torpedo.destroyOnCollide = true;
                torpedo.useTimer = true;
                torpedo.Position = Position;

                torpedo.Velocity = direction * 950;

                GameMode.shootSound.Play();
            }

            if (GameApp.instance.IsKeyHeld(Keys.A))
            {
                Rotation -= 5 * (MathHelper.Pi / 180);
            }

            if (GameApp.instance.IsKeyHeld(Keys.D))
            {
                Rotation += 5 * (MathHelper.Pi / 180);
            }

            //Activate ships thruster
            if (GameApp.instance.IsKeyHeld(Keys.W))
            {
                //Increase velocity in forward direction of ship
                Velocity += direction * 5;
            }
            else
            {
                //Key not pressed, no thrusters so decrease velocity to 0
                if (Velocity.X != 0)
                {
                    if (Velocity.X > 0)
                    {
                        Velocity.X -= 1.5f;
                    }
                    else
                    {
                        Velocity.X += 1.5f;
                    }
                }

                if (Velocity.Y != 0)
                {
                    if (Velocity.Y > 0)
                    {
                        Velocity.Y -= 1.5f;
                    }
                    else
                    {
                        Velocity.Y += 1.5f;
                    }
                }
            }
        }


        public override void OnDestroy()
        {
            isActive = false;

            //Play death sound 
            GameMode.explosionShipSound.Play();

            //Respawn if lives are available
            if (GameMode.playerLives > 0)
            {
                //Subtract from lives counter
                GameMode.playerLives--;
                
                //Remove an icon from lives
                GameMode.playerLifeIcons.RemoveAt(GameMode.playerLives);

                //Call Spawn Player to make new one
                GameMode.SpawnPlayer();
            }
        }
    }
}

/*
    asteroid -> torpedo (both destroyed, asteroid brought down a level)
    asteroid -> ship (ship dissapears, life lost, if no lives don't respawn)
*/
