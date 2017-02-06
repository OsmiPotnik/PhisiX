using System;
using Microsoft.Xna.Framework;

namespace PhisiX.Mathematics
{
	public class HalfPlane
	{
		float Distance { get; set; }
		public Vector2 Normal { get; set; }

		public HalfPlane (Vector2 normal, float distance)
		{
			Normal = normal;
			Distance = distance;
		}

		public static HalfPlane halfPlane (Vector2 normal, float distance){
			return new HalfPlane (normal, distance);
		}

		public virtual void setNormal (Vector2 normal){
			Normal = normal;
			if (Normal.LengthSquared() != 1)
				Normal.Normalize ();
		}
	}
}

