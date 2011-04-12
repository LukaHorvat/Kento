using OpenTK;

namespace FireflyGL
{
	internal class DisplayObject : IRenderable
	{
		protected Matrix4 modelMatrix;
		                  //Camera and window matrices are stored in separate classes because they are common to all display objects

		private bool requiresUpdate = true;
		private float rotation;

		protected Matrix4 rotationMatrix;
		                  //Camera and window matrices are stored in separate classes because they are common to all display objects

		protected Matrix4 scaleMatrix;
		                  //Camera and window matrices are stored in separate classes because they are common to all display objects

		private float scaleX = 1, scaleY = 1;

		protected Matrix4 translationMatrix;
		                  //Camera and window matrices are stored in separate classes because they are common to all display objects

		private float x, y;

		public float ScaleX
		{
			get { return scaleX; }
			set
			{
				requiresUpdate = true;
				scaleX = value;
			}
		}

		public float ScaleY
		{
			get { return scaleY; }
			set
			{
				requiresUpdate = true;
				scaleY = value;
			}
		}

		public float X
		{
			get { return x; }
			set
			{
				requiresUpdate = true;
				x = value;
			}
		}

		public float Y
		{
			get { return y; }
			set
			{
				requiresUpdate = true;
				y = value;
			}
		}

		public float Rotation
		{
			get { return rotation; }
			set
			{
				requiresUpdate = true;
				rotation = value;
			}
		}

		#region IRenderable Members

		public virtual void Render()
		{
			if (requiresUpdate) updateMatrices();
		}

		#endregion

		private void updateMatrices()
		{
			scaleMatrix = Matrix4.Scale(scaleX, scaleY, 1); //0,0 = scaleX, 1,1 = scaleY
			rotationMatrix = Matrix4.CreateRotationZ(rotation);
			translationMatrix = Matrix4.CreateTranslation(x, y, 0); //0,3 = x, 1,3 = y

			modelMatrix = scaleMatrix*rotationMatrix*translationMatrix;
			requiresUpdate = false;
		}
	}
}