using System;
using PhisiX.Mathematics;

namespace PhisiX.Colliders
{
	public interface IConvexCollider
	{
		ConvexPolygon bounds { get; set; }
	}
}

