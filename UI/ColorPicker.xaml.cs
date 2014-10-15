using System.Windows.Media;
using System.ComponentModel;

namespace SlightPenLighter.UI {

    public partial class ColorPicker : INotifyPropertyChanged {

        private readonly Color _startColor = new Color {
            A = 100,
            R = 255,
            B = 0,
            G = 0
        };

        public event PropertyChangedEventHandler PropertyChanged;

        public void InvokeNotify(string propertyName) {

            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public byte A {
            get {
                return ColorBrush.Color.A;
            }
            set {
                ColorBrush.Color = new Color {
                    A = value,
                    R = ColorBrush.Color.R,
                    B = ColorBrush.Color.B,
                    G = ColorBrush.Color.G
                };
                InvokeNotify("ColorBrush");
                InvokeNotify("A");
            }
        }

        public byte R {
            get {
                return ColorBrush.Color.R;
            }
            set {
                ColorBrush.Color = new Color {
                    A = ColorBrush.Color.A,
                    R = value,
                    B = ColorBrush.Color.B,
                    G = ColorBrush.Color.G
                };
                InvokeNotify("ColorBrush");
                InvokeNotify("R");
            }
        }

        public byte G {
            get {
                return ColorBrush.Color.G;
            }
            set {
                ColorBrush.Color = new Color {
                    A = ColorBrush.Color.A,
                    R = ColorBrush.Color.R,
                    B = ColorBrush.Color.B,
                    G = value
                };
                InvokeNotify("ColorBrush");
                InvokeNotify("G");
            }
        }

        public byte B {
            get {
                return ColorBrush.Color.B;
            }
            set {
                ColorBrush.Color = new Color {
                    A = ColorBrush.Color.A,
                    R = ColorBrush.Color.R,
                    B = value,
                    G = ColorBrush.Color.G
                };
                InvokeNotify("ColorBrush");
                InvokeNotify("B");
            }
        }

        public Color Color {
            get {
                return ColorBrush.Color;
            }
            set {
                ColorBrush.Color = value;
                InvokeNotify("A");
                InvokeNotify("B");
                InvokeNotify("G");
                InvokeNotify("R");
                InvokeNotify("ColorBrush");
                InvokeNotify("Color");
            }
        }

        public SolidColorBrush ColorBrush {
            get;
            private set;
        }

        public ColorPicker() {

            InitializeComponent();
            ColorBrush = new SolidColorBrush(_startColor);
            DataContext = this;
        }

    }

}