namespace SlightPenLighter.Models
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct MouseHook
    {
        public PhysicalPoint PhysicalPoint;
    }
}