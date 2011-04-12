using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace FireflyGL {

	class Camera {

		private static Camera currentCamera;
		public static Camera CurrentCamera {
			get { return Camera.currentCamera; }
			set { Camera.currentCamera = value; }
		}

		private Matrix4 translationMatrix, rotationMatrix, offsetMatrix, scaleMatrix, finalMatrix;
		private bool requiresUpdate = true;

		float rotation, x, y, zoom = 1;
		public float Rotation {
			get { return rotation; }
			set {
				rotation = value;
				rotationMatrix = Matrix4.CreateRotationZ( rotation );
				requiresUpdate = true;
			}
		}
		public float X {
			get { return x; }
			set {
				x = value;
				requiresUpdate = true;
				translationMatrix.Row3.X = x;
			}
		}
		public float Y {
			get { return y; }
			set {
				y = value;
				requiresUpdate = true;
				translationMatrix.Row3.Y = y;
			}
		}
		public float Zoom {
			get { return zoom; }
			set {
				zoom = value;
				scaleMatrix.Row0.X = zoom;
				scaleMatrix.Row1.Y = zoom;
				requiresUpdate = true;
			}
		}
		public Matrix4 Matrix {
			get {
				if ( requiresUpdate )
					updateMatrices();
				return finalMatrix;
			}
		}

		public Camera () {

			finalMatrix = Matrix4.Identity;
			scaleMatrix = Matrix4.Scale( zoom, zoom, 1 );
			offsetMatrix = Matrix4.CreateTranslation(
				Firefly.Window.Width / 2,
				Firefly.Window.Height / 2,
				0 );
			rotationMatrix = Matrix4.CreateRotationZ( rotation );
			translationMatrix = Matrix4.CreateTranslation( x, y, 0 );
			X -= 400;
			Y -= 250;
		}

		void updateMatrices () {

			finalMatrix = translationMatrix * scaleMatrix * rotationMatrix * offsetMatrix;
			requiresUpdate = false;
		}

		public void Activate () {

			currentCamera = this;
		}

		public Vector2 GetApsoluteMouse ( int X, int Y ) {

			float x, y;
			x = X;
			y = Y;
			x -= Firefly.Window.Width / 2;
			y -= Firefly.Window.Height / 2;
			x /= zoom;
			y /= zoom;
			float angle= (float)Math.Atan2( y, x );
			angle -= rotation;
			float distance = (float)Math.Sqrt( x * x + y * y );
			x = (float)Math.Cos( angle ) * distance;
			y = (float)Math.Sin( angle ) * distance;
			x -= this.x;
			y -= this.y;

			return new Vector2( x, y );
		}
	}
}
