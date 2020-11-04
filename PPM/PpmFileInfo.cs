using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using static PPM.PpmFileReader;
using Color = Microsoft.Xna.Framework.Color;

namespace PPM
{
    public class PpmFileInfo
    {
        public PpmFileType FileType { get; private set; }
        public Size Size { get; private set; }
        public ushort MaximumColorValue { get; private set; }
        string data;
        int dataReadIndexEnd;
        readonly string filename;
        //byte[] colorData;
        //WriteableBitmap writeableBitmap;

        public PpmFileInfo(PpmFileType type, Size size, ushort maximumColorVal, string data, int dataReadIndexEnd, string filename)
        {
            FileType = type;
            Size = size;
            MaximumColorValue = maximumColorVal;
            this.data = data;
            this.dataReadIndexEnd = dataReadIndexEnd;
            this.filename = filename;

            //writeableBitmap = new WriteableBitmap(Size.Width, Size.Height, 96, 96, PixelFormats.Bgra32, null);
            //var rect = new System.Windows.Int32Rect(0, 0, Size.Width, Size.Height);
            //var stride = (rect.Width * writeableBitmap.Format.BitsPerPixel + 7) / 8;
            //var bufferSize = rect.Height* stride;
            //colorData = new byte[bufferSize];
            //Random r = new Random();
            //r.NextBytes(colorData);
            //writeableBitmap.WritePixels(rect, colorData, stride, 0);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Type: " + FileType);
            builder.AppendLine("Size: " + Size);
            builder.AppendLine("Maximum color value: " + MaximumColorValue);
            return builder.ToString();
        }

        public Texture2D CreateTexture()
        {
            var texture = new Texture2D(MyGame.Instance.GraphicsDevice, Size.Width, Size.Height);
            Microsoft.Xna.Framework.Color[] colorData = new Microsoft.Xna.Framework.Color[Size.Width * Size.Height];
            switch (FileType)
            {
                case PpmFileType.P3:
                    return Ppm3(texture, colorData);
                case PpmFileType.P6:
                    return Ppm6(texture, colorData);
                default:
                case PpmFileType.Invalid:
                    throw new ArgumentException("Invalid PPM file type");
            }
        }

        public Texture2D Ppm3(Texture2D texture, Microsoft.Xna.Framework.Color[] colorData)
        {
            char commentStartChar = '#';
            bool isComment = false;
            int currentColorIndex = 0;
            double multiplier = this.MaximumColorValue / 255;

            StringBuilder valueAsStringBuilder = new StringBuilder();
            int r = -1, g = -1, b = -1;
            for (int i = dataReadIndexEnd; i < data.Length; i++)
            {
                if (isComment)
                {
                    if (data[i] == '\n'/* || text[i] == '\r\n'*/)
                    {
                        isComment = false;
                    }
                }
                else if (data[i] == commentStartChar)
                {
                    isComment = true;
                }
                else if (Char.IsDigit(data[i]))
                {
                    valueAsStringBuilder.Append(data[i]);
                }
                else if (Char.IsWhiteSpace(data[i]))
                {
                    if (valueAsStringBuilder.ToString() != String.Empty)
                    {
                        int val = int.Parse(valueAsStringBuilder.ToString());
                        val = (int)(val / multiplier);
                        if (r == -1)
                        {
                            r = val;
                            valueAsStringBuilder.Clear();
                        }
                        else if (g == -1)
                        {
                            g = val;
                            //currentStep = 3;
                            //i += 1;
                            valueAsStringBuilder.Clear();
                        }
                        else if (b == -1)
                        {
                            b = val;
                            colorData[currentColorIndex] = new Color(r, g, b);
                            r = -1;
                            g = -1;
                            b = -1;
                            currentColorIndex++;
                            valueAsStringBuilder.Clear();
                        }
                    }
                }
                else
                {
                    throw new FileFormatException(
                        String.Format("Found illegal character!\n Character index: {0}\n Character: {1}", i, data[i]));
                }
            }
            texture.SetData(colorData);
            return texture;
        }

        public Texture2D Ppm6(Texture2D texture, Microsoft.Xna.Framework.Color[] colorData)
        {
            int currentColorIndex = 0;

            StringBuilder valueAsStringBuilder = new StringBuilder();
            int r = -1, g = -1, b = -1;

            //var reader = new BinaryReader(new FileStream(filename, FileMode.Open));
            //string rData = reader.ReadString();
            //byte[] bData = reader.ReadAllBytes();
            byte[] bData = File.ReadAllBytes(filename);
            string rData = Encoding.ASCII.GetString(bData);
            int[] values = null;
            if (MaximumColorValue < 256)
                values = bData.Select(x => (int)x).ToArray();
            //foreach (var c in rData)
            //{
            //    System.Diagnostics.Debug.WriteLine(c);
            //}
            foreach(var val in values)
            {
                System.Diagnostics.Debug.WriteLine(val);
            }
            //byte[] binaryData = Encoding.UTF8.GetBytes(data.Substring(dataReadIndexEnd));
            //string rData = Encoding.ASCII.GetString(binaryData);

            //for (int i = dataReadIndexEnd; i < data.Length; i++)
            //for (int i = 0; i < binaryData.Length; i++)
            //string rData = "1";
            for (int i = dataReadIndexEnd /* ?? */; i < values.Length; i++)
            {
                //byte currentByte = binaryData[i];
                //char currentVal = Encoding.UTF8.ConConvert.ToChar(currentByte);
                char currentVal = rData[i];
                if (Char.IsDigit(currentVal))
                {
                    valueAsStringBuilder.Append(currentVal);
                }
                else if (Char.IsWhiteSpace(currentVal))
                {
                    if (valueAsStringBuilder.ToString() != String.Empty)
                    {
                        int val = int.Parse(valueAsStringBuilder.ToString());
                        if (r == -1)
                        {
                            r = val;
                            valueAsStringBuilder.Clear();
                        }
                        else if (g == -1)
                        {
                            g = val;
                            valueAsStringBuilder.Clear();
                        }
                        else if (b == -1)
                        {
                            b = val;
                            colorData[currentColorIndex] = new Color(r, g, b);
                            r = -1;
                            g = -1;
                            b = -1;
                            currentColorIndex++;
                            valueAsStringBuilder.Clear();
                        }
                    }
                }
                else
                {
                    //throw new FileFormatException(
                    //    String.Format("Found illegal character!\n Character: {0}", currentVal));
                }
            }
            texture.SetData(colorData);
            return texture;
        }
    }
}
