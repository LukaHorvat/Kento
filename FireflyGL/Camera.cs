using System;
using OpenTK;

namespace FireflyGL
{
	internal class Camera
	{
		private static Camera currentCamera;

		private readonly Matrix4 offsetMatrix;
		private Matrix4 finalMatrix;
		private bool requiresUpdate = true;

		private float rotation;
		private Matrix4 rotationMatrix;
		private Matrix4 scaleMatrix;
		private Matrix4 translationMatrix;
		private float x, y, zoom = 1;

		public Camera()
		{
			finalMatrix = Matrix4.Identity;
			scaleMatrix = Matrix4.Scale(zoom, zoom, 1);
			offsetMatrix = Matrix4.CreateTranslation(
				Firefly.Window.Width/2,
				Firefly.Window.Height/2,
				0);
			rotationMatrix = Matrix4.CreateRotationZ(rotation);
			translationMatrix = Matrix4.CreateTranslation(x, y, 0);
			X -= 400;
			Y -= 250;
		}

		public static Camera CurrentCamera
		{
			get { return currentCamera; }
			set { currentCamera = value; }
		}

		public float Rotation
		{
			get { return rotation; }
			set
			{
				rotation = value;
				rotationMatrix = Matrix4.CreateRotationZ(rotation);
				requiresUpdate = true;
			}
		}

		public float X
		{
			get { return x; }
			set
			{
				x = value;
				requiresUpdate = true;
				translationMatrix.Row3.X = x;
			}
		}

		public float Y
		{
			get { return y; }
			set
			{
				y = value;
				requiresUpdate = true;
				translationMatrix.Row3.Y = y;
			}
		}

		public float Zoom
		{
			get { return zoom; }
			set
			{
				zoom = value;
				scaleMatrix.Row0.X = zoom;
				scaleMatrix.Row1.Y = zoom;
				requiresUpdate = true;
			}
		}

		public Matrix4 Matrix
		{
			get
			{
				if (requiresUpdate)
					updateMatrices();
				return finalMatrix;
			}
		}

		private void updateMatrices()
		{
			finalMatrix = translationMatrix*scaleMatrix*rotationMatrix*offsetMatrix;
			requiresUpdate = false;
		}

		public void Activate()
		{
			currentCamera = this;
		}

		public Vector2 GetApsoluteMouse(int X, int Y)
		{
			float x, y;
			x = X;
			y = Y;
			x -= Firefly.Window.Width/2;
			y -= Firefly.Window.Height/2;
			x /= zoom;
			y /= zoom;
			var angle = (float) Math.Atan2(y, x);
			angle -= rotation;
			var distance = (float) Math.Sqrt(x*x + y*y);
			x = (float) Math.Cos(angle)*distance;
			y = (float) Math.Sin(angle)*distance;
			x -= this.x;
			y -= this.y;

			return new Vector2(x, y);
		}
	}
}