using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

using SlightPenLighter.Hooks;
using SlightPenLighter.Models;

namespace SlightPenLighter.UI
{
    using System.Runtime.CompilerServices;

    using SlightPenLighter.Annotations;

    public partial class OptionWindow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public PenHighlighter Highlighter { get; private set; }

        private double _size = 30;

        public double Size
        {
            get { return _size; }
            set
            {
                _size = value;
                Shape.Height = _size;
                Shape.Width = _size;
                RemoteShape.Height = _size;
                RemoteShape.Width = _size;
                CenterCanvasItem(Shape);
                CenterCanvasItem(RemoteShape);
                OnPropertyChanged();
            }
        }

        public Shape Shape { get; private set; }

        public Shape RemoteShape { get; private set; }

        public OptionWindow(PenHighlighter highlighter)
        {
            InitializeComponent();
            Highlighter = highlighter;
            DataContext = this;
        }

        public static void CenterCanvasItem(Shape shape)
        {
            const int halfOfCanvas = 60;
            Canvas.SetTop(shape, (halfOfCanvas) - (shape.Height / 2));
            Canvas.SetLeft(shape, (halfOfCanvas) - (shape.Width / 2));
        }

        private void OptionWindow_OnInitialized(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(LoadLighter), DispatcherPriority.ContextIdle, null);
            DwmHelper.DropShadowToWindow(this);
        }

        private void LoadLighter()
        {
            Shape = new Ellipse
            {
                Fill = Picker.ColorBrush
            };

            RemoteShape = new Ellipse
            {
                Fill = Picker.ColorBrush
            };

            DataContext = this;

            Canvas.Children.Add(Shape);
            Highlighter.Canvas.Children.Add(RemoteShape);

            Size = 30;

            LoadSettings();
        }

        public void LoadSettings()
        {
            var file = new FileInfo("settings.db");

            if (file.Exists)
            {
                var data = Save.DeserializeObject(file.FullName);

                Picker.Color = new Color
                {
                    A = data.A,
                    R = data.R,
                    G = data.G,
                    B = data.B,
                };
                Size = data.Size;
            }
        }

        public void SaveSettings()
        {
            var file = new FileInfo("settings.db");

            var data = new Save
            {
                A = Picker.Color.A,
                R = Picker.Color.R,
                G = Picker.Color.G,
                B = Picker.Color.B,
                Size = Size
            };

            Save.SerializeObject(file.FullName, data);
        }

        private void SaveOnClick(object sender, RoutedEventArgs e)
        {
            SaveSettings();
            Hide();
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