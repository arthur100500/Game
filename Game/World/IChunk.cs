using Game.World.Tile;
using LadaEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.World
{
	public interface IChunk
	{
		iPos Position { get; set; }
		List<Platformer.Common.Tile> RenderableTiles { get; set; }
		List<WorldTile> Tiles { get; set; }

		void SetTile(int v1, int v2, WorldTile tile);
		void AssambleRenderableTiles();
		WorldTile GetTile(int v1, int v2);
	}
}
