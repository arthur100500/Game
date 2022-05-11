using Game.Physics;
using LadaEngine;

namespace Platformer.Common
{
	public class Entity
	{
		public Pos Position;
		public BoxCollider Collider;
		public bool MarkedToDelete;

		public virtual void OnAttack()
		{
			
		}
		public virtual void OnCollide()
		{
			
		}

		public void DropMoney()
		{

		}
	}
}