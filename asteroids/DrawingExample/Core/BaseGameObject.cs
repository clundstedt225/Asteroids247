using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using LineDraw;

namespace DrawingExample
{

    public class BaseGameObject
    {
        //Visual components
        public Circle objectCircle;
        public Sprite objectSprite;
        public string spriteName;

        //Circle specific data
        public Color circleColor = Color.White;
        public float circleWidth = 5f;
        public float circleRadius = 5f;
        public int circleSides = 12;

        public bool isActive = true;
        public bool collisionChecks = false;
        public bool destroyOnCollide = false;

        public Vector2 Position = Vector2.Zero;
        public float Scale = 0.5f;
        public float Rotation = 0;
        public Vector2 Velocity;
        public bool HasMaxiumVelocity = false;
        public float MaxiumVelocity = float.MaxValue;

        //Whether or not to use a self clean up timer
        public bool useTimer = false;
        float delay = 1f;

        //Screen Wrapping Buffer
        int sBuffer = 25;

        public BaseGameObject()
        {
            GameApp.instance.SceneList.Add(this);

            InitalizeObject();
        }

        //Overloaded ctor if sprite name given
        public BaseGameObject(string spriteName)
        {
            GameApp.instance.SceneList.Add(this);
            objectSprite = new Sprite(spriteName);
            InitalizeObject();
        }

        public virtual void InitalizeObject()
        {

        }

        public void ObjectUpdate(GameTime gameTime)
        {
            if (isActive)
            {

                Update(gameTime);
                if (HasMaxiumVelocity)
                {
                    ScrubVelocity(); //enforce maxmium velocity
                }

                float gT = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Position += Velocity * gT;
            }
        }

        public void ScrubVelocity()
        {
            if (Velocity.Length() > MaxiumVelocity)
            {
                Velocity.Normalize();
                Velocity *= MaxiumVelocity;
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            //Update sprite based on object data
            if (objectSprite != null)
            {
                //Sprites position needs to be set at the sprite object level
                objectSprite.position = Position;
                objectSprite.rotation = Rotation;
                objectSprite.scale = Scale;
            }

            if (objectCircle != null)
            {
                objectCircle.color = circleColor;
                //circle is just drawn directly at GO's position
                //radius uses local object radius variable for drawing
                objectCircle.Sides = circleSides;
                objectCircle.Width = circleWidth;
            }

            if (useTimer)
            {
                float timer = (float)gameTime.ElapsedGameTime.TotalSeconds;

                delay -= timer;

                if (delay <= 0)
                {
                    Destroy();
                }
            }

            //Should this game object check for collisions?
            if (collisionChecks)
            {
                CollisionCheck();
            }

            //Wrapped play field logic
            //Handle the X axis checks
            if (Position.X > (GameMode.screenWidth + sBuffer))
            {
                Position.X = 0 + -sBuffer;
            } else if (Position.X < (0 - sBuffer))
            {
                Position.X = GameMode.screenWidth + sBuffer;
            }

            //Handle the Y axis checks
            if (Position.Y > (GameMode.screenHeight + sBuffer))
            {
                Position.Y = 0 + -sBuffer;
            } else if (Position.Y < (0 - sBuffer))
            {
                Position.Y = GameMode.screenHeight + sBuffer;
            }
        
        }

        public void ObjectDraw(SpriteBatch spriteBatch)
        {
            if (isActive)
            {
                Draw(spriteBatch);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //Called if object is active

            //Draw this game object on screen
            if(objectSprite != null)
                objectSprite.Draw(spriteBatch);

            if (objectCircle != null)
                LinePrimatives.DrawCircle(spriteBatch, objectCircle.Width, objectCircle.color, Position, circleRadius, objectCircle.Sides);
        }

        public void Destroy()
        {
            OnDestroy();

            //GameApp.instance.SceneList.Remove(this);

            //Changed code to add to destroy list instead, so as to not modify collection while iterating
            GameApp.instance.DestroyList.Add(this);
        }

        public virtual void OnDestroy()
        {
            isActive = false;
        }

        public void CollisionCheck()
        {
            //Go through each object in scene for circle collision check
            foreach (BaseGameObject go in GameApp.instance.SceneList)
            {
                //Compare if distance to other given object is less then the combined radius of objects
                if (Vector2.Distance(this.Position, go.Position) <= (this.circleRadius + go.circleRadius))
                {
                    //Has collided with current go in list...

                    //Destroy both objects (torpedo -> asteroid collision)
                    if ((this is AsteroidTools.Asteroid) && go.destroyOnCollide)
                    {
                        //Destroy other object first
                        go.Destroy();
                        this.Destroy();

                    } else if ((this is AsteroidTools.Asteroid) && (go is AsteroidTools.PlayerShip)) 
                    {
                        //(Asteroid -> Player)
                        //Destroy only other object 
                        go.Destroy();
                    }
                }
            }
        }
    }

}
