using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop;
using MonoGame.Framework.WpfInterop.Input;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;

namespace PPM
{
    public class MyGame : WpfGame
    {
        public static MyGame Instance 
        {
            get 
            {
                return instance;
            }
        }
        private static MyGame instance;
        private SpriteBatch _spriteBatch;
        private IGraphicsDeviceService _graphicsDeviceManager;
        private WpfKeyboard _keyboard;
        private WpfMouse _mouse;
        private Texture2D texture;

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
            _keyboard = new WpfKeyboard(this);
            _mouse = new WpfMouse(this);

            // must be called after the WpfGraphicsDeviceService instance was created
            base.Initialize();
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // content loading now possible
        }

        protected override void Update(GameTime time)
        {
            // every update we can now query the keyboard & mouse for our WpfGame
            var mouseState = _mouse.GetState();
            var keyboardState = _keyboard.GetState();
        }

        protected override void Draw(GameTime time)
        {
            base.Draw(time);
            GraphicsDevice.Clear(Color.AliceBlue);
            _spriteBatch.Begin();
            if (texture != null)
                _spriteBatch.Draw(texture, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            _spriteBatch.End();
        }

        public void SetTexture(Texture2D texture)
        {
            this.texture = texture;
        }
    }

}
