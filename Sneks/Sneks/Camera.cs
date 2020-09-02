using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Sneks {
    public class Camera : GameComponent {
        public Camera(Game game) : base(game) {

        }

        #region MonoGame Pipeline
        public override void Initialize() {
            Position = new Vector3(0, 0, 3000.0f);
            Zoom = 1.0f;

            previousKeyboardState = Keyboard.GetState();
            currentKeyboardState = Keyboard.GetState();
            previousMouseState = Mouse.GetState();
            currentMouseState = Mouse.GetState();

            base.Initialize();
        }

        public override void Update(GameTime gameTime) {
            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();

            // Horizontal Panning
            if (currentKeyboardState.IsKeyDown(Keys.D)) {
                Position = new Vector3(Position.X - 25, Position.Y, Position.Z);
            }

            if (currentKeyboardState.IsKeyDown(Keys.A)) {
                Position = new Vector3(Position.X + 25, Position.Y, Position.Z);
            }

            // Vertical Panning
            if (currentKeyboardState.IsKeyDown(Keys.W)) {
                Position = new Vector3(Position.X, Position.Y + 25, Position.Z);
            }

            if (currentKeyboardState.IsKeyDown(Keys.S)) {
                Position = new Vector3(Position.X, Position.Y - 25, Position.Z);
            }

            // Zooming
            if (currentKeyboardState.IsKeyDown(Keys.Q)) {
                Position = new Vector3(Position.X, Position.Y, Position.Z + 50);
            }

            if (currentKeyboardState.IsKeyDown(Keys.E)) {
                Position = new Vector3(Position.X, Position.Y, Position.Z - 50);
            }

            previousKeyboardState = currentKeyboardState;
            previousMouseState = currentMouseState;

            base.Update(gameTime);
        }
        #endregion
        #region Camera Properties
        public Vector3 Position { get; set; }
        public float Zoom { get; set; }
        #endregion

        #region Input States
        // Current and previous keyboard and mouse states
        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;
        private MouseState currentMouseState;
        private MouseState previousMouseState;
        #endregion

        #region Public Properties
        public Matrix View {
            get {
                return Matrix.CreateLookAt(new Vector3(-Position.X, Position.Y, Position.Z), new Vector3(-Position.X, Position.Y, 0), Vector3.Up) * Matrix.CreateScale(Zoom);
            }
        }
        #endregion

    }
}