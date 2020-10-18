using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using static PPM.PpmFileReader;

namespace PPM
{
    public class PpmFileInfo
    {
        public PpmFileType FileType { get; private set; }
        public Size Size { get; private set; }
        public ushort MaximumColorValue { get; private set; }
        public PpmFileInfo(PpmFileType type, Size size, ushort maximumColorVal)
        {
            FileType = type;
            Size = size;
            MaximumColorValue = maximumColorVal;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Type: " + FileType);
            builder.AppendLine("Size: " + Size);
            builder.AppendLine("Maximum color value: " + MaximumColorValue);
            return builder.ToString();
        }
    }
}
