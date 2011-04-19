using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace FireflyGL
{
	public struct Rectangle
	{
		public float Height;
		public float Width;
		public float X, Y;

		public Rectangle(float X, float Y, float Width, float Height)
		{
			this.X = X;
			this.Y = Y;
			this.Width = Width;
			this.Height = Height;
		}
	}

	public class Utility
	{
		private static readonly Stopwatch timer = new Stopwatch();
		private static readonly Random rand = new Random();

		public static double GetRandom()
		{
			return rand.NextDouble();
		}

		public static float GetRandomF()
		{
			return (float) rand.NextDouble();
		}

		public static string LoadTextFromFile(string Path)
		{
			using (var Stream = new StreamReader(Path))
			{
				string ret = Stream.ReadToEnd();
				return ret;
			}
		}

		public static void StartTimer()
		{
			timer.Restart();
		}

		public static double StopTimer()
		{
			timer.Stop();
			return timer.ElapsedTicks/(double) Stopwatch.Frequency*1000;
		}

		public static bool RectanglePointIntersection(Rectangle Rect, Vector2 Point)
		{
			if (Point.X > Rect.X && Point.X < Rect.X + Rect.Width && Point.Y > Rect.Y && Point.Y < Rect.Y + Rect.Height)
			{
				return true;
			}
			return false;
		}

		public static Bitmap MakeTextBitmap(string String, Font Font, Brush Brush)
		{
			var lines = new List<string>();
			var buffer = new StringBuilder();
			int maxLength = 0;
			int i = 0;
			while (i < String.Length)
			{
				if (String[i] != '\n')
				{
					buffer.Append(String[i]);
				}
				else
				{
					lines.Add(buffer.ToString());
					if (buffer.Length > maxLength)
					{
						maxLength = buffer.Length;
					}
					buffer.Clear();
				}
				++i;
			}
			lines.Add(buffer.ToString());
			if (buffer.Length > maxLength)
			{
				maxLength = buffer.Length;
			}
			buffer.Clear();
			var toReturn = new Bitmap((int) (maxLength*Font.Size), (int) (lines.Count*Font.Size*2));
			Graphics gfx = Graphics.FromImage(toReturn);

			gfx.DrawString(String, Font, Brush, new PointF(0, 0));

			gfx.Dispose();

			return toReturn;
		}

		public static void ProcessOGLErrors(bool Process = false)
		{
			if (!Process) return;
			ErrorCode errCode = GL.GetError();
			if (errCode != ErrorCode.NoError)
			{
				throw new Exception(errCode.ToString());
			}
		}
	}
}