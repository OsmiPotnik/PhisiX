using System;

namespace PhisiX
{
	public interface ICollisionAlgorithm
	{
		void CollisionBetween(Object item1, Object item2);
		bool DetectCollisionBetween(Object item1, Object item2);
		void ResolveCollisionBetween(Object item1, Object item2);


	}
}

