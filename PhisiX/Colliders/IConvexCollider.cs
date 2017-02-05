using System;
using PhisiX.Math;

namespace PhisiX.Colliders
{
	public interface IConvexCollider
	{
		ConvexPolygon bounds { get; set; }
	}
}

