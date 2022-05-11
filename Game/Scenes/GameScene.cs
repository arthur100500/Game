using System;
using Avoid.Drawing.Common;
using Avoid.Drawing.UI;
using Avoid.UI;
using Game.World;
using LadaEngine;
using OpenTK.Graphics.OpenGL4;

namespace Platformer.Common
{
	public class GameScene : IScene
	{
		private World _world;
		private Window _window;

		// UI
		private Healthbar hb;
		private Button moneyLabel;
		private Avoid.Drawing.Common.Sprite moneyIcon;
		private int _moneyPrev = 0;
		public string GetName()
		{
			return "A Game";
		}

		public GameScene(Window window)
		{
			_window = window;
		}

		public void Load()
		{
			_world = new World(_window);
			hb = new Healthbar(new Bounds(0.990, 0.990, 0.05, 0.92), _window);
			moneyLabel = new Button(new Bounds(-0.05, 0.990, -0.900, 0.7), "0", () => { }, _window);
			Texture coinIcon = Texture.LoadFromFile("Files/Textures/UI/coinsign.png");
			moneyIcon = new Avoid.Drawing.Common.Sprite(new Bounds(-0.86, 0.997, -1, 0.857), StandartShaders.GenStandartShader(), coinIcon);
			moneyIcon.Load();
			moneyLabel.Load();
			hb.Load();
		}

		public void Render()
		{
			_world.Render();
			hb.Render();
			moneyIcon.Render();
			moneyLabel.Render();
		}

		public void Update()
		{
			_world.player.Update();
			hb.SetHealth(_world.player.Health);

			if(_world.player.Money != _moneyPrev)
			{
				_moneyPrev = _world.player.Money;
				moneyLabel.UpdateText(_moneyPrev.ToString());
			}
		}

		public void FixedUpdate()
		{
			_world.Update();
			_world.player.FixedUpdate();
		}

		public void Resize()
		{
			_world.LightMap.Resize();
		}
	}
}