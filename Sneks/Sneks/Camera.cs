using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sneks {
    class Camera {
        public Camera() {
            this.position = new Vector2(0, 0);
        }
        public Camera(Vector2 position) {
            this.position = position;
        }
        public Vector2 position { get; set; }
    }
}
