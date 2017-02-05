using System;

namespace PhisiX.Colliders
{
	public interface ICustomCollider
	{
		bool CollidingWithItem (Object item);
		void CollidedWithItem (Object item);
	}
}

