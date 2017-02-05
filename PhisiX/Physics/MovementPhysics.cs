using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace PhisiX.Physics
{
	public class MovementPhysics
	{
		public static void SimulateMovementOn (Object item, float elapsed){
			IMovable movable = item as IMovable;
			IRotatable rotatable = item as IRotatable;

			if (movable != null) {
				movable.Position = Vector2.Add (movable.Position, Vector2.Multiply(movable.Velocity,elapsed));
			}

			if (rotatable != null) {
				rotatable.RotationAngle += rotatable.AngularVelocity * elapsed;
			}
		}
	}
}

