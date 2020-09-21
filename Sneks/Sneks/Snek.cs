using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sneks {
    public class Snek: Entity, Collidable {

        public Snek(Vector2 size, Vector2 position, Dictionary<Guid, Entity> entities) {
            this.size = size;
            this.position = position;
            this.acceleration = new Vector2(0, 0);
            this.velocity = new Vector2(0, 0);
            addToDictionary(entities);
        }

        public override void update(GameTime gameTime) {
            // IMPLEMENT CORRECTLY BELOW
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            gravity();
            updateVelocity(delta);
            this.position = potentialPosition(delta);
            resetAcceleration();
        }

        private void gravity() {

            this.acceleration = new Vector2(0, acceleration.Y + 35.0f);
        }

        private void updateVelocity(float delta) {
            velocity = new Vector2(velocity.X + acceleration.X * delta, velocity.Y + acceleration.Y * delta);
        }

        private Vector2 potentialPosition(float delta) {
            return  new Vector2(position.X + velocity.X * delta, position.Y + velocity.Y * delta);
        }

        private void resetAcceleration() {
            this.acceleration = new Vector2(0, 0);
            this.stable = false;
        }

        public bool checkOverlap() {
            // IMPLEMENT CORRECTLY BELOW IS JUST PLACEHOLDER
            return false;
        }

        public Vector2 collisionPoint() {
            // IMPLEMENT CORRECTLY BELOW IS JUST PLACEHOLDER
            return new Vector2(0, 0);
        }

        public override void addToDictionary(Dictionary<Guid, Entity> entities) {
            entities.Add(this.generateGuid(), this);
        }
    }
}