using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid 
{
    public static int radius;
    public static float cellSize;

    public readonly List<Triangle> triangles = new List<Triangle>();
    public readonly List<Vertex_hex> allHex = new List<Vertex_hex>();
    public readonly List<Edge> edges = new List<Edge>();
    public readonly List<Quad> quads = new List<Quad>();

    public Grid(int radius, float cellSize)
    {
        Grid.radius = radius;
        Grid.cellSize = cellSize;
        Vertex_hex.Hex(allHex);
        Triangle.Triangles_Hex(allHex, edges, triangles);

        while (Triangle.RandomlyMergeTriangles(edges, triangles, quads));
    }


}
