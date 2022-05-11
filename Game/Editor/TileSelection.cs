using System.Collections.Generic;
using LadaEngine;

namespace Platformer.Common
{
	public class TileSelection
	{
		public List<Tile> selected;

		public void AddToSelection(Tile t)
		{
			selected.Add(t);
		}
		
		public void Move(Pos dir)
		{
			foreach (var t in selected)
			{
				t.Position += dir;
			}
		}
	}
}