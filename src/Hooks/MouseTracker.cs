using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using SlightPenLighter.Models;
using SlightPenLighter.UI;

namespace SlightPenLighter.Hooks
{
    public class MouseTracker : IDisposable
    {
        private readonly HookManager _hookManager = new HookManager();

        public IntPtr WindowPointer { get; }

        public PenHighlighter Highlighter { get; }

        public MouseTracker(IntPtr window, PenHighlighter highlighter)
        {
            WindowPointer = window;
            Highlighter = highlighter;

            _hookManager.MouseMove += HookManagerOnMouseMove;
            _hookManager.MouseClick += HookManagerOnMouseClick;

            _hookManager.Start();
        }

        private async void HookManagerOnMouseMove(PhysicalPoint next)
        {
            var bounds = DwmHelper.GetWindowBounds(WindowPointer);
            var withinPaintX = next.X >= bounds.Left && next.X <= bounds.Right;
            var withinPaintY = next.Y >= bounds.Top && next.Y <= bounds.Bottom;

            await Highlighter.Dispatcher.InvokeAsync(() => { DispatchUpdate(next, bounds, withinPaintX, withinPaintY); });
        }

        private void DispatchUpdate(PhysicalPoint next, Bounds bounds, bool withinPaintX, bool withinPaintY)
        {
            var dpiRatio = bounds.Width / Highlighter.Width;

            if (withinPaintX && withinPaintY)
            {
                Canvas.SetTop(Highlighter.Lighter, (next.Y - bounds.Top - Highlighter.Lighter.Height / 2) / dpiRatio);
                Canvas.SetLeft(Highlighter.Lighter, (next.X - bounds.Left - Highlighter.Lighter.Width / 2) / dpiRatio);
            }
            else
            {
                var screen = Screen.FromPoint(new Point(next.X, next.Y));
                DwmHelper.MoveWindow(WindowPointer, screen.Bounds.X, screen.Bounds.Y, screen.Bounds.Width, screen.Bounds.Height);

                Canvas.SetTop(Highlighter.Lighter, (0 - Highlighter.Lighter.Height) / dpiRatio);
                Canvas.SetLeft(Highlighter.Lighter, (0 - Highlighter.Lighter.Width) / dpiRatio);
            }
        }

        private async void HookManagerOnMouseClick()
        {
            Highlighter.ClickEvent = true;
            await Task.Delay(5);
            Highlighter.ClickEvent = false;
        }

        public void Dispose()
        {
            _hookManager.MouseMove -= HookManagerOnMouseMove;
            _hookManager.MouseClick -= HookManagerOnMouseClick;

            _hookManager?.Dispose();
        }
    }
}