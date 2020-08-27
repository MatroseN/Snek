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
            addToDictionary(entities);
        }

        public override void update(GameTime gameTime) {
            // IMPLEMENT CORRECTLY BELOW
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
