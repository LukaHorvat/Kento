using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace FireflyGL {

	public enum ShaderType : int {

		Vertex = 0,
		Fragment
	}

	public abstract class Shader {

		private OpenTK.Graphics.OpenGL.ShaderType type;
		public ShaderType Type {
			get {
				if ( type == OpenTK.Graphics.OpenGL.ShaderType.FragmentShader ) {
					return ShaderType.Fragment;
				} else {
					return ShaderType.Vertex;
				}
			}
			set {
				if ( value == ShaderType.Fragment ) {
					type = OpenTK.Graphics.OpenGL.ShaderType.FragmentShader;
				} else {
					type = OpenTK.Graphics.OpenGL.ShaderType.VertexShader;
				}
			}
		}

		private int id;
		public int Id {
			get { return id; }
			set { id = value; }
		}

		public Shader ( ShaderType Type ) {

			this.Type = Type;
			id = GL.CreateShader( type );
		}

		public void LoadFromFile ( string Path ) {

			LoadFromSource( Utility.LoadTextFromFile( Path ) );
		}

		public void LoadFromSource ( string Source ) {

			GL.ShaderSource( id, Source );
			GL.CompileShader( id );
			Utility.ProcessOGLErrors();
		}
	}
}
