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

            char commentStartChar = '#';
            bool isComment = false;
            string text = System.IO.File.ReadAllText(filename);
            char fileType = 'P';
            StringBuilder valueAsStringBuilder = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                if (currentStep == 1)
                {
                    if (isComment)
                    {
                        if (text[i] == '\n'/* || text[i] == '\r\n'*/)
                        {
                            isComment = false;
                        }
                    }
                    else if (text[i] == commentStartChar)
                    {
                        isComment = true;
                    }
                    else if (text[i] == fileType)
                    {
                        if (i + 1 < text.Length)
                        {
                            if (text[i + 1] == '3')
                            {
                                ppmFileType = PpmFileType.P3;
                                currentStep = 2;
                                i += 2;
                            }
                            else if (text[i + 1] == '6')
                            {
                                ppmFileType = PpmFileType.P6;
                                currentStep = 2;
                                i += 2;
                            }
                        }
                    }
                    else if (!Char.IsWhiteSpace(text[i]))
                    {
                        throw new FileFormatException(
                            String.Format("Found illegal character!\n Character index: {0}\n Character: {1}", i, text[i]));
                    }
                }
                if (currentStep == 2)
                {
                    if (isComment)
                    {
                        if (text[i] == '\n'/* || text[i] == '\r\n'*/)
                        {
                            isComment = false;
                        }
                    }
                    else if (text[i] == commentStartChar)
                    {
                        isComment = true;
                    }
                    else if (Char.IsDigit(text[i]))
                    {
                        valueAsStringBuilder.Append(text[i]);
                    }
                    else if (Char.IsWhiteSpace(text[i]))
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
                                i += 1;
                            }
                        }
                    }
                    else
                    {
                        throw new FileFormatException(
                            String.Format("Found illegal character!\n Character index: {0}\n Character: {1}", i, text[i]));
                    }
                }
                if (currentStep == 3)
                {
                    //for (; j < line.Length; j++)
                    //{
                    //    if (line[j] == commentStartChar)
                    //    {
                    //        //The rest of the line is a comment
                    //        break;
                    //    }
                    //    else
                    //    {

                    //    }
                    //}
                }
            }
            return new PpmFileInfo(ppmFileType, size, maximumColorValue);
        }
    }
}
