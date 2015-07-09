using System;
using SlightPenLighter.Models;

namespace SlightPenLighter.Hooks
{
    public class MouseTracker
    {
        public IntPtr WindowPointer
        {
            get;
            set;
        }

        public MouseTracker(IntPtr window)
        {
            WindowPointer = window;
            HookManager.MouseMove += HookManagerOnMouseMove;
        }

        private void HookManagerOnMouseMove(PhysicalPoint next)
        {
            DwmHelper.MoveWindow(WindowPointer, next.X, next.Y);
        }
    }
}