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

using Start9.Api.DiskItems;
using static Start9.Api.SystemContext;
using static Start9.Api.SystemScaling;
using System.ComponentModel;

namespace OriginMenu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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
                        item.Background = new SolidColorBrush(Colors.Cyan);
                        item.Icon = new Canvas()
                        {
                            Background = (ImageBrush)(new DiskItemToIconImageBrushConverter().Convert(new DiskItem(expS), null, "1024", null)),
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

        public enum MenuMode
        {
            Normal,
            AllApps,
            Search,
            LeftColumnJumpList
        }

        public MenuMode CurrentMenuMode
        {
            get => (MenuMode)GetValue(CurrentMenuModeProperty);
            set => SetValue(CurrentMenuModeProperty, value);
        }

        public static readonly DependencyProperty CurrentMenuModeProperty =
            DependencyProperty.Register("CurrentMenuMode", typeof(MenuMode), typeof(MainWindow), new PropertyMetadata(MenuMode.Normal, OnCurrentMenuModePropertyChangedCallback));

        public static void OnCurrentMenuModePropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (((sender as MainWindow).CurrentMenuMode == MenuMode.Normal) | ((sender as MainWindow).CurrentMenuMode == MenuMode.LeftColumnJumpList))
            {
                (sender as MainWindow).TopButtonsDockPanel.Visibility = Visibility.Visible;
                (sender as MainWindow).LeftColumnBody.Visibility = Visibility.Visible;
            }
            else
            {
                (sender as MainWindow).TopButtonsDockPanel.Visibility = Visibility.Hidden;
                (sender as MainWindow).LeftColumnBody.Visibility = Visibility.Hidden;
            }

            if ((sender as MainWindow).CurrentMenuMode == MenuMode.Search)
            {
                (sender as MainWindow).SearchListView.Visibility = Visibility.Visible;
            }
            else
            {
                (sender as MainWindow).SearchListView.Visibility = Visibility.Hidden;
            }

            if ((sender as MainWindow).CurrentMenuMode == MenuMode.AllApps)
            {
                (sender as MainWindow).AllAppsTreeView.Visibility = Visibility.Hidden;
            }
            else
            {
                (sender as MainWindow).AllAppsTreeView.Visibility = Visibility.Hidden;
            }
        }

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
                    Background = (ImageBrush)(new DiskItemToIconImageBrushConverter().Convert(new DiskItem(expS), null, "1024", null)),
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
            //base.Show();
            IsManipulationEnabled = true;
        }

        new public void Hide()
        {
            IsManipulationEnabled = false;
            //BeginStoryboard(board);
        }

        private void OriginMenu_Loaded(Object sender, RoutedEventArgs e)
        {
            Hide();
            PowerMenu.Style = (Style)Resources["PowerMenuStyle"];
            UserMenu.Style = (Style)Resources["UserMenuStyle"];
        }

        public void DisplayMenu()
        {
            Topmost = true;
            Show();
            Focus();
            Activate();
            SearchBox.Focus();
        }

        private void TopButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;



            var mnu = btn.ContextMenu;

            if (btn == UserButton)
                mnu.HorizontalOffset = ((btn.PointToScreen(new System.Windows.Point(0, 0)).X - CursorPosition.X));
            else
                mnu.HorizontalOffset = ((btn.PointToScreen(new System.Windows.Point(0, 0)).X - CursorPosition.X) + ((btn.ActualWidth - mnu.ActualWidth)) / 2);

            mnu.VerticalOffset = ((btn.PointToScreen(new System.Windows.Point(0, 0)).Y - CursorPosition.Y) + (btn.ActualHeight));

            mnu.IsOpen = true;
            if (btn == UserButton)
                mnu.HorizontalOffset = ((btn.PointToScreen(new System.Windows.Point(0, 0)).X - CursorPosition.X));
            else
                mnu.HorizontalOffset = ((btn.PointToScreen(new System.Windows.Point(0, 0)).X - CursorPosition.X) + ((btn.ActualWidth - mnu.ActualWidth)) / 2);

            mnu.VerticalOffset = ((btn.PointToScreen(new System.Windows.Point(0, 0)).Y - CursorPosition.Y) + (btn.ActualHeight));



            mnu.IsOpen = true;
        }

        private void TopButton_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            (sender as Button).ContextMenu.IsOpen = false;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(SearchBox.Text))
                CurrentMenuMode = MenuMode.Normal;
            else
                CurrentMenuMode = MenuMode.Search;
        }

        private void AllAppsToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (AllAppsToggleButton.IsChecked == true)
                CurrentMenuMode = MenuMode.AllApps;
            else
                CurrentMenuMode = MenuMode.Normal;
        }
    }
}
