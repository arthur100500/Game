using LadaEngine;

namespace Platformer.Common.EditorChange
{
	public class CreateChange : EditorChange
	{
		private Pos _position;
		private Tile _tile;

		public CreateChange(Tile tile)
		{
			_tile = tile;
		}

		public override void Undo(Editor editor)
		{
			/*
			foreach (var _tile in editor.Variables.selected)
			{
				editor.Level.tiles.Remove(_tile);
			}
			*/
		}

		public override void Redo(Editor editor)
		{
			editor.Level.tiles.Add(_tile);
		}
	}
}