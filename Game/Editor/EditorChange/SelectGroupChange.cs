using System.Collections.Generic;

namespace Platformer.Common.EditorChange
{
	public class SelectGroupChange : EditorChange
	{
		public List<Tile> selectionPrev;
		public List<Tile> selectionNew;
		
		public override void Undo(Editor editor)
		{
			/*
			editor.Variables.selected = selectionPrev;
			*/
		}

		public override void Redo(Editor editor)
		{
			/*
			editor.Variables.selected = selectionNew;
			*/
		}
	}
}