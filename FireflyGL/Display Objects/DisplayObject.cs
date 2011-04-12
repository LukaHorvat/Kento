using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace FireflyGL {

	class DisplayObject : IRenderable {

		protected Matrix4 scaleMatrix, rotationMatrix, translationMatrix, modelMatrix; //Camera and window matrices are stored in separate classes because they are common to all display objects
		
		float scaleX = 1, scaleY = 1, x, y, rotation;
		bool requiresUpdate = true;

		public float ScaleX {
			get { return scaleX; }
			set { requiresUpdate = true; scaleX = value; }
		}

		public float ScaleY {
			get { return scaleY; }
			set { requiresUpdate = true; scaleY = value; }
		}

		public float X {
			get { return x; }
			set { requiresUpdate = true; x = value; }
		}

		public float Y {
			get { return y; }
			set { requiresUpdate = true; y = value; }
		}

		public float Rotation {
			get { return rotation; }
			set { requiresUpdate = true; rotation = value; }
		}

		void updateMatrices () {

			scaleMatrix = Matrix4.Scale( scaleX, scaleY, 1 ); //0,0 = scaleX, 1,1 = scaleY
			rotationMatrix = Matrix4.CreateRotationZ( rotation );
			translationMatrix = Matrix4.CreateTranslation( x, y, 0 ); //0,3 = x, 1,3 = y

			modelMatrix = scaleMatrix * rotationMatrix * translationMatrix;
			requiresUpdate = false;
		}

		public virtual void Render () {

			if ( requiresUpdate ) updateMatrices();
		}
	}
}
