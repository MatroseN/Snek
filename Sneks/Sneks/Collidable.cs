using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Sneks {
    interface Collidable {

        // Check if there is any overlap with other collidables points
        bool checkOverlap();

        // A point that represents the collision area (Only one pixel)
        Vector2 collisionPoint();
    }
}