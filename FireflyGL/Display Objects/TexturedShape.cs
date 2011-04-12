using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL;

namespace FireflyGL
{
	internal class TexturedShape : Shape
	{
		private Texture texture;

		public TexturedShape(string Path)
			: base(Path)
		{
			program = Firefly.DefaultTextureProgram;
		}

		public TexturedShape()
		{
			program = Firefly.DefaultTextureProgram;
			floatsPerVertex = 6;
		}

		public Texture Texture
		{
			get { return texture; }
			set { texture = value; }
		}

		public override void SetPolygons()
		{
			base.SetPolygons();

			var tempList = new LinkedList<float>();
			foreach (Polygon poly in filledPolygons)
			{
				for (int i = 2; i < poly.Points.Count; ++i)
				{
					tempList.AddLast(poly.Points[0].X);
					tempList.AddLast(poly.Points[0].Y);
					tempList.AddLast(1);
					tempList.AddLast(1);
					tempList.AddLast(poly.Texcoords[0].X);
					tempList.AddLast(poly.Texcoords[0].Y);

					tempList.AddLast(poly.Points[i - 1].X);
					tempList.AddLast(poly.Points[i - 1].Y);
					tempList.AddLast(1);
					tempList.AddLast(1);
					tempList.AddLast(poly.Texcoords[i - 1].X);
					tempList.AddLast(poly.Texcoords[i - 1].Y);

					tempList.AddLast(poly.Points[i].X);
					tempList.AddLast(poly.Points[i].Y);
					tempList.AddLast(1);
					tempList.AddLast(1);
					tempList.AddLast(poly.Texcoords[i].X);
					tempList.AddLast(poly.Texcoords[i].Y);
				}
			}
			fillArray = tempList.ToArray();
			tempList.Clear();

			foreach (Polygon poly in outlinePolygons)
			{
				for (int i = 0; i < poly.Points.Count; ++i)
				{
					tempList.AddLast(poly.Points[i].X);
					tempList.AddLast(poly.Points[i].Y);
					tempList.AddLast(1);
					tempList.AddLast(1);
					tempList.AddLast(poly.Texcoords[i].X);
					tempList.AddLast(poly.Texcoords[i].Y);
				}
			}
			outlineArray = tempList.ToArray();

			GenerateBuffers();
		}

		public override void Render()
		{
			base.Render();

			(program.Locations["texture"] as Uniform).LoadTexture(texture);
			(program.Locations["window_matrix"] as Uniform).LoadMatrix(Firefly.WindowMatrix);
			(program.Locations["projection_matrix"] as Uniform).LoadMatrix(Firefly.ProjectionMatrix);
			(program.Locations["camera_matrix"] as Uniform).LoadMatrix(Camera.CurrentCamera.Matrix);
			(program.Locations["model_matrix"] as Uniform).LoadMatrix(modelMatrix);

			GL.EnableClientState(ArrayCap.VertexArray);

			GL.EnableVertexAttribArray(program.Locations["vertex_coord"].Location);
			GL.EnableVertexAttribArray(program.Locations["vertex_texcoord"].Location);

			fillBuffer.Bind(BufferTarget.ArrayBuffer);
			(program.Locations["vertex_coord"] as Attribute).AttributePointerFloat(4, 6, 0);
			(program.Locations["vertex_texcoord"] as Attribute).AttributePointerFloat(2, 6, 4);
			GL.DrawArrays(BeginMode.Triangles, 0, fillArray.Length/floatsPerVertex);

			outlineBuffer.Bind(BufferTarget.ArrayBuffer);
			(program.Locations["vertex_coord"] as Attribute).AttributePointerFloat(4, 6, 0);
			(program.Locations["vertex_texcoord"] as Attribute).AttributePointerFloat(2, 6, 4);
			GL.DrawArrays(BeginMode.LineStrip, 0, outlineArray.Length/floatsPerVertex);

			GL.DisableVertexAttribArray(program.Locations["vertex_coord"].Location);
			GL.DisableVertexAttribArray(program.Locations["vertex_texcoord"].Location);

			Utility.ProcessOGLErrors();
		}
	}
}