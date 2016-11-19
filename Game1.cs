using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UniverseV2
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public List<Planet> galaxy;
        public Planet earth;
        private List<Particles> particles;
        private Random rng;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 900;
            graphics.PreferredBackBufferWidth = 1500;
            this.IsMouseVisible = true;
        }
        
        protected override void Initialize()
        {
            base.Initialize();
            this.galaxy = new List<Planet>();
            this.galaxy.Add(new Planet("planet1", new Vector2((float)graphics.PreferredBackBufferWidth / 2, (float)graphics.PreferredBackBufferHeight / 2), 0.5f, new Vector2(0, 0), Color.LightGoldenrodYellow));
            this.galaxy.Add(new Planet("planet1", new Vector2((float)graphics.PreferredBackBufferWidth / 2 - 300, (float)graphics.PreferredBackBufferHeight / 2), 0.05f, new Vector2(0, 40), Color.Blue));
            this.galaxy.Add(new Planet("planet1", new Vector2((float)graphics.PreferredBackBufferWidth / 2 + 300, (float)graphics.PreferredBackBufferHeight / 2), 0.05f, new Vector2(0, -40), Color.SandyBrown));
            this.galaxy.Add(new Planet("planet1", new Vector2((float)graphics.PreferredBackBufferWidth / 2 - 400, (float)graphics.PreferredBackBufferHeight / 2), 0.05f, new Vector2(0, -80), Color.YellowGreen));
            this.galaxy.Add(new Planet("planet1", new Vector2((float)graphics.PreferredBackBufferWidth / 2 + 500, (float)graphics.PreferredBackBufferHeight / 2), 0.05f, new Vector2(0, 90), Color.DarkRed));
            this.galaxy.Add(new Planet("planet1", new Vector2((float)graphics.PreferredBackBufferWidth / 2, (float)graphics.PreferredBackBufferHeight / 2 + 300), 0.05f, new Vector2(40, 0), Color.Blue));
            this.galaxy.Add(new Planet("planet1", new Vector2((float)graphics.PreferredBackBufferWidth / 2, (float)graphics.PreferredBackBufferHeight / 2 - 300), 0.05f, new Vector2(-50, 0), Color.SandyBrown));
            this.galaxy.Add(new Planet("planet1", new Vector2((float)graphics.PreferredBackBufferWidth / 2, (float)graphics.PreferredBackBufferHeight / 2 + 400), 0.05f, new Vector2(-80, 0), Color.YellowGreen));
            this.galaxy.Add(new Planet("planet1", new Vector2((float)graphics.PreferredBackBufferWidth / 2, (float)graphics.PreferredBackBufferHeight / 2 - 500), 0.05f, new Vector2(-90, 0), Color.DarkRed));

            this.rng = new Random();
            this.particles = new List<Particles>();
            for (int i = 0; i < 15000; i++)
            {
                this.particles.Add(new Particles(new Vector2(rng.Next(graphics.PreferredBackBufferWidth) + 1, rng.Next(graphics.PreferredBackBufferHeight) + 1)));
            }
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Ressources.LoadSprites(this.Content);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            for (int planet1 = galaxy.Count-1; planet1 >= 0; planet1--)
            {
                if (galaxy[planet1] != galaxy[0])
                {
                    for (int j = 0; j < particles.Count; j++)
                        particles[j].Update(delta, galaxy[planet1]);
                }
                
                for (int planet2 = galaxy.Count - 1; planet2 >= 0; planet2--)
                    galaxy[planet1].Update(delta, galaxy[planet2], galaxy, rng);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            this.spriteBatch.Begin();
            foreach (Planet planet in galaxy)
            {
                planet.Draw(spriteBatch);
            }
            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].Draw(spriteBatch);
            }
            this.spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
