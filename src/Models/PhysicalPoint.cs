using System.Runtime.InteropServices;

namespace SlightPenLighter.Models
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PhysicalPoint
    {
        public readonly int X;

        public readonly int Y;
    }
}