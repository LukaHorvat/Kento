using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace FireflyGL {

	public class ShaderProgram {

		private int id;
		public int Id {
			get { return id; }
			set { id = value; }
		}

		private LinkedList<Shader> shaders;
		public LinkedList<Shader> Shaders {
			get { return shaders; }
			set { shaders = value; }
		}

		private Dictionary<string, IShaderObject> locations;
		public Dictionary<string, IShaderObject> Locations {
			get { return locations; }
			set { locations = value; }
		}

		public ShaderProgram () {

			id = GL.CreateProgram();
			shaders = new LinkedList<Shader>();
		}

		public ShaderProgram ( VertexShader VertexShader, FragmentShader FragmentShader ) {

			id = GL.CreateProgram();
			shaders = new LinkedList<Shader>();
			locations = new Dictionary<string, IShaderObject>();
			AttachShader( VertexShader );
			AttachShader( FragmentShader );
		}

		public void AttachShader ( Shader Shader ) {

			GL.AttachShader( id, Shader.Id );
			shaders.AddLast( Shader );
			Utility.ProcessOGLErrors();
		}

		public void AddUniformLocation ( string Uniform ) {

			if ( !locations.ContainsKey( Uniform ) ) {
				locations.Add( Uniform, new Uniform( Uniform, GL.GetUniformLocation( id, Uniform ) ) );
				Utility.ProcessOGLErrors();
			}
		}

		public void AddAttribLocation ( string Attribute ) {

			if ( !locations.ContainsKey( Attribute ) ) {
				locations.Add( Attribute, new Attribute( Attribute, GL.GetAttribLocation( id, Attribute ) ) );
				Utility.ProcessOGLErrors();
			}
		}

		public void Link () {

			GL.LinkProgram( id );
			Utility.ProcessOGLErrors();
		}

		public void Use () {

			GL.UseProgram( id );
		}
	}
}
