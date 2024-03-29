﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.ComponentModel;
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

        public class PaletteCollection : INotifyPropertyChanged
        {
            public string _comment { get; set; }
            [JsonProperty("palette-groups")]
            public IList<PaletteGroup> _paletteGroups;

            public IList<PaletteGroup> PaletteGroups
            {
                get
                {
                    return _paletteGroups;
                }
                set
                {
                    _paletteGroups = value;
                    NotifyPropertyChanged("PaletteGroups");
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            public void NotifyPropertyChanged(string name)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
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

                paletteFile = JsonConvert.DeserializeObject<PaletteCollection>(File.ReadAllText(dlg.FileName));

                foreach (PaletteGroup entity in paletteFile.PaletteGroups)
                {
                    EntityList.Items.Add(entity.name);
                }
            }
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem item = ItemsControl.ContainerFromElement(sender as ListBox, e.OriginalSource as DependencyObject) as ListBoxItem;

            if (item != null)
            {
                foreach (PaletteGroup entity in paletteFile.PaletteGroups)
                {
                    if (item.Content.ToString() == entity.name)
                    {
                        for (int i = 0; i < entity.palettes.Count; i++)
                        {
                            StackPanel newSP = new StackPanel() { Orientation = Orientation.Horizontal };

                            if (PalettePanel.Children.Count <= i)
                            {
                                PalettePanel.Children.Add(newSP);

                                newSP.Children.Add(new Rectangle());
                                TextBlock textBox = new TextBlock();
                                newSP.Children.Add(textBox);

                                for (int k = 0; k < entity.palettes[i].colors.Count; k++)
                                {
                                    for (int j = 0; j < entity.palettes[i].colors[k].Count; j++)
                                    {
                                        textBox.Text += entity.palettes[i].colors[k][j].ToString() + " ";
                                    }
                                    textBox.Text += "\n";
                                }
                            }
                            else
                            {
                                var temp = PalettePanel.Children[i] as StackPanel;

                                var textBox = temp.Children[1] as TextBlock;

                                textBox.Text = "";

                                for (int k = 0; k < entity.palettes[i].colors.Count; k++)
                                {
                                    for (int j = 0; j < entity.palettes[i].colors[k].Count; j++)
                                    {
                                        textBox.Text += entity.palettes[i].colors[k][j].ToString() + " ";
                                    }
                                    textBox.Text += "\n";
                                }
                            }

                            if (PalettePanel.Children.Count > entity.palettes.Count)
                            {
                                for (int k = entity.palettes.Count; k < PalettePanel.Children.Count; k++)
                                {
                                    PalettePanel.Children.RemoveAt(k);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
