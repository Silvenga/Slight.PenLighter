namespace SlightPenLighter.UI
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using System.Windows.Interop;
    using System.Windows.Threading;

    using SlightPenLighter.Annotations;
    using SlightPenLighter.Hooks;

    using Application = System.Windows.Application;

    public partial class PenHighlighter : INotifyPropertyChanged
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private MouseTracker MouseTracker { get; set; }

        private static IntPtr WindowPointer { get; set; }

        private OptionWindow OptionWindow { get; set; }

        private NotifyIcon NotifyIcon { get; set; }

        public bool PulseClick { get; set; } // TODO: Add this to be part of the save data

        private bool clickEvent;

        public bool ClickEvent
        {
            get { return clickEvent; }
            set
            {
                if (value == clickEvent || !PulseClick)
                {
                    return;
                }
                clickEvent = value;
                OnPropertyChanged();
            }
        }

        public PenHighlighter()
        {
            InitializeComponent();

            DataContext = this;

            CreateIcon();

            Top = 0;
            Left = 0;
        }

        private void MainWindow_OnSourceInitialized(object sender, EventArgs e)
        {
            Dispatcher.Invoke(new Action(Target), DispatcherPriority.ContextIdle, null);
        }

        private void Target()
        {
            WindowPointer = new WindowInteropHelper(this).Handle;
            DwmHelper.SetWindowExTransparent(WindowPointer);
            MouseTracker = new MouseTracker(WindowPointer, this);

            OptionWindow = new OptionWindow(this);
        }

        private void CreateIcon()
        {
            NotifyIcon = new NotifyIcon
            {
                ContextMenu = new ContextMenu(),
                Icon = Properties.Resources.icon,
                Text = @"Pen Highlighter",
                Visible = true
            };

            RefreshMenu();
        }

        private void RefreshMenu()
        {
            var menu = new ContextMenu();

            var optionsItem = new MenuItem("Show Options");
            optionsItem.Click += OpenOptions;
            menu.MenuItems.Add(optionsItem);

            var hideItem = new MenuItem(string.Format("{0} Highlighter", (IsVisible) ? "Hide" : "Show"));
            hideItem.Click += VisibilityToggle;
            menu.MenuItems.Add(hideItem);

            var autoItem = new MenuItem(string.Format("{0} Click Pulsing", (PulseClick) ? "Disable" : "Enable"));
            autoItem.Click += BlinkToggle;
            menu.MenuItems.Add(autoItem);

            var exitItem = new MenuItem("Exit");
            exitItem.Click += Exit;
            menu.MenuItems.Add(exitItem);

            NotifyIcon.ContextMenu = menu;
        }

        private void OpenOptions(object sender, EventArgs eventArgs)
        {
            OptionWindow.Show();
        }

        private static void Exit(object sender, EventArgs eventArgs)
        {
            Application.Current.Shutdown(0);
        }

        private void VisibilityToggle(object sender, EventArgs eventArgs)
        {
            if (IsVisible)
            {
                Hide();
            }
            else
            {
                Show();
            }

            RefreshMenu();
        }

        private void BlinkToggle(object sender, EventArgs eventArgs)
        {
            PulseClick = !PulseClick;

            RefreshMenu();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}