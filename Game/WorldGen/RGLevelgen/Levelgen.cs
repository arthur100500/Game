using Game.World;
using Game.World.Tile;
using LadaEngine;
using Platformer.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.WorldGen.RGLevelgen
{
	public class LevelGen
	{
		static Random random = new Random();
		public static int chunkMaterial;
		public RGChunk GenerateChunk(iPos position, World.World world)
		{
			//Console.WriteLine("Generating: " + position);
			RGChunk result = new RGChunk(world);
			result.Position = position;

			chunkMaterial = 2 * (random.Next() % 7);
			if (position == new iPos(0, 0))
				chunkMaterial = 12;
			Pos pos;

			for (int i = 0; i < world.ChunkSize * world.ChunkSize; i++)
			{
				var d = random.Next() % 2;
				if (chunkMaterial == 8)
				{
					if (random.Next() % 10 != 0)
						d = 0;
				}
				iPos tpos = new iPos(0 + d + chunkMaterial, 97);
				Pos tilePos = new Pos(position.X * world.ChunkSize + i % world.ChunkSize, position.Y * world.ChunkSize + i / world.ChunkSize);
				Tile res = new Tile(tilePos.Copy(), world.Atlas, tpos.Copy());
				tpos.Y++;
				Tile resW = new Tile(tilePos.Copy(), world.Atlas, tpos.Copy());
				tpos.Y++;
				Tile resW2 = new Tile(tilePos.Copy(), world.Atlas, tpos.Copy());
				Tile empty = new Tile(tilePos.Copy(), world.Atlas, new iPos(0, 0));

				res.Width = res.Height = 1.00f;
				if (chunkMaterial == 8)
				{
					if (random.NextDouble() < 0.5)
						res.Width *= -1;
					if (random.NextDouble() < 0.5)
						res.Height *= -1;
				}

				WorldTile tile = null;
				// Doorways
				if (tile == null && (i % world.ChunkSize == 7 || i % world.ChunkSize == 8))
					tile = new WorldTile(res, resW, true);
				if (tile == null && (i / world.ChunkSize == 7 || i / world.ChunkSize == 6))
					tile = new WorldTile(res, resW, true);
				if (tile == null && i % world.ChunkSize == 15)
				{
					if (tile == null && (i / world.ChunkSize == 8))
						tile = new WorldTile(resW, resW, false);
					if (tile == null && (i / world.ChunkSize == 9))
						tile = new WorldTile(resW2, resW, false);
				}
				if (tile == null && i % world.ChunkSize == 15)
					tile = new WorldTile(empty, resW, false);
				if (tile == null && i / world.ChunkSize == 15)
					tile = new WorldTile(empty, resW, false);
				if (tile == null && i / world.ChunkSize == 14)
					tile = new WorldTile(resW2, resW, false);
				if (tile == null && i / world.ChunkSize == 13)
					tile = new WorldTile(resW, resW, false);
				if (tile == null)
					tile = new WorldTile(res, resW, true);

				result.Tiles.Add(tile);
			}
			GenFurniture(result, world);
			GenEnemies(result, world);
			return result;
		}

		private void GenEnemies(IChunk chunk, World.World world)
		{
			var c = 2 + random.Next() % 3;
			for (int i = 0; i < c; i++)
				world.Enemies.Add(new Enemy.Slime(world, new Pos(chunk.Position * world.ChunkSize) + new Pos(5 + random.Next() % 3, 5 + random.Next() % 3)));
		}

		private void GenFurniture(IChunk chunk, World.World world)
		{
			// Torches
			world.Entities.Add(MultitileStructure.Torch(new Pos(chunk.Position.X * world.ChunkSize + 3, chunk.Position.Y * world.ChunkSize + 13), world));
			world.Entities.Add(MultitileStructure.Torch(new Pos(chunk.Position.X * world.ChunkSize + 11, chunk.Position.Y * world.ChunkSize + 13), world));

			bool skipOnceAgain = false;

			for (int y = world.ChunkSize - 2; y > 0; y--)
			{
				for (int x = world.ChunkSize - 2; x > 0; x--)
				{
					if (skipOnceAgain)
					{
						skipOnceAgain = false;
						continue;
					}



					if (!chunk.GetTile(x, y).IsFloor)
						continue;

					if (chunk.GetTile(x + 1, y).IsFloor && chunk.GetTile(x, y + 1).IsFloor && chunk.GetTile(x - 1, y).IsFloor && chunk.GetTile(x, y - 1).IsFloor)
					{

						if (random.Next() % 10 == 0)
						{
							if (chunkMaterial == 12)
								if (random.Next() % 2 == 0)
									world.Entities.Add(MultitileStructure.Amethyst(new Pos(chunk.Position.X * world.ChunkSize + x, chunk.Position.Y * world.ChunkSize + y), world));
								else
									world.Entities.Add(MultitileStructure.GreenAmethyst(new Pos(chunk.Position.X * world.ChunkSize + x, chunk.Position.Y * world.ChunkSize + y), world));
							else
								if(random.Next() % 3 == 0)
								world.Entities.Add(MultitileStructure.Table(new Pos(chunk.Position.X * world.ChunkSize + x, chunk.Position.Y * world.ChunkSize + y), world));
							continue;
						}
					}
					if (!chunk.GetTile(x + 1, y).IsFloor || !chunk.GetTile(x, y + 1).IsFloor || !chunk.GetTile(x - 1, y).IsFloor || !chunk.GetTile(x, y - 1).IsFloor)
					{
						if (random.Next() % 5 == 0)
						{
							world.Entities.Add(MultitileStructure.Barrel(new Pos(chunk.Position.X * world.ChunkSize + x, chunk.Position.Y * world.ChunkSize + y), world));
							continue;
						}
					}
					if (!chunk.GetTile(x, y + 1).IsFloor)
					{
						if (random.Next() % 15 == 0)
						{
							world.Entities.Add(MultitileStructure.BigBookshelf(new Pos(chunk.Position.X * world.ChunkSize + x - 1, chunk.Position.Y * world.ChunkSize + y), world));
							skipOnceAgain = true;
							continue;
						}
						if (random.Next() % 5 == 0)
						{
							world.Entities.Add(MultitileStructure.Bookshelf(new Pos(chunk.Position.X * world.ChunkSize + x, chunk.Position.Y * world.ChunkSize + y), world));
							continue;
						}
						if (random.Next() % 60 == 0)
						{
							world.Entities.Add(MultitileStructure.ChestClosed(new Pos(chunk.Position.X * world.ChunkSize + x, chunk.Position.Y * world.ChunkSize + y), world));
							continue;
						}
						if (random.Next() % 60 == 0)
						{
							world.Entities.Add(MultitileStructure.ChestOpened(new Pos(chunk.Position.X * world.ChunkSize + x, chunk.Position.Y * world.ChunkSize + y), world));
							continue;
						}
						if (random.Next() % 10 == 0)
						{
							if (random.Next() % 2 == 1)
								world.Entities.Add(MultitileStructure.PaintingHills(new Pos(chunk.Position.X * world.ChunkSize + x, chunk.Position.Y * world.ChunkSize + y + 1.0f), world));
							else
								world.Entities.Add(MultitileStructure.PaintingShip(new Pos(chunk.Position.X * world.ChunkSize + x, chunk.Position.Y * world.ChunkSize + y + 1.0f), world));
							skipOnceAgain = true;
							continue;
						}
					}
				}
			}
		}
	}
}
