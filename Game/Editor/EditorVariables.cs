using System.Collections.Generic;
using LadaEngine;

namespace Platformer.Common
{
	public class EditorVariables
	{
		public iPos SelectedTexture;
		public bool ShowGrid;
		public Tile selected;

		public EditorVariables()
		{
			ShowGrid = true;
			SelectedTexture = new iPos(0, 0);
		}
	}
}