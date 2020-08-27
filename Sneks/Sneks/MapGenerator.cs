using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Xna.Framework;

namespace Sneks {
    public class MapGenerator {

        public MapGenerator(Vector2 size) {
            this.size = size;
            initialize();
        }
        private void initialize() {
            map = new int[(int)size.X * (int)size.Y];
            noiseGenerator = new NoiseGenerator((int)size.X, 8, 2.0f);
        }

        // Occupies map array with values by using the perlin noise. This creates a randomly generated map
        public int[] generateTerrain() {
            float[] perlinNoise = noiseGenerator.perlinNoise1DGenerator();

            for (int x = 0; x < (int)size.X; x++) {
                for (int y = 0; y < (int)size.Y; y++) {
                    if (y >= perlinNoise[x] * (int)size.Y) {
                        map[y * (int)size.X + x] = 1;
                    } else {
                        if ((float)y < size.Y / 3.0f) {
                            float temp = (-8.0f * ((float)y / (size.Y / 3.0f))) - 1.0f;
                            map[y * (int)size.X + x] = (int)temp;
                        } else {
                            map[y * (int)size.X + x] = 0;
                        }
                    }
                }
            }
            return map;
        }

        private Vector2 size;
        private NoiseGenerator noiseGenerator;
        private int[] map;
    }
}