using System;
using LadaEngine;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Platformer.Common
{
	public class TestScene : IScene
	{
		private Random _random;
		public Camera camera;
		public Level level;

		public string GetName()
		{
			return "Debug test";
		}

		public void Load()
		{
			_random = new Random();
			camera = new Camera();
			level = new Level("Files/Textures/sprites2.png", 100, 100);
/*
			var spread = 200;
			for (int i = 0; i < 10000; i++)
			{
				Tile n = new Tile(
					new Pos(spread * (1 - 2 * (float)_random.NextDouble()), spread * (1 - 2 * (float)_random.NextDouble())),
					level.textureAtlas,
					new iPos(_random.Next() % 32, _random.Next() % 20 + 12)
				);
				n.Height = n.Width = (float) _random.NextDouble();
				
				n.Rotation = (float) _random.NextDouble() * 5;
				level.tiles.Add(n);
			}
			*/
			for (var i = 0; i < 160; i++)
			for (var j = 0; j < 160; j++)
			{
				var n = new Tile(
					new Pos(1 - 2 * (i / (float) 32), 1 - 2 * (j / (float) 32)),
					level.textureAtlas,
					new iPos(i % 32, j % 32)
				);
				n.Height = 2 / 32f;
				n.Width = 2 / 32f;
				level.tiles.Add(n);
			}
		}

		public void Render()
		{
			level.Render(camera);
		}

		public void Update()
		{
		}

		public void FixedUpdate()
		{
			foreach (var n in level.tiles)
			{
				//n.Rotation += 0.01f;
			}

			camera.Position += 0.008f * Controls.control_direction_f * camera.Zoom;
			if (Controls.keyboard.IsKeyDown(Keys.Q))
				camera.Zoom *= 1.02f;
			if (Controls.keyboard.IsKeyDown(Keys.E))
				camera.Zoom /= 1.02f;
		}

		public void Resize()
		{
			throw new NotImplementedException();
		}
	}
}