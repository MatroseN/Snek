using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace Sneks {
    public class Camera {

        public Camera(Vector2 windowSize, Vector2 mapSize) {
            scrollSpeed = 450.0f;
            x = 0.0f;
            y = 0.0f;
            this.mapSize = mapSize;
        }

        public void update(GameTime gameTime) {
            move(gameTime);
        }

        private void move(GameTime gameTime) {
            if ((x > 0) && (Keyboard.GetState().IsKeyDown(Keys.Left))) {
                x -= scrollSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if ((x + windowSize.X < mapSize.X - 20) && Keyboard.GetState().IsKeyDown(Keys.Right)) {
                x += scrollSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if ((y > 0 ) && (Keyboard.GetState().IsKeyDown(Keys.Up))) {
                y -= scrollSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if ((y + windowSize.Y + 800 < mapSize.Y) && (Keyboard.GetState().IsKeyDown(Keys.Down))) {
                Debug.WriteLine(y + windowSize.Y + " " + mapSize.Y);
                y += scrollSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public float x { get; set; }
        public float y { get; set; }
        public Vector2 positionTarget {get; set;}
        public float scrollSpeed { get; set; }
        public Vector2 windowSize { get; set; }
        private Vector2 mapSize { get; set; }
    }
}
