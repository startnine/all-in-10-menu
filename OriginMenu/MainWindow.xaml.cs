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
using System.Collections.ObjectModel;

namespace OriginMenu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        String _pinnedPath = Environment.ExpandEnvironmentVariables(@"%appdata%\Start9\TempData\OriginMenu_PinnedApps.txt");
        String _placesPath = Environment.ExpandEnvironmentVariables(@"%appdata%\Start9\TempData\OriginMenu_Places.txt");
        String _lastUsedPath = Environment.ExpandEnvironmentVariables(@"%appdata%\Start9\TempData\OriginMenu_LastUsed.txt");

        String _appDataAllAppsDir = Environment.ExpandEnvironmentVariables(@"%appdata%\Microsoft\Windows\Start Menu\Programs");
        String _programDataAllAppsDir = Environment.ExpandEnvironmentVariables(@"%programdata%\Microsoft\Windows\Start Menu\Programs");

        public List<String> sizes = new List<String>();

        public ObservableCollection<DiskItem> PinnedItems
        {
            get
            {
                string[] pathStrings = File.ReadAllLines(_pinnedPath);
                ObservableCollection<DiskItem> items = new ObservableCollection<DiskItem>();
                sizes.Clear();
                foreach (string s in pathStrings)
                {
                    items.Add(new DiskItem(s.Substring(0, s.IndexOf(';'))));
                    sizes.Add(s.Substring(s.IndexOf(';')));
                }
                return items;
            }
            set
            {
                List<string> pathStrings = new List<string>();
                foreach (DiskItem d in value)
                {
                    pathStrings.Add(d.ItemPath + sizes[value.IndexOf(d)]);
                }
                File.WriteAllLines(_pinnedPath, pathStrings);
            }
        }

        public ObservableCollection<DiskItem> Places
        {
            get
            {
                string[] pathStrings = File.ReadAllLines(_placesPath);
                ObservableCollection<DiskItem> items = new ObservableCollection<DiskItem>();
                foreach (string s in pathStrings)
                {
                    items.Add(new DiskItem(s));
                }
                return items;
            }
            set
            {
                List<string> pathStrings = new List<string>();
                foreach (DiskItem d in value)
                {
                    pathStrings.Add(d.ItemPath);
                }
                File.WriteAllLines(_placesPath, pathStrings);
            }
        }

        public ObservableCollection<DiskItem> LastUsed
        {
            get
            {
                string[] pathStrings = File.ReadAllLines(_lastUsedPath);
                ObservableCollection<DiskItem> items = new ObservableCollection<DiskItem>();
                foreach (string s in pathStrings)
                {
                    items.Add(new DiskItem(s));
                }
                return items;
            }
            set
            {
                List<string> pathStrings = new List<string>();
                foreach (DiskItem d in value)
                {
                    pathStrings.Add(d.ItemPath);
                }
                File.WriteAllLines(_lastUsedPath, pathStrings);
            }
        }

        public ObservableCollection<DiskItem> AllApps
        {
            get
            {
                IEnumerable<String> appDataFolderPathStrings = Directory.EnumerateDirectories(_appDataAllAppsDir);
                IEnumerable<String> programDataFolderPathStrings = Directory.EnumerateDirectories(_programDataAllAppsDir);
                ObservableCollection<DiskItem> items = new ObservableCollection<DiskItem>();
                sizes.Clear();

                foreach (string s in appDataFolderPathStrings)
                {
                    items.Add(new DiskItem(s));
                }
                foreach (string s in programDataFolderPathStrings)
                {
                    items.Add(new DiskItem(s));
                }

                IEnumerable<String> appDataFilePathStrings = Directory.EnumerateFiles(_appDataAllAppsDir);
                IEnumerable<String> programDataFilePathStrings = Directory.EnumerateFiles(_programDataAllAppsDir);

                foreach (string s in appDataFilePathStrings)
                {
                    items.Add(new DiskItem(s));
                }
                foreach (string s in programDataFilePathStrings)
                {
                    items.Add(new DiskItem(s));
                }

                return items;
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

        public double RightColumnWidth
        {
            get => PinnedItemsListView.ActualWidth;
        }

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

            if (((sender as MainWindow).CurrentMenuMode == MenuMode.Search) || ((sender as MainWindow).CurrentMenuMode == MenuMode.LeftColumnJumpList))
            {
                (sender as MainWindow).PinnedItemsListView.Visibility = Visibility.Hidden;
            }
            else
            {
                (sender as MainWindow).PinnedItemsListView.Visibility = Visibility.Visible;
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
                (sender as MainWindow).AllAppsTreeView.Visibility = Visibility.Visible;
                if ((sender as MainWindow).AllAppsToggleButton.IsChecked != true)
                {
                    (sender as MainWindow).AllAppsToggleButton.IsChecked = true;
                }
            }
            else
            {
                (sender as MainWindow).AllAppsTreeView.Visibility = Visibility.Hidden;
                if ((sender as MainWindow).AllAppsToggleButton.IsChecked != false)
                {
                    (sender as MainWindow).AllAppsToggleButton.IsChecked = false;
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            Left = 0;
            Top = SystemParameters.WorkArea.Bottom - Height;

            Deactivated += delegate { Hide(); };
        }

        /*public IconListViewItem GetListViewItem(String expS)
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
        }*/

        new public void Show()
        {
            //base.Show();
            IsManipulationEnabled = true;
        }

        new public void Hide()
        {
            CurrentMenuMode = MenuMode.Normal;
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

        private void PlacesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = PlacesListView.SelectedItem as DiskItem;

            Process.Start(Environment.ExpandEnvironmentVariables(selected.ItemPath));

            if (LastUsed.Contains(selected))
            {
                LastUsed.Remove(selected);
            }
            LastUsed.Insert(0, selected);
            Hide();
        }

        private void LastUsedListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = LastUsedListView.SelectedItem as DiskItem;
            LastUsed.Remove(selected);
            LastUsed.Insert(0, selected);
            Hide();
        }

        private void AllAppsTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selected = (AllAppsTreeView.SelectedItem as DiskItem);

            Process.Start(Environment.ExpandEnvironmentVariables(selected.ItemPath));

            if (LastUsed.Contains(selected))
            {
                LastUsed.Remove(selected);
            }
            LastUsed.Insert(0, selected);
        }
    }
}
