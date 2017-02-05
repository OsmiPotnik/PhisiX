using System;
using PhisiX.Colliders;
using Microsoft.Xna.Framework;

namespace PhisiX
{
	public class Collision
	{

		public static void CollisionBetween(Object item1, Object item2){
			CollisionBetween(item1,item1,true);
		}

		public static void CollisionBetween (Object item1, Object item2, bool recursive){
			//TODO Resolve collision between two objects
			// Algorithms not finished yet

			/*IParticleCollider item1Particle = item1 as IParticleCollider;
			IAARectangleColider item1AARectangle = item1 as IAARectangleColider;

			IParticleCollider item2Particle = item2 as IParticleCollider;
			IAAHalfPlaneCollider item2AAHalfPlane = item2 as IAAHalfPlaneCollider;
			IAARectangleColider item2AARectangle = item2 as IAARectangleColider;

			IHalfPlaneCollider item2HalfPlane = item2 as IHalfPlaneCollider;
			IConvexCollider item2Convex = item2 as IConvexCollider;*/

		}

		public static void CollisionBetween (Object item1, Object item2, CollisionAlgorithm collisionAlgorithm){
			if (collisionAlgorithm.DetectCollisionBetween (item1, item2)) {
				if (Collision.ShouldResolveCollision (item1, item2)) {
					collisionAlgorithm.ResolveCollisionBetween (item1, item2);
					Collision.ReportCollisionBetween (item1,item2);
				}
			}
		}

		public static bool ShouldResolveCollision (Object item1, Object item2){
			ICustomCollider customCollider1 = item1 as ICustomCollider;
			ICustomCollider customCollider2 = item2 as ICustomCollider;

			bool result = true;
			if (customCollider1 != null && customCollider1.GetType ().GetMethod ("CollidingWithItem") != null)
				result &= customCollider1.CollidingWithItem (item2);
			if (customCollider2 != null && customCollider2.GetType ().GetMethod ("CollidingWithItem") != null)
				result &= customCollider2.CollidingWithItem (item1);

			return result;
		}

		public static void ReportCollisionBetween(Object item1, Object item2){
			ICustomCollider customCollider1 = item1 as ICustomCollider;
			ICustomCollider customCollider2 = item2 as ICustomCollider;

			if (customCollider1 != null && customCollider1.GetType ().GetMethod ("CollidedWithItem") != null)
				customCollider1.CollidedWithItem (item2);
			if (customCollider2 != null && customCollider2.GetType ().GetMethod ("CollidedWithItem") != null)
				customCollider2.CollidedWithItem (item1);
		}

		public static void RelaxCollisionBetween (Object item1, Object item2, Vector2 relaxDistance){
			float relaxPercent1 = 0.5f;
			float relaxPercent2 = 0.5f;

			IMass itemWithMass1 = item1 as IMass;
			IMass itemWithMass2 = item2 as IMass;
			IPosition itemWithPosition1 = item1 as IPosition;
			IPosition itemWithPosition2 = item2 as IPosition;

			//Get the relative masses of items
			if (itemWithMass1 != null && itemWithMass2 != null) {
				float mass1 = itemWithMass1.Mass;
				float mass2 = itemWithMass2.Mass;
				relaxPercent1 = mass2 / (mass1 + mass2);
				relaxPercent2 = mass1 / (mass1 + mass2);
			} else if (itemWithMass1 != null) {
				relaxPercent1 = 1;
				relaxPercent2 = 0;
			} else if (itemWithMass2 != null) {
				relaxPercent1 = 0;
				relaxPercent2 = 1;
			} else {
				//no item has mass, calculate with position
				if (itemWithPosition1 != null && itemWithPosition2 == null) {
					relaxPercent1 = 1;
					relaxPercent2 = 0;
				} else if (itemWithPosition1 == null && itemWithPosition2 != null) {
					relaxPercent1 = 0;
					relaxPercent2 = 1;
				}
			}

			//Turn percentages into real distances
			if (itemWithPosition1 != null)
				itemWithPosition1.Position = Vector2.Subtract (itemWithPosition1.Position, Vector2.Multiply (relaxDistance, relaxPercent1));
			if (itemWithPosition2 != null)
				itemWithPosition2.Position = Vector2.Add (itemWithPosition2.Position, Vector2.Multiply (relaxDistance, relaxPercent2));
		}



	}
}

