using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace FireflyGL {

	abstract class Shape : DisplayObject {

		protected LinkedList<Polygon> filledPolygons;
		public LinkedList<Polygon> FilledPolygons {
			get { return filledPolygons; }
			set { filledPolygons = value; }
		}

		protected LinkedList<Polygon> outlinePolygons;
		public LinkedList<Polygon> OutlinePolygons {
			get { return outlinePolygons; }
			set { outlinePolygons = value; }
		}

		protected float[] fillArray;
		protected float[] outlineArray;

		protected Buffer fillBuffer;
		protected Buffer outlineBuffer;

		protected ShaderProgram program;
		public ShaderProgram Program {
			get { return program; }
			set { program = value; }
		}

		protected bool visible = true;
		public bool Visible {
			get { return visible; }
			set { visible = value; }
		}

		protected int floatsPerVertex = 0;

		public Shape ( string Path ) {

			filledPolygons = new LinkedList<Polygon>();
			outlinePolygons = new LinkedList<Polygon>();
			fillArray = new float[ 0 ];
			outlineArray = new float[ 0 ];
			fillBuffer = new Buffer();
			outlineBuffer = new Buffer();
		}

		public Shape () {

			filledPolygons = new LinkedList<Polygon>();
			outlinePolygons = new LinkedList<Polygon>();
			fillArray = new float[ 0 ];
			outlineArray = new float[ 0 ];
			fillBuffer = new Buffer();
			outlineBuffer = new Buffer();
		}

		public virtual void SetPolygons () {

		}

		public void GenerateBuffers () {

			fillBuffer.SetDataFloat( BufferTarget.ArrayBuffer, BufferUsageHint.StaticDraw, fillArray );
			outlineBuffer.SetDataFloat( BufferTarget.ArrayBuffer, BufferUsageHint.StaticDraw, outlineArray );
		}

		public void LoadFromFile ( string Path ) {

			using ( StreamReader stream = new StreamReader( Path ) ) {
				byte[] length = new byte[ 4 ];
				length[ 0 ] = (byte)stream.Read(); length[ 1 ] = (byte)stream.Read(); length[ 2 ] = (byte)stream.Read(); length[ 3 ] = (byte)stream.Read(); //Read first 4 bytes
				int intLength = BitConverter.ToInt32( length, 0 ); //Convert them to int
				fillArray = new float[ intLength ]; //Resize the array to fir vertices
				for ( int i = 0 ; i < intLength * 4 ; i += 4 ) { //Use the int as length
					byte[] readBytes = new byte[ 4 ];

					readBytes[ 0 ] = (byte)stream.Read();
					readBytes[ 1 ] = (byte)stream.Read();
					readBytes[ 2 ] = (byte)stream.Read();
					readBytes[ 3 ] = (byte)stream.Read(); //Read next 4 bytes

					fillArray[ i / 4 ] = BitConverter.ToSingle( readBytes, 0 ); //Fill the array with the floats
				}

				length[ 0 ] = (byte)stream.Read(); length[ 1 ] = (byte)stream.Read(); length[ 2 ] = (byte)stream.Read(); length[ 3 ] = (byte)stream.Read(); //Read first 4 bytes
				intLength = BitConverter.ToInt32( length, 0 ); //Convert them to int
				outlineArray = new float[ intLength ]; //Resize the array to fir vertices
				for ( int i = 0 ; i < intLength * 4 ; i += 4 ) { //Use the int as length
					byte[] readBytes = new byte[ 4 ];

					readBytes[ 0 ] = (byte)stream.Read();
					readBytes[ 1 ] = (byte)stream.Read();
					readBytes[ 2 ] = (byte)stream.Read();
					readBytes[ 3 ] = (byte)stream.Read(); //Read next 4 bytes

					outlineArray[ i / 4 ] = BitConverter.ToSingle( readBytes, 0 ); //Fill the array with the floats
				}
			}

			GenerateBuffers();
		}

		public void SaveToFile ( string Path ) {

			using ( StreamWriter stream = new StreamWriter( Path ) ) {
				byte[] length;
				length = BitConverter.GetBytes( fillArray.Length );
				for ( int i = 0 ; i < 4 ; ++i ) {
					stream.Write( (char)length[ i ] );
				}
				for ( int i = 0 ; i < fillArray.Length ; ++i ) {
					float current = fillArray[ i ];
					byte[] array = BitConverter.GetBytes( current );
					for ( int j = 0 ; j < 4 ; ++j ) {
						stream.Write( (char)array[ j ] );
					}
				}
				length = BitConverter.GetBytes( outlineArray.Length );
				for ( int i = 0 ; i < 4 ; ++i ) {
					stream.Write( (char)length[ i ] );
				}
				for ( int i = 0 ; i < outlineArray.Length ; ++i ) {
					float current = outlineArray[ i ];
					byte[] array = BitConverter.GetBytes( current );
					for ( int j = 0 ; j < 4 ; ++j ) {
						stream.Write( (char)array[ j ] );
					}
				}
			}
		}

		public override void Render () {
			if ( !visible ) return;
			base.Render();

			program.Use();
		}
	}
}
