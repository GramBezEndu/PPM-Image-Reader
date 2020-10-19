using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace PPM
{
    public class Camera : INotifyPropertyChanged
    {
        private float zoom = 1f;

        public Matrix ViewMatrix { get; private set; } = Matrix.CreateTranslation(0, 0, 0);
        public float Zoom 
        { 
            get => zoom;
            set
            {
                if (zoom != value)
                {
                    zoom = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public Vector2 Origin { get; set; } = Vector2.Zero;
        public Vector2 Position { get; set; } = Vector2.Zero;

        public event PropertyChangedEventHandler PropertyChanged;

        public void Update(GameTime gameTime)
        {
            CalculateViewMatrix();
        }

        private void CalculateViewMatrix()
        {
            ViewMatrix = Matrix.CreateTranslation(-Position.X, -Position.Y, 0) * Matrix.CreateTranslation(Origin.X * (1 / Zoom), Origin.Y * (1 / Zoom), 0);
            ViewMatrix *= Matrix.CreateScale(new Vector3(Zoom, Zoom, 1));
        }
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
