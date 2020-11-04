using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Framework.WpfInterop;
using MonoGame.Framework.WpfInterop.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;

namespace PPM
{
    public class MyGame : WpfGame, INotifyPropertyChanged
    {
        public static MyGame Instance
        {
            get
            {
                return instance;
            }
        }
        //public Camera Camera { get; private set; } = new Camera();
        private static MyGame instance;
        private SpriteBatch spriteBatch;
        private IGraphicsDeviceService _graphicsDeviceManager;
        private WpfMouse mouse;
        private Texture2D texture;
        private Vector2 texturePos = Vector2.Zero;

        private MouseState previousMouseState;
        private MouseState mouseState;
        private float zoom = 1;

        public event PropertyChangedEventHandler PropertyChanged;

        public int PreviousScrollWheelValue { get; private set; }
        public int ScrollWheelValue { get; private set; }

        public float Zoom 
        { 
            get => zoom;
            set
            {
                if (zoom != value)
                {
                    zoom = (float)(Math.Round(value, 2));
                    NotifyPropertyChanged();
                }
            }
        }
        public MyGame()
        {
            instance = this;
        }

        protected override void Initialize()
        {
            // must be initialized. required by Content loading and rendering (will add itself to the Services)
            // note that MonoGame requires this to be initialized in the constructor, while WpfInterop requires it to
            // be called inside Initialize (before base.Initialize())
            _graphicsDeviceManager = new WpfGraphicsDeviceService(this);

            // wpf and keyboard need reference to the host control in order to receive input
            // this means every WpfGame control will have it's own keyboard & mouse manager which will only react if the mouse is in the control
            mouse = new WpfMouse(this);

            // must be called after the WpfGraphicsDeviceService instance was created
            base.Initialize();
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // content loading now possible
        }

        protected override void Update(GameTime time)
        {
            // every update we can now query the keyboard & mouse for our WpfGame
            previousMouseState = mouseState;
            mouseState = mouse.GetState();
            PreviousScrollWheelValue = ScrollWheelValue;
            ScrollWheelValue = mouseState.ScrollWheelValue;
            if (texture != null)
            {
                if (MouseScrolledDown())
                {
                    Zoom += 0.2f;
                    //Camera.Zoom = (float)Math.Round(Camera.Zoom + 1f, 1);
                }
                else if (MouseScrolledUp())
                {
                    Zoom -= 0.2f;
                    //Camera.Zoom = (float)Math.Round(Camera.Zoom - 1f, 1);
                }
            }
            //Camera.Update(time);
        }

        protected override void Draw(GameTime time)
        {
            this.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            base.Draw(time);
            GraphicsDevice.Clear(Color.GhostWhite);
            spriteBatch.Begin(sortMode: SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp/*transformMatrix: Camera.ViewMatrix*/);
            if (texture != null)
                spriteBatch.Draw(texture, texturePos, null, Color.White, 0f, Vector2.Zero, Zoom, SpriteEffects.None, 0f);
            spriteBatch.End();
        }

        public void SetTexture(Texture2D texture)
        {
            this.texture = texture;
            texturePos = Vector2.Zero;
            //texturePos = new Vector2((float)(this.ActualWidth / 2 - texture.Width / 2), (float)(this.ActualHeight / 2 - texture.Height / 2));
            //Camera.Position = texturePos;
            //camera.Origin = new Vector2((float)(ActualWidth/*texture.Width*/ / 2), (float)(ActualHeight/*texture.Height*/ / 2));
            //camera.Zoom = 0.5f;
        }

        public Texture2D GetTexture()
        {
            return texture;
        }
        public bool MouseScrolledUp()
        {
            return ScrollWheelValue < PreviousScrollWheelValue;
        }

        public bool MouseScrolledDown()
        {
            return ScrollWheelValue > PreviousScrollWheelValue;
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
