using System;
using System.Collections.Generic;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace FireflyGL
{
	public delegate void OnLoadHandler(object Target, EventArgs Arguments);

	public class Firefly
	{
		private static bool kill;

		private static Window window;

		private static float updateTime;

		private static float renderTime;
		private static float totalTime;

		private static VertexShader defaultShapeVertexShader, defaultTextureVertexShader;
		private static FragmentShader defaultShapeFragmentShader, defaultTexturedFragmentShader;

		private static ShaderProgram defaultShapeProgram, defaultTextureProgram;

		private static Matrix4 windowMatrix, projectionMatrix;

		private static LinkedList<IRenderable> renderList, renderRemoveList;
		private static LinkedList<IUpdatable> updateList, updateRemoveList;

		public static Window Window
		{
			get
			{
				if (window == null) throw new Exception("Window not open, call the Initialize method first");
				else return window;
			}
			set { window = value; }
		}

		public static float UpdateTime { get; set; }
		public static float RenderTime { get; set; }

		public static ShaderProgram DefaultTextureProgram
		{
			get { return defaultTextureProgram; }
			set { defaultTextureProgram = value; }
		}

		public static ShaderProgram DefaultShapeProgram
		{
			get { return defaultShapeProgram; }
			set { defaultShapeProgram = value; }
		}

		public static Matrix4 ProjectionMatrix
		{
			get { return projectionMatrix; }
			set { projectionMatrix = value; }
		}

		public static Matrix4 WindowMatrix
		{
			get { return windowMatrix; }
			set { windowMatrix = value; }
		}

		public static void Initialize(int Width, int Height, string Title, OnLoadHandler LoadHandler, bool UseOGL3 = false)
		{
			window = new Window(Width, Height, Title, LoadHandler, UseOGL3);

			defaultTextureVertexShader = new VertexShader();
			defaultTextureVertexShader.LoadFromFile(@"shaders\defTextureVert.vert.c");
			defaultTexturedFragmentShader = new FragmentShader();
			defaultTexturedFragmentShader.LoadFromFile(@"shaders\defTextureFrag.frag.c");
			defaultShapeVertexShader = new VertexShader();
			defaultShapeVertexShader.LoadFromFile(@"shaders\defShapeVert.vert.c");
			defaultShapeFragmentShader = new FragmentShader();
			defaultShapeFragmentShader.LoadFromFile(@"shaders\defShapeFrag.frag.c");

			defaultShapeProgram = new ShaderProgram(defaultShapeVertexShader, defaultShapeFragmentShader);
			defaultShapeProgram.Link();
			defaultShapeProgram.AddUniformLocation("window_matrix");
			defaultShapeProgram.AddUniformLocation("model_matrix");
			defaultShapeProgram.AddUniformLocation("projection_matrix");
			defaultShapeProgram.AddUniformLocation("camera_matrix");
			defaultShapeProgram.AddAttribLocation("vertex_coord");
			defaultShapeProgram.AddAttribLocation("vertex_color");

			defaultTextureProgram = new ShaderProgram(defaultTextureVertexShader, defaultTexturedFragmentShader);
			defaultTextureProgram.Link();
			defaultTextureProgram.AddUniformLocation("window_matrix");
			defaultTextureProgram.AddUniformLocation("model_matrix");
			defaultTextureProgram.AddUniformLocation("projection_matrix");
			defaultTextureProgram.AddUniformLocation("camera_matrix");
			defaultTextureProgram.AddUniformLocation("texture");
			defaultTextureProgram.AddAttribLocation("vertex_coord");
			defaultTextureProgram.AddAttribLocation("vertex_texcoord");

			windowMatrix = Matrix4.Identity;
			projectionMatrix = new Matrix4(new Vector4(2F/window.Width, 0, 0, 0),
			                               new Vector4(0, -2F/window.Height, 0, 0),
			                               new Vector4(0, 0, 1, 0),
			                               new Vector4(0, 0, 0, 1))*Matrix4.CreateTranslation(-1, 1, 0);

			var newCam = new Camera();
			newCam.Activate();

			renderList = new LinkedList<IRenderable>();
			updateList = new LinkedList<IUpdatable>();
			renderRemoveList = new LinkedList<IRenderable>();
			updateRemoveList = new LinkedList<IUpdatable>();

			Utility.ProcessOGLErrors();

			window.GameWindow.Load += mainLoop;
			window.GameWindow.Run();
		}

		public static void Kill()
		{
			kill = true;
		}

		public static void ForceKill()
		{
			window.GameWindow.Close();
		}

		private static void mainLoop(object sender, EventArgs e)
		{
			float updateLock = 1/60F;
			float renderLock = 1/60F;
			float updateOverTime = 0;
			float renderOverTime = 0;
			var individualTimer = new Stopwatch();
			var renderTimer = new Stopwatch();
			var updateTimer = new Stopwatch();
			var totalTimer = new Stopwatch();

			setupOpenGL();
			Input.Initialize();

			renderTimer.Start();
			updateTimer.Start();
			while (!kill)
			{
				totalTimer.Restart();

				if (renderTimer.ElapsedTicks/(float) Stopwatch.Frequency + renderOverTime > renderLock)
				{
					renderTimer.Stop();
					renderOverTime += renderTimer.ElapsedTicks/(float) Stopwatch.Frequency;
					renderOverTime -= renderLock;

					individualTimer.Start();
					render();

					individualTimer.Stop();
					renderTime = individualTimer.ElapsedTicks/(float) Stopwatch.Frequency;
					individualTimer.Reset();

					renderTimer.Restart();
				}
				if (updateTimer.ElapsedTicks/(float) Stopwatch.Frequency + updateOverTime > updateLock)
				{
					updateTimer.Stop();
					updateOverTime += updateTimer.ElapsedTicks/(float) Stopwatch.Frequency;
					updateOverTime -= updateLock;

					individualTimer.Start();
					Input.Update();
					window.GameWindow.ProcessEvents();
					update();

					individualTimer.Stop();
					updateTime = individualTimer.ElapsedTicks/(float) Stopwatch.Frequency;
					individualTimer.Reset();

					updateTimer.Restart();
				}

				if (totalTimer.ElapsedMilliseconds > 1)
				{
					totalTimer.Stop();
					totalTime = totalTimer.ElapsedTicks/(float) Stopwatch.Frequency*1000;
				}
			}
		}

		private static void setupOpenGL()
		{
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Enable(EnableCap.Multisample);
			Utility.ProcessOGLErrors();
		}

		private static void render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);

			foreach (IRenderable renderable in renderRemoveList)
			{
				renderList.Remove(renderable);
			}
			renderRemoveList.Clear();

			for (LinkedListNode<IRenderable> node = renderList.First; node != null; node = node.Next)
			{
				node.Value.Render();
			}

			try
			{
				window.GameWindow.SwapBuffers();
			}
			catch (Exception e)
			{
				string stupidWarningMessages = e.Message;
				Kill();
			}
		}

		private static void update()
		{
			window.GameWindow.Title =
				"UpdateTime( " + updateList.Count + " ): " + (int) (updateTime*1000) +
				" RenderTime( " + renderList.Count + " ): " + (int) (renderTime*1000) +
				" TotalTime: " + (int) totalTime;

			foreach (IUpdatable updatable in updateRemoveList)
			{
				updateList.Remove(updatable);
			}
			updateRemoveList.Clear();

			for (LinkedListNode<IUpdatable> node = updateList.First; node != null; node = node.Next)
			{
				node.Value.Update();
			}
		}

		public static void AddToUpdateList(IUpdatable Updatable)
		{
			updateList.AddLast(Updatable);
		}

		public static void RemoveFromUpdateList(IUpdatable Updatable)
		{
			updateRemoveList.AddLast(Updatable);
		}

		public static void AddToRenderList(IRenderable Renderable)
		{
			renderList.AddLast(Renderable);
		}

		public static void RemoveFromRenderList(IRenderable Renderable)
		{
			renderRemoveList.AddLast(Renderable);
		}

		public static void RemoveEntity(object Entity)
		{
			if (Entity is IUpdatable)
			{
				updateRemoveList.AddLast(Entity as IUpdatable);
			}
			if (Entity is IRenderable)
			{
				renderRemoveList.AddLast(Entity as IRenderable);
			}
		}
	}
}