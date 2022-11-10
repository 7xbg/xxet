using Internal;
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace xxet {
    class xtm {
        public static xxet.publics pubdata;

        static void Main(string[] args) {
            if(!Directory.Exists("plugins")) {
                Directory.CreateDirectory("plugins");
            }

            if(!File.Exists("plugins/load.txt")) {
                var fs = File.Create("plugins/load.txt");
                fs.Close();
            }

            if(args != null) {
                if(args.Length > 0) {
                    if(args[0] == "-log"){
                        publics.dbg = true;
                    }
                    else if(args[0] == "-help") {
                        _printhelp();
                        return;
                    }
                    else {
                        Console.WriteLine($"Invalid command line argument, \"{args[0]}\", type -help for help");
                        return;
                    }
                }
            }

            List<string> plugins = new List<string>();
            foreach(string s in File.ReadAllLines("plugins/load.txt")) {
                plugins.Add(s);
            }

            Console.Clear();
            writer.WriteColoredText("Press", ConsoleColor.White);
            writer.WriteColoredText(" esc ", ConsoleColor.Red);
            writer.WriteLineColoredText("To stop", ConsoleColor.White);
            
            pubdata = new publics(plugins);

            int v = (int)pubdata.GetVariable("inc");
            pubdata.EditVariable("inc", v, 0);
            //_printascii();

            while(true) {
                ConsoleKeyInfo cki = Console.ReadKey(true);
                if(cki.Key == ConsoleKey.Enter) {
                    v = (int)pubdata.GetVariable("inc");
                    if(publics.dbg) {
                        Console.WriteLine();
                    }
                    else {
                        writer.WriteLineColoredText(v.ToString(), ConsoleColor.White);
                    }

                    pubdata.EditVariable("inc", v, 1);
                    //Console.ReadLine();
                }
                else if(cki.Key == ConsoleKey.Escape) {
                    break;
                }
            }
        }

        private static void _printhelp() {
            Console.Clear();

            //use help
            Console.WriteLine("How to use plugins: ");
            Console.WriteLine("To name a plugin, type \"~\" and put the name in front. Example: ~test plugin");
            Console.WriteLine("NOTE: you must declate the plugin name before anything else (besides comments)");
            Console.WriteLine("To make a comment just put a \".\" before the line. Example: \". this is a comment\"");
            Console.WriteLine("To modify the value the numbers increased by, you need to modify the oninc function.");
            Console.WriteLine("To do that you enter into a plugin file \"modify oninc\", then create a new line.");
            Console.WriteLine("There is currently only 1 variable that can be modified when modifying oninc, which is inc.");
            Console.WriteLine("To edit that, write inc = {increase amount}. Example: \"inc = 2\".");
            Console.WriteLine("The example above would make the program increase your number by 2 instead of 1.");
            Console.WriteLine("To end the modify portion make a new line that's just \";\". Plugin example:\n");
            Console.WriteLine(". this program makes the number increase by 3 if the total is greater than 25\n");
            Console.WriteLine("~test plugin\nmodify oninc\ninc = 3 if [total > 25]\n;\n");

            //if statement help
            Console.WriteLine("If statements can be used after a variable assignment. Example: inc = 2 if [total > 50]");
            Console.WriteLine("What that did was when the code runs if the total (number) is > 50, the total will increase by 2.");
            Console.WriteLine("Your if conditions must be between \"[\" and \"]\", or the program wont work.");

            //loading plugin help
            Console.WriteLine("To load a plugin, you enter the name declared (after the \"~\"), in a file called load.txt");
            Console.WriteLine("in the plugins directory. If that file doesn't exist, the program will create it for you.");

            //command line help
            Console.WriteLine("Arguments for command line:");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("|  arg  |      Argument description:      |");
            Console.WriteLine("|-------|---------------------------------|");
            Console.WriteLine("| -help |         Print help menu         |");
            Console.WriteLine("| -log  |      Enables logging (ugly)     |");
            Console.WriteLine("-------------------------------------------\n");

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        private static void _printascii() {
            if(!File.Exists("res/ascii.txt")) return;

            foreach(string s in File.ReadAllLines("res/ascii.txt")) {
                Console.WriteLine(s);
            }

            Console.WriteLine();
        }
    }
}