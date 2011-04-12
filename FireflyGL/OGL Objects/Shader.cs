using OpenTK.Graphics.OpenGL;

namespace FireflyGL
{
	public enum ShaderType
	{
		Vertex = 0,
		Fragment
	}

	public abstract class Shader
	{
		private int id;
		private OpenTK.Graphics.OpenGL.ShaderType type;

		public Shader(ShaderType Type)
		{
			this.Type = Type;
			id = GL.CreateShader(type);
		}

		public ShaderType Type
		{
			get
			{
				if (type == OpenTK.Graphics.OpenGL.ShaderType.FragmentShader)
				{
					return ShaderType.Fragment;
				}
				else
				{
					return ShaderType.Vertex;
				}
			}
			set
			{
				if (value == ShaderType.Fragment)
				{
					type = OpenTK.Graphics.OpenGL.ShaderType.FragmentShader;
				}
				else
				{
					type = OpenTK.Graphics.OpenGL.ShaderType.VertexShader;
				}
			}
		}

		public int Id
		{
			get { return id; }
			set { id = value; }
		}

		public void LoadFromFile(string Path)
		{
			LoadFromSource(Utility.LoadTextFromFile(Path));
		}

		public void LoadFromSource(string Source)
		{
			GL.ShaderSource(id, Source);
			GL.CompileShader(id);
			Utility.ProcessOGLErrors();
		}
	}
}