using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace PPM
{
    public static class PpmFileReader
    {
        public enum PpmFileType
        {
            Invalid,
            P3,
            P6,
        };
        public static PpmFileInfo ReadFile(string filename)
        {
            int currentStep = 1;
            Size size = new Size(-1, -1);
            ushort maximumColorValue = 0;
            PpmFileType ppmFileType = PpmFileType.Invalid;

            char comment = '#';
            string[] lines = System.IO.File.ReadAllLines(filename);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = (string)lines[i];
                int j = 0;
                if (currentStep == 1)
                {
                    char fileType = 'P';
                    for (; j < line.Length; j++)
                    {
                        if (line[j] == comment)
                        {
                            //The rest of the line is a comment
                            break;
                        }
                        else if (line[j] == fileType)
                        {
                            if (j + 1 < line.Length)
                            {
                                if (line[j + 1] == '3')
                                {
                                    ppmFileType = PpmFileType.P3;
                                    currentStep = 2;
                                    j += 2;
                                    break;
                                }
                                else if (line[j + 1] == '6')
                                {
                                    ppmFileType = PpmFileType.P6;
                                    currentStep = 2;
                                    j += 2;
                                    break;
                                }
                            }
                        }
                        else if (!Char.IsWhiteSpace(line[j]))
                        {
                            throw new FileFormatException(
                                String.Format("Found illegal character!\n Line index: {0}\n Character index: {1}\n Character: {2}", i, j, line[j]));
                        }
                    }
                }
                if (currentStep == 2)
                {
                    StringBuilder valueAsStringBuilder = new StringBuilder();
                    for (; j < line.Length; j++)
                    {
                        if (line[j] == comment)
                        {
                            //The rest of the line is a comment
                            break;
                        }
                        else if(Char.IsDigit(line[j]))
                        {
                            valueAsStringBuilder.Append(line[j]);
                            //
                            //int multiplier = 1;
                            //while(j + 1 < line.Length && Char.IsDigit(line[j + 1]))
                            //{
                            //    int.Parse()
                            //}
                        }
                        else if (Char.IsWhiteSpace(line[j]))
                        {
                            if (valueAsStringBuilder.ToString() != String.Empty)
                            {
                                int val = int.Parse(valueAsStringBuilder.ToString());
                                if (size.Width == -1)
                                {
                                    size.Width = val;
                                    valueAsStringBuilder.Clear();
                                }
                                else if (size.Height == -1)
                                {
                                    size.Height = val;
                                    currentStep = 3;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            throw new FileFormatException(
                                String.Format("Found illegal character!\n Line index: {0}\n Character index: {1}\n Character: {2}", i, j, line[j]));
                        }
                    }
                }
                if (currentStep == 3)
                {
                    for (; j < line.Length; j++)
                    {
                        if (line[j] == comment)
                        {
                            //The rest of the line is a comment
                            break;
                        }
                        else
                        {

                        }
                    }
                }
            }
            return new PpmFileInfo(ppmFileType, size, maximumColorValue);
        }
    }
}
