using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DrawingExample;

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
            //base.OnDestroy();
            isActive = false;

            // - Award Point value
            GameMode.playerScore += pointValue;

            // - Play explosion sound

            // - Spawn 2 smaller level asteroids (if not already small)         
        }
    }

    class PlayerShip : BaseGameObject
    {
        public int playerLives = 4;

        public PlayerShip(string spriteName) : base(spriteName)
        {
            
        }

        public override void OnDestroy()
        {
            //base.OnDestroy();

            //Play death sound 

            //Respawn if lives are available
            if (playerLives > 0)
            {
                playerLives--;
                GameMode.playerLifeIcons.RemoveAt(playerLives);
            }
        }
    }
}

/*
    asteroid -> torpedo (both destroyed, asteroid brought down a level)
    asteroid -> ship (ship dissapears, life lost, if no lives don't respawn)
*/
