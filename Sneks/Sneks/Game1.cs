using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;

namespace Sneks {
    public class Game1 : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private BasicEffect _basicEffect;
        private Desktop _desktop;

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
            // Induvidual block that gets rendered to the screen
            block = new Block(new Vector2(3, 3));
            block.createTexture(_graphics.GraphicsDevice, pixel=> Color.Green);

            // Map default build settings
            mapSize = new Vector2(2000, 500);
            mapGenerator = new MapGenerator(mapSize, defaultMapBias);
            map = mapGenerator.generateTerrain();

            frameCounter = new FrameCounter();

            // Can be deleted later
            legacyCamera = new LegacyCamera(windowSize, mapSize);

            // Includes all game entities
            entities = new Dictionary<Guid, Entity>();

            // A player that gets placed at the specified location
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

            // Look through and refactor
            #region Legacy
            fpsFont = Content.Load<SpriteFont>("fpsFont");
            snek.texture = Content.Load<Texture2D>("Snek");
            #endregion

            #region Myra GUI

            // Map builder GUI that you can use to change the map builder parameters on the fly. Including regenerate the entire map
            MyraEnvironment.Game = this;

            // Main panel that all other map builder ui elements will get added to
            var panel = new Panel();

            // Creates the panel that holds all other ui elements
            var childPanel = new Panel {
                Margin = new Thickness(24, 50, 1600, 800),
                BorderThickness = new Thickness(5, 0),
                Padding = new Thickness(5, 5, 0, 0),
                Background = new SolidBrush("#2F363F"),
            };

            // Create grid
            var grid = new Grid {
                ShowGridLines = false,
                ColumnSpacing = 8,
                RowSpacing = 8,
            };

            // Set partitioning configuration
            grid.ColumnsProportions.Add(new Proportion());
            grid.ColumnsProportions.Add(new Proportion());
            grid.RowsProportions.Add(new Proportion());
            grid.RowsProportions.Add(new Proportion());

            // Adds a label for the height
            var mapHeightLabel = new Label();
            mapHeightLabel.Text = "Map height:";
            mapHeightLabel.GridRow = 0;
            mapHeightLabel.GridColumn = 0;
            mapHeightLabel.Margin = new Thickness(40, 15, 0, 0);
            grid.Widgets.Add(mapHeightLabel);

            // Lets you change the valuue for the map height
            var mapHeightSpinButton = new SpinButton();
            mapHeightSpinButton.GridRow = 0;
            mapHeightSpinButton.GridColumn = 1;
            mapHeightSpinButton.Value = mapSize.Y;
            mapHeightSpinButton.Margin = new Thickness(10, 15, 0 ,0);
            mapHeightSpinButton.ValueChanged += (s, a) => {
                mapSize.Y = mapHeightSpinButton.Value.Value;
            };
            grid.Widgets.Add(mapHeightSpinButton);

            // Adds a label for the width
            var mapWidthLabel = new Label();
            mapWidthLabel.Text = "Map width:";
            mapWidthLabel.GridRow = 1;
            mapWidthLabel.Margin = new Thickness(40, 15, 0, 0);
            grid.Widgets.Add(mapWidthLabel);

            // Lets you change the valuue for the map width
            var mapWidthSpinButton = new SpinButton();
            mapWidthSpinButton.GridRow = 1;
            mapWidthSpinButton.GridColumn = 1;
            mapWidthSpinButton.Value = mapSize.X;
            mapWidthSpinButton.Margin = new Thickness(10, 15, 0, 0);
            mapWidthSpinButton.ValueChanged += (s, a) => {
                 mapSize.X = mapWidthSpinButton.Value.Value;
             };
            grid.Widgets.Add(mapWidthSpinButton);

            // Label for the bias section on the ui
            var biasLabel = new Label();
            biasLabel.Text = "Bias:";
            biasLabel.GridRow = 2;
            biasLabel.Margin = new Thickness(40, 15, 0, 0);
            grid.Widgets.Add(biasLabel);

            // Slider that lets you modify the bias value for map generation. The lower the value the more "spiky" the map will be. If the value is higher the map will be flatter. Goes from 0.5f to 2.5f
            var slider = new HorizontalSlider();
            slider.GridColumn = 1;
            slider.GridRow = 2;
            slider.Width = 100;
            slider.Margin = new Thickness(0, 15, 0, 0);
            slider.Value = mapGenerator.bias;
            slider.Maximum = 2.5f;
            slider.Minimum = 0.5f;
            slider.ValueChanged += (s, a) => {
                mapGenerator.bias = slider.Value;
            };
            grid.Widgets.Add(slider);

            // TODO: Rename this button to be more reflective of the property it is modifying.
            var veryLongButton = new TextButton();
            veryLongButton.Text = "Generate";
            veryLongButton.GridColumn = 0;
            veryLongButton.GridRow = 3;
            veryLongButton.Margin = new Thickness(30, 10, 25, 25);
            veryLongButton.Width = 90;
            veryLongButton.Height = 50;
            veryLongButton.OverBackground = new SolidBrush("#26ae60");
            veryLongButton.PressedBackground = new SolidBrush("#EA7773");
            veryLongButton.Background = new SolidBrush("#019031");
            veryLongButton.TouchDown += (s, a) => {
                generate = true;
            };
            grid.Widgets.Add(veryLongButton);

            childPanel.Widgets.Add(grid);
            panel.Widgets.Add(childPanel);

            // Add it to the desktop
            _desktop = new Desktop();
            _desktop.Root = panel;

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

            // legacyCamera.update(gameTime);

            if (generate) {
                mapGenerator.size = mapSize;
                map = mapGenerator.generateTerrain();
                quadIWidth = (int)mapSize.X;
                quadIHeight = (int)mapSize.Y;
                quadITotal = 0;
                quadGenerator.createQuads(GraphicsDevice, quadTotalVertecies, mapSize, map);
                generate = false;
            }

            foreach (Entity entity in entities.Values) {
                entity.update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _basicEffect.View = camera.View;

            // TODO: Refactor like crazy
            // Go through and delete what isnt needed anymore
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

            _desktop.Render();
            
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
        private bool generate = false;
        private const float defaultMapBias = 1.25f;
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
        // check usability
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