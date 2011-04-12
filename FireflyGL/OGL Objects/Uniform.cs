using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace FireflyGL {

	class Uniform : IShaderObject {

		private string identifier;
		public string Identifier {
			get { return identifier; }
			set { identifier = value; }
		}

		private int location;
		public int Location {
			get { return location; }
			set { location = value; }
		}

		public Uniform ( string Identifier, int Location ) {

			identifier = Identifier;
			location = Location;
		}

		public void LoadMatrix ( Matrix4 Matrix ) {

			GL.UniformMatrix4( location, false, ref Matrix );
		}

		public void LoadVector2 ( Vector2 Vector ) {

			GL.Uniform2( location, ref Vector );
		}

		public void LoadVector3 ( Vector3 Vector ) {

			GL.Uniform3( location, ref Vector );
		}

		public void LoadInt ( int Number ) {

			GL.Uniform1( location, Number );
		}

		public void LoadFloat ( float Number ) {

			GL.Uniform1( location, Number );
		}

		public void LoadTexture ( Texture Texture ) {

			Texture.Bind();
			GL.Uniform1( location, 0 );
		}
	}
}
