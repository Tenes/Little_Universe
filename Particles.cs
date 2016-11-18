using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UniverseV2
{
    class Particles
    {
        //FIELDS
        private Texture2D texture;
        private Vector2 position;
        private Vector2 origin;
        private Vector2 velocity;
        private Vector2 acceleration;
        private float friction;

        //CONSTRUCTOR
        public Particles(Vector2 position)
        {
            this.texture = Ressources.sprites["Dot"];
            this.position = position;
            this.origin = new Vector2(this.position.X, this.position.Y);
            this.velocity = new Vector2(0,0);
            this.acceleration = new Vector2(0, 0);
            this.friction = 0.8f;
        }

        //METHODS
        public void PushingMouse(Planet planet)
        {
            if (!planet.destroyed)
            {
                Vector2 planetPosition = planet.position;
                float distance = Vector2.Distance(planetPosition, this.position);
                Vector2.Subtract(ref planetPosition, ref this.position, out planetPosition);
                float magnitude = planetPosition.Length();
                if (distance < planet.radius)
                {
                    this.acceleration = planetPosition;
                    Vector2.Multiply(ref this.acceleration, -1 / magnitude * magnitude, out this.acceleration);
                }
                else
                    this.acceleration = new Vector2(0, 0);
            }
        }

        public void UpdateVelocity(float delta)
        {
            Vector2.Divide(ref this.acceleration, 10, out this.acceleration);
            Vector2.Add(ref this.velocity, ref this.acceleration, out this.velocity);
            Vector2.Multiply(ref this.velocity, 0.9f, out this.velocity);
        }

        public void UpdatePosition(float delta)
        {
            Vector2.Add(ref this.position, ref this.velocity, out this.position);
        }

        public void GravityToOrigin()
        {
            Vector2 distanceToOriginV = new Vector2(this.position.X - this.origin.X, this.position.Y - this.origin.Y);
            Vector2.Multiply(ref distanceToOriginV, 0.1f, out distanceToOriginV);
            float distanceToOriginF = Vector2.Distance(this.position, this.origin);
            if (distanceToOriginF != 0)
                Vector2.Subtract(ref this.acceleration, ref distanceToOriginV, out this.acceleration);
        }

        //UPDATE & DRAW
        public void Update(float delta, Planet planet)
        {
            PushingMouse(planet);
            GravityToOrigin();
            UpdateVelocity(delta);
            UpdatePosition(delta);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.position, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
        }
    }
}
