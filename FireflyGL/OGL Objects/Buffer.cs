using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace FireflyGL {

	class Buffer {

		public int ID;
		public int Length;

		public Buffer () {

			GL.GenBuffers( 1, out ID );
		}

		public void Bind ( BufferTarget Target) {

			GL.BindBuffer( Target, ID );
		}

		public void SetDataFloat ( BufferTarget Target, BufferUsageHint Hint, float[] Data ) {

			GL.BindBuffer( Target, ID );
			GL.BufferData( Target, (IntPtr)( Data.Length * sizeof( float ) ), Data, Hint );
			Length = Data.Length;
		}

		public void SetDataUint ( BufferTarget Target, BufferUsageHint Hint, uint[] Data ) {

			GL.BindBuffer( Target, ID );
			GL.BufferData( Target, (IntPtr)( Data.Length * sizeof( uint ) ), Data, Hint );
			Length = Data.Length;
		}
	}
}
