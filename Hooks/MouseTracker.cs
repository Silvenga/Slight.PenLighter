namespace SlightPenLighter.Hooks
{
    using System;
    using System.Windows.Threading;

    using SlightPenLighter.Models;
    using SlightPenLighter.UI;

    public class MouseTracker
    {
        public IntPtr WindowPointer { get; set; }

        public PenHighlighter Highlighter { get; private set; }

        public MouseTracker(IntPtr window, PenHighlighter highlighter)
        {
            WindowPointer = window;
            Highlighter = highlighter;
            HookManager.MouseMove += HookManagerOnMouseMove;
            HookManager.MouseClick += HookManagerOnMouseClick;
        }

        private void HookManagerOnMouseMove(PhysicalPoint next)
        {
            DwmHelper.MoveWindow(WindowPointer, next.X, next.Y);
        }

        private void HookManagerOnMouseClick()
        {
            Highlighter.ClickEvent = true;
            Highlighter.Dispatcher.Invoke(() => Highlighter.ClickEvent = false, DispatcherPriority.Background);
        }
    }
}