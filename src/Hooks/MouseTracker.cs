using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using SlightPenLighter.Models;
using SlightPenLighter.UI;

namespace SlightPenLighter.Hooks
{
    public class MouseTracker
    {
        public IntPtr WindowPointer { get; set; }

        public PenHighlighter Highlighter { get; private set; }

        public MouseTracker(IntPtr window, PenHighlighter highlighter)
        {
            WindowPointer = window;
            Highlighter = highlighter;

            var hookManager = new HookManager();
            hookManager.MouseMove += HookManagerOnMouseMove;
            hookManager.MouseClick += HookManagerOnMouseClick;

            hookManager.Start();
        }

        private void HookManagerOnMouseMove(PhysicalPoint next)
        {
            var bounds = DwmHelper.GetWindowBounds(WindowPointer);
            var withinPaintX = next.X >= bounds.Left && next.X <= bounds.Right;
            var withinPaintY = next.Y >= bounds.Top && next.Y <= bounds.Bottom;

            Highlighter.Dispatcher.Invoke(() =>
            {
                var dpiRatio = bounds.Width / Highlighter.Width;
                Console.WriteLine(next.X + "," + next.Y);

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
            });
        }

        private void HookManagerOnMouseClick()
        {
            Task.Run(() =>
            {
                Highlighter.ClickEvent = true;
                Thread.Sleep(5);
                Highlighter.ClickEvent = false;
            });
        }
    }
}