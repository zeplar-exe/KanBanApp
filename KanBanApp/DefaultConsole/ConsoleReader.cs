﻿using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace KanBanApp.DefaultConsole;

// Work of the gods: https://stackoverflow.com/a/12366307/16324801

public class ConsoleReader
{
    public static IEnumerable<string> ReadFromBuffer(short x, short y, short width, short height)
    {
        var buffer = Marshal.AllocHGlobal(width * height * Marshal.SizeOf(typeof(CHAR_INFO)));
        
        if (buffer == null)
            throw new OutOfMemoryException();

        try
        {
            var coord = new COORD();
            var rc = new SMALL_RECT();
            rc.Left = x;
            rc.Top = y;
            rc.Right = (short)(x + width - 1);
            rc.Bottom = (short)(y + height - 1);

            var size = new COORD();
            size.X = width;
            size.Y = height;

            const int STD_OUTPUT_HANDLE = -11;
            if (!ReadConsoleOutput(GetStdHandle(STD_OUTPUT_HANDLE), buffer, size, coord, ref rc))
            {
                // 'Not enough storage is available to process this command' may be raised for buffer size > 64K (see ReadConsoleOutput doc.)
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            var ptr = buffer;
            for (var h = 0; h < height; h++)
            {
                var sb = new StringBuilder();
                for (var w = 0; w < width; w++)
                {
                    var ci = (CHAR_INFO)Marshal.PtrToStructure(ptr, typeof(CHAR_INFO));
                    var chars = Console.OutputEncoding.GetChars(ci.charData);
                    sb.Append(chars[0]);
                    ptr += Marshal.SizeOf(typeof(CHAR_INFO));
                }
                yield return sb.ToString();
            }
        }
        finally
        {
            Marshal.FreeHGlobal(buffer);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct CHAR_INFO
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] charData;
        public short attributes;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct COORD
    {
        public short X;
        public short Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct SMALL_RECT
    {
        public short Left;
        public short Top;
        public short Right;
        public short Bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct CONSOLE_SCREEN_BUFFER_INFO
    {
        public COORD dwSize;
        public COORD dwCursorPosition;
        public short wAttributes;
        public SMALL_RECT srWindow;
        public COORD dwMaximumWindowSize;
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool ReadConsoleOutput(IntPtr hConsoleOutput, IntPtr lpBuffer, COORD dwBufferSize, COORD dwBufferCoord, ref SMALL_RECT lpReadRegion);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetStdHandle(int nStdHandle);
}