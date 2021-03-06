using LadaEngine;
using Platformer.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Physics
{
	public class BoxCollider : GameObject
	{
		public BoxCollider(Pos position, float width, float height) : base(position, width, height)
		{
		}

		public bool IsNotCollidable { get; internal set; }

		public bool CheckCollision(GameObject other)
		{
			if (Position.X + Width / 2 >= other.Position.X - other.Width / 2 &&
				Position.X - Width / 2 <= other.Position.X + other.Width / 2 &&
				Position.Y + Height / 2 >= other.Position.Y - other.Height / 2 &&
				Position.Y - Height / 2 <= other.Position.Y + other.Height / 2)
				return true;
			return false;
		}
	}
}
