using System;
using LadaEngine;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Window = LadaEngine.Window;

namespace Platformer.Common
{
	public class Editor
	{
		private readonly EditorGrid _grid;
		public readonly Level Level;
		private readonly EditorTilePalette _palette;
		private Random _random;
		public EditorVariables Variables;
		private float _dragDelta;
		private Camera _camera;

		public Editor(Camera camera)
		{
			_camera = camera;
			_random = new Random();
			_grid = new EditorGrid(camera);
			Level = Level.FromFile("SampleName.plv");
			Variables = new EditorVariables();

			_palette = new EditorTilePalette(this, Level.textureAtlas);
		}

		public void Render(Camera camera)
		{
			Level.Render(camera);
			if (Variables.ShowGrid)
				_grid.Render();
			_palette.Render();
		}

		public void Update()
		{
			if (UpdateUI())
				return;

			if (Controls.ButtonPressedOnce(Keys.H) && Controls.keyboard.IsKeyDown(Keys.LeftControl))
				Variables.ShowGrid = !Variables.ShowGrid;

			UpdateGrid();
			UpdateSelectionHotkeys();
		}

		private void UpdateSelectionHotkeys()
		{
			if (Controls.keyboard.IsKeyPressed(Keys.A))
				Variables.selected.Position.X--;
			if (Controls.keyboard.IsKeyPressed(Keys.D))
				Variables.selected.Position.X++;
			if (Controls.keyboard.IsKeyPressed(Keys.W))
				Variables.selected.Position.Y++;
			if (Controls.keyboard.IsKeyPressed(Keys.S))
				Variables.selected.Position.Y--;
			
			if (Controls.keyboard.IsKeyPressed(Keys.E))
				Variables.selected.Rotation += MathF.PI / 2;
			if (Controls.keyboard.IsKeyPressed(Keys.Q))
				Variables.selected.Rotation -= MathF.PI / 2;

			if (Controls.keyboard.IsKeyDown(Keys.LeftControl))
			{
				if(Controls.keyboard.IsKeyPressed(Keys.S))
					this.Level.SaveToFile("SampleName.plv");
			}
		}

		private void UpdateGrid()
		{
			if (Controls.mouse.ScrollDelta.Y > 0)
				_camera.Zoom *= 1.02f;
			if (Controls.mouse.ScrollDelta.Y < 0)
				_camera.Zoom /= 1.02f;

			if (!Controls.mouse.IsButtonDown(MouseButton.Left) && Controls.mouse.WasButtonDown(MouseButton.Left) &&
			    MathF.Abs(_dragDelta) < 0.03f)
			{
				var position = _grid.GetClickPosition(Controls.cursor_position * 0.5f);
				Variables.selected = new Tile(new Pos(position.X,
						position.Y), Level.textureAtlas,
					Variables.SelectedTexture);
				Level.tiles.Add(Variables.selected);
			}

			if (Controls.mouse.IsButtonDown(MouseButton.Left))
			{
				_dragDelta += Controls.mouse.Delta.X / Misc.window.Size.X + Controls.mouse.Delta.Y / Misc.window.Size.Y;

				if (MathF.Abs(_dragDelta) > 0.03f)
				{
					_dragDelta = Single.MaxValue;
					_camera.Position += new Pos(-Controls.mouse.Delta.X / Misc.window.Size.X,
						Controls.mouse.Delta.Y / Misc.window.Size.Y) * _camera.Zoom * 2;
				}
			}
			else
				_dragDelta = 0;
		}

		private bool UpdateUI()
		{
			return _palette.Update();
		}
	}
}