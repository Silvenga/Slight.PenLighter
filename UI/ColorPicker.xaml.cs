namespace SlightPenLighter.UI
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Media;

    using SlightPenLighter.Annotations;

    public partial class ColorPicker : INotifyPropertyChanged
    {
        private readonly Color startColor = new Color
        {
            A = 100,
            R = 255,
            B = 0,
            G = 0
        };

        public event PropertyChangedEventHandler PropertyChanged;

        public byte A
        {
            get { return ColorBrush.Color.A; }
            set
            {
                ColorBrush.Color = new Color
                {
                    A = value,
                    R = ColorBrush.Color.R,
                    B = ColorBrush.Color.B,
                    G = ColorBrush.Color.G
                };
                OnPropertyChanged("ColorBrush");
                OnPropertyChanged();
            }
        }

        public byte R
        {
            get { return ColorBrush.Color.R; }
            set
            {
                ColorBrush.Color = new Color
                {
                    A = ColorBrush.Color.A,
                    R = value,
                    B = ColorBrush.Color.B,
                    G = ColorBrush.Color.G
                };
                OnPropertyChanged("ColorBrush");
                OnPropertyChanged();
            }
        }

        public byte G
        {
            get { return ColorBrush.Color.G; }
            set
            {
                ColorBrush.Color = new Color
                {
                    A = ColorBrush.Color.A,
                    R = ColorBrush.Color.R,
                    B = ColorBrush.Color.B,
                    G = value
                };
                OnPropertyChanged("ColorBrush");
                OnPropertyChanged();
            }
        }

        public byte B
        {
            get { return ColorBrush.Color.B; }
            set
            {
                ColorBrush.Color = new Color
                {
                    A = ColorBrush.Color.A,
                    R = ColorBrush.Color.R,
                    B = value,
                    G = ColorBrush.Color.G
                };
                OnPropertyChanged("ColorBrush");
                OnPropertyChanged();
            }
        }

        public Color Color
        {
            get { return ColorBrush.Color; }
            set
            {
                ColorBrush.Color = value;
                OnPropertyChanged("A");
                OnPropertyChanged("B");
                OnPropertyChanged("G");
                OnPropertyChanged("R");
                OnPropertyChanged("ColorBrush");
                OnPropertyChanged();
            }
        }

        public SolidColorBrush ColorBrush { get; private set; }

        public ColorPicker()
        {
            InitializeComponent();
            ColorBrush = new SolidColorBrush(startColor);
            DataContext = this;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}