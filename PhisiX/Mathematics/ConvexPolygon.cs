using System;
using System.Collections;
using Microsoft.Xna.Framework;

namespace PhisiX.Mathematics
{
	public class ConvexPolygon
	{
		ArrayList Vertices { get; set; }
		ArrayList Edges { get; set;}
		ArrayList HalfPlanes { get; set; }

		public ConvexPolygon (ArrayList vertices)
		{
			Vertices = vertices;
			ArrayList edges = new ArrayList ();
			ArrayList halfPlanes = new ArrayList ();

			for (int i = 0; i < vertices.Count; i++) {
				int j = (i + 1) % vertices.Count;

				Vector2 edge = Vector2.Subtract ((Vector2)vertices [j], (Vector2)vertices [i]);
				edges.Add (edge);

				Vector2 normal = Vector2.Normalize(new Vector2 (edge.Y, -edge.X));
				float distance = Vector2.Dot ((Vector2)vertices[i], normal);
				halfPlanes.Add (new HalfPlane(normal,distance));

			}

			Edges = edges;
			HalfPlanes = halfPlanes;

		}

		public static ConvexPolygon convexPolygon(ArrayList vertices){
			return new ConvexPolygon (vertices);
		}
	}
}

