using Start9.Api.Controls;
using Start9.Api.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace OriginMenu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : QuadContentWindow
    {
        String pinnedPath = Environment.ExpandEnvironmentVariables(@"%appdata%\Start9\TempData\OriginMenu_PinnedApps.txt");
        String placesPath = Environment.ExpandEnvironmentVariables(@"%appdata%\Start9\TempData\OriginMenu_Places.txt");
        String lastUsedPath = Environment.ExpandEnvironmentVariables(@"%appdata%\Start9\TempData\OriginMenu_LastUsed.txt");

        public List<IconButton> PinnedItems
        {
            get
            {
                var list = new List<IconButton>();
                foreach (var s in File.ReadAllLines(pinnedPath))
                {
                    String[] splitS = s.Split(';');
                    var expS = Environment.ExpandEnvironmentVariables(splitS[0]);
                    var item = new IconButton()
                    {
                        Content = Path.GetFileNameWithoutExtension(expS),
                        Tag = expS
                    };
                    Debug.WriteLine(expS);
                    if (File.Exists(expS))
                    {
                        item.Background = new SolidColorBrush(Colors.DarkRed);
                        item.Icon = new Canvas()
                        {
                            Background = new ImageBrush(MiscTools.GetIconFromFilePath(expS, 64, 64, 0x00000000 | 0x100)),
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            VerticalAlignment = VerticalAlignment.Stretch
                        };
                    }
                    item.PreviewMouseLeftButtonUp += (sneder, args) =>
                    {
                        if (File.Exists(expS) | Directory.Exists(expS))
                        {
                            try
                            {
                                Process.Start(expS);
                            }
                            catch (System.ComponentModel.Win32Exception ex)
                            {
                                Debug.WriteLine(ex);
                            }
                        }
                        else
                        {
                            Process.Start("cmd.exe", @"/C r" + s);
                        }
                        Hide();
                    };

                    switch (splitS[1].ToLower())
                    {
                        case "small":
                            item.Style = (Style)Resources["SmallTileStyle"];
                            break;
                        case "wide":
                            item.Style = (Style)Resources["WideTileStyle"];
                            break;
                        case "large":
                            item.Style = (Style)Resources["LargeTileStyle"];
                            break;
                        default:
                            item.Style = (Style)Resources["MediumTileStyle"];
                            break;
                    }

                    list.Add(item);
                }
                return list;
            }
            set
            {
                var list = new List<String>();
                foreach (IconButton i in value)
                {
                    var size = "medium";
                    if (i.Style == (Style)Resources["SmallTileStyle"])
                    {
                        size = "small";
                    }
                    else if (i.Style == (Style)Resources["WideTileStyle"])
                    {
                        size = "wide";
                    }
                    else if (i.Style == (Style)Resources["LargeTileStyle"])
                    {
                        size = "large";
                    }

                    list.Add(i.Tag.ToString() + ";" + size);
                }
                File.WriteAllLines(pinnedPath, list.ToArray());
            }
        }

        public List<IconListViewItem> Places
        {
            get
            {
                var list = new List<IconListViewItem>();
                foreach (var s in File.ReadAllLines(placesPath))
                {
                    var expS = Environment.ExpandEnvironmentVariables(s);
                    var item = GetListViewItem(expS);
                    item.MouseLeftButtonUp += (sneder, args) =>
                    {
                        if (File.Exists(expS) | Directory.Exists(expS))
                        {
                            Process.Start(expS);
                        }
                        else
                        {
                            Process.Start("cmd.exe", @"/C " + s);
                        }

                        AddLastUsedEntry(s);

                        Hide();
                    };
                    list.Add(item);
                }
                return list;
            }
            set
            {
                var list = new List<String>();
                foreach (var i in value)
                {
                    list.Add(i.Tag.ToString());
                }
                File.WriteAllLines(placesPath, list.ToArray());
            }
        }

        public List<IconListViewItem> LastUsed
        {
            get
            {
                var list = new List<IconListViewItem>();
                foreach (var s in File.ReadAllLines(lastUsedPath))
                {
                    var expS = Environment.ExpandEnvironmentVariables(s);
                    var item = GetListViewItem(expS);
                    item.MouseLeftButtonUp += (sneder, args) =>
                    {
                        if (File.Exists(expS) | Directory.Exists(expS))
                        {
                            Process.Start(expS);
                        }
                        else
                        {
                            Process.Start("cmd.exe", @"/C " + s);
                        }

                        AddLastUsedEntry(s);

                        Hide();
                    };
                    list.Add(item);
                }
                return list;
            }
            set
            {
                var list = new List<String>();
                foreach (var i in value)
                {
                    list.Add(i.Tag.ToString());
                }
                File.WriteAllLines(lastUsedPath, list.ToArray());
            }
        }

        ToggleButton tempStart;

        public MainWindow()
        {
            InitializeComponent();
            Left = 0;
            Top = SystemParameters.WorkArea.Bottom - Height;

            Deactivated += delegate { Hide(); };
        }

        public IconListViewItem GetListViewItem(String expS)
        {
            var item = new IconListViewItem()
            {
                Content = Path.GetFileNameWithoutExtension(expS),
                Tag = expS
            };
            if (File.Exists(expS) | Directory.Exists(expS))
            {
                item.Icon = new Canvas()
                {
                    Background = new ImageBrush(MiscTools.GetIconFromFilePath(expS, 32, (UInt32)(0x00000000 | 0x100))),
                    Width = 32,
                    Height = 32
                };
            }
            return item;
        }

        public void AddLastUsedEntry(String path)
        {
            var expS = Environment.ExpandEnvironmentVariables(path);
            for (var i = 0; i < LastUsed.Count; i++)
            {
                var it = (LastUsed[i] as IconListViewItem);
                if ((it.Tag.ToString() == path) | (it.Tag.ToString() == expS))
                {
                    LastUsed.RemoveAt(i);
                    i -= 1;
                }
            }

            LastUsed.Insert(0, GetListViewItem(expS));
        }

        new public void Show()
        {
            IsManipulationEnabled = true;
            var board = (Storyboard)Resources["ShowMenu"];
            BeginStoryboard(board);
        }

        new public void Hide()
        {
            var board = (Storyboard)Resources["HideMenu"];
            board.Completed += (sneder, args) =>
            {
                base.Hide();
            };

            IsManipulationEnabled = false;
            BeginStoryboard(board);
        }

        private void OriginMenu_Loaded(Object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
