using Microsoft.Win32;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PPM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = MyGame.Instance;
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PPM Files (*.ppm)|*.ppm|JPEG Files (*.jpeg;*.jpg)|*.jpeg;*.jpg";
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                string filename = openFileDialog.FileName;
                string extension = System.IO.Path.GetExtension(filename);
                if (extension == ".ppm")
                {
                    var info = PpmFileReader.ReadFileInfo(filename);
                    fileContent.Text = info.ToString();
                    MyGame.Instance.SetTexture(info.CreateTexture());
                }
                //JPG file read
                else
                {
                    using (var stream = new System.IO.FileStream(filename, FileMode.Open))
                    {
                        MyGame.Instance.SetTexture(Texture2D.FromStream(MyGame.Instance.GraphicsDevice, stream));
                    }
                }
            }
        }

        private void btnSaveFile_Click(object sender, RoutedEventArgs e)
        {
            var texture = MyGame.Instance.GetTexture();
            if (texture != null)
            {
                var window = new CompressionLevel();
                Nullable<bool> result = window.ShowDialog();
                if (result == true)
                {
                    MemoryStream memoryStream = new MemoryStream();
                    texture.SaveAsPng(memoryStream, texture.Width, texture.Height);
                    System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(memoryStream);
                    SaveAsJPEG(bitmap, window.FileName, window.Compression);
                }
            }
        }

        private void SaveAsJPEG(Bitmap bitmap, string filename, int compressionLevel)
        {
            ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);

            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, compressionLevel);
            myEncoderParameters.Param[0] = myEncoderParameter;
            bitmap.Save(filename, jgpEncoder, myEncoderParameters);
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}
