using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Triangle 
{
    public readonly Vertex_hex a;
    public readonly Vertex_hex b;
    public readonly Vertex_hex c;

    public Triangle(Vertex_hex a, Vertex_hex b, Vertex_hex c, List<Triangle> triangles)
    {
        this.a = a;
        this.b = b;
        this.c = c;

        triangles.Add(this);    
    }

    /// <summary>
    /// Create triangles of ring radius and add to list triangles
    /// </summary>
    /// <param name="radius"></param>
    /// <param name="vertices"></param>
    /// <param name="triangles"></param>
    public static void triangles_Ring(int radius, List<Vertex_hex> vertices, List<Triangle> triangles)
    {
        
        List<Vertex_hex> inner = Vertex_hex.GrabRing(radius - 1, vertices); // get the inner ring vertex
        List<Vertex_hex> outer = Vertex_hex.GrabRing(radius, vertices); // get the outer ring vertex

        //for all 6 direction
        for(int i = 0; i < 6; i++)
        {
            //for all triangle of radius
            for (int j = 0; j < radius; j++)
            {
                //Yellow Triangle contain 1 inner point and two outer points
                Vertex_hex a = outer[i * radius + j];
                Vertex_hex b = outer[(i * radius + j + 1) % outer.Count];
                Vertex_hex c = inner[(i * (radius - 1) + j) % inner.Count];
                new Triangle(a, b, c, triangles);
                //Blue Triangle contain 1 inner point and two outer points
                if (j > 0)
                {
                    Vertex_hex d = inner[i * (radius - 1) + j - 1];
                    new Triangle(a, c, d, triangles);
                }
            }
        }   
    }

    /// <summary>
    /// Generate all triangle of grid in function of all vertice generated on grid
    /// </summary>
    /// <param name="vertices"></param>
    /// <param name="triangles"></param>
    public static void Triangles_Hex(List<Vertex_hex> vertices, List<Triangle> triangles)
    {
        for(int i = 1; i <= Grid.radius; i++)
        {
            triangles_Ring(i, vertices, triangles);
        }
    }
}
