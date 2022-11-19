using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UIElements;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class GridGenerator : MonoBehaviour
{
    [SerializeField]
    private int radius;
    [SerializeField]
    private float cellsize;
    [SerializeField]
    private int smoothTime;
    [SerializeField]
    private bool showSmooth;


    private Grid grid;
    private void Awake()
    {
        grid = new Grid(radius, cellsize, smoothTime);
    }

    private void Update()
    {
        if(Time.frameCount % 120 == 0)
            { return; }
        if (smoothTime > 0 )
        {
            for (int i = 0; i < grid.subQuads.Count; i++)
            {
                grid.subQuads[i].CalculateSmoothOffset();
            }
            for (int i = 0; i < grid.vertices.Count; i++)
            {
                grid.vertices[i].Smooth();
            }
            smoothTime -= 1;
        }
        
        
    }

    private void OnDrawGizmos()
    {
        if(grid != null)
        {
            if (!showSmooth)
            {
                foreach (Vertex_hex vertex in grid.allHex)
                {
                    Gizmos.DrawSphere(vertex.coord.worldPosition, 0.15f);
                }
                Gizmos.color = Color.yellow;
                foreach (Triangle triangle in grid.triangles)
                {
                    Gizmos.DrawLine(triangle.a.coord.worldPosition, triangle.b.coord.worldPosition);
                    Gizmos.DrawLine(triangle.b.coord.worldPosition, triangle.c.coord.worldPosition);
                    Gizmos.DrawLine(triangle.c.coord.worldPosition, triangle.a.coord.worldPosition);
                    Gizmos.DrawSphere((triangle.a.coord.worldPosition + triangle.b.coord.worldPosition + triangle.c.coord.worldPosition) / 3, 0.15f);
                }
                Gizmos.color = Color.white;
                foreach (Quad quad in grid.quads)
                {
                    Gizmos.DrawLine(quad.a.coord.worldPosition, quad.b.coord.worldPosition);
                    Gizmos.DrawLine(quad.b.coord.worldPosition, quad.c.coord.worldPosition);
                    Gizmos.DrawLine(quad.c.coord.worldPosition, quad.d.coord.worldPosition);
                    Gizmos.DrawLine(quad.d.coord.worldPosition, quad.a.coord.worldPosition);
                }
                Gizmos.color = Color.red;
                foreach (Vertex_mid mid in grid.mids)
                {
                    Gizmos.DrawSphere(mid.initialPosition, 0.15f);
                }
                Gizmos.color = Color.blue;
                foreach (Vertex_center center in grid.centers)
                {
                    Gizmos.DrawSphere(center.initialPosition, 0.15f);
                }
                Gizmos.color = Color.white;
                foreach (SubQuad sub in grid.subQuads)
                {
                    Gizmos.DrawLine(sub.a.initialPosition, sub.b.initialPosition);
                    Gizmos.DrawLine(sub.b.initialPosition, sub.c.initialPosition);
                    Gizmos.DrawLine(sub.c.initialPosition, sub.d.initialPosition);
                    Gizmos.DrawLine(sub.d.initialPosition, sub.a.initialPosition);
                }
            }
            else
            {
                foreach (Vertex_hex vertex in grid.allHex)
                {
                    Gizmos.DrawSphere(vertex.currentPosition, 0.15f);
                }
                /*Gizmos.color = Color.yellow;
                foreach (Triangle triangle in grid.triangles)
                {
                    Gizmos.DrawLine(triangle.a.currentPosition, triangle.b.currentPosition);
                    Gizmos.DrawLine(triangle.b.currentPosition, triangle.c.currentPosition);
                    Gizmos.DrawLine(triangle.c.currentPosition, triangle.a.currentPosition);
                    Gizmos.DrawSphere((triangle.a.currentPosition + triangle.b.currentPosition + triangle.c.currentPosition) / 3, 0.15f);
                }*/
                /*Gizmos.color = Color.white;
                foreach (Quad quad in grid.quads)
                {
                    Gizmos.DrawLine(quad.a.currentPosition, quad.b.currentPosition);
                    Gizmos.DrawLine(quad.b.currentPosition, quad.c.currentPosition);
                    Gizmos.DrawLine(quad.c.currentPosition, quad.d.currentPosition);
                    Gizmos.DrawLine(quad.d.currentPosition, quad.a.currentPosition);
                }*/
                Gizmos.color = Color.red;
                foreach (Vertex_mid mid in grid.mids)
                {
                    Gizmos.DrawSphere(mid.currentPosition, 0.15f);
                }
                Gizmos.color = Color.blue;
                foreach (Vertex_center center in grid.centers)
                {
                    Gizmos.DrawSphere(center.currentPosition, 0.15f);
                }
                Gizmos.color = Color.white;
                foreach (SubQuad sub in grid.subQuads)
                {
                    Gizmos.DrawLine(sub.a.currentPosition, sub.b.currentPosition);
                    Gizmos.DrawLine(sub.b.currentPosition, sub.c.currentPosition);
                    Gizmos.DrawLine(sub.c.currentPosition, sub.d.currentPosition);
                    Gizmos.DrawLine(sub.d.currentPosition, sub.a.currentPosition);
                }
            }
            
        }
        
    }
}
