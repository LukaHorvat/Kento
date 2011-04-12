using System;
using System.Collections.Generic;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace FireflyGL
{
	internal abstract class Shape : DisplayObject
	{
		protected float[] fillArray;

		protected Buffer fillBuffer;
		protected LinkedList<Polygon> filledPolygons;
		protected int floatsPerVertex;
		protected float[] outlineArray;
		protected Buffer outlineBuffer;
		protected LinkedList<Polygon> outlinePolygons;

		protected ShaderProgram program;

		protected bool visible = true;

		public Shape(string Path)
		{
			filledPolygons = new LinkedList<Polygon>();
			outlinePolygons = new LinkedList<Polygon>();
			fillArray = new float[0];
			outlineArray = new float[0];
			fillBuffer = new Buffer();
			outlineBuffer = new Buffer();
		}

		public Shape()
		{
			filledPolygons = new LinkedList<Polygon>();
			outlinePolygons = new LinkedList<Polygon>();
			fillArray = new float[0];
			outlineArray = new float[0];
			fillBuffer = new Buffer();
			outlineBuffer = new Buffer();
		}

		public LinkedList<Polygon> FilledPolygons
		{
			get { return filledPolygons; }
			set { filledPolygons = value; }
		}

		public LinkedList<Polygon> OutlinePolygons
		{
			get { return outlinePolygons; }
			set { outlinePolygons = value; }
		}

		public ShaderProgram Program
		{
			get { return program; }
			set { program = value; }
		}

		public bool Visible
		{
			get { return visible; }
			set { visible = value; }
		}

		public virtual void SetPolygons() {}

		public void GenerateBuffers()
		{
			fillBuffer.SetDataFloat(BufferTarget.ArrayBuffer, BufferUsageHint.StaticDraw, fillArray);
			outlineBuffer.SetDataFloat(BufferTarget.ArrayBuffer, BufferUsageHint.StaticDraw, outlineArray);
		}

		public void LoadFromFile(string Path)
		{
			using (var stream = new StreamReader(Path))
			{
				var length = new byte[4];
				length[0] = (byte) stream.Read();
				length[1] = (byte) stream.Read();
				length[2] = (byte) stream.Read();
				length[3] = (byte) stream.Read(); //Read first 4 bytes
				int intLength = BitConverter.ToInt32(length, 0); //Convert them to int
				fillArray = new float[intLength]; //Resize the array to fir vertices
				for (int i = 0; i < intLength*4; i += 4)
				{
					//Use the int as length
					var readBytes = new byte[4];

					readBytes[0] = (byte) stream.Read();
					readBytes[1] = (byte) stream.Read();
					readBytes[2] = (byte) stream.Read();
					readBytes[3] = (byte) stream.Read(); //Read next 4 bytes

					fillArray[i/4] = BitConverter.ToSingle(readBytes, 0); //Fill the array with the floats
				}

				length[0] = (byte) stream.Read();
				length[1] = (byte) stream.Read();
				length[2] = (byte) stream.Read();
				length[3] = (byte) stream.Read(); //Read first 4 bytes
				intLength = BitConverter.ToInt32(length, 0); //Convert them to int
				outlineArray = new float[intLength]; //Resize the array to fir vertices
				for (int i = 0; i < intLength*4; i += 4)
				{
					//Use the int as length
					var readBytes = new byte[4];

					readBytes[0] = (byte) stream.Read();
					readBytes[1] = (byte) stream.Read();
					readBytes[2] = (byte) stream.Read();
					readBytes[3] = (byte) stream.Read(); //Read next 4 bytes

					outlineArray[i/4] = BitConverter.ToSingle(readBytes, 0); //Fill the array with the floats
				}
			}

			GenerateBuffers();
		}

		public void SaveToFile(string Path)
		{
			using (var stream = new StreamWriter(Path))
			{
				byte[] length;
				length = BitConverter.GetBytes(fillArray.Length);
				for (int i = 0; i < 4; ++i)
				{
					stream.Write((char) length[i]);
				}
				for (int i = 0; i < fillArray.Length; ++i)
				{
					float current = fillArray[i];
					byte[] array = BitConverter.GetBytes(current);
					for (int j = 0; j < 4; ++j)
					{
						stream.Write((char) array[j]);
					}
				}
				length = BitConverter.GetBytes(outlineArray.Length);
				for (int i = 0; i < 4; ++i)
				{
					stream.Write((char) length[i]);
				}
				for (int i = 0; i < outlineArray.Length; ++i)
				{
					float current = outlineArray[i];
					byte[] array = BitConverter.GetBytes(current);
					for (int j = 0; j < 4; ++j)
					{
						stream.Write((char) array[j]);
					}
				}
			}
		}

		public override void Render()
		{
			if (!visible) return;
			base.Render();

			program.Use();
		}
	}
}