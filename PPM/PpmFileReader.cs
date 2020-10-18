using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Media;

namespace PPM
{
    public static class PpmFileReader
    {
        public enum Step 
        {
            PpmFileType,
            Dimensions,
            MaximumColorValue,
        };
        public enum PpmFileType
        {
            Invalid,
            P3,
            P6,
        };
        public static PpmFileInfo ReadFile(string filename)
        {
            Step currentStep = Step.PpmFileType;
            Size size = new Size(-1, -1);
            ushort maximumColorValue = 0;
            PpmFileType ppmFileType = PpmFileType.Invalid;

            char comment = '#';
            string[] lines = System.IO.File.ReadAllLines(filename);
            foreach(var line in lines)
            {
                switch(currentStep)
                {
                    case Step.PpmFileType:
                        char fileType = 'P';
                        for (int i = 0; i < line.Length; i++)
                        {
                            if (line[i] == comment)
                            {
                                //The rest of the line is a comment
                                break;
                            }
                            else
                            {
                                if (line[i] == fileType)
                                {
                                    if(i + 1 < line.Length)
                                    {
                                        if (line[i + 1] == '3')
                                        {
                                            ppmFileType = PpmFileType.P3;
                                            currentStep = Step.Dimensions;
                                            break;
                                        }
                                        else if (line[i + 1] == '6')
                                        {
                                            ppmFileType = PpmFileType.P6;
                                            currentStep = Step.Dimensions;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case Step.Dimensions:
                        for (int i = 0; i < line.Length; i++)
                        {
                            if (line[i] == comment)
                            {
                                //The rest of the line is a comment
                                break;
                            }
                            else
                            {
                                if (Char.IsDigit(line[i]))
                                {
                                    if (size.Width == -1)
                                    {

                                    }
                                    else
                                    {

                                    }
                                }
                            }
                        }
                        break;
                    case Step.MaximumColorValue:
                        break;
                }
            }
            return new PpmFileInfo(ppmFileType, size, maximumColorValue);
        }
    }
}
