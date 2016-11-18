using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UniverseV2
{
    public class Planet
    {
        public Vector2 origin;
        public Texture2D texture;
        public Vector2 position;
        public float scale;
        public SpriteEffects effects;
        public float layerDepth;
        public float radius;
        
        public double mass;
        public Vector2 velocity;
        public Vector2 acceleration;
        public Color color;
        public bool destroyed;


        public Planet(string imageKey, Vector2 position, float scale, Color color)
        {
            this.texture = Ressources.sprites[imageKey];
            this.position = new Vector2(position.X, position.Y);
            this.scale = scale;
            this.effects = SpriteEffects.None;
            this.layerDepth = 0f;
            this.radius = (this.texture.Width * this.scale)/2;
            this.origin = new Vector2((float)(0.5 * this.texture.Width), (float)(0.5 * this.texture.Height));
            this.acceleration = new Vector2(0, 0);
            this.velocity = new Vector2(0,0);
            this.mass = (10*this.radius);
            this.color = color;
            this.destroyed = false;
        }

        public Planet(string imageKey, Vector2 position, float scale, Vector2 speed, Color color)
        {
            this.texture = Ressources.sprites[imageKey];
            this.position = new Vector2(position.X, position.Y);
            this.scale = scale;
            this.effects = SpriteEffects.None;
            this.layerDepth = 0f;
            this.radius = (this.texture.Width * this.scale) / 2;
            this.origin = new Vector2((float)(0.5 * this.texture.Width), (float)(0.5 * this.texture.Height));
            this.acceleration = new Vector2(0, 0);
            this.velocity = speed;
            this.mass = (5 * this.radius);
            this.color = color;
            this.destroyed = false;
        }


        //METHODS
        public void Divide(List<Planet> galaxy, Random rng)
        {
            for (int n = 0; n < 2; n++)
            {
                galaxy.Add(new Planet("planet1", new Vector2((float)rng.Next((int)this.position.X-50, (int)this.position.X+50), (float)rng.Next((int)this.position.Y - 50, (int)this.position.Y + 50)), (float)(this.scale/2), new Vector2(rng.Next(-10, 10), rng.Next(-10,10)), this.color));
            }
            this.scale = this.scale/2;
            this.radius = (this.texture.Width * this.scale) / 2;
            this.mass = (5*this.radius);
            this.velocity = new Vector2(rng.Next(-10, 10), rng.Next(-10, 10));
        }
        public void Absorb(double otherMass, float otherRadius, ref double newMass, ref float newRadius, Planet actual)
        {
            newRadius += otherRadius/2;
            newMass += otherMass/2;
            actual.scale = (newRadius*2)/actual.texture.Width;
        }
        public void UpdatePosition(float delta)
        {
            this.position.X += this.velocity.X * delta;
            this.position.Y += this.velocity.Y * delta;
        }

        public void CalculateGravity(Planet other)
        {
            Vector2 dist = new Vector2(other.position.X - this.position.X, other.position.Y - this.position.Y);
            var length = Math.Sqrt(dist.X*dist.X + dist.Y*dist.Y);
            this.acceleration.X += (float)(other.mass * (dist.X / length));
            this.acceleration.Y += (float)(other.mass * (dist.Y / length));
        }

        public void ApplyAcceleration(float delta)
        {
            this.velocity.X += (float)(this.acceleration.X * delta * 0.02);
            this.velocity.Y += (float)(this.acceleration.Y * delta * 0.02);
            this.acceleration = new Vector2(0,0);
        }

        public void CollisionWith(Planet other, List<Planet> galaxy, Random rng)
        {
            if (Vector2.Distance(this.position, other.position) < this.radius + other.radius)
            {
                if (this.mass < other.mass)
                {
                    this.destroyed = true;
                    other.Absorb(this.mass, this.radius, ref other.mass, ref other.radius, other);
                    galaxy.Remove(this);
                }
                else if (this.mass > other.mass)
                {
                    other.destroyed = true;
                    this.Absorb(other.mass, other.radius, ref this.mass, ref this.radius, this);
                    galaxy.Remove(other);
                }
                else if (this.mass == other.mass)
                {
                    this.Divide(galaxy, rng);
                    other.Divide(galaxy, rng);
                }
            }
        }
        //UPDATE & DRAW
        public void Update(float delta, Planet planet2, List<Planet> galaxy, Random rng)
        {
            if (this != planet2 )
            {
                this.CollisionWith(planet2, galaxy, rng);
                this.CalculateGravity(planet2);
            }
            if (this != galaxy[0])
            {
                this.ApplyAcceleration(delta);
                this.UpdatePosition(delta);
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(!this.destroyed)
                spriteBatch.Draw(this.texture, this.position, null, this.color, 0, this.origin , this.scale, this.effects, this.layerDepth);
        }
    }
}