using OriginMenu;
using OriginMenu.Views;
using System;
using System.AddIn;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Windows;

namespace OriginMenu
{
    [AddIn("Origin Menu", Description = "The menu that started it all", Version = "1.0.0.0", Publisher = "Start9")]
    public class OriginMenuAddIn : IModule
    {
        public static OriginMenuAddIn Instance { get; private set; }

        public IConfiguration Configuration { get; set; } = new OriginMenuConfiguration();

        public IMessageContract MessageContract => null;

        public IReceiverContract ReceiverContract { get; } = new OriginMenuReceiverContract();

        public IHost Host { get; private set; }

        public void Initialize(IHost host)
        {
            void Start()
            {
                Instance = this;
                AppDomain.CurrentDomain.UnhandledException += (sender, e) => MessageBox.Show(e.ExceptionObject.ToString(), "Uh Oh E R R O R E");

                Application.ResourceAssembly = Assembly.GetExecutingAssembly();
                App.Main();
            }

            var t = new Thread(Start);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

    }

    public class OriginMenuReceiverContract : IReceiverContract
    {
        public OriginMenuReceiverContract()    
        {
            StartMenuOpenedEntry.MessageReceived += (sender, e) =>
            {
                ((MainWindow)Application.Current.MainWindow).DisplayMenu();
                ((MainWindow)Application.Current.MainWindow).SearchBox.Text = String.Empty;
            };

            SearchResultsEntry.MessageReceived += (sender, e) =>
            {
                ((MainWindow)Application.Current.MainWindow).DisplayMenu();
                ((MainWindow)Application.Current.MainWindow).SearchBox.Text = e.Message.Object.ToString();
            };
        }
        public IList<IReceiverEntry> Entries => new[] { StartMenuOpenedEntry, SearchResultsEntry };

        public IReceiverEntry StartMenuOpenedEntry { get; } = new ReceiverEntry(typeof(DBNull), "Open menu");
        public IReceiverEntry SearchResultsEntry { get; } = new ReceiverEntry(typeof(String), "Show search results");
    }


    public class OriginMenuConfiguration : IConfiguration
    {
        public IList<IConfigurationEntry> Entries => new[] 
        {
            new ConfigurationEntry(PinnedItems, "Pinned Items"),
            new ConfigurationEntry(Places, "Places"),
            new ConfigurationEntry(MRU, "Most Recently Used"),
        };

        public IList<String> PinnedItems { get; } = new List<String>();

        public IList<String> MRU { get; } = new List<String>();

        public IList<String> Places { get; } = new List<String>();
    }
}