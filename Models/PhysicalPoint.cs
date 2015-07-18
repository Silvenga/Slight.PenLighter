namespace SlightPenLighter.Models
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct PhysicalPoint
    {
        public readonly int X;

        public readonly int Y;
    }
}