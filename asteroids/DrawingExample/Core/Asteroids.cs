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
        public int pointValue;
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
            //On Destroyed
            // - Award Point value
            // - Explode
            // - Spawn 2 smaller level asteroids (if not already small)         
        }
    }
}

/*
    asteroid -> torpedo (both destroyed, asteroid brought down a level)
    asteroid -> ship (ship dissapears, life lost, if no lives don't respawn)
*/
