using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using SlightPenLighter.Models;

namespace SlightPenLighter.Hooks
{
    public class HookManager
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

        private event MousePointChange MouseMoveBackend;
        private event MouseClickHandler MouseClickBackend;

        public event MousePointChange MouseMove
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

        public event MouseClickHandler MouseClick
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

        private readonly BlockingCollection<(int nCode, int wParam, IntPtr lParam)> _eventQueue = new BlockingCollection<(int nCode, int wParam, IntPtr lParam)>();

        private delegate int Hook(int nCode, int wParam, IntPtr lParam);

        private Hook _mouseDelegate;

        private int _mouseHookHandle;
        private int _oldX;
        private int _oldY;

        public void Start()
        {
            Task.Run(() =>
            {
                foreach ((var nCode, var wParam, var lParam) in _eventQueue.GetConsumingEnumerable())
                {
                    HandleEvent(nCode, wParam, lParam);
                }
            });
        }

        private int MouseHookProc(int nCode, int wParam, IntPtr lParam)
        {
            _eventQueue.Add((nCode, wParam, lParam));
            return CallNextHookEx(_mouseHookHandle, nCode, wParam, lParam);
        }

        private void HandleEvent(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                var mouseHook = (MouseHook)Marshal.PtrToStructure(lParam, typeof(MouseHook));

                if (MouseMoveBackend != null && mouseHook.PhysicalPoint.X != -1 && mouseHook.PhysicalPoint.Y != -1 && (_oldX != mouseHook.PhysicalPoint.X || _oldY != mouseHook.PhysicalPoint.Y))
                {
                    _oldX = mouseHook.PhysicalPoint.X;
                    _oldY = mouseHook.PhysicalPoint.Y;

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
        }

        private void EnsureSubscribedToGlobalMouseEvents()
        {
            if (_mouseHookHandle == 0)
            {
                _mouseDelegate = MouseHookProc;
                _mouseHookHandle = SetWindowsHookEx(WhMouseLl, _mouseDelegate, IntPtr.Zero, 0);

                if (_mouseHookHandle == 0)
                {
                    var status = Marshal.GetLastWin32Error();
                    throw new Win32Exception(status);
                }
            }
        }

        private void TryUnsubscribeFromGlobalMouseEvents()
        {
            if (MouseMoveBackend == null && MouseClickBackend == null)
            {
                ForceUnsunscribeFromGlobalMouseEvents();
            }
        }

        private void ForceUnsunscribeFromGlobalMouseEvents()
        {
            if (_mouseHookHandle != 0)
            {
                var result = UnhookWindowsHookEx(_mouseHookHandle);

                _mouseHookHandle = 0;
                _mouseDelegate = null;

                if (result == 0)
                {
                    var status = Marshal.GetLastWin32Error();
                    throw new Win32Exception(status);
                }
            }
        }
    }
}