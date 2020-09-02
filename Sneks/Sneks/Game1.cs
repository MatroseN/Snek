using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Sneks {
    public class Game1 : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private BasicEffect _basicEffect;

        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.IsFullScreen = false;
        }
        
        protected override void Initialize() {
            // TODO: Add your initialization logic here

            if (isLegacy) {
                windowSize = new Vector2(800, 600);

                _graphics.PreferredBackBufferWidth = (int)windowSize.X;
                _graphics.PreferredBackBufferHeight = (int)windowSize.Y;
                _graphics.ApplyChanges();
            } else {
                windowSize = new Vector2(1920, 1080);

                _graphics.PreferredBackBufferWidth = (int)windowSize.X;
                _graphics.PreferredBackBufferHeight = (int)windowSize.Y;
                _graphics.ApplyChanges();
            }


            #region Legacy
            block = new Block(new Vector2(3, 3));
            block.createTexture(_graphics.GraphicsDevice, pixel=> Color.Green);

            mapSize = new Vector2(1000, 500);
            mapGenerator = new MapGenerator(mapSize);
            map = mapGenerator.generateTerrain();

            frameCounter = new FrameCounter();

            legacyCamera = new LegacyCamera(windowSize, mapSize);


            entities = new Dictionary<Guid, Entity>();

            snek = new Snek(new Vector2(32, 64), new Vector2(400, 100), entities);
            #endregion

            #region VertexTest
            #region Components
            camera = new Camera(this);
            Components.Add(camera);
            #endregion

            #region Quad
            quadGenerator = new QuadGenerator();
            #region Quad Data
            quadIWidth = (int)mapSize.X;
            quadIHeight = (int)mapSize.Y;
            quadITotal = 0;
            #endregion
            #endregion
            #endregion

            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            #region Legacy
            fpsFont = Content.Load<SpriteFont>("fpsFont");
            snek.texture = Content.Load<Texture2D>("Snek");
            #endregion

            #region Vertex Test
            // Quad Texture
            quadTexture = block.texture;

            // Effects
            _basicEffect = new BasicEffect(GraphicsDevice);
            _basicEffect.TextureEnabled = true;
            _basicEffect.Texture = quadTexture;
            _basicEffect.VertexColorEnabled = true;

            // Camera
            Vector3 cameraTarget = new Vector3(0.0f, 0.0f, 0.0f);

            // Matrix  Preperation
            float fovAngle = MathHelper.ToRadians(45); // 45 degrees to radians
            float aspectRatio = GraphicsDevice.Viewport.Width / GraphicsDevice.Viewport.Height;
            float near = 0.01f; // Near clipping plane
            float far = 1500000; // Far clipping plane

            // Matrix
            Matrix world = Matrix.CreateTranslation(0.0f, 0.0f, 0.0f);
            Matrix view = Matrix.CreateLookAt(camera.Position, cameraTarget, Vector3.Up);
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(fovAngle, aspectRatio, near, far);
            _basicEffect.World = world;
            _basicEffect.View = view;
            _basicEffect.Projection = projection;

            // Generate Quads
            quadGenerator.createQuads(GraphicsDevice, quadTotalVertecies, mapSize, map);
            #endregion
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            legacyCamera.update(gameTime);

            foreach (Entity entity in entities.Values) {
                entity.update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _basicEffect.View = camera.View;

            #region Legacy
            if (isLegacy) {

                _spriteBatch.Begin();

                // TODO: Add your drawing code here

                if (legacyCamera.zoom) {
                    // Zoomed in           
                    for (int y = 0; y < (int)windowSize.Y; y++) {
                        for (int x = 0; x < (int)windowSize.X; x++) {
                            int blockPos = (y + (int)legacyCamera.Y) * (int)mapSize.X + (x + (int)legacyCamera.X);
                            if (!((x + windowSize.X > legacyCamera.X + windowSize.X) &&
                                  (x + windowSize.X < legacyCamera.X + windowSize.X) && 
                                  (y + windowSize.Y > legacyCamera.Y + windowSize.Y) && 
                                  (y + windowSize.Y < legacyCamera.Y + windowSize.Y))) {
                                switch (map[blockPos]) {
                                    case 1:
                                        _spriteBatch.Draw(block.texture, new Vector2(x, y), Color.White);
                                        break;
                                }
                            }
                        }
                    }
                    
                } else {
                    // Not zoomed in
                    for (int y = 0; y < (int)windowSize.Y; y++) {
                        for (int x = 0; x < (int)windowSize.X; x++) {
                            float xPos = (float)x / windowSize.X * mapSize.X;
                            float yPos = (float)y / windowSize.Y * mapSize.Y;
                            int blockPos = ((int)yPos) * (int)mapSize.X + ((int)xPos);
                            switch (map[blockPos]) {
                                case 1:
                                    _spriteBatch.Draw(block.texture, new Vector2(x, y), Color.White);
                                    break;
                            }
                        }
                    }
                }

                foreach (Entity entity in entities.Values) {
                    _spriteBatch.Draw(entity.texture, entity.position, Color.White);
                }
                _spriteBatch.End();
                #endregion
            } else {
                GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
                GraphicsDevice.SetVertexBuffer(quadGenerator.VertexBuffer);
                foreach (EffectPass effectPass in _basicEffect.CurrentTechnique.Passes) {
                    effectPass.Apply();

                    GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, quadGenerator.iQuadTotal * 2);
                }

            }

            
            #region FPS Counter
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            frameCounter.Update(deltaTime);

            int fpsCount = (int)Math.Round(frameCounter.AverageFramesPerSecond, 0);

            var fps = string.Format("FPS: {0}", fpsCount);

            _spriteBatch.Begin();

            _spriteBatch.DrawString(fpsFont, fps, new Vector2(1, 1), Color.Black);

            _spriteBatch.End();

            #endregion

            base.Draw(gameTime);
        }

        private bool isLegacy = false;

        private LegacyCamera legacyCamera;
        private Snek snek;
        private Dictionary<Guid, Entity> entities;
        private MapGenerator mapGenerator;
        private int[] map;
        private Vector2 mapSize;
        private Block block;
        private Vector2 windowSize;
        private FrameCounter frameCounter;
        private SpriteFont fpsFont;

        #region Vertex Test
        #region Constants
        private const int quadPixelSize = 3;
        private const int quadTotalVertecies = 6;
        private const int quadVertexSizeInBytes = (sizeof(float) * 3) + 4 + (sizeof(float) * 2);
        #endregion

        #region Quad
        private QuadGenerator quadGenerator;
        #region Quad Data
        private int quadIWidth;
        private int quadIHeight;
        private int quadITotal;
        #endregion

        #region Quad Texture
        Texture2D quadTexture;
        #endregion
        #endregion

        #region Camera
        private Camera camera;
        #endregion
        #endregion
    }
}