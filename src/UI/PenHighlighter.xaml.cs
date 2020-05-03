using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Threading;
using SlightPenLighter.Annotations;
using SlightPenLighter.Hooks;
using Application = System.Windows.Application;

namespace SlightPenLighter.UI
{
    public partial class PenHighlighter : INotifyPropertyChanged
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private MouseTracker MouseTracker { get; set; }

        private static IntPtr WindowPointer { get; set; }

        private OptionWindow OptionWindow { get; set; }

        private NotifyIcon NotifyIcon { get; set; }

        public bool PulseClick { get; set; } // TODO: Add this to be part of the save data

        private bool _clickEvent;

        public bool ClickEvent
        {
            get => _clickEvent;
            set
            {
                if (value == _clickEvent || !PulseClick)
                {
                    return;
                }

                _clickEvent = value;
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

        private void PenHighlighter_OnLoaded(object sender, EventArgs e)
        {
            Dispatcher.Invoke(new Action(OnSourceInitialized), DispatcherPriority.ContextIdle, null);
        }

        private void PenHighlighter_OnUnloaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(OnSourceUninitialized), DispatcherPriority.ContextIdle, null);
        }

        private void OnSourceInitialized()
        {
            WindowPointer = new WindowInteropHelper(this).Handle;
            DwmHelper.SetWindowExTransparent(WindowPointer);
            MouseTracker = new MouseTracker(WindowPointer, this);

            OptionWindow = new OptionWindow(this);
        }

        private void OnSourceUninitialized()
        {
            MouseTracker.Dispose();
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

            var hideItem = new MenuItem($"{(IsVisible ? "Hide" : "Show")} Highlighter");
            hideItem.Click += VisibilityToggle;
            menu.MenuItems.Add(hideItem);

            var autoItem = new MenuItem($"{(PulseClick ? "Disable" : "Enable")} Click Pulsing");
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
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}