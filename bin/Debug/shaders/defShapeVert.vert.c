#version 110

attribute vec4 vertex_coord;
attribute vec4 vertex_color;

uniform mat4 window_matrix;
uniform mat4 model_matrix;
uniform mat4 projection_matrix;
uniform mat4 camera_matrix;

varying vec4 frag_color;

void main () {

	frag_color = vertex_color;
	gl_Position = projection_matrix * camera_matrix * model_matrix * vertex_coord;
} 