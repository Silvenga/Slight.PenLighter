using System;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Threading;

using SlightPenLighter.Hooks;

using Application = System.Windows.Application;

namespace SlightPenLighter.UI {

    public partial class PenHighlighter {

        private MouseTracker MouseTracker {
            get;
            set;
        }

        private static IntPtr WindowPointer {
            get;
            set;
        }

        private OptionWindow OptionWindow {
            get;
            set;
        }

        private bool AutoHide {
            get;
            set;
        }

        private NotifyIcon NotifyIcon {
            get;
            set;
        }

        public PenHighlighter() {

            InitializeComponent();

            CreateIcon();

            Top = 0;
            Left = 0;
        }

        private void MainWindow_OnSourceInitialized(object sender, EventArgs e) {

            Dispatcher.Invoke(new Action(Target), DispatcherPriority.ContextIdle, null);
        }

        private void Target() {

            WindowPointer = new WindowInteropHelper(this).Handle;
            DwmHelper.SetWindowExTransparent(WindowPointer);
            MouseTracker = new MouseTracker(WindowPointer);

            OptionWindow = new OptionWindow(this);
        }

        private void CreateIcon() {

            NotifyIcon = new NotifyIcon {
                ContextMenu = new ContextMenu(),
                Icon = Properties.Resources.icon,
                Text = @"Pen Highlighter",
                Visible = true
            };

            RefreshMenu();
        }

        private void RefreshMenu() {

            var menu = new ContextMenu();

            var optionsItem = new MenuItem("Show Options");
            optionsItem.Click += OpenOptions;
            menu.MenuItems.Add(optionsItem);

            if(AutoHide) {

                var hideItem = new MenuItem(string.Format("{0} Highlighter", (IsVisible) ? "Hide" : "Show"));
                hideItem.Click += VisiblityToggle;
                menu.MenuItems.Add(hideItem);
            } else {

                var hideItem = new MenuItem(string.Format("{0} Highlighter (Automatic Mode)", (IsVisible) ? "Hide" : "Show")) {
                    Enabled = false
                };
                menu.MenuItems.Add(hideItem);
            }

            var autoItem = new MenuItem(string.Format("{0} Autohide", (AutoHide) ? "Enable" : "Disable"));
            autoItem.Click += AutoHideToggle;
            menu.MenuItems.Add(autoItem);

            var exitItem = new MenuItem("Exit");
            exitItem.Click += Exit;
            menu.MenuItems.Add(exitItem);

            NotifyIcon.ContextMenu = menu;
        }

        private void OpenOptions(object sender, EventArgs eventArgs) {

            OptionWindow.Show();
        }

        private static void Exit(object sender, EventArgs eventArgs) {

            Application.Current.Shutdown(0);
        }

        private void VisiblityToggle(object sender, EventArgs eventArgs) {

            if(IsVisible) {
                Hide();
            } else {
                Show();
            }

            AutoHide = IsVisible;
            RefreshMenu();
        }

        private void AutoHideToggle(object sender, EventArgs eventArgs) {

            if(AutoHide) {
                Hide();
            } else {
                Show();
            }

            RefreshMenu();
        }

    }

}