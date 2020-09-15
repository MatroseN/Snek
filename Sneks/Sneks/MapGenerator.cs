using Microsoft.Xna.Framework;

namespace Sneks {
    public class MapGenerator {

        public MapGenerator(Vector2 size, float bias) {
            this.size = size;
            this.bias = bias;
            initialize();
        }
        private void initialize() {
        }

        // Occupies map array with values by using the perlin noise. This creates a randomly generated map
        public int[] generateTerrain() {
            map = new int[(int)size.X * (int)size.Y];
            noiseGenerator = new NoiseGenerator((int)size.X, 8, this.bias);
            float[] perlinNoise = noiseGenerator.perlinNoise1DGenerator();

            for (int x = 0; x < (int)size.X; x++) {
                for (int y = 0; y < (int)size.Y; y++) {
                    if (y >= perlinNoise[x] * (int)size.Y) {
                        map[y * (int)size.X + x] = 1;
                    } 
                }
            }
            return map;
        }

        private NoiseGenerator noiseGenerator;
        private int[] map;
        
        public float bias { get; set; }
        public Vector2 size { get; set; }
    }
}