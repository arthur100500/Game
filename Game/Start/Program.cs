using System;
using System.IO;

namespace Platformer
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			/*
			var game = new Game();
			game.Run();
			*/
			
			try
			{
				var game = new Game();
				game.Run();
			}
			catch (Exception ex)
			{
				File.WriteAllText("Error.txt", ex.ToString());
				Console.WriteLine(ex.ToString());
				Console.ReadKey();
			}
			
		}
	}
}