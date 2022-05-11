using LadaEngine;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Platformer.Common
{
	public class EditorTilePalette
	{
		private readonly TextureAtlas _atlas;

		private readonly Sprite _background;
		private readonly Camera _camera;

		private readonly Editor _editor;

		private readonly Pos _lu;

		private readonly int _paletteXcount = 2;
		private readonly int _paletteYcount = 7;
		private readonly Level _previewLevel;
		private readonly Pos _rd;
		private int _startingTexture;

		private readonly float innerOffset = 0.03f;
		private readonly float outerOffset = 0.05f;

		private readonly float widthX;
		private readonly float widthY;


		public EditorTilePalette(Editor editor, TextureAtlas atlas)
		{
			_editor = editor;

			_atlas = atlas;
			_camera = new Camera();
			_previewLevel = new Level("Files/Textures/sprites2.png", 100, 100);
			_startingTexture = 0;

			_lu = new Pos(0.6f, 0.75f);
			_rd = new Pos(1f, -0.75f);

			_background = new Sprite(Texture.LoadFromFile("Files/Textures/UI/Editor/palette.png"));
			_background.quad.ReshapeWithCoords(-_rd.X, _lu.Y, -_lu.X, _rd.Y);

			widthX = (_rd.X - _lu.X - outerOffset * 2 - innerOffset * (_paletteXcount - 1)) / _paletteXcount;
			widthY = (_lu.Y - _rd.Y - outerOffset * 2 - innerOffset * (_paletteYcount - 1)) / _paletteYcount;

			LoadNewTextureSet();
		}

		public iPos GetTextureFromClick(Pos click)
		{
			for (var i = 0; i < _paletteXcount; i++)
			for (var j = 0; j < _paletteYcount; j++)
				if (click.X > _lu.X + outerOffset + widthX / 2 + i * (widthX + innerOffset) - widthX / 2 &&
				    click.Y > _rd.Y + outerOffset + widthY / 2 + j * (widthY + innerOffset) - widthY / 2 &&
				    click.X < _lu.X + outerOffset + widthX / 2 + i * (widthX + innerOffset) + widthX / 2 &&
				    click.Y < _rd.Y + outerOffset + widthY / 2 + j * (widthY + innerOffset) + widthY / 2)
					return new iPos((_startingTexture + _paletteYcount * i + j) % 32,
						(_startingTexture + 7 * i + j) / 32);

			return null;
		}

		private void LoadNewTextureSet()
		{
			_previewLevel.tiles.Clear();
			for (var i = 0; i < _paletteXcount; i++)
			for (var j = 0; j < _paletteYcount; j++)
			{
				var n = new Tile(
					new Pos(
						_lu.X + outerOffset + widthX / 2 + i * (widthX + innerOffset),
						_rd.Y + outerOffset + widthY / 2 + j * (widthY + innerOffset)
					),
					_atlas,
					new iPos((_startingTexture + _paletteYcount * i + j) % 32, (_startingTexture + 7 * i + j) / 32)
				);
				n.Width = widthX;
				n.Height = widthY;
				_previewLevel.tiles.Add(n);
			}
		}

		public void Render()
		{
			_background.Render(new Pos(0, 0));
			_previewLevel.Render(_camera);
		}

		private void IncreaseTextureBegin()
		{
			_startingTexture += 14;
			if (_startingTexture > _atlas.Width * _atlas.Height - _paletteXcount * _paletteYcount)
				_startingTexture = _atlas.Width * _atlas.Height - _paletteXcount * _paletteYcount;
			LoadNewTextureSet();
		}

		private void DecreaseTextureBegin()
		{
			_startingTexture -= 14;
			if (_startingTexture < 0)
				_startingTexture = 0;
			LoadNewTextureSet();
		}

		public bool Update()
		{
			if (!(Controls.cursor_position.X - 1 > _lu.X && Controls.cursor_position.X - 1 < _rd.X &&
			      -Controls.cursor_position.Y + 1 < _lu.Y && -Controls.cursor_position.Y + 1 > _rd.Y))
				return false;

			if (!Controls.mouse.IsButtonDown(MouseButton.Left) && Controls.mouse.WasButtonDown(MouseButton.Left))
			{
				var selected =
					GetTextureFromClick(new Pos(Controls.cursor_position.X - 1, -Controls.cursor_position.Y + 1));
				if (!(selected is null))
					_editor.Variables.SelectedTexture = selected;
			}

			if (Controls.mouse.ScrollDelta.Y > 0)
				IncreaseTextureBegin();
			if (Controls.mouse.ScrollDelta.Y < 0)
				DecreaseTextureBegin();

			return true;
		}
	}
}