using System;
using Game.World;
using Game.World.Tile;
using LadaEngine;
using Platformer.Common;

namespace Game.WorldGen
{
	public class WorldGenerator
	{
		private Perlin noise;
		
		private float _xScale = 0.1f;
		private float _yScale = 0.1f;

		public WorldGenerator()
		{
			noise = new Perlin(2);
		}
		
		public virtual Chunk GenerateChunk(iPos position, World.World world)
		{
			Chunk result = new Chunk(world);
			result.Position = position;

			Pos pos;

			for (int i = 0; i < world.ChunkSize * world.ChunkSize; i++)
			{
				pos = new Pos((position.X * world.ChunkSize + i % world.ChunkSize) * _xScale, (position.Y * world.ChunkSize + i / world.ChunkSize) * _yScale);
				
				iPos tileTexture = GetTileFromNoise(0.5 + noise.Noise(pos.X * 0.1f, pos.Y * 0.1f, 0), 0.5 + noise.Noise(pos.X, pos.Y, 0));

				Tile res = new Tile(new Pos(position.X * world.ChunkSize + i % world.ChunkSize, position.Y * world.ChunkSize + i / world.ChunkSize), world.Atlas,
					tileTexture);

				tileTexture.Y++;
				Tile resW = new Tile(new Pos(position.X * world.ChunkSize + i % world.ChunkSize, position.Y * world.ChunkSize + i / world.ChunkSize), world.Atlas,
					tileTexture);

				res.Width = res.Height = 1.00f;

				result.Tiles.Add(new WorldTile(res, resW, 0.5 + noise.Noise(pos.X, pos.Y, 0) > 0.5));
			}
			return result;
		}

		private iPos GetTileFromNoise(double octavePerlin, double biomePerlin)
		{
			//if (octavePerlin > 0.7)
				//return new iPos(13, 2);
				//return new iPos(1, 13);
			return new iPos((int)(octavePerlin * 6), 9);
		}
	}
}