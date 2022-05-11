using LadaEngine;
using Platformer.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Game.World;
namespace Game.Enemy
{
	public class Slime : Entity
	{
		public Tile Tile;

		private int frame;
		private int travelFrame = 10000;
		private Pos _destination;
		private Random _random = new Random();
		private World.World _world;
		private Pos PrevPosition;
		private Pos startPosition;
		private float Health = 1;
		private int damageFrame = 10000;
		private int MoneyAmount = 8;
		public float BaseDamage = 0.3f;
		private int jumpCooldown = 400;
		private int baseJumpCooldown = 400;

		public Slime(World.World world, Pos position)
		{
			int x = 93;
			if (_random.Next() % 5 == 0)
			{
				x = 91;
				BaseDamage = 0.9f;
				Health = 2;
				MoneyAmount = 20;
				baseJumpCooldown = jumpCooldown = 500;
			}
			if (_random.Next() % 3 == 0)
			{
				x = 92;
				BaseDamage = 0.1f;
				Health = 0.5f;
				MoneyAmount = 5;
				baseJumpCooldown = jumpCooldown = 300;
			}
			Position = position;
			PrevPosition = position;
			_world = world;
			Texture t = Texture.LoadFromFile("Files/Textures/Enemy/slime.png");
			Tile = new Tile(position, world.Atlas, new iPos(0, x));
			Collider = new Physics.BoxCollider(Position, 1, 1);
		}

		public override void OnAttack()
		{
			base.OnAttack();
			Health -= 0.5f;
			damageFrame = 0;
			if (Health <= 0)
				Die();
		}

		public void Update()
		{

			damageFrame++;

			var stateDecided = false;
			if (damageFrame < 40)
			{
				stateDecided = true;
				Tile.TextureInAtlas.X = 3;
			}


			frame++;
			if (frame % jumpCooldown ==	 jumpCooldown - 50)
				if (!stateDecided)
				{
					Tile.TextureInAtlas.X = 1;
				}
			if (frame % jumpCooldown == jumpCooldown - 1)
			{
				frame = 0;
				jumpCooldown = baseJumpCooldown + _random.Next() % 50;
				float distToPlayer = Misc.Len(_world.player.Position, Position);
				if (distToPlayer < 10)
					_destination = Position - (Position - _world.player.Position) * (2 / distToPlayer);
				else
				{
					var angle = 666 * (float)_random.NextDouble();
					_destination = Position + new Pos(MathF.Cos(angle) * 1, MathF.Sin(angle) * 1);
				}
				if (!stateDecided)
					Tile.TextureInAtlas.X = 2;

				travelFrame = 0;
				startPosition = Position;
			}

			if (travelFrame++ >= 200 || CheckCollision())
			{
				Position = PrevPosition.Copy();
				Tile.Position = Position;
				Collider.Position = Position;
				if (!stateDecided && (travelFrame == 200 || travelFrame == 201))
					Tile.TextureInAtlas.X = 0;
				travelFrame = 201;
				if (CheckCollision())
					Die();
				return;
			}

			float impactVal = (float)travelFrame / 200;
			Pos mVector = startPosition * (1 - impactVal) + (impactVal * _destination);

			PrevPosition = Position.Copy();
			Position = mVector;
			Tile.Position = mVector + new Pos(0, (0.25f + (impactVal - 0.5f) * (-impactVal + 0.5f)) * 4);
			Collider.Position = mVector;
		}

		private void Die()
		{
			for (int i = 0; i < MoneyAmount; i++)
				_world.NewEntities.Add(Drop.Money(Position, _world, 0.52f * 2));

			MarkedToDelete = true;
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
	}
}
