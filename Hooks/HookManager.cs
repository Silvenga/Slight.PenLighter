namespace SlightPenLighter.Hooks
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;

    using SlightPenLighter.Models;

    public static class HookManager
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int SetWindowsHookEx(int idHook, Hook hook, IntPtr hMod, int dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int UnhookWindowsHookEx(int idHook);

        private const int WhMouseLl = 14;
        private const int WmLButtonDown = 513;
        private const int WmRButtonDown = 516;
        private const int WmMButtonDown = 519;

        public delegate void MousePointChange(PhysicalPoint physicalPoint);
        public delegate void MouseClickHandler();

        private static event MousePointChange MouseMoveBackend;
        private static event MouseClickHandler MouseClickBackend;

        public static event MousePointChange MouseMove
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                MouseMoveBackend += value;
            }

            remove
            {
                MouseMoveBackend -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }

        public static event MouseClickHandler MouseClick
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                MouseClickBackend += value;
            }

            remove
            {
                MouseClickBackend -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }

        private delegate int Hook(int nCode, int wParam, IntPtr lParam);

        private static Hook mouseDelegate;

        private static int mouseHookHandle;
        private static int oldX;
        private static int oldY;

        private static int MouseHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                var mouseHook = (MouseHook) Marshal.PtrToStructure(lParam, typeof(MouseHook));

                if (MouseMoveBackend != null && (oldX != mouseHook.PhysicalPoint.X || oldY != mouseHook.PhysicalPoint.Y))
                {
                    oldX = mouseHook.PhysicalPoint.X;
                    oldY = mouseHook.PhysicalPoint.Y;

                    MouseMoveBackend.Invoke(mouseHook.PhysicalPoint);
                }
            }

            if (MouseClickBackend != null)
            {
                switch (wParam)
                {
                    case WmLButtonDown:
                    case WmRButtonDown:
                    case WmMButtonDown:
                        MouseClickBackend.Invoke();
                        break;
                }
            }

            return CallNextHookEx(mouseHookHandle, nCode, wParam, lParam);
        }

        private static void EnsureSubscribedToGlobalMouseEvents()
        {
            if (mouseHookHandle == 0)
            {
                mouseDelegate = MouseHookProc;
                mouseHookHandle = SetWindowsHookEx(WhMouseLl, mouseDelegate, IntPtr.Zero, 0);

                if (mouseHookHandle == 0)
                {
                    var status = Marshal.GetLastWin32Error();
                    throw new Win32Exception(status);
                }
            }
        }

        private static void TryUnsubscribeFromGlobalMouseEvents()
        {
            if (MouseMoveBackend == null && MouseClickBackend == null)
            {
                ForceUnsunscribeFromGlobalMouseEvents();
            }
        }

        private static void ForceUnsunscribeFromGlobalMouseEvents()
        {
            if (mouseHookHandle != 0)
            {
                var result = UnhookWindowsHookEx(mouseHookHandle);

                mouseHookHandle = 0;
                mouseDelegate = null;

                if (result == 0)
                {
                    var status = Marshal.GetLastWin32Error();
                    throw new Win32Exception(status);
                }
            }
        }
    }
}