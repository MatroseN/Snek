using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sneks {
    public abstract class Entity {
        // Override with update logic 
        public abstract void update(GameTime gameTime);

        public abstract void addToDictionary(Dictionary<Guid, Entity> entities);

        public Guid generateGuid() {
            return Guid.NewGuid();
        }

        public Vector2 size { get; set; }
        public Vector2 position { get; set; }
        public Texture2D texture { get; set; }
        public Vector2 velocity { get; set; }
        public Vector2 acceleration { get; set; }
        public Guid id { get; protected set; }
        public float collisionBoundry { get; set; }
        public bool stable { get; set; }
    }
}
