using Game.Physics;
using LadaEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Weapon
{
	public abstract class Weapon
	{
		public int Attack;

		public AnimatedSprite playerAnimationSprite;
		public BoxCollider attackCollider;
		public AnimatedSprite attackSprite;

		public void OnAttack()
		{

		}
	}
}
