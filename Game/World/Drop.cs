using LadaEngine;
using Platformer.Common;
using System;

namespace Game.World
{
	public class Drop : Entity
	{
		public Platformer.Common.Tile Tile { get; set; }
		private static Random _random = new Random();
		private Pos _destination;
		private int travelFrame;
		private World _world;
		private bool _isFoundByPlayer;
		private int animPickup;
		private Pos _startPos;

		public override void OnCollide()
		{
			base.OnCollide();

		}
		public Drop(iPos textureLocation, Pos pos, Pos destination, World world)
		{
			_world = world;
			_destination = destination;
			Position = pos;
			Tile = new Platformer.Common.Tile(pos, world.Atlas, textureLocation);
			Collider = new Physics.BoxCollider(Position, 0.1f, 0.1f);
			Collider.IsNotCollidable = true;
		}

		public static Drop Money(Pos pos, World world, float dropRadius)
		{
			float angle = 666 * (float)_random.NextDouble();
			dropRadius *= (float)_random.NextDouble();
			int x = 0;
			if (_random.Next() % 10 == 0)
				x = 2;
			return new Drop(new iPos(_random.Next() % 2 + x, 94), pos, pos + new Pos(MathF.Cos(angle) * dropRadius, MathF.Sin(angle) * dropRadius), world);
		}

		public void Update()
		{
			if(_isFoundByPlayer && travelFrame >= 40)
			{
				_destination = _world.player.Position;
				float pickupImpactVal = (float)animPickup / 60;
				pickupImpactVal = MathF.Pow(pickupImpactVal, 1.6f);
				Pos pickupIVector = _startPos * (1 - pickupImpactVal) + (pickupImpactVal * _destination);

				Position = pickupIVector;
				Tile.Position = pickupIVector;
				Collider.Position = pickupIVector;

				animPickup++;
				if(animPickup > 60)
				{
					_world.player.Money++;
					MarkedToDelete = true;
				}
				return;
			}

			travelFrame++;
			if (travelFrame >= 40 || CheckCollision())
				return;
			
			float impactVal = (float)travelFrame / 40;
			impactVal = MathF.Pow(impactVal, 0.6f);
			Pos mVector = Position * (1 - impactVal) + (impactVal * _destination);

			Tile.Position = mVector;
			Collider.Position = mVector;
		}

		private bool CheckCollision()
		{
			foreach (var chunk in _world._renderLoadedChunks)
				foreach (var tile in chunk.Tiles)
					if (!tile.IsFloor)
						if (Collider.CheckCollision(tile.TextureTile))
							return true;
			return false;
		}

		internal void PickUp()
		{
			if (travelFrame < 40)
				return;
			if (!_isFoundByPlayer)
				_startPos = Tile.Position;
			_isFoundByPlayer = true;
		}
	}
}