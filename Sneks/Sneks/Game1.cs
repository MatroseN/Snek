using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

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

            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.PreferredBackBufferHeight = 512;
            _graphics.ApplyChanges();

            block = new Rectangle(new Vector2(3, 3));
            block.createTexture(_graphics.GraphicsDevice, pixel=> Color.Green);

            mapGenerator = new MapGenerator(new Vector2(1024, 512));
            map = mapGenerator.generateTerrain();

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

            int mapIterator = map.Length / 256;

            for (int y = 0; y < 512; y++) {              
                for (int x = 0; x < 1024; x++) {
                    switch (map[mapIterator]) {
                        case 1: 
                            _spriteBatch.Draw(block.texture, new Vector2(x, y), Color.White);
                            break;
                    }
                    if(mapIterator < map.Length - 1) {
                        mapIterator++;
                    } else {
                        break;
                    }
                }
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private Snek snek;
        private Dictionary<Guid, Entity> entities;
        private MapGenerator mapGenerator;
        private int[] map;
        private Rectangle block;
    }
}