using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using TexLib;

namespace FireflyGL {

	class Texture {

		private int id;
		public int Id {
			get { return id; }
			set { id = value; }
		}

		public float width, height;
		public float Height {
			get { return height; }
			set { height = value; }
		}
		public float Width {
			get { return width; }
			set { width = value; }
		}

		private int frames;
		public int Frames {
			get { return frames; }
			set { frames = value; }
		}

		private bool animated;
		public bool Animated {
			get { return animated; }
			set { animated = value; }
		}

		public Texture ( Bitmap Bmp ) {

			this.Width = Bmp.Width;
			this.Height = Bmp.Height;

			this.Id = TexUtil.CreateTextureFromBitmap( Bmp );
			Animated = false;
		}

		public Texture ( string Path ) {

			Bitmap Bmp = new Bitmap( Path );
			this.Width = Bmp.Width;
			this.Height = Bmp.Height;

			this.Id = TexUtil.CreateTextureFromBitmap( Bmp );
			Animated = false;
		}

		public Texture ( Bitmap Bmp, int Frames ) {

			this.Width = Bmp.Width / Frames;
			this.Height = Bmp.Height;

			this.Id = TexUtil.CreateTextureFromBitmap( Bmp );
			this.Frames = Frames;
			Animated = true;
		}

		public Texture ( string Path, int Frames ) {

			Bitmap Bmp = new Bitmap( Path );
			this.Width = Bmp.Width / Frames;
			this.Height = Bmp.Height;

			this.Id = TexUtil.CreateTextureFromBitmap( Bmp );
			this.Frames = Frames;
			Animated = true;
		}

		public void Bind () {

			GL.ActiveTexture( TextureUnit.Texture0 );
			GL.BindTexture( TextureTarget.Texture2D, Id );
		}
	}
}
