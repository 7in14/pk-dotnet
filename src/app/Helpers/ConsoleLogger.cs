using System.Drawing;
using Colorful;

namespace pkdotnet
{
	public class ConsoleLogger
	{
		public static void Ascii(string text)
		{
			Console.WriteAscii(text, Color.MediumSlateBlue);
		}
	}
}
