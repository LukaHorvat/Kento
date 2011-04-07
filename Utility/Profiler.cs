using System.Diagnostics;
using System;

namespace Kento.Utility
{
	class Profiler
	{
		static readonly Stopwatch timer = Stopwatch.StartNew();
		static long elapsedTime;
		static long lastStart;
		public static void StartTimer ()
		{
			lastStart = timer.ElapsedTicks;
		}
		public static void StopTimer ()
		{
			elapsedTime += timer.ElapsedTicks - lastStart;
			lastStart = timer.ElapsedTicks;
		}
		public static void OutputTime ()
		{
			timer.Stop();
			Console.WriteLine( "Total time (Utility.Profiler): " + elapsedTime / (double)Stopwatch.Frequency * 1000 );
		}
	}
}
