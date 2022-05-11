using Avoid.Drawing.Common;
using LadaEngine;
using OpenTK.Graphics.OpenGL4;
using Sprite = Avoid.Drawing.Common.Sprite;
using IRenderable = Avoid.Drawing.Common.IRenderable;
using System.IO;

namespace Game.UI
{
	public class AnimatedBackground : IRenderable
	{
		private Sprite sprite;

		public AnimatedBackground(string fragmentShaderPath)
		{
			var fshader = File.ReadAllText(fragmentShaderPath);
			var vshader = File.ReadAllText("Files/shaders/basic.vert");

			sprite = new Sprite(new Bounds(1, 1, -1, -1), new Shader(vshader, fshader, 0), null);
		}

		public void Load()
		{
			sprite.Load();
		}

		public void Render()
		{
			sprite.Render();
		}

		public void Update(Window app, float t)
		{
			sprite.shader.Use();

			var loc = sprite.shader.GetUniformLocation("iResolution");
			var locTime = sprite.shader.GetUniformLocation("iTime");

			GL.Uniform2(loc, app.Size);
			GL.Uniform1(locTime, t);
		}
	}
}
