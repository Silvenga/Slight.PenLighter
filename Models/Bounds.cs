namespace SlightPenLighter.Models
{
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Global")]
    public struct Bounds
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
}