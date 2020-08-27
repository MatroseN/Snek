using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sneks {
    class Rectangle : Shape {
        public Rectangle(Vector2 size) {
            this.size = size;
        }
        public override void createTexture(GraphicsDevice graphics, Func<int, Color> paint) {
            this.texture = new Texture2D(graphics, (int)size.X, (int)size.Y);

            Color[] data = new Color[(int)size.X * (int)size.Y];
            for (int pixel = 0; pixel < data.Count(); pixel++) {
                data[pixel] = paint(pixel);
            }

            this.texture.SetData(data);
        }

        public Vector2 size { get; private set; }
    }
}
