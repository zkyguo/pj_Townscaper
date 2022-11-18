using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Grid 
{
    public static int radius;
    public static float cellSize;

    public readonly List<Triangle> triangles = new List<Triangle>();
    public readonly List<Vertex_hex> allHex = new List<Vertex_hex>();
    public readonly List<Vertex_mid> mids = new List<Vertex_mid>();
    public readonly List<Vertex_center> centers = new List<Vertex_center>();
    public readonly List<Edge> edges = new List<Edge>();
    public readonly List<Quad> quads = new List<Quad>();
    public readonly List<SubQuad> subQuads = new List<SubQuad>();

    public Grid(int radius, float cellSize)
    {
        Grid.radius = radius;
        Grid.cellSize = cellSize;
        Vertex_hex.Hex(allHex);
        Triangle.Triangles_Hex(allHex, edges, triangles);

        while (Triangle.RandomlyMergeTriangles(edges, triangles, quads));

        AddMids(mids, edges);
        addCenterAndSubdivise(centers, triangles, quads, subQuads);
    }

    private void AddMids(List<Vertex_mid> mids, List<Edge> edges)
    {
        for (int i = 0; i < edges.Count; i++)
        {
            edges[i].addMid(mids);
        }
    }

    private void addCenterAndSubdivise(List<Vertex_center> centers, List<Triangle> triangles, List<Quad> quads, List<SubQuad> subQuads)
    {
        for (int i = 0; i < triangles.Count; i++)
        {
            Triangle triangle = triangles[i];
            triangle.AddCenterPoint(centers);
            triangle.Subdivide(subQuads);
        }

        for(int i = 0; i < quads.Count; i++) 
        {
            Quad quad = quads[i];
            quad.AddCenter(centers);
            quad.Subdivide(subQuads);
        }
    }

    
}
