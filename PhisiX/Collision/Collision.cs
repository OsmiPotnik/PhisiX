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

		public void ExchangeEnergyBetween (Object item1, Object item2, Vector2 collisionNormal, Vector2 pointOfImpact){
			IPosition item1WithPosition = item1 as IPosition;
			IMovable movableItem1 = item1 as IMovable;
			IRotatable rotatableItem1 = item1 as IRotatable;
			IPosition item2WithPosition = item2 as IPosition;
			IMovable movableItem2 = item2 as IMovable;
			IRotatable rotatableItem2 = item2 as IRotatable;

			Vector2 velocity1 = (movableItem1 != null) ? new Vector2 (movableItem1.Velocity.X, movableItem1.Velocity.Y) : Vector2.Zero;
			Vector2 velocity2 = (movableItem2 != null) ? new Vector2 (movableItem2.Velocity.X, movableItem2.Velocity.Y) : Vector2.Zero;

			Vector2 lever1 = Vector2.Zero;
			Vector2 lever2 = Vector2.Zero;
			Vector2 tangentialDirection1 = Vector2.Zero;
			Vector2 tangentialDirection2 = Vector2.Zero;

			if (!pointOfImpact.Equals(Vector2.Zero)) {
				if (item1WithPosition != null && rotatableItem1 != null) {
					lever1 = Vector2.Subtract (pointOfImpact, item1WithPosition.Position);
					tangentialDirection1 = Vector2.Normalize (new Vector2(-lever1.Y,lever1.X));
					Vector2 rotationalVelocity = Vector2.Multiply (tangentialDirection1,lever1.Length()*rotatableItem1.AngularVelocity);
					velocity1 = Vector2.Add (velocity1,rotationalVelocity);
				}
				if (item2WithPosition != null && rotatableItem2 != null) {
					lever2 = Vector2.Subtract (pointOfImpact, item2WithPosition.Position);
					tangentialDirection2 = Vector2.Normalize (new Vector2(-lever2.Y,lever2.X));
					Vector2 rotationalVelocity = Vector2.Multiply (tangentialDirection2,lever2.Length()*rotatableItem2.AngularVelocity);
					velocity2 = Vector2.Add (velocity2,rotationalVelocity);
				}
			}

			//Energy exchange
			float speed1 = !velocity1.Equals (Vector2.Zero) ? Vector2.Dot (velocity1, collisionNormal) : 0;
			float speed2 = !velocity2.Equals (Vector2.Zero) ? Vector2.Dot (velocity2, collisionNormal) : 0;
			float speedDiference = speed1 - speed2;

			if (speedDiference < 0) {
				return;
			}

			float cor1 = (item1 is ICoefficientOfRestitution) ? ((ICoefficientOfRestitution)item1).CoefficientOfRestitution : 1;
			float cor2 = (item2 is ICoefficientOfRestitution) ? ((ICoefficientOfRestitution)item2).CoefficientOfRestitution : 1;
			float cor = cor1 * cor2;

			float mass1inverse = (item1 is IMass) ? 1.0f / ((IMass)item1).Mass : 0;
			float mass2inverse = (item2 is IMass) ? 1.0f / ((IMass)item2).Mass : 0;

			IAngularMass item1WitAngularMass = item1 as IAngularMass;
			IAngularMass item2WitAngularMass = item2 as IAngularMass;

			float angularMass1Inverse = (item1WitAngularMass != null && !tangentialDirection1.Equals (Vector2.Zero)) ?
				(float)Math.Pow (Vector2.Dot (tangentialDirection1, collisionNormal) * lever1.Length (), 2) / item1WitAngularMass.AngularMass : 0;
			float angularMass2Inverse = (item2WitAngularMass != null && !tangentialDirection2.Equals (Vector2.Zero)) ?
				(float)Math.Pow (Vector2.Dot (tangentialDirection2, collisionNormal) * lever2.Length (), 2) / item2WitAngularMass.AngularMass : 0;

			float impact = -(cor + 1) * speedDiference / (mass1inverse + mass2inverse + angularMass1Inverse + angularMass2Inverse);

			//Translation change

			if (mass1inverse > 0 && movableItem1 != null) {
				movableItem1.Velocity = Vector2.Add (movableItem1.Velocity,Vector2.Multiply(collisionNormal, impact*mass1inverse));
			}

			if (mass2inverse > 0 && movableItem2 != null) {
				movableItem2.Velocity = Vector2.Subtract (movableItem2.Velocity,Vector2.Multiply(collisionNormal,impact*mass2inverse));
			}

			//Rotation change

			if (item1WitAngularMass != null && !tangentialDirection1.Equals (Vector2.Zero)) {
				float tangentialForce = Vector2.Dot (tangentialDirection1, collisionNormal) * impact;
				float change = tangentialForce * lever1.Length () / item1WitAngularMass.AngularMass;
				rotatableItem1.AngularVelocity += change;
			}

			if (item2WitAngularMass != null && !tangentialDirection2.Equals (Vector2.Zero)) {
				float tangentialForce = Vector2.Dot (tangentialDirection2, collisionNormal) * -impact;
				float change = tangentialForce * lever2.Length () / item2WitAngularMass.AngularMass;
				rotatableItem2.AngularVelocity += change;
			}

		}




	}
}

