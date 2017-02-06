using System;
using Microsoft.Xna.Framework;

namespace PhisiX.Mathematics
{
	public class AAHalfPlane : HalfPlane
	{
		AxisDirection Direction { get; set; }
		public AAHalfPlane (AxisDirection direction,float distance) : base(computeDirection(direction), distance)
		{

			Direction = direction;
		}

		public static Vector2 computeDirection(AxisDirection direction){
			Vector2 normal;
			switch (direction) {
				default:
				case AxisDirection.AxisDirectionPositiveX:
					normal = Vector2.UnitX;
					break;
				case AxisDirection.AxisDirectionNegativeX:
					normal = Vector2.Negate (Vector2.UnitX);
					break;
				case AxisDirection.AxisDirectionPositiveY:
					normal = Vector2.UnitY;
					break;
				case AxisDirection.AxisDirectionNegativeY:
					normal = Vector2.Negate (Vector2.UnitY);
					break;
			}
			return normal;
		}

		public static AAHalfPlane aaHalfPlane (AxisDirection direction, float distance){
			return new AAHalfPlane (direction,distance);
		}

		public void setDirection (AxisDirection direction){
			switch (direction) {
				default:
				case AxisDirection.AxisDirectionPositiveX:
					base.Normal = Vector2.UnitX;
					break;
				case AxisDirection.AxisDirectionNegativeX:
					base.Normal = Vector2.Negate (Vector2.UnitX);
					break;
				case AxisDirection.AxisDirectionPositiveY:
					base.Normal = Vector2.UnitY;
					break;
				case AxisDirection.AxisDirectionNegativeY:
					base.Normal = Vector2.Negate (Vector2.UnitY);
					break;
			}
		}

		public override void setNormal (Vector2 normal){
			if ((normal.X == 0 && normal.Y == 0) || (normal.X != 0 && normal.X != 0))
				throw new System.ArgumentException ("Axis aligne half plane requires axis aligned normal","normal");

			base.setNormal (normal);

			if (normal.X > 0)
				Direction = AxisDirection.AxisDirectionPositiveX;
			else if (normal.X < 0)
				Direction = AxisDirection.AxisDirectionNegativeX;
			else if (normal.Y > 0)
				Direction = AxisDirection.AxisDirectionPositiveY;
			else if (normal.Y < 0)
				Direction = AxisDirection.AxisDirectionNegativeY;

		}

	}
}

