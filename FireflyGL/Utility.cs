using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using OpenTK;

namespace FireflyGL {

	struct Rectangle {

		public float X, Y, Width, Height;

		public Rectangle ( float X, float Y, float Width, float Height ) {

			this.X = X;
			this.Y = Y;
			this.Width = Width;
			this.Height = Height;
		}
	}

	class Utility {

		static Stopwatch timer = new Stopwatch();
		static Random rand = new Random();

		public static double GetRandom () {

			return rand.NextDouble();
		}

		public static float GetRandomF () {

			return (float)rand.NextDouble();
		}

		public static string LoadTextFromFile ( string Path ) {

			using ( StreamReader Stream = new StreamReader( Path ) ) {

				string ret = Stream.ReadToEnd();
				return ret;
			}
		}

		public static void StartTimer () {

			timer.Restart();
		}

		public static double StopTimer () {

			timer.Stop();
			return timer.ElapsedTicks / (double)Stopwatch.Frequency * 1000;
		}

		public static bool RectanglePointIntersection ( Rectangle Rect, Vector2 Point ) {

			if ( Point.X > Rect.X && Point.X < Rect.X + Rect.Width && Point.Y > Rect.Y && Point.Y < Rect.Y + Rect.Height ) {

				return true;
			}
			return false;
		}

		public static Bitmap MakeTextBitmap ( string String, Font Font, Brush Brush ) {

			List<string> lines = new List<string>();
			StringBuilder buffer = new StringBuilder();
			int maxLength = 0;
			int i = 0;
			while ( i < String.Length ) {

				if ( String[ i ] != '\n' ) {

					buffer.Append( String[ i ] );
				} else {

					lines.Add( buffer.ToString() );
					if ( buffer.Length > maxLength ) {

						maxLength = buffer.Length;
					}
					buffer.Clear();
				}
				++i;
			}
			lines.Add( buffer.ToString() );
			if ( buffer.Length > maxLength ) {

				maxLength = buffer.Length;
			}
			buffer.Clear();
			Bitmap toReturn = new Bitmap( (int)( maxLength * Font.Size ), (int)( lines.Count * Font.Size * 2 ) );
			Graphics gfx = Graphics.FromImage( toReturn );

			gfx.DrawString( String, Font, Brush, new PointF( 0, 0 ) );

			gfx.Dispose();

			return toReturn;
		}

		public static void ProcessOGLErrors ( bool Process = false ) {

			if ( !Process ) return;
			OpenTK.Graphics.OpenGL.ErrorCode errCode = OpenTK.Graphics.OpenGL.GL.GetError();
			if ( errCode != OpenTK.Graphics.OpenGL.ErrorCode.NoError ) {
				throw new Exception( errCode.ToString() );
			}
		}
	}
}
