using Game.World;
using LadaEngine;
using System;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Game.Physics;
using Game.Enemy;

namespace Platformer.Common
{
	public class Player
	{
		public Camera Camera;
		int movementAnimF;
		public Pos Position;
		public Pos PrevPosition;

		private World _world;
		private LadaEngine.Window _window;
		private bool isFacingRight;
		public AnimatedSprite torsoSprite;
		public AnimatedSprite bottomSprite;
		public AnimatedSprite swordSprite;
		public AnimatedSprite shieldSprite;
		public AnimatedSprite attackSprite;

		private int _money;
		public int Money
		{
			get { return _money; }
			set { _money = value; }
		}

		private bool isBlocking;
		private float speedBlockingModifier = 0.3f;
		private float baseSpeed = 0.03f;

		private bool isAttacking;
		private int attackingF = 90;

		private BoxCollider bodyCollider;
		private BoxCollider attackCollider;
		private BoxCollider pickupCollider;
		private Pos bodyColliderStride = new Pos(0, -0.3f);


		private int InvincibleFrame;
		public float Health { get; set; }

		public Player(World world, LadaEngine.Window window)
		{
			Health = 1;
			Camera = new Camera();
			Camera.Zoom = 5;
			_world = world;
			_window = window;

			Position = new Pos(3, 5);
			Camera.Position = Position;
			PrevPosition = Position;
			var texture = Texture.LoadFromFile("Files/Textures/player.png");
			var attackTexture = Texture.LoadFromFile("Files/Textures/attack.png");

			int x = 15;
			int y = 15;

			torsoSprite = new AnimatedSprite(texture, x, y);
			torsoSprite.centre = new Pos(0, 0);
			torsoSprite.width = torsoSprite.height = 0.1f;
			torsoSprite.ReshapeVertexArray();

			bottomSprite = new AnimatedSprite(texture, x, y);
			bottomSprite.centre = new Pos(0, 0);
			bottomSprite.width = bottomSprite.height = 0.1f;
			bottomSprite.ReshapeVertexArray();

			swordSprite = new AnimatedSprite(texture, x, y);
			swordSprite.centre = new Pos(0, 0);
			swordSprite.width = swordSprite.height = 0.1f;
			swordSprite.ReshapeVertexArray();

			shieldSprite = new AnimatedSprite(texture, x, y);
			shieldSprite.centre = new Pos(0, 0);
			shieldSprite.width = shieldSprite.height = 0.1f;
			shieldSprite.ReshapeVertexArray();

			attackSprite = new AnimatedSprite(attackTexture, 2, 2);
			attackSprite.centre = new Pos(0, 0);
			attackSprite.width = attackSprite.height = 0.2f;
			attackSprite.ReshapeVertexArray();

			bodyCollider = new BoxCollider(Position + bodyColliderStride, 0.2f, 0.4f);
			attackCollider = new BoxCollider(Position, 2f, 2f);
			pickupCollider = new BoxCollider(Position, 2f, 2f);
		}

		public void Update()
		{
			if (_window.MouseState.IsButtonDown(MouseButton.Button1) && !_window.MouseState.WasButtonDown(MouseButton.Button1))
			{
				if (attackingF > 90)
					isAttacking = true;
			}
			isBlocking = _window.MouseState.IsButtonDown(MouseButton.Button2) || Controls.keyboard.IsKeyDown(Keys.F);
		}

		public void Render()
		{
			bottomSprite.Render(new Pos(0, 0));
			torsoSprite.Render(new Pos(0, 0));
			if (attackingF > 90)
				swordSprite.Render(new Pos(0, 0));
			else
				attackSprite.Render(new Pos(0, 0));
			shieldSprite.Render(new Pos(0, 0));

		}
		public void FixedUpdate()
		{
			Health += 0.001f;
			Health = Math.Max(0, Math.Min(Health, 1));
			InvincibleFrame++;
			// Anim
			if (MathF.Abs(Controls.control_direction.X) > 0.1f || MathF.Abs(Controls.control_direction.Y) > 0.1f)
				bottomSprite.state = 15 + (movementAnimF / 30) % 6;
			else
				bottomSprite.state = 0;
			if (Controls.control_direction.X < -0.1f && !isFacingRight)
			{
				Flip();
				isFacingRight = true;
			}
			if (Controls.control_direction.X > 0.1f && isFacingRight)
			{
				Flip();
				isFacingRight = false;
			}

			// Movement
			Move();

			// Attack and block
			if (isAttacking && !isBlocking)
			{
				attackingF = 0;
				isAttacking = false;
			}
			if (attackingF % 20 == 0 && attackingF < 90)
			{
				attackSprite.state = attackingF % 20 + attackingF / 20;
				if (attackingF > 88)
					attackSprite.state = 0;
				if (attackingF == 0)
					Attack();
			}
			movementAnimF++;
			attackingF++;
			SetState();
			PrevPosition = Position;
		}

		private void Attack()
		{

			foreach (var entity in _world.Entities)
				if (entity.Collider is null)
					continue;
				else if (attackCollider.CheckCollision(entity.Collider))
					entity.OnAttack();

			foreach (var enemy in _world.Enemies)
				if (enemy.Collider is null)
					continue;
				else if (attackCollider.CheckCollision(enemy.Collider))
					enemy.OnAttack();

		}

		private void Move()
		{
			if (!isBlocking)
				Position += Controls.control_direction_f * baseSpeed;
			else
				Position += Controls.control_direction_f * baseSpeed * speedBlockingModifier;

			bodyCollider.Position = attackCollider.Position = pickupCollider.Position = Position + bodyColliderStride;

			// Collision with walls
			foreach (var chunk in _world._renderLoadedChunks)
				foreach (var tile in chunk.Tiles)
					if (!tile.IsFloor)
						if (bodyCollider.CheckCollision(tile.TextureTile))
						{
							bodyCollider.Position = new Pos(Position.X, PrevPosition.Y) + bodyColliderStride;
							if (bodyCollider.CheckCollision(tile.TextureTile))
								Position.X = PrevPosition.X;

							bodyCollider.Position = new Pos(PrevPosition.X, Position.Y) + bodyColliderStride;
							if (bodyCollider.CheckCollision(tile.TextureTile))
								Position.Y = PrevPosition.Y;

							bodyCollider.Position = Position + bodyColliderStride;
						}

			// Collision with entities
			foreach (var entity in _world.Entities)
				if (entity.Collider is null)
					continue;
				else if (bodyCollider.CheckCollision(entity.Collider))
				{
					if (entity.Collider.IsNotCollidable)
						continue;
					bodyCollider.Position = new Pos(Position.X, PrevPosition.Y) + bodyColliderStride;
					if (bodyCollider.CheckCollision(entity.Collider))
						Position.X = PrevPosition.X;

					bodyCollider.Position = new Pos(PrevPosition.X, Position.Y) + bodyColliderStride;
					if (bodyCollider.CheckCollision(entity.Collider))
						Position.Y = PrevPosition.Y;

					bodyCollider.Position = Position + bodyColliderStride;


				}

			// Drop pickup
			foreach (var entity in _world.Entities)
				if (entity.Collider is null || !(entity is Drop))
					continue;
				else if (pickupCollider.CheckCollision(entity.Collider))
				{
					if (entity.Collider.IsNotCollidable)
						((Drop)entity).PickUp();
				}
			// Collision with enemies (damage)
			foreach (var enemy in _world.Enemies)
				if (enemy.Collider is null || enemy.Collider.IsNotCollidable)
					continue;
				else if (bodyCollider.CheckCollision(enemy.Collider))
				{
					Damage(enemy);
				}
		}

		private void Damage(Slime enemy)
		{
			if (InvincibleFrame > 100 && !isBlocking)
			{
				Health -= enemy.BaseDamage;
				InvincibleFrame = 0;
			}
		}

		private void Flip()
		{
			bottomSprite.FlipY();
			torsoSprite.FlipY();
			swordSprite.FlipY();
			shieldSprite.FlipY();
			attackSprite.FlipY();
		}
		private void SetState()
		{
			var isMoving = bottomSprite.state == 0;
			if (isMoving)
				torsoSprite.state = 75 + bottomSprite.state % 6;
			else
				torsoSprite.state = 1;
			swordSprite.state = 45 + bottomSprite.state % 6;
			shieldSprite.state = 30 + bottomSprite.state % 6;
			if (isBlocking)
			{
				shieldSprite.state = 60;
				torsoSprite.state = 61;
			}
		}
	}
}