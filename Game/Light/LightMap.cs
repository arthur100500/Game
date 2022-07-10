using Avoid.Drawing.Common;
using LadaEngine;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;
using Platformer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrameBuffer = Avoid.Drawing.Common.FrameBuffer;

namespace Game.Light
{
	public class LightMap
	{
		private FrameBuffer lightfbo;
		private FrameBuffer worldfbo;
		public Level LightLevel;
		internal LightRenderer Renderer;
		private World.World world;
		private GameWindow window;

		public LightMap(GameWindow window, World.World world)
		{
			this.window = window;

			lightfbo = new FrameBuffer();
			worldfbo = new FrameBuffer();
			lightfbo.Load(window.Size);
			worldfbo.Load(window.Size);

			LightLevel = new Level("Files/Textures/lightmap.png", 3, 3);

			Renderer = new LightRenderer(LightLevel.textureAtlas, LightLevel);

			this.world = world;
		}
		public void RenderLights()
		{
			Renderer.UpdateBuffers();
			Renderer.UpdateVerts(new Camera(new Pos(0, 0), world.player.Camera.Zoom));
			lightfbo.Start();
			GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.One);
			Renderer.Render(new Camera(new Pos(0, 0), world.player.Camera.Zoom));
			lightfbo.Stop();

			worldfbo.sprite.Render();
			GL.BlendFunc(BlendingFactor.DstColor, BlendingFactor.Zero);
			lightfbo.sprite.Render();
			GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
		}

		public void RenderWorldStart()
		{
			worldfbo.Start();
		}
		public void RenderWorldStop()
		{
			worldfbo.Stop();
		}

		public void Load()
		{

		}

		public void Resize()
		{
			lightfbo.Resize(new iPos(window.Size.X, window.Size.Y));
			worldfbo.Resize(new iPos(window.Size.X, window.Size.Y));
		}
	}
}
