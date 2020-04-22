using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DrawingExample
{

    public class BaseGameObject
    {
        //public Vector2 ScreenSize = new Vector2(1280, 960);
        public Sprite objectSprite;
        public string spriteName;

        public bool isActive = true;
        public Vector2 Position = Vector2.Zero;
        public float Scale = 0.5f;
        public float Rotation = 0;
        public Vector2 Velocity;
        public bool HasMaxiumVelocity = false;
        public float MaxiumVelocity = float.MaxValue;
   
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

            //Wrapped play field logic (all game objects should behave this way)
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
        }

        public void Destroy()
        {
            OnDestroy();
            GameApp.instance.SceneList.Remove(this);
        }

        public virtual void OnDestroy()
        {
            isActive = false;
        }
    }

}
