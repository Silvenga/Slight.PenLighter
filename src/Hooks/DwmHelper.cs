using System;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

using SlightPenLighter.Models;

namespace SlightPenLighter.Hooks
{
    public static class DwmHelper
    {
        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall,
            ExactSpelling = true, SetLastError = true)]
        private static extern bool GetWindowRect(IntPtr hWnd, ref Bounds rect);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall,
            ExactSpelling = true, SetLastError = true)]
        private static extern void MoveWindow(IntPtr hwnd, int x, int y, int nWidth, int nHeight, bool bRepaint);

        private const int WsExTransparent = 0x00000020;
        private const int GwlExstyle = -20;

        public static Point PixelsToPoints(double x, double y)
        {
            var currentScreen = Screen.PrimaryScreen;

            return new Point
            {
                X = x * SystemParameters.WorkArea.Width / currentScreen.WorkingArea.Width,
                Y = y * SystemParameters.WorkArea.Height / currentScreen.WorkingArea.Height
            };
        }

        public static void SetWindowExTransparent(IntPtr hwnd)
        {
            var style = GetWindowLong(hwnd, GwlExstyle);
            SetWindowLong(hwnd, GwlExstyle, style | WsExTransparent);
        }

        public static void MoveWindow(IntPtr id, int x, int y)
        {
            var rect = new Bounds();
            GetWindowRect(id, ref rect);

            var width = rect.Right - rect.Left;
            var height = rect.Bottom - rect.Top;

            x -= width / 2;
            y -= height / 2;

            MoveWindow(id, x, y, width, height, true);
        }
    }
}