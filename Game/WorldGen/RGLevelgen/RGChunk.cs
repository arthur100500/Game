using System;
using System.Collections.Generic;
using System.Linq;
using Game.Light.Shadow;
using Game.World.Tile;
using LadaEngine;
using Platformer.Common;

namespace Game.World
{
	public class RGChunk : IChunk
	{
		public List<WorldTile> Tiles { get; set; }
		public List<Platformer.Common.Tile> RenderableTiles { get; set; }
		public List<Platformer.Common.Tile> ShadowTiles;
		public iPos Position { get; set; }

		private World _world;

		public RGChunk(World w)
		{
			_world = w;
			Tiles = new List<WorldTile>();
			RenderableTiles = new List<Platformer.Common.Tile>();
		}

		public void AssambleRenderableTiles()
		{
			RenderableTiles.Clear();
			
			foreach (var tile in Tiles)
				RenderableTiles.Add(tile.TextureTile);
			
			CreateShadows();
		}

		public void SetTile(int x, int y, WorldTile tile)
		{
			Tiles[y * _world.ChunkSize + x] = tile;
			AssambleRenderableTiles();
			_world.GetChunk(Position + new iPos(0, -1)).AssambleRenderableTiles();
		}

		public void CreateShadows()
		{
			for (int x = 0; x < _world.ChunkSize; x++)
				for (int y = 0; y < _world.ChunkSize; y++)
				{
					bool[] dataAroud = new bool[9] { false, false, false, false, false, false, false, false, false };

					for (int i = 0; i < 3; i++)
						for (int j = 0; j < 3; j++)
							if (!_world.GetTile(new iPos(Position.X * _world.ChunkSize + x + i - 1, Position.Y * _world.ChunkSize + y + j - 1)).IsFloor)
								dataAroud[i + j * 3] = true;

					if(!dataAroud.SequenceEqual(new bool[9] { false, false, false, false, false, false, false, false, false }))
						RenderableTiles.Add(new ShadowTile(new Pos(Position.X * _world.ChunkSize + x, Position.Y * _world.ChunkSize + y), _world.Atlas, dataAroud));
				}
		}

		public WorldTile GetTile(int x, int y)
		{
			return Tiles[y * _world.ChunkSize + x];
		}
	}
}