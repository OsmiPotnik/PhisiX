using System;
using PhisiX.Colliders;
using Microsoft.Xna.Framework;

namespace PhisiX
{
	public class ParticleParticleCollision : CollisionAlgorithm
	{
		public static bool DetectCollisionBetween(IParticleCollider particle1, IParticleCollider particle2){
			Console.WriteLine (particle1.Position.ToString());
			Console.WriteLine (particle2.Position.ToString());
			float distanceBetweenParticles = Vector2.Subtract (particle1.Position, particle2.Position).Length ();
			Console.WriteLine (distanceBetweenParticles);
			return distanceBetweenParticles < particle1.Radius + particle1.Radius;
		}

		public static void ResolveCollisionBetween(IParticleCollider particle1, IParticleCollider particle2){
			Vector2 positionDifference = Vector2.Subtract (particle2.Position, particle1.Position);
			float collidedDistance = positionDifference.Length ();
			float minimumDistance = particle1.Radius + particle2.Radius;
			float relaxDistance = minimumDistance - collidedDistance;
			Vector2 collisionNormal = collidedDistance != 0 ? Vector2.Normalize (new Vector2 (positionDifference.X, positionDifference.Y)) : Vector2.UnitX;
			Vector2 relaxDistanceVector = Vector2.Multiply (collisionNormal,relaxDistance);
			Collision.RelaxCollisionBetween (particle1,particle1,relaxDistanceVector);

			Collision.ExchangeEnergyBetween (particle1, particle2, collisionNormal, Vector2.Zero);

		}

		public void  CollisionBetween(IParticleCollider item1, IParticleCollider item2){
			//base.CollisionBetween (item1,item2);
			if (ParticleParticleCollision.DetectCollisionBetween (item1, item2)) {
				Console.WriteLine ("Collided");
				ParticleParticleCollision.ResolveCollisionBetween (item1, item2);
			}

		}


	}
}

