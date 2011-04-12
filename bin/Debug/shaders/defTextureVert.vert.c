#version 110

attribute vec4 vertex_coord;
attribute vec2 vertex_texcoord;

uniform mat4 window_matrix;
uniform mat4 model_matrix;
uniform mat4 projection_matrix;
uniform mat4 camera_matrix;

varying vec2 frag_texcoord;

void main () {

	frag_texcoord = vertex_texcoord;
	gl_Position = window_matrix * projection_matrix * camera_matrix * model_matrix * vertex_coord;
}