using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace FireflyGL {

	class ColoredShape : Shape {

		public ColoredShape ( string Path )
			: base( Path ) {

			program = Firefly.DefaultShapeProgram;
		}

		public ColoredShape ()
			: base() {

			program = Firefly.DefaultShapeProgram;
			floatsPerVertex = 8;
		}

		public override void SetPolygons () {
			base.SetPolygons();

			LinkedList<float> tempList = new LinkedList<float>();
			foreach ( Polygon poly in filledPolygons ) {
				for ( int i = 2 ; i < poly.Points.Count ; ++i ) {
					tempList.AddLast( poly.Points[ 0 ].X );
					tempList.AddLast( poly.Points[ 0 ].Y );
					tempList.AddLast( 1 );
					tempList.AddLast( 1 );
					tempList.AddLast( poly.Colors[ 0 ].X );
					tempList.AddLast( poly.Colors[ 0 ].Y );
					tempList.AddLast( poly.Colors[ 0 ].Z );
					tempList.AddLast( poly.Colors[ 0 ].W );

					tempList.AddLast( poly.Points[ i - 1 ].X );
					tempList.AddLast( poly.Points[ i - 1 ].Y );
					tempList.AddLast( 1 );
					tempList.AddLast( 1 );
					tempList.AddLast( poly.Colors[ i - 1 ].X );
					tempList.AddLast( poly.Colors[ i - 1 ].Y );
					tempList.AddLast( poly.Colors[ i - 1 ].Z );
					tempList.AddLast( poly.Colors[ i - 1 ].W );

					tempList.AddLast( poly.Points[ i ].X );
					tempList.AddLast( poly.Points[ i ].Y );
					tempList.AddLast( 1 );
					tempList.AddLast( 1 );
					tempList.AddLast( poly.Colors[ i ].X );
					tempList.AddLast( poly.Colors[ i ].Y );
					tempList.AddLast( poly.Colors[ i ].Z );
					tempList.AddLast( poly.Colors[ i ].W );
				}
			}
			fillArray = new float[ tempList.Count ];
			int j = 0;
			for ( LinkedListNode<float> i = tempList.First ; i != null ; i = i.Next, tempList.RemoveFirst() ) {
				fillArray[ j ] = i.Value;
				++j;
			}
			//fillArray = tempList.ToArray();
			tempList.Clear();

			foreach ( Polygon poly in outlinePolygons ) {
				for ( int i = 0 ; i < poly.Points.Count ; ++i ) {
					tempList.AddLast( poly.Points[ i ].X );
					tempList.AddLast( poly.Points[ i ].Y );
					tempList.AddLast( 1 );
					tempList.AddLast( 1 );
					tempList.AddLast( poly.Colors[ i ].X );
					tempList.AddLast( poly.Colors[ i ].Y );
					tempList.AddLast( poly.Colors[ i ].Z );
					tempList.AddLast( poly.Colors[ i ].W );
				}
			}
			outlineArray = tempList.ToArray();

			GenerateBuffers();
		}

		public override void Render () {
			base.Render();

			loadUniforms();
			enableVertexAttribArrays();
			if ( fillArray.Length > 0 ) drawFillBuffer();
			if ( outlineArray.Length > 0 ) drawOutlineBuffer();
			diableVertexArrays();
			Utility.ProcessOGLErrors();
		}

		void loadUniforms () {
			( program.Locations[ "window_matrix" ] as Uniform ).LoadMatrix( Firefly.WindowMatrix );
			( program.Locations[ "projection_matrix" ] as Uniform ).LoadMatrix( Firefly.ProjectionMatrix );
			( program.Locations[ "camera_matrix" ] as Uniform ).LoadMatrix( Camera.CurrentCamera.Matrix );
			( program.Locations[ "model_matrix" ] as Uniform ).LoadMatrix( modelMatrix );
		}

		void enableVertexAttribArrays () {
			GL.EnableVertexAttribArray( program.Locations[ "vertex_coord" ].Location );
			GL.EnableVertexAttribArray( program.Locations[ "vertex_color" ].Location );
		}

		void drawFillBuffer () {
			fillBuffer.Bind( BufferTarget.ArrayBuffer );
			( program.Locations[ "vertex_coord" ] as Attribute ).AttributePointerFloat( 4, 8, 0 );
			( program.Locations[ "vertex_color" ] as Attribute ).AttributePointerFloat( 4, 8, 4 );
			GL.DrawArrays( BeginMode.Triangles, 0, fillArray.Length / floatsPerVertex );
		}

		void drawOutlineBuffer () {
			outlineBuffer.Bind( BufferTarget.ArrayBuffer );
			( program.Locations[ "vertex_coord" ] as Attribute ).AttributePointerFloat( 4, 8, 0 );
			( program.Locations[ "vertex_color" ] as Attribute ).AttributePointerFloat( 4, 8, 4 );
			GL.DrawArrays( BeginMode.LineStrip, 0, outlineArray.Length / floatsPerVertex );
		}

		void diableVertexArrays () {
			GL.DisableVertexAttribArray( program.Locations[ "vertex_coord" ].Location );
			GL.DisableVertexAttribArray( program.Locations[ "vertex_color" ].Location );
		}
	}
}
