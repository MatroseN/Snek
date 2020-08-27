using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sneks {
    abstract class Shape {
        public abstract void createTexture(GraphicsDevice graphics, Func<int, Color> paint);
        public Texture2D texture { get; protected set; }
    }
}
