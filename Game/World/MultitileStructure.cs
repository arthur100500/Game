using System;
using System.Collections.Generic;
using System.Text;
using Game.Physics;
using LadaEngine;
using Platformer.Common;

namespace Game.World
{
	public class MultitileStructure : Entity
	{
		public Platformer.Common.Tile[] InnerTiles;
		private World _world;
		public int Money;
		private static Random random = new Random();
		public MultitileStructure(World world, Pos position)
		{
			Money = 2 + random.Next() % 3;
			_world = world;
			Position = position;
		}

		public override void OnAttack()
		{
			MarkedToDelete = true;
			// Draw money
			for (int i = 0; i < Money; i++)
			{
				_world.NewEntities.Add(Drop.Money(Position, _world, 0.52f * (Money / 20 + 1)));
			}
		}

		public static MultitileStructure Barrel(Pos position, World world)
		{
			var structure = new MultitileStructure(world, new Pos(position.X, position.Y));
			structure.InnerTiles = new Platformer.Common.Tile[]
			{
				new Platformer.Common.Tile(new Pos(position.X, position.Y), world.Atlas, new iPos(0, 95)),
				new Platformer.Common.Tile(new Pos(position.X, position.Y + 1), world.Atlas, new iPos(0, 96))
			};
			structure.Collider = new BoxCollider(new Pos(position.X, position.Y + 0.4f), 1, 1.2f);
			return structure;
		}

		public static MultitileStructure Table(Pos position, World world)
		{
			var structure = new MultitileStructure(world, new Pos(position.X, position.Y));
			structure.InnerTiles = new Platformer.Common.Tile[]
			{
				new Platformer.Common.Tile(new Pos(position.X, position.Y), world.Atlas, new iPos(1, 95)),
				new Platformer.Common.Tile(new Pos(position.X, position.Y + 1), world.Atlas, new iPos(1, 96))
			};
			structure.Collider = new BoxCollider(new Pos(position.X, position.Y + 0.5f), 1, 2);
			return structure;
		}

		public static MultitileStructure Bookshelf(Pos position, World world)
		{
			var structure = new MultitileStructure(world, new Pos(position.X, position.Y));
			structure.InnerTiles = new Platformer.Common.Tile[]
			{
				new Platformer.Common.Tile(new Pos(position.X, position.Y), world.Atlas, new iPos(2, 95)),
				new Platformer.Common.Tile(new Pos(position.X, position.Y + 1), world.Atlas, new iPos(2, 96))
			};
			structure.Collider = new BoxCollider(new Pos(position.X, position.Y + 0.4f), 1, 1.2f);
			return structure;
		}
		public static MultitileStructure PaintingHills(Pos position, World world)
		{
			var structure = new MultitileStructure(world, new Pos(position.X, position.Y));
			structure.InnerTiles = new Platformer.Common.Tile[]
			{
				new Platformer.Common.Tile(new Pos(position.X, position.Y), world.Atlas, new iPos(9, 95)),
				new Platformer.Common.Tile(new Pos(position.X, position.Y + 1), world.Atlas, new iPos(9, 96)),
				new Platformer.Common.Tile(new Pos(position.X + 1, position.Y), world.Atlas, new iPos(10, 95)),
				new Platformer.Common.Tile(new Pos(position.X + 1, position.Y + 1), world.Atlas, new iPos(10, 96))
			};
			return structure;
		}

		public static MultitileStructure PaintingShip(Pos position, World world)
		{
			var structure = new MultitileStructure(world, new Pos(position.X, position.Y));
			structure.InnerTiles = new Platformer.Common.Tile[]
			{
				new Platformer.Common.Tile(new Pos(position.X, position.Y), world.Atlas, new iPos(11, 95)),
				new Platformer.Common.Tile(new Pos(position.X, position.Y + 1), world.Atlas, new iPos(11, 96)),
				new Platformer.Common.Tile(new Pos(position.X + 1, position.Y), world.Atlas, new iPos(12, 95)),
				new Platformer.Common.Tile(new Pos(position.X + 1, position.Y + 1), world.Atlas, new iPos(12, 96))
			};
			return structure;
		}

		public static MultitileStructure BigBookshelf(Pos position, World world)
		{
			var structure = new MultitileStructure(world, new Pos(position.X, position.Y));
			structure.InnerTiles = new Platformer.Common.Tile[]
			{
				new Platformer.Common.Tile(new Pos(position.X, position.Y + 1), world.Atlas, new iPos(7, 95)),
				new Platformer.Common.Tile(new Pos(position.X, position.Y + 2), world.Atlas, new iPos(7, 96)),
				new Platformer.Common.Tile(new Pos(position.X, position.Y), world.Atlas, new iPos(7, 94)),
				new Platformer.Common.Tile(new Pos(position.X + 1, position.Y), world.Atlas, new iPos(8, 94)),
				new Platformer.Common.Tile(new Pos(position.X + 1, position.Y + 1), world.Atlas, new iPos(8, 95)),
				new Platformer.Common.Tile(new Pos(position.X + 1, position.Y + 2), world.Atlas, new iPos(8, 96))
			};
			structure.Collider = new BoxCollider(new Pos(position.X + 0.5f, position.Y + 1.2f), 2, 2.7f);
			structure.Money = 40;
			return structure;
			
		}

		public static MultitileStructure ChestClosed(Pos position, World world)
		{
			var structure = new MultitileStructure(world, new Pos(position.X, position.Y));
			structure.InnerTiles = new Platformer.Common.Tile[]
			{
				new Platformer.Common.Tile(new Pos(position.X, position.Y), world.Atlas, new iPos(3, 95)),
				new Platformer.Common.Tile(new Pos(position.X, position.Y + 1), world.Atlas, new iPos(3, 96))
			};
			structure.Collider = new BoxCollider(new Pos(position.X, position.Y + 0.5f), 1, 1);
			structure.Money = 100;
			return structure;
		}

		public static MultitileStructure ChestOpened(Pos position, World world)
		{
			var structure = new MultitileStructure(world, new Pos(position.X, position.Y));
			structure.InnerTiles = new Platformer.Common.Tile[]
			{
				new Platformer.Common.Tile(new Pos(position.X, position.Y), world.Atlas, new iPos(4, 95)),
				new Platformer.Common.Tile(new Pos(position.X, position.Y + 1), world.Atlas, new iPos(4, 96))
			};
			structure.Collider = new BoxCollider(new Pos(position.X, position.Y + 0.5f), 1, 1);
			structure.Money = 120;
			return structure;
		}

		public static MultitileStructure Torch(Pos position, World world)
		{
			var structure = new MultitileStructure(world, new Pos(position.X, position.Y));
			structure.InnerTiles = new Platformer.Common.Tile[]
			{
				new Platformer.Common.Tile(new Pos(position.X, position.Y), world.Atlas, new iPos(5, 95)),
				new Platformer.Common.Tile(new Pos(position.X, position.Y + 1), world.Atlas, new iPos(5, 96))
			};

			var l = new Light.Light(new Pos(position.X, position.Y + 0.5f), world.LightMap.LightLevel.textureAtlas, new iPos(0, 2));
			l.Width = l.Height = 10;
			world.LightMap.LightLevel.tiles.Add(l);
			return structure;
		}
	}
}
