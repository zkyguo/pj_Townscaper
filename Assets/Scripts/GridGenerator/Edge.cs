using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;

public class Edge 
{
    public readonly HashSet<Vertex_hex> Hexes;
    public Vertex_mid mid;

    /// <summary>
    /// Create an edge by two vertex a,b and add this to allEdge list
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="edges"> all edge of grid </param>
    public Edge(Vertex_hex a, Vertex_hex b, List<Edge> edges)
    {
        Hexes = new HashSet<Vertex_hex> { a,b};
        edges.Add(this);
        
    }

    public void addMid(List<Vertex_mid> mids)
    {
        mid = new Vertex_mid(this, mids);
    }

    /// <summary>
    /// Find the edge which contains vertex a and b.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="edges"> all edge in grid </param>
    /// <returns></returns>
    public static Edge FindEdge(Vertex_hex a, Vertex_hex b, List<Edge> edges)
    {
        for (int i = 0; i < edges.Count; i++)
        {
            if (edges[i].Hexes.Contains(a) && edges[i].Hexes.Contains(b))
            {
                return edges[i];
            }
        }

        return null;
    }
}
