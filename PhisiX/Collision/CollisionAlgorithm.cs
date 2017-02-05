using System;

namespace PhisiX
{
	public class CollisionAlgorithm : ICollisionAlgorithm
	{
		public void CollisionBetween(Object item1, Object item2){
			Collision.CollisionBetween(item1,item2,this);
		}

		public bool DetectCollisionBetween(Object item1, Object item2){
			return false;
		}

		public void ResolveCollisionBetween(Object item1, Object item2){
			
		}
	}
}

