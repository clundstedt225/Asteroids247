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
        public Vector2 Position = Vector2.Zero;
        public float Scale = 0.5f;
        public float Rotation = 0;
        public Vector2 Velocity;
        public bool HasMaxiumVelocity = false;
        public float MaxiumVelocity = float.MaxValue;

        //Whether or not to use a self clean up timer
        public bool useTimer = false;
        float delay = 2f;

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
                objectSprite.position = Position;
                objectSprite.rotation = Rotation;
                objectSprite.scale = Scale;
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

            //Wrapped play field logic
            //Handle the X axis checks
            if (Position.X > (800 + sBuffer))
            {
                Position.X = 0 + -sBuffer;
            } else if (Position.X < (0 - sBuffer))
            {
                Position.X = 800 + sBuffer;
            }

            //Handle the Y axis checks
            if (Position.Y > (600 + sBuffer))
            {
                Position.Y = 0 + -sBuffer;
            } else if (Position.Y < (0 - sBuffer))
            {
                Position.Y = 600 + sBuffer;
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
    }

}
