using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class GridGenerator : MonoBehaviour
{
    [SerializeField]
    private int radius;
    [SerializeField]
    private float cellsize;

    private Grid grid;
    private void Awake()
    {
        grid = new Grid(radius, cellsize);
    }

    private void OnDrawGizmos()
    {
        if(grid != null)
        {
            foreach (Vertex_hex vertex in grid.allHex)
            {
                Gizmos.DrawSphere(vertex.coord.worldPosition, 0.05f);
            }
            Gizmos.color = Color.yellow;
            foreach(Triangle triangle in grid.triangles)
            {
                Gizmos.DrawLine(triangle.a.coord.worldPosition, triangle.b.coord.worldPosition);
                Gizmos.DrawLine(triangle.b.coord.worldPosition, triangle.c.coord.worldPosition);
                Gizmos.DrawLine(triangle.c.coord.worldPosition, triangle.a.coord.worldPosition);
                Gizmos.DrawSphere((triangle.a.coord.worldPosition + triangle.b.coord.worldPosition + triangle.c.coord.worldPosition) / 3, 0.05f);
            }
            Gizmos.color = Color.white;
            foreach (Quad quad in grid.quads)
            {
                Gizmos.DrawLine(quad.a.coord.worldPosition, quad.b.coord.worldPosition);
                Gizmos.DrawLine(quad.b.coord.worldPosition, quad.c.coord.worldPosition);
                Gizmos.DrawLine(quad.c.coord.worldPosition, quad.d.coord.worldPosition);
                Gizmos.DrawLine(quad.d.coord.worldPosition, quad.a.coord.worldPosition);
            }

        }
        
    }
}
