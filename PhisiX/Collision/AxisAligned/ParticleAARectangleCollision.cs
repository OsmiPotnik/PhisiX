using System;
using PhisiX.Colliders;
using Microsoft.Xna.Framework;

namespace PhisiX
{
	public class ParticleAARectangleCollision : CollisionAlgorithm
	{
		public static bool DetectCollisionBetween(IParticleCollider particle, IAARectangleColider rectangle){
			Vector2 relaxedDistance = ParticleAARectangleCollision.CalculateRelaxDistanceBetween (particle, rectangle);
			return relaxedDistance.LengthSquared () > 0;
		}

		public static void ResolveCollisionBetween (IParticleCollider particle, IAARectangleColider rectangle){
			Vector2 relaxDistance = ParticleAARectangleCollision.CalculateRelaxDistanceBetween (particle, rectangle);
			Collision.RelaxCollisionBetween (particle, rectangle, relaxDistance);

			Vector2 collisionNormal = Vector2.Normalize (new Vector2(relaxDistance.X,relaxDistance.Y));
			Collision.ExchangeEnergyBetween (particle,rectangle,collisionNormal,Vector2.Zero);
		}

		public static void CollisionBetween (IParticleCollider particle, IAARectangleColider rectangle){
			if (ParticleAARectangleCollision.DetectCollisionBetween (particle, rectangle)) {
				ParticleAARectangleCollision.ResolveCollisionBetween (particle, rectangle);
			}
		}

		public static Vector2 CalculateRelaxDistanceBetween(IParticleCollider particle, IAARectangleColider rectangle){
			Vector2 relaxDistance = Vector2.Zero;
			Vector2 nearestVertex = new Vector2 (rectangle.Position.X,rectangle.Position.Y);

			float halfWidth = rectangle.Width / 2;
			float halfHeight = rectangle.Height / 2;

			float leftDifference = (rectangle.Position.X - halfWidth) - (particle.Position.X + particle.Radius);
			if (leftDifference > 0)
				return relaxDistance;

			float rightDifference = (particle.Position.X - particle.Radius) - (rectangle.Position.X + halfWidth);
			if (rightDifference > 0)
				return relaxDistance;

			float topDifference = (rectangle.Position.Y - halfHeight) - (particle.Position.Y + particle.Radius);
			if (topDifference > 0)
				return relaxDistance;

			float bottomDifference = (particle.Position.Y - particle.Radius) - (rectangle.Position.Y + halfHeight);
			if (bottomDifference > 0)
				return relaxDistance;

			bool horizontalyInside = false;
			bool verticalyInside = false;

			if (particle.Position.X < rectangle.Position.X - halfWidth) {
				nearestVertex.X -= halfWidth;
			} else if (particle.Position.X > rectangle.Position.X + halfWidth) {
				nearestVertex.X += halfWidth;
			} else {
				horizontalyInside = true;
			}

			if (particle.Position.Y < rectangle.Position.Y - halfHeight) {
				nearestVertex.Y -= halfHeight;
			} else if (particle.Position.Y > rectangle.Position.Y + halfHeight) {
				nearestVertex.Y += halfHeight;
			} else {
				verticalyInside = true;
			}

			if (!horizontalyInside && !verticalyInside) {
				Vector2 particleVertex = Vector2.Subtract (nearestVertex,particle.Position);
				float vertexDistance = particleVertex.Length ();
				if (vertexDistance > particle.Radius) {
					return relaxDistance;
				} else {
					return Vector2.Multiply (Vector2.Normalize(particleVertex),particle.Radius - vertexDistance);
				}
			}

			if (leftDifference > rightDifference) {
				relaxDistance.X = -leftDifference;
			} else {
				relaxDistance.X = rightDifference;
			}

			if (topDifference > bottomDifference) {
				relaxDistance.Y = -topDifference;
			} else {
				relaxDistance.Y = bottomDifference;
			}

			if (Math.Abs (relaxDistance.X) < Math.Abs (relaxDistance.Y)) {
				relaxDistance.Y = 0;
			} else {
				relaxDistance.X = 0;
			}

			return relaxDistance;
			
		}

	}
}

