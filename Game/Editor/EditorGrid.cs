using System;
using LadaEngine;
using OpenTK.Graphics.OpenGL4;

namespace Platformer.Common
{
	public class EditorGrid
	{
		private int _ebo;
		private readonly Shader _shader;
		private readonly Camera _source;

		private int _vao;
		private int _vbo;

		private readonly string fragmentShaderCode = @"#version 330
                                        out vec4 outputColor;
                                        void main()
                                        {
	                                        outputColor = vec4(1.0);
                                        }";

		private readonly string vertexShaderCode = @"#version 330 core
                                        layout(location = 0) in vec2 aPosition;
                                        layout(location = 1) in vec2 aTexCoord;

                                        void main(void)
                                        {
                                            gl_Position = vec4(aPosition, 0.5, 1.0);
                                        }";


		public EditorGrid(Camera camera)
		{
			_shader = new Shader(vertexShaderCode, fragmentShaderCode, 0);

			_source = camera;

			LoadGlStuff();

			CountLineVerts();
		}

		public iPos GetClickPosition(Pos click)
		{
			var result = _source.Position +
			             new Pos(click.X + 1 / (_source.Zoom * 4), -click.Y + 1 / (_source.Zoom * 4)) *
			             (_source.Zoom * 2);
			result -= new Pos(_source.Zoom, -_source.Zoom);
			return new iPos((int) MathF.Floor(result.X), (int) MathF.Floor(result.Y));
		}

		private void CountLineVerts()
		{
			// Gen verts
			var _verts = new float[((int) (_source.Zoom * 2) + 2) * 2 * 2 * 2];
			var _indices = new int[((int) (_source.Zoom * 2) + 2) * 2 * 2];
			var i = 0;
			// Horizontal stripes
			for (i = 0; i < (int) (_source.Zoom * 2) + 2; i++)
			{
				_verts[i * 4 + 0] = -1;
				_verts[i * 4 + 1] = (Fract(-_source.Position.Y) + (i - 0.5f) - (int) _source.Zoom) / _source.Zoom;
				_verts[i * 4 + 2] = 1;
				_verts[i * 4 + 3] = (Fract(-_source.Position.Y) + (i - 0.5f) - (int) _source.Zoom) / _source.Zoom;

				_indices[i * 2] = 2 * i;
				_indices[i * 2 + 1] = 2 * i + 1;
			}

			// Vertical stripes
			for (i = 0; i < (int) (_source.Zoom * 2) + 2; i++)
			{
				_verts[i * 4 + 0 + ((int) (_source.Zoom * 2) + 2) * 4] =
					(-Fract(_source.Position.X) + (i - 0.5f) - (int) _source.Zoom) / _source.Zoom;
				_verts[i * 4 + 1 + ((int) (_source.Zoom * 2) + 2) * 4] = -1;
				_verts[i * 4 + 2 + ((int) (_source.Zoom * 2) + 2) * 4] =
					(-Fract(_source.Position.X) + (i - 0.5f) - (int) _source.Zoom) / _source.Zoom;
				_verts[i * 4 + 3 + ((int) (_source.Zoom * 2) + 2) * 4] = 1;

				_indices[i * 2 + ((int) (_source.Zoom * 2) + 2) * 2] = 2 * i + ((int) (_source.Zoom * 2) + 2) * 2;
				_indices[i * 2 + 1 + ((int) (_source.Zoom * 2) + 2) * 2] =
					2 * i + 1 + ((int) (_source.Zoom * 2) + 2) * 2;
			}

			GL.BindVertexArray(_vao);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
			GL.BufferData(BufferTarget.ArrayBuffer, _verts.Length * sizeof(float), _verts,
				BufferUsageHint.DynamicDraw);

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
			GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices,
				BufferUsageHint.StaticDraw);

			_shader.Use();

			var vertexLocation = _shader.GetAttribLocation("aPosition");
			GL.EnableVertexAttribArray(vertexLocation);
			GL.VertexAttribPointer(vertexLocation, 2, VertexAttribPointerType.Float, false, 0, 0);
		}

		private static float Fract(float input)
		{
			return input - MathF.Floor(input);
		}

		private void LoadGlStuff()
		{
			_vao = GL.GenVertexArray();
			GL.BindVertexArray(_vao);

			_vbo = GL.GenBuffer();

			_ebo = GL.GenBuffer();
		}

		public void Render()
		{
			CountLineVerts();

			GL.BindVertexArray(_vao);

			_shader.Use();

			GL.DrawElements(PrimitiveType.Lines, ((int) (_source.Zoom * 2) + 2) * 2 * 2, DrawElementsType.UnsignedInt,
				0);
		}
	}
}