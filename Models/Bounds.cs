using System.Runtime.InteropServices;

namespace SlightPenLighter.Models {

    [StructLayout(LayoutKind.Sequential)]
    public struct Bounds {

        // ReSharper disable UnassignedField.Global
        // ReSharper disable FieldCanBeMadeReadOnly.Global
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
        // ReSharper restore FieldCanBeMadeReadOnly.Global
        // ReSharper restore UnassignedField.Global
    }

}