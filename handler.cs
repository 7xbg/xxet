using System.ComponentModel;
using System.Reflection.Emit;
using Internal;
using System;

namespace xxet {
	class handler {
		/// <summary>
		/// Handles variable editing with plugins involved
		/// </summary>
		/// <param name="v">Value of current number</param>
		/// <param name="s">Line of code from plugin being ran</param>
		/// <returns>Returns the total with plugins run on it</returns>
		public static int HandleVariableEdit(int v, string s) {
			if(s.Split(' ').Length < 2) {
				Console.WriteLine($"\nError modifying oninc: No assign set (=), from: {s}, returning {v}");
				return v;
			}
			else if(s.Split(' ').Length < 3) {
				Console.WriteLine($"\nError modifying oninc: No value put for set, from: {s}, returning {v}");
				return v;
			}

			if(s.Split(' ').Length > 3 && s.Split(' ')[3] == "if") {
				string cfs; // compare full statement
				try {
					cfs = s.Split('[')[1].Split(']')[0];
				}
				catch {
					Console.WriteLine($"Error modifying oninc: if statement isnt contained (isnt between \"[\" and \"]\"), from {s}, returning {v}");
					return v;
				}

				bool ires = HandleIf(v, cfs);
				if(ires) {
					if(publics.dbg)
						writer.WriteLineColoredText($"Returning {int.Parse(s.Split(' ')[2])}\n", ConsoleColor.White);

					return int.Parse(s.Split(' ')[2]);
				}
				else {
					if(publics.dbg)
						writer.WriteLineColoredText($"Returning -1 (gets old inc value)\n", ConsoleColor.White);

					return -1;
				}
			}
			
			if(publics.dbg)
				Console.WriteLine($"returning {v}");

			return v;
		}

		/// <summary>
		/// Handles if when setting variable
		/// </summary>
		/// <param name="v">Value of variable</param>
		/// <param name="s">If codition being ran</param>
		/// <returns></returns>
		public static bool HandleIf(int v, string s) {
			if(publics.dbg) {
				writer.WriteLineColoredText(" * IF statement *: ", ConsoleColor.Magenta);
				writer.WriteColoredText("s: ", ConsoleColor.White);
				writer.WriteLineColoredText($"{s}", ConsoleColor.Gray);
			}

			if(s.Split(' ').Length < 2) {
				Console.WriteLine($"\nError modifying oninc: No assign set (>, <, =, etc), from: {s} in if, returning false");
				return false;
			}
			else if(s.Split(' ').Length < 3) {
				Console.WriteLine($"\nError modifying oninc: No value put for comparison, from: {s} in if, returning false");
				return false;
			}

			if(s.Split(' ')[0] != "total") {
				Console.WriteLine($"\nError modifying oninc: Invalid value put for comparison (use -help cmd args for help), from: {s} in if, returning false");
				return false;
			}

			string mod = s.Split(' ')[1];
			int cmp = int.Parse(s.Split(' ')[2]);

			if(publics.dbg) {
				writer.WriteColoredText("v: ", ConsoleColor.White);
				writer.WriteColoredText($"{v}, ", ConsoleColor.Gray);
				writer.WriteColoredText("mod: ", ConsoleColor.White);
				writer.WriteColoredText($"{mod}, ", ConsoleColor.Gray);
				writer.WriteColoredText("cmp: ", ConsoleColor.White);
				writer.WriteLineColoredText($"{cmp}", ConsoleColor.Gray);
			}

			if(_istrue(v, cmp, mod, s)) {
				if(publics.dbg) {
					writer.WriteColoredText(" * ", ConsoleColor.White);
					writer.WriteColoredText($"{v}", ConsoleColor.Gray);
					writer.WriteColoredText(" is ", ConsoleColor.Green);
					writer.WriteLineColoredText($"{mod} {cmp}", ConsoleColor.Gray);
				}
					

				return true;
			}
			else {
				if(publics.dbg) {
					writer.WriteColoredText(" * ", ConsoleColor.White);
					writer.WriteColoredText($"{v}", ConsoleColor.Gray);
					writer.WriteColoredText(" isnt ", ConsoleColor.Red);
					writer.WriteLineColoredText($"{mod} {cmp}", ConsoleColor.Gray);
				}

				return false;
			}
		}

		/// <summary>
		/// Checks if an if statement is true or false
		/// </summary>
		/// <param name="v">Number to be compared</param>
		/// <param name="cmp">Number to compare to</param>
		/// <param name="mod">(<, >, =, etc...)</param>
		/// <param name="s">If statement code</param>
		/// <returns>Returns if if statement is true or false</returns>
		private static bool _istrue(int v, int cmp, string mod, string s) {
			if(mod == ">") {
				if(v > cmp) 
					return true;
				else 
					return false;
			}
			else if(mod == "<") {
				if(v < cmp) 
					return true;
				else
					return false;
			}       
			else if(mod == "==") {
				if(v == cmp)
					return true;
				else
					return false;
			}
			else if(mod == ">=")
				if(v >= cmp) 
					return true;
				else 
					return false;
			else if(mod == "<=") {
				if(v <= cmp) 
					return true;
				else
					return false;
			}
			else {
				Console.WriteLine($"Error modifying oninc: Invalid assign (>, <, =, etc), from {s} in if, returning false");
				return false;
			}
		}
	}
}