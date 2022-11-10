using Internal;
using System;

namespace xxet {
    class writer {
        /// <summary>
        /// Writes a line of text with color
        /// </summary>
        /// <param name="s">String to print</param>
        /// <param name="cc">Color of string</param>
        public static void WriteLineColoredText(string s, ConsoleColor cc) {
            Console.ForegroundColor = cc;
            Console.WriteLine(s);
            Console.ResetColor();
        }
        
        /// <summary>
        /// Writes text with color
        /// </summary>
        /// <param name="s">String to print</param>
        /// <param name="cc">Color of string</param>
        public static void WriteColoredText(string s, ConsoleColor cc) {
            Console.ForegroundColor = cc;
            Console.Write(s);
            Console.ResetColor();
        }
    }
}