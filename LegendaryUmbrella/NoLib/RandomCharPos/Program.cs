using System;

namespace LegendaryUmbrella.NoLib.RandomCharPos
{
	class Program
	{
		public static void Main()
		{
			Console.BufferWidth = Console.WindowWidth;
			Console.BufferHeight = Console.WindowHeight;
			Random r = new Random();
			while (true)
			{
				Console.CursorLeft = r.Next(0, Console.WindowWidth);
				Console.CursorTop = r.Next(0, Console.WindowHeight);
				System.Threading.Thread.Sleep(250);
			}
		}
	}
}
