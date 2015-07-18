namespace SlightPenLighter.Hooks
{
    using System;
    using System.Drawing.Printing;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Forms;
    using System.Windows.Interop;

    using SlightPenLighter.Models;

    public static class DwmHelper
    {
        [DllImport("dwmapi.dll", PreserveSig = true)]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("dwmapi.dll")]
        private static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref Margins pMarInset);

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall,
            ExactSpelling = true, SetLastError = true)]
        internal static extern bool GetWindowRect(IntPtr hWnd, ref Bounds rect);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall,
            ExactSpelling = true, SetLastError = true)]
        internal static extern void MoveWindow(IntPtr hwnd, int x, int y, int nWidth, int nHeight, bool bRepaint);

        private const int WsExTransparent = 0x00000020;
        private const int GwlExstyle = -20;

        public static void DropShadowToWindow(Window window)
        {
            if (!DropShadow(window))
            {
                window.SourceInitialized += WindowInitialized;
            }
        }

        public static Point PixelsToPoints(double x, double y)
        {
            var currentScreen = Screen.PrimaryScreen;

            return new Point
            {
                X = x * SystemParameters.WorkArea.Width / currentScreen.WorkingArea.Width,
                Y = y * SystemParameters.WorkArea.Height / currentScreen.WorkingArea.Height
            };
        }

        private static void WindowInitialized(object sender, EventArgs e)
        {
            var window = (Window) sender;
            DropShadow(window);
            window.SourceInitialized -= WindowInitialized;
        }

        private static bool DropShadow(Window window)
        {
            try
            {
                var helper = new WindowInteropHelper(window);
                var attrValue = 2;
                var attribute = DwmSetWindowAttribute(helper.Handle, 2, ref attrValue, 4);

                if (attribute != 0)
                {
                    return false;
                }

                var margins = new Margins
                {
                    Bottom = 0,
                    Left = 0,
                    Right = 0,
                    Top = 0
                };

                return DwmExtendFrameIntoClientArea(helper.Handle, ref margins) == 0;
            }
            catch
            {
                return false;
            }
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