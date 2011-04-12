using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Input;

namespace FireflyGL {

	public delegate void MouseHandler ( MouseEventArgs Args );

	public struct MouseEventArgs {

	}

	public enum Key : int {
		Unknown = 0,

		// Modifiers
		ShiftLeft,
		LShift = ShiftLeft,
		ShiftRight,
		RShift = ShiftRight,
		ControlLeft,
		LControl = ControlLeft,
		ControlRight,
		RControl = ControlRight,
		AltLeft,
		LAlt = AltLeft,
		AltRight,
		RAlt = AltRight,
		WinLeft,
		LWin = WinLeft,
		WinRight,
		RWin = WinRight,
		Menu,

		// Function keys (hopefully enough for most keyboards - mine has 26)
		// <keysymdef.h> on X11 reports up to 35 function keys.
		F1,
		F2,
		F3,
		F4,
		F5,
		F6,
		F7,
		F8,
		F9,
		F10,
		F11,
		F12,
		F13,
		F14,
		F15,
		F16,
		F17,
		F18,
		F19,
		F20,
		F21,
		F22,
		F23,
		F24,
		F25,
		F26,
		F27,
		F28,
		F29,
		F30,
		F31,
		F32,
		F33,
		F34,
		F35,

		// Direction arrows
		Up,
		Down,
		Left,
		Right,

		Enter,
		Escape,
		Space,
		Tab,
		BackSpace,
		Back = BackSpace,
		Insert,
		Delete,
		PageUp,
		PageDown,
		Home,
		End,
		CapsLock,
		ScrollLock,
		PrintScreen,
		Pause,
		NumLock,

		// Special keys
		Clear,
		Sleep,
		/*LogOff,
		Help,
		Undo,
		Redo,
		New,
		Open,
		Close,
		Reply,
		Forward,
		Send,
		Spell,
		Save,
		Calculator,
         
		// Folders and applications
		Documents,
		Pictures,
		Music,
		MediaPlayer,
		Mail,
		Browser,
		Messenger,
         
		// Multimedia keys
		Mute,
		PlayPause,
		Stop,
		VolumeUp,
		VolumeDown,
		TrackPrevious,
		TrackNext,*/

		// Keypad keys
		Keypad0,
		Keypad1,
		Keypad2,
		Keypad3,
		Keypad4,
		Keypad5,
		Keypad6,
		Keypad7,
		Keypad8,
		Keypad9,
		KeypadDivide,
		KeypadMultiply,
		KeypadSubtract,
		KeypadMinus = KeypadSubtract,
		KeypadAdd,
		KeypadPlus = KeypadAdd,
		KeypadDecimal,
		KeypadEnter,

		// Letters
		A,
		B,
		C,
		D,
		E,
		F,
		G,
		H,
		I,
		J,
		K,
		L,
		M,
		N,
		O,
		P,
		Q,
		R,
		S,
		T,
		U,
		V,
		W,
		X,
		Y,
		Z,

		// Numbers
		Number0,
		Number1,
		Number2,
		Number3,
		Number4,
		Number5,
		Number6,
		Number7,
		Number8,
		Number9,

		// Symbols
		Tilde,
		Minus,
		//Equal,
		Plus,
		BracketLeft,
		LBracket = BracketLeft,
		BracketRight,
		RBracket = BracketRight,
		Semicolon,
		Quote,
		Comma,
		Period,
		Slash,
		BackSlash,
		LastKey
	}
	public enum MouseButton : int {
		Left = 0,
		Middle,
		Right,
		Button1,
		Button2,
		Button3,
		Button4,
		Button5,
		Button6,
		Button7,
		Button8,
		Button9,
		LastButton
	}
	public enum InputState : int {
		Up = 0,
		Down,
		Click
	}


	class Input {

		public static event MouseHandler MouseClick;
		public static event MouseHandler MouseDown;
		public static event MouseHandler MousePress;
		public static event MouseHandler MouseRelease;
		public static event MouseHandler MouseMove;

		static OpenTK.Vector2 absoluteMouse;
		static bool mouseMoved = true;

		static int relativeMouseX, relativeMouseY;

		static int mouseX;
		public static int MouseX {
			get {
				if ( mouseMoved ) updateMouse();
				return (int)absoluteMouse.X;
			}
		}

		static int mouseY;
		public static int MouseY {
			get {
				if ( mouseMoved ) updateMouse();
				return (int)absoluteMouse.Y;
			}
		}

		static float absoluteWheel;
		static float wheelDelta;
		public static float WheelDelta {
			get { return Input.wheelDelta; }
			set { Input.wheelDelta = value; }
		}

		static Dictionary<Key, InputState> keys;
		public static Dictionary<Key, InputState> Keys {
			get { return Input.keys; }
			set { Input.keys = value; }
		}

		static Dictionary<MouseButton, InputState> mouseButtons;
		public static Dictionary<MouseButton, InputState> MouseButtons {
			get { return Input.mouseButtons; }
			set { Input.mouseButtons = value; }
		}

		static LinkedList<Key> keysToRelease;
		static LinkedList<MouseButton> mouseButtonsToRelease;

		public static void Initialize () {

			MouseDown = new MouseHandler( downHandler );
			MouseClick = new MouseHandler( clickHandler );
			MousePress = new MouseHandler( pressHandler );
			MouseRelease = new MouseHandler( releaseHandler );
			MouseMove = new MouseHandler( moveHandler );

			keysToRelease = new LinkedList<Key>();
			mouseButtonsToRelease = new LinkedList<MouseButton>();
			keys = new Dictionary<Key, InputState>();
			mouseButtons = new Dictionary<MouseButton, InputState>();

			string[] names = Enum.GetNames( typeof( Key ) );
			for ( int i = 0 ; i < names.Length ; ++i ) {
				try {
					keys.Add( (Key)Enum.Parse( typeof( Key ), names[ i ] ), InputState.Up );
				} catch ( Exception e ) {
					string stupingWarnings = e.Message;
				}
			}

			names = Enum.GetNames( typeof( MouseButton ) );
			for ( int i = 0 ; i < names.Length ; ++i ) {
				try {
					mouseButtons.Add( (MouseButton)Enum.Parse( typeof( MouseButton ), names[ i ] ), InputState.Up );
				} catch ( Exception e ) {
					string stupidWarnings = e.Message;
				}
			}

			Firefly.Window.GameWindow.Mouse.Move += new EventHandler<MouseMoveEventArgs>( opentkMove );
			Firefly.Window.GameWindow.Mouse.ButtonDown += new EventHandler<MouseButtonEventArgs>( opentkMouseDown );
			Firefly.Window.GameWindow.Mouse.ButtonUp += new EventHandler<MouseButtonEventArgs>( opentkMouseUp );
			Firefly.Window.GameWindow.Keyboard.KeyDown += new EventHandler<KeyboardKeyEventArgs>( opentkKeyDown );
			Firefly.Window.GameWindow.Keyboard.KeyUp += new EventHandler<KeyboardKeyEventArgs>( opentkKeyUp );
			Firefly.Window.GameWindow.Mouse.WheelChanged += new EventHandler<MouseWheelEventArgs>( opentkWheelChange );
		}

		static void opentkWheelChange ( object sender, MouseWheelEventArgs e ) {

			wheelDelta = e.ValuePrecise - absoluteWheel;
			absoluteWheel = e.ValuePrecise;
		}

		public static void Update () {

			wheelDelta = 0;
			foreach ( Key key in keysToRelease ) {
				keys[ key ] = InputState.Up;
			}
			foreach ( MouseButton button in mouseButtonsToRelease ) {
				mouseButtons[ button ] = InputState.Up;
			}
			keysToRelease.Clear();
			mouseButtonsToRelease.Clear();
		}

		static void updateMouse () {

			mouseMoved = false;
			absoluteMouse = Camera.CurrentCamera.GetApsoluteMouse( relativeMouseX, relativeMouseY );
		}

		static void opentkMouseUp ( object sender, MouseButtonEventArgs e ) {

			MouseButton temp = (MouseButton)Enum.Parse(
				typeof( MouseButton ),
				Enum.GetName( typeof( MouseButton ), (int)e.Button )
				);
			mouseButtons[ temp ] = InputState.Click;
			mouseButtonsToRelease.AddLast( temp );
		}
		static void opentkMouseDown ( object sender, MouseButtonEventArgs e ) {

			mouseButtons[ (MouseButton)Enum.Parse(
				typeof( MouseButton ),
				Enum.GetName( typeof( MouseButton ), (int)e.Button )
				)
				] = InputState.Down;
		}
		static void opentkKeyUp ( object sender, KeyboardKeyEventArgs e ) {

			Key temp = (Key)Enum.Parse(
				typeof( Key ),
				Enum.GetName( typeof( Key ), (int)e.Key )
				);
			keys[ temp ] = InputState.Click;
			keysToRelease.AddLast( temp );
		}
		static void opentkKeyDown ( object sender, KeyboardKeyEventArgs e ) {

			keys[ (Key)Enum.Parse(
				typeof( Key ),
				Enum.GetName( typeof( Key ), (int)e.Key )
				)
				] = InputState.Down;
		}
		static void opentkMove ( object sender, MouseMoveEventArgs e ) {

			relativeMouseX = e.X;
			relativeMouseY = e.Y;
			mouseMoved = true;
		}

		static void pressHandler ( MouseEventArgs Args ) {

		}
		static void downHandler ( MouseEventArgs Args ) {

		}
		static void clickHandler ( MouseEventArgs Args ) {

		}
		static void releaseHandler ( MouseEventArgs Args ) {

		}
		static void moveHandler ( MouseEventArgs Args ) {

		}
	}
}
