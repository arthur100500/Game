using System;
using System.Collections.Generic;
using Game.World.Tile;
using LadaEngine;
using Platformer.Common;

namespace Game.World
{
	public class Chunk : IChunk
	{
		public List<WorldTile> Tiles { get; set; }
		public List<Platformer.Common.Tile> RenderableTiles { get; set; }
		public List<Platformer.Common.Tile> ShadowTiles;
		public iPos Position { get; set; }

		private World _world;

		public Chunk(World w)
		{
			_world = w;
			Tiles = new List<WorldTile>();
			RenderableTiles = new List<Platformer.Common.Tile>();
		}

		public void AssambleRenderableTiles()
		{
			RenderableTiles.Clear();
			for (int x = 0; x < _world.ChunkSize; x++)
				for (int y = 0; y < _world.ChunkSize; y++)
				{
					WorldTile t = _world.GetTile(new iPos(Position.X * _world.ChunkSize + x, Position.Y * _world.ChunkSize + y));
					WorldTile tBelow = _world.GetTile(new iPos(Position.X * _world.ChunkSize + x, Position.Y * _world.ChunkSize + y - 1));

					if (t.IsFloor && tBelow.IsFloor)
					{
						RenderableTiles.Add(t.TextureTile);
					}
					if (!t.IsFloor && tBelow.IsFloor)
					{
						RenderableTiles.Add(t.TextureWallTile);
					}
					if (!tBelow.IsFloor)
					{
						var newTile = tBelow.TextureTile.Copy();
						//newTile.Position.Y++;
						RenderableTiles.Add(newTile);
					}

					AddShadowTile(x, y);
				}
		}

		public void SetTile(int x, int y, WorldTile tile)
		{
			Tiles[y * _world.ChunkSize + x] = tile;
			AssambleRenderableTiles();
			_world.GetChunk(Position + new iPos(0, -1)).AssambleRenderableTiles();
		}

		private void AddShadowTile(int x, int y)
		{
			if (!_world.GetTile(new iPos(Position.X * _world.ChunkSize + x, Position.Y * _world.ChunkSize + y - 1)).IsFloor)
			{
				var shadowTile = new Platformer.Common.Tile(new Pos(Position.X * _world.ChunkSize + x, Position.Y * _world.ChunkSize + y), _world.Atlas, new iPos(3, 8));
				RenderableTiles.Add(shadowTile);
			}
			return;
			// 1 1 2
			// 4 x 2
			// 4 3 3
			bool[] corners = new bool[4];
			bool[] sides = new bool[4];


			corners[0] = !_world.GetTile(new iPos(x - 1, y)).IsFloor;
			corners[1] = !_world.GetTile(new iPos(x + 1, y)).IsFloor;
			corners[2] = !_world.GetTile(new iPos(x + 1, y - 2)).IsFloor;
			corners[3] = !_world.GetTile(new iPos(x - 1, y - 2)).IsFloor;

			sides[0] = !_world.GetTile(new iPos(x, y)).IsFloor;
			sides[1] = !_world.GetTile(new iPos(x + 1, y - 1)).IsFloor;
			sides[2] = !_world.GetTile(new iPos(x, y - 2)).IsFloor;
			sides[3] = !_world.GetTile(new iPos(x - 1, y - 1)).IsFloor;

			for (int i = 0; i < 4; i++)
			{
				Platformer.Common.Tile cornerShadow = new Platformer.Common.Tile(new Pos(Position.X * _world.ChunkSize + x, Position.Y * _world.ChunkSize + y), _world.Atlas, new iPos(1, 8));
				Platformer.Common.Tile sideShadow = new Platformer.Common.Tile(new Pos(Position.X * _world.ChunkSize + x, Position.Y * _world.ChunkSize + y), _world.Atlas, new iPos(0, 8));
				if (corners[i])
				{
					cornerShadow.Rotation = MathF.PI * (i + 4) / 2;
					cornerShadow.Height *= -1;
					RenderableTiles.Add(cornerShadow);
				}
				if (sides[i])
				{
					sideShadow.Rotation = MathF.PI * (i + 4) / 2;
					sideShadow.Height *= -1;
					RenderableTiles.Add(sideShadow);
				}
			}

			//if (_world.GetTile(new iPos(x, y - 1)).IsFloor)
			//	RenderableTiles.Add(new Platformer.Common.Tile(new Pos(Position.X * _world.ChunkSize + x, Position.X * _world.ChunkSize + y), _world.Atlas, new iPos(2, 8)));
		}

		public WorldTile GetTile(int x, int y)
		{
			return Tiles[y * _world.ChunkSize + x];
		}
	}
}