using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace SlightPenLighter.UI {

    public partial class WindowControl {

        public WindowControl() {

            InitializeComponent();
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(WindowControl), new PropertyMetadata(""));

        public string Header {
            get {
                return (string) GetValue(HeaderProperty);
            }
            set {
                SetValue(HeaderProperty, value);
            }
        }

        public static readonly DependencyProperty ParentWindowProperty =
            DependencyProperty.Register(
                "ParentWindow",
                typeof(Window),
                typeof(WindowControl),
                new PropertyMetadata(default(Window)));

        public Window ParentWindow {
            get {
                return (Window) GetValue(ParentWindowProperty);
            }
            set {
                SetValue(ParentWindowProperty, value);
            }
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e) {

            if(ParentWindow != null)
                ParentWindow.DragMove();
        }

        private void Close(object sender, MouseButtonEventArgs e) {

            if(ParentWindow != null && e.LeftButton == MouseButtonState.Pressed)
                ParentWindow.Hide();
        }

        private void Min(object sender, MouseButtonEventArgs e) {

            if(ParentWindow != null && e.LeftButton == MouseButtonState.Pressed)
                ParentWindow.WindowState = WindowState.Minimized;
        }

        private void Link(object sender, MouseButtonEventArgs e) {

            Process.Start("https://silvenga.com");
        }

    }

}