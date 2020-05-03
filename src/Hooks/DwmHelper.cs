using System;
using System.Runtime.InteropServices;
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
        private const int GwlExStyle = -20;

        public static void SetWindowExTransparent(IntPtr hwnd)
        {
            var style = GetWindowLong(hwnd, GwlExStyle);
            SetWindowLong(hwnd, GwlExStyle, style | WsExTransparent);
        }

        public static Bounds GetWindowBounds(IntPtr id)
        {
            var rect = new Bounds();
            GetWindowRect(id, ref rect);

            return rect;
        }

        public static void MoveWindow(IntPtr id, int x, int y, int width, int height)
        {
            MoveWindow(id, x, y, width, height, false);
        }
    }
}