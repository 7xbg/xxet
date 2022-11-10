using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace xxet
{
	/// <summary>
	/// Basically just a readable tuple, source: https://stackoverflow.com/questions/7787994/is-there-a-version-of-the-class-tuple-whose-items-properties-are-not-readonly-an
	/// </summary>
	/// <typeparam name="T1">First variable</typeparam>
	/// <typeparam name="T2">Second variable</typeparam>
	public class Pair<T1, T2>
	{
		public T1 First { get; set; }
		public T2 Second { get; set; }
	}
}