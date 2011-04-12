using OpenTK.Graphics.OpenGL;

namespace FireflyGL
{
	internal class Attribute : IShaderObject
	{
		private int location;

		public Attribute(string Identifier, int Location)
		{
			this.Identifier = Identifier;
			location = Location;
		}

		public string Identifier { get; set; }

		#region IShaderObject Members

		public int Location
		{
			get { return location; }
			set { location = value; }
		}

		#endregion

		public void AttributePointerFloat(int Size, int Stride, int Offset)
		{
			GL.VertexAttribPointer(location, Size, VertexAttribPointerType.Float, false, Stride*sizeof (float),
			                       Offset*sizeof (float));
		}

		public void AttributePointerInt(int Size, int Stride, int Offset)
		{
			GL.VertexAttribPointer(location, Size, VertexAttribPointerType.Int, false, Stride*sizeof (int), Offset*sizeof (int));
		}
	}
}