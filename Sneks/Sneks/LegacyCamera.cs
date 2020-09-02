using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace Sneks {
    public class LegacyCamera {

        public LegacyCamera(Vector2 windowSize, Vector2 mapSize) {
            scrollSpeed = 400.0f;
            x = 0.0f;
            y = 0.0f;
            this.mapSize = mapSize;
            this.zoom = true;
        }

        public void update(GameTime gameTime) {
            move(gameTime);
        }

        private void move(GameTime gameTime) {
            if (Keyboard.GetState().IsKeyDown(Keys.Left)) {
                X -= scrollSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right)) {
                X += scrollSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up)) {
                Y -= scrollSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down)) {
                Y += scrollSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Tab)) {
                zoom = false;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Tab)) {
                zoom = true;
            }
        }

        public float X {
            get { return x; }
            set {x = value; if (x < 0) x = 0; if (x + windowSize.X * 2 >= (mapSize.X / 2) - (windowSize.X)) x = (mapSize.X / 2) - (windowSize.X); } 
        }
        public float Y {
            get { return y; }
            set { y = value; if (y < 0) y = 0; if (y >= 200) y = 200; }
        }
        public Vector2 positionTarget {get; set;}
        public float scrollSpeed { get; set; }
        public Vector2 windowSize { get; set; }
        public bool zoom { get; set; }
        private Vector2 mapSize { get; set; }
        private float x;
        private float y;
    }
}