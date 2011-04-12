using System.Collections.Generic;
using OpenTK;

namespace FireflyGL
{
	internal class Polygon
	{
		private List<Vector4> colors;
		private List<Vector4> points;
		private List<Vector2> texcoords;

		public Polygon(bool Textured, params float[] Coordinates)
		{
			points = new List<Vector4>();
			colors = new List<Vector4>();
			texcoords = new List<Vector2>();

			if (Textured)
			{
				for (int i = 0; i < Coordinates.Length; i += 4)
				{
					points.Add(new Vector4(Coordinates[i], Coordinates[i + 1], 1, 1));
					texcoords.Add(new Vector2(Coordinates[i + 2], Coordinates[i + 3]));
				}
			}
			else
			{
				for (int i = 0; i < Coordinates.Length; i += 6)
				{
					points.Add(new Vector4(Coordinates[i], Coordinates[i + 1], 1, 1));
					colors.Add(new Vector4(Coordinates[i + 2], Coordinates[i + 3], Coordinates[i + 4], Coordinates[i + 5]));
				}
			}
		}

		public List<Vector4> Points
		{
			get { return points; }
			set { points = value; }
		}

		public List<Vector4> Colors
		{
			get { return colors; }
			set { colors = value; }
		}

		public List<Vector2> Texcoords
		{
			get { return texcoords; }
			set { texcoords = value; }
		}
	}
}