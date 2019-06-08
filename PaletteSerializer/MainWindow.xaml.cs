using System;
using System.Collections.Generic;
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
using System.IO;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace PaletteSerializer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class Palette
        {
            public string name { get; set; }
            public IList<IList<int>> colors { get; set; }
        }

        public class PaletteGroup
        {
            public string name { get; set; }
            [JsonProperty("texture-names")]
            public IList<string> textureNames { get; set; }
            public IList<Palette> palettes { get; set; }
        }

        public class PaletteCollection
        {
            public string _comment { get; set; }
            [JsonProperty("palette-groups")]
            public IList<PaletteGroup> paletteGroups { get; set; }
        }

        PaletteCollection paletteFile = new PaletteCollection();
        string currentFilePath = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "JSON files (.json)|*.json";

            if (dlg.ShowDialog() == true)
            {
                currentFilePath = dlg.FileName;

                paletteFile = JsonConvert.DeserializeObject<PaletteCollection>(dlg.FileName);

                foreach (PaletteGroup entity in paletteFile.paletteGroups)
                {
                    EntityList.Items.Add(entity.name);
                }
            }
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
