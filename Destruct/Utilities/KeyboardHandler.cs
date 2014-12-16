using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destruct.Utilities
{
    public enum KeyCode : int
    {
        Space = 0x20,
        A = 0x41,
        D = 0x44,
        E = 0x45,
        F = 0x46,
        S = 0x53,
        W = 0x57,
        v0 = 0x30,
        v1 = 0x31,
        v2 = 0x32,

        LeftMouse = 0x01,

        Left = 0x25,

        Up,

        Right,

        Down,

        Shift = 0x10,

    }

    public static class NativeKeyboard
    {
        private const int KeyPressed = 0x8000;

        public static bool IsKeyDown(KeyCode key)
        {
            return (GetKeyState((int)key) & KeyPressed) != 0;
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern short GetKeyState(int key);
    }
}
