using System.Drawing;
using OpenTK.Graphics.OpenGL;
using TexLib;

namespace FireflyGL
{
	internal class Texture
	{
		public float height;
		public float width;

		public Texture(Bitmap Bmp)
		{
			Width = Bmp.Width;
			Height = Bmp.Height;

			Id = TexUtil.CreateTextureFromBitmap(Bmp);
			Animated = false;
		}

		public Texture(string Path)
		{
			var Bmp = new Bitmap(Path);
			Width = Bmp.Width;
			Height = Bmp.Height;

			Id = TexUtil.CreateTextureFromBitmap(Bmp);
			Animated = false;
		}

		public Texture(Bitmap Bmp, int Frames)
		{
			Width = Bmp.Width/Frames;
			Height = Bmp.Height;

			Id = TexUtil.CreateTextureFromBitmap(Bmp);
			this.Frames = Frames;
			Animated = true;
		}

		public Texture(string Path, int Frames)
		{
			var Bmp = new Bitmap(Path);
			Width = Bmp.Width/Frames;
			Height = Bmp.Height;

			Id = TexUtil.CreateTextureFromBitmap(Bmp);
			this.Frames = Frames;
			Animated = true;
		}

		public int Id { get; set; }

		public float Height
		{
			get { return height; }
			set { height = value; }
		}

		public float Width
		{
			get { return width; }
			set { width = value; }
		}

		public int Frames { get; set; }

		public bool Animated { get; set; }

		public void Bind()
		{
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, Id);
		}
	}
}