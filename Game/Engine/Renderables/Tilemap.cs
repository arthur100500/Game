using System;
using OpenTK.Graphics.OpenGL4;

namespace LadaEngine
{
	// Tilemap used to draw plane that contains of textures given
	public class Tilemap : BaseObject
	{
		private readonly int SSBO;
		public int grid_length = 10;
		public int grid_width = 10;
		public int[] map;
		public Texture normal_map;

		private Quad quad;
		public Shader shader;
		public Texture textures;
		public int tm_height;
		public int tm_width;

		public Tilemap(Texture textureLocation, int[] map, int height, int width, int gridLength, int gridRowLength)
		{
			tm_height = height;
			tm_width = width;
			this.map = (int[]) map.Clone();
			// shader is a default tilemap shader
			shader = StandartShaders.GenTilemapShader();
			grid_length = gridLength;
			grid_width = gridRowLength;
			textures = textureLocation;

			SSBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ShaderStorageBuffer, SSBO);
			Load();
		}

		public void Load()
		{
			quad = new Quad(Misc.fullscreenverticies, shader, textures);

			quad.Load();
			UpdateMap();

			if (GlobalOptions.full_debug)
				Misc.Log("Tilemap loaded");
		}


		// Sends needed info to shader as uniform
		public void UpdateMap()
		{
			// Set map array to shader
			// 50x50 max grid
			try
			{
				GL.BindBuffer(BufferTarget.ShaderStorageBuffer, SSBO);
				GL.BufferData(BufferTarget.ShaderStorageBuffer, map.Length * 4, map, BufferUsageHint.DynamicDraw);

				shader.SetInt("width", tm_width);
				shader.SetInt("height", tm_height);
				shader.SetInt("texture_length", grid_length);
				shader.SetInt("texture_width", grid_width);
			}
			catch (Exception ex)
			{
				shader.PrintUniformLocations();
				Console.WriteLine(ex);
			}
		}

		public override void Render(Pos cam)
		{
			if (GlobalOptions.full_debug)
				Misc.Log("--- Tilemap render begin ---");
			GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 1, SSBO);
			quad.texture = textures;
			quad.Render(cam);
			if (GlobalOptions.full_debug)
				Misc.Log(" --- Tilemap render end ---");
		}

		public override void ReshapeVertexArray()
		{
			quad.ReshapeVertexArray(this);
		}

		public void Rotate(float angle)
		{
			rotation = angle;
			quad.rotate(angle);
		}

		public void FlipX()
		{
			quad.FlipX();
		}

		public void FlipY()
		{
			quad.FlipY();
		}
	}
}