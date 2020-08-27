using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sneks {
    public class Game1 : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.PreferredBackBufferHeight = 512;
        }

        protected override void Initialize() {
            // TODO: Add your initialization logic here

            windowSize = new Vector2(1024, 512);

            _graphics.PreferredBackBufferWidth = (int)windowSize.X;
            _graphics.PreferredBackBufferHeight = (int)windowSize.Y;
            _graphics.ApplyChanges();

            block = new Block(new Vector2(3, 3));
            block.createTexture(_graphics.GraphicsDevice, pixel=> Color.Green);

            mapSize = new Vector2(1000, 900);
            mapGenerator = new MapGenerator(mapSize);
            map = mapGenerator.generateTerrain();

            camera = new Camera(windowSize, mapSize);


            entities = new Dictionary<Guid, Entity>();

            snek = new Snek(new Vector2(32, 64), new Vector2(400, 100), entities);
      

            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            camera.update(gameTime);

            foreach (Entity entity in entities.Values) {
                entity.update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // TODO: Add your drawing code here
            foreach (Entity entity in entities.Values) {
               // _spriteBatch.Draw(entity.texture, entity.position, Color.White);
            }

            for (int y = 0; y < (int)windowSize.Y; y++) {              
                for (int x = 0; x < (int)windowSize.X; x++) {
                    int blockPos = (y + (int)camera.y) * (int)mapSize.X + (x + (int)camera.x);
                    switch (map[blockPos]) {
                        case 1:
                            _spriteBatch.Draw(block.texture, new Vector2(x, y), Color.White);
                            break;
                    }
                }
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private Camera camera;
        private Snek snek;
        private Dictionary<Guid, Entity> entities;
        private MapGenerator mapGenerator;
        private int[] map;
        private Vector2 mapSize;
        private Block block;
        private Vector2 windowSize;
    }
}