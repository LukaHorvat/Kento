using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace FireflyGL
{
	internal class Uniform : IShaderObject
	{
		private int location;

		public Uniform(string Identifier, int Location)
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

		public void LoadMatrix(Matrix4 Matrix)
		{
			GL.UniformMatrix4(location, false, ref Matrix);
		}

		public void LoadVector2(Vector2 Vector)
		{
			GL.Uniform2(location, ref Vector);
		}

		public void LoadVector3(Vector3 Vector)
		{
			GL.Uniform3(location, ref Vector);
		}

		public void LoadInt(int Number)
		{
			GL.Uniform1(location, Number);
		}

		public void LoadFloat(float Number)
		{
			GL.Uniform1(location, Number);
		}

		public void LoadTexture(Texture Texture)
		{
			Texture.Bind();
			GL.Uniform1(location, 0);
		}
	}
}