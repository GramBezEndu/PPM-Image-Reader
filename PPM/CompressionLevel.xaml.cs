using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PPM
{
    /// <summary>
    /// Interaction logic for CompressionLevel.xaml
    /// </summary>
    public partial class CompressionLevel : Window
    {
        public string FileName;
        public int Compression;
        public CompressionLevel()
        {
            InitializeComponent();
        }
        
        private void Save(object sender, EventArgs eventArgs)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JPG File(*.jpg)| *.jpg";
            Nullable<bool> result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                Compression = (int)this.CompressionSlider.Value;
                FileName = saveFileDialog.FileName;
                DialogResult = true;
            }
        }
    }
}
