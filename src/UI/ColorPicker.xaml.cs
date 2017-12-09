using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

using SlightPenLighter.Annotations;

namespace SlightPenLighter.UI
{
    public partial class ColorPicker : INotifyPropertyChanged
    {
        private readonly Color _startColor = new Color
        {
            A = 100,
            R = 255,
            B = 0,
            G = 0
        };

        public event PropertyChangedEventHandler PropertyChanged;

        public byte A
        {
            get => ColorBrush.Color.A;
            set
            {
                ColorBrush.Color = new Color
                {
                    A = value,
                    R = ColorBrush.Color.R,
                    B = ColorBrush.Color.B,
                    G = ColorBrush.Color.G
                };
                OnPropertyChanged(nameof(ColorBrush));
                OnPropertyChanged();
            }
        }

        public byte R
        {
            get => ColorBrush.Color.R;
            set
            {
                ColorBrush.Color = new Color
                {
                    A = ColorBrush.Color.A,
                    R = value,
                    B = ColorBrush.Color.B,
                    G = ColorBrush.Color.G
                };
                OnPropertyChanged(nameof(ColorBrush));
                OnPropertyChanged();
            }
        }

        public byte G
        {
            get => ColorBrush.Color.G;
            set
            {
                ColorBrush.Color = new Color
                {
                    A = ColorBrush.Color.A,
                    R = ColorBrush.Color.R,
                    B = ColorBrush.Color.B,
                    G = value
                };
                OnPropertyChanged(nameof(ColorBrush));
                OnPropertyChanged();
            }
        }

        public byte B
        {
            get => ColorBrush.Color.B;
            set
            {
                ColorBrush.Color = new Color
                {
                    A = ColorBrush.Color.A,
                    R = ColorBrush.Color.R,
                    B = value,
                    G = ColorBrush.Color.G
                };
                OnPropertyChanged(nameof(ColorBrush));
                OnPropertyChanged();
            }
        }

        public Color Color
        {
            get => ColorBrush.Color;
            set
            {
                ColorBrush.Color = value;
                OnPropertyChanged(nameof(A));
                OnPropertyChanged(nameof(B));
                OnPropertyChanged(nameof(G));
                OnPropertyChanged(nameof(R));
                OnPropertyChanged(nameof(ColorBrush));
                OnPropertyChanged();
            }
        }

        public SolidColorBrush ColorBrush { get; }

        public ColorPicker()
        {
            InitializeComponent();
            ColorBrush = new SolidColorBrush(_startColor);
            DataContext = this;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}