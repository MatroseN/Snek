using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sneks {
    class NoiseGenerator {
        public NoiseGenerator(int outputSize, int octaves, float bias) {
            this.outputSize = outputSize;
            this.octaves = octaves;
            this.bias = bias;
            this.noiseSeed1D = new float[outputSize];
            this.perlinNoise1D = new float[outputSize];
            noiseFill();
        }

        // Fills the seed array with random values between 0.0f and 1.0f
        private void noiseFill() {
            this.random = new Random();
            for (int i = 0; i < outputSize; i++) {
                float randomNum = (float)random.NextDouble();
                noiseSeed1D[i] = randomNum;
            }

            noiseSeed1D[0] = 0.5f;
        }

        // Generates a 1 dimensional perlin-like noise based on the outputSize, number of octaves and bias
        public float[] perlinNoise1DGenerator() {
            for (int x = 0; x < outputSize; x++) {
                float noise = 0.0f;
                float scaleAcc = 0.0f;
                float scale = 1.0f;

                for (int o = 0; o < octaves; o++) {
                    int pitch = outputSize >> o;
                    int sample1 = (x / pitch) * pitch;
                    int sample2 = (sample1 + pitch) % outputSize;
                    float blend = (float)(x - sample1) / (float)pitch; // How far into the pitch we are. Value between 1.0 and 0.0
                    float sample = (1.0f - blend) * noiseSeed1D[sample1] + blend * noiseSeed1D[sample2];
                    scaleAcc += scale;
                    noise += sample * scale;
                    scale = scale / bias;
                }
                perlinNoise1D[x] = noise / scaleAcc;
            }
            return perlinNoise1D;
        }

        private int outputSize;
        private int octaves;
        private float bias;
        private float[] noiseSeed1D;
        private float[] perlinNoise1D;
        private Random random;
    }
}