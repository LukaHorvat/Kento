using System;
using System.Diagnostics;

namespace Kento.Utility
{
	internal class Profiler
	{
		private static readonly Stopwatch timer = Stopwatch.StartNew();
		private static long elapsedTime;
		private static long lastStart;

		public static void StartTimer()
		{
			lastStart = timer.ElapsedTicks;
		}

		public static void StopTimer()
		{
			elapsedTime += timer.ElapsedTicks - lastStart;
			lastStart = timer.ElapsedTicks;
		}

		public static void OutputTime()
		{
			timer.Stop();
			Console.WriteLine("Total time (Utility.Profiler): " + elapsedTime/(double) Stopwatch.Frequency*1000);
		}
	}
}