using System;
using System.Runtime.InteropServices;

namespace BinanceOptionsApp
{
    public class MouseCapture
    {
        struct Point32
        {
            public int X;
            public int Y;
        }
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(ref Point32 lpPoint);

        public static System.Windows.Point GetMousePosition()
        {
            Point32 p = new Point32();
            GetCursorPos(ref p);
            return new System.Windows.Point(p.X, p.Y);
        }
    }
    public static class MouseImitator
    {

        [DllImport("user32.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void SetCursorPos(int x, int y);

        [DllImport("user32.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        static extern void mouse_event(uint dwFlags, int dx, int dy, int dwData, int dwExtraInfo);


        private const int MOUSEEVENTF_ABSOLUTE = 0x8000;
        private const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const int MOUSEEVENTF_LEFTUP = 0x0004;
        private const int MOUSEEVENTF_MOVE = 0x0001;

        public static void Move(int x, int y)
        {
            SetCursorPos(x, y);
        }
        public static void RelativeMove(int dx, int dy)
        {
            mouse_event(MOUSEEVENTF_MOVE, dx, dy, 0, 0);
        }
        public static void Click(int x, int y, bool doubleclick)
        {
            SetCursorPos(x, y);
            int n = doubleclick ? 2 : 1;
            for (int i = 0; i < n; i++)
            {
                mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
                mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0);
            }
        }
    }
}
