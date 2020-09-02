using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sneks {
    class QuadGenerator {

        public void createQuads(GraphicsDevice graphicsDevice, int quadTotalVertecies, Vector2 mapSize, int[] map) {
            // Setup a vertex buffer
            iQuadTotal = (int)mapSize.X * (int)mapSize.Y;
            VertexPositionColorTexture[] vertecies = new VertexPositionColorTexture[iQuadTotal * quadTotalVertecies];
            VertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColorTexture), vertecies.Length, BufferUsage.WriteOnly);

            int x = 0;
            int y = 0;

            int iMapX = (int)mapSize.X - 1;
            int iMapY = (int)mapSize.Y - 1;

            for (int cell = 0; cell < iQuadTotal; cell++) {
                int cellIndex = cell * 6;
                Color color = Color.White;


                switch (map[iMapX + (int)mapSize.X * iMapY]) {
                    case 1:
                         // Vertex Position
                // - Triangle 1
                vertecies[cellIndex + 0].Position = new Vector3((x * 3), (y * 3), 0);
                vertecies[cellIndex + 1].Position = new Vector3((x * 3), 3 + (y * 3), 0);
                vertecies[cellIndex + 2].Position = new Vector3(3 + (x * 3), (y * 3), 0);

                // - Triangle 2
                vertecies[cellIndex + 3].Position = vertecies[cellIndex + 1].Position;
                vertecies[cellIndex + 4].Position = new Vector3(3 + (x * 3), 3 + (y * 3), 0);
                vertecies[cellIndex + 5].Position = vertecies[cellIndex + 2].Position;

                // Vertex Color
                // - Triangle 1
                vertecies[cellIndex + 0].Color = color;
                vertecies[cellIndex + 1].Color = color;
                vertecies[cellIndex + 2].Color = color;

                // - Triangle 2
                vertecies[cellIndex + 3].Color = color;
                vertecies[cellIndex + 4].Color = color;
                vertecies[cellIndex + 5].Color = color;

                // Vertex Texture
                // - Triangle 1
                vertecies[cellIndex + 0].TextureCoordinate = new Vector2(0, 1);
                vertecies[cellIndex + 1].TextureCoordinate = new Vector2(0, 0);
                vertecies[cellIndex + 2].TextureCoordinate = new Vector2(1, 1);

                // - Triangle 2
                vertecies[cellIndex + 3].TextureCoordinate = vertecies[cellIndex + 1].TextureCoordinate;
                vertecies[cellIndex + 4].TextureCoordinate = new Vector2(1, 0);
                vertecies[cellIndex + 5].TextureCoordinate = vertecies[cellIndex + 2].TextureCoordinate;
                        break;
                }


                if (iMapX >= 0) {
                    iMapX--;
                } else {
                    iMapX = (int)mapSize.X - 1;
                    iMapY--;
                }

                if (x < ((int)mapSize.X)) {
                    x++;
                } else {
                    x = 0;
                    y++;
                }
            }

            VertexBuffer.SetData(vertecies);
        }

        public VertexBuffer VertexBuffer { get; set; }
        public int iQuadTotal { get; set; }
    }
}