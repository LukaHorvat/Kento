using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace FireflyGL {

	public class Window {

		internal GameWindow GameWindow;

		public int Width {
			get { return GameWindow.Width; }
			set { GameWindow.Width = value; }
		}
		public int Height {
			get { return GameWindow.Height; }
			set { GameWindow.Height = value; }
		}

		internal KeyboardDevice Keyboard {
			get { return GameWindow.Keyboard; }
		}

		private System.Drawing.Color clearColor;
		public System.Drawing.Color ClearColor {
			get { return clearColor; }
			set {
				clearColor = value;
				GL.ClearColor( value );
			}
		}

		public Window ( int Width, int Height, string Title, OnLoadHandler LoadHandler, bool UseOGL3 = false ) {

			GameWindow = new GameWindow( Width, Height,
				new GraphicsMode( new ColorFormat( 8, 8, 8, 8 ),//RGBA
					8, 8, 8 ),//Depth, stencil, samples 
				Title,
				GameWindowFlags.Default,//Fullscreen/windowed
				DisplayDevice.Default, //Monitor
				UseOGL3 ? 3 : 2, 0, GraphicsContextFlags.ForwardCompatible );//OGL version
			GameWindow.Load += new EventHandler<EventArgs>( LoadHandler );
		}
	}
}
