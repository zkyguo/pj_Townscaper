using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Triangle 
{
    public readonly Vertex_hex a;
    public readonly Vertex_hex b;
    public readonly Vertex_hex c;

    public readonly Edge ab;
    public readonly Edge bc;
    public readonly Edge ca;

    public readonly Edge[] edges;
    public readonly Vertex_hex[] vertices;

    public Vertex_triangleCenter center;

    public Triangle(Vertex_hex a, Vertex_hex b, Vertex_hex c, List<Edge> edges, List<Triangle> triangles)
    {
        this.a = a;
        this.b = b;
        this.c = c;

        vertices = new Vertex_hex[] { a, b, c };

        ab = Edge.FindEdge(a, b, edges);
        bc = Edge.FindEdge(b, c, edges);
        ca = Edge.FindEdge(c, a, edges);

        if(ab == null)
        {
            ab = new Edge(a, b, edges);
        }
        if(bc == null)
        {
            bc = new Edge(b, c, edges);
        }
        if(ca == null)
        {
            ca = new Edge(c, a, edges); 
        }
        this.edges = new Edge[] { ab, bc, ca };
        triangles.Add(this);
    }

    /// <summary>
    /// Create triangles of ring radius and add to list triangles
    /// </summary>
    /// <param name="radius"></param>
    /// <param name="vertices"></param>
    /// <param name="triangles"></param>
    public static void Triangles_Ring(int radius, List<Vertex_hex> vertices, List<Edge> edges, List<Triangle> triangles)
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
                new Triangle(a, b, c, edges, triangles);
                //Blue Triangle contain 1 inner point and two outer points
                if (j > 0)
                {
                    Vertex_hex d = inner[i * (radius - 1) + j - 1];
                    new Triangle(a, c, d, edges, triangles);
                }
            }
        }   
    }

    /// <summary>
    /// Generate all triangle of grid in function of all vertice generated on grid
    /// </summary>
    /// <param name="vertices"></param>
    /// <param name="triangles"></param>
    public static void Triangles_Hex(List<Vertex_hex> vertices, List<Edge> edges, List<Triangle> triangles)
    {
        for(int i = 1; i <= Grid.radius; i++)
        {
            Triangles_Ring(i, vertices, edges, triangles);
        }
    }

    /// <summary>
    /// Check if this and target triangle is neighbor
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public bool isNeighbor(Triangle target)
    {
        //Convert edges list to hashset
        HashSet<Edge> intersection = new HashSet<Edge>(edges);
        intersection.IntersectWith(target.edges);
        //if this and target Edges hashset has only 1 edge common. they are neighbor
        return intersection.Count == 1;
    }

    /// <summary>
    /// Find all neighbor triangle of this
    /// </summary>
    /// <param name="Triangles"></param>
    /// <returns></returns>
    public List<Triangle> FindAllNeighbor(List<Triangle> triangles)
    {
        List<Triangle> allNeighbor = new List<Triangle>();
        int maxNeighbor = 3;
        for (int i = 0; i < triangles.Count; i++)
        {
            if (this.isNeighbor(triangles[i]))
            {
                allNeighbor.Add(triangles[i]);  
            }
            if(allNeighbor.Count == maxNeighbor)
            {
                return allNeighbor;
            }
        }
        return allNeighbor;

    }

    /// <summary>
    /// Find the neighbor edge 
    /// </summary>
    /// <param name="neighbor"> </param>
    /// <returns></returns>
    public Edge NeighborEdge(Triangle neighbor)
    {
        //Convert edges list to hashset
        HashSet<Edge> intersection = new HashSet<Edge>(edges);
        intersection.IntersectWith(neighbor.edges);
        //Return the single common edge of both triangle
        return intersection.Single();
    }

    /// <summary>
    /// Find the self no common vertex of target neighbor
    /// </summary>
    /// <param name="neighbor"> target Neighbor of this triangle </param>
    /// <returns></returns>
    public Vertex_hex NoCommonVertex_Self(Triangle neighbor)
    {
        HashSet<Vertex_hex> except = new HashSet<Vertex_hex>(vertices);
        except.ExceptWith(neighbor.vertices);
        return except.Single();
    }

    /// <summary>
    /// Find the neighbor no common vertex 
    /// </summary>
    /// <param name="neighbor"> target Neighbor of this triangle </param>
    /// <returns></returns>
    public Vertex_hex NoCommonVertex_Neighbor(Triangle neighbor)
    {
        HashSet<Vertex_hex> except = new HashSet<Vertex_hex>(neighbor.vertices);
        except.ExceptWith(vertices);
        return except.Single();
    }

    /// <summary>
    /// Merge this triangle and neighbor triangle into a new quad
    /// </summary>
    /// <param name="neighbor"></param>
    /// <param name="edges"> all edge in grid</param>
    /// <param name="triangles"> all triangle in grid</param>
    /// <param name="quads"> all quad in grid</param>
    public void MergeNeighborTriangle(Triangle neighbor, List<Edge> edges, List<Triangle> triangles, List<Quad> quads, List<Vertex_mid> mids)
    {
        // Find the no common vertex of this triangles
        Vertex_hex a = NoCommonVertex_Self(neighbor);
        // Find the next vertex of preview vertex
        Vertex_hex b = vertices[(Array.IndexOf(vertices, a) + 1) % 3];
        // Find the no common vertex of neighbor triangle 
        Vertex_hex c = NoCommonVertex_Neighbor(neighbor);
        // Find the last vertex of Quad
        Vertex_hex d = neighbor.vertices[(Array.IndexOf(neighbor.vertices, c) + 1) % 3];

        Quad quad = new Quad(a, b, c, d, edges, quads);

        //Remove both triangle and edges 
        edges.Remove(NeighborEdge(neighbor));
        mids.Remove(NeighborEdge(neighbor).mid);
        triangles.Remove(this);
        triangles.Remove(neighbor);
    }

    /// <summary>
    /// Randomly merge triangles into quad, return if it still has  neighbour triangle in grid
    /// </summary>
    /// <param name="edges"></param>
    /// <param name="triangles"></param>
    /// <param name="quads"></param>
    public static bool RandomlyMergeTriangles(List<Edge> edges, List<Triangle> triangles, List<Quad> quads,List<Vertex_mid> mids)
    {
        if (triangles.Count == 0) return false;
        //Pick a random triangle
        int randomIndex = UnityEngine.Random.Range(0, triangles.Count);
        Triangle randomTriangle = triangles[randomIndex];

        //Find all neigbor of this triangle 
        List<Triangle> neighbors = randomTriangle.FindAllNeighbor(triangles);
        //if this triangle has neighbor 
        if(neighbors.Count != 0)
        {
            //Pick a random neighbor
            int randomNeighborIndex = UnityEngine.Random.Range(0, neighbors.Count);
            randomTriangle.MergeNeighborTriangle(neighbors[randomNeighborIndex], edges, triangles, quads, mids);
            return true;
        }

        return HasNeighborTriangles(triangles); 

    }

    /// <summary>
    /// Check if it still has triangles who has neighbour on grid
    /// </summary>
    /// <param name="triangles"></param>
    /// <returns></returns>
    public static bool HasNeighborTriangles(List<Triangle> triangles)
    {
        for (int i = 0; i < triangles.Count; i++)
        {
            if (triangles[i].FindAllNeighbor(triangles).Count > 0) return true;
        }

        return false;
    }

    /// <summary>
    /// Add center point to triangle
    /// </summary>
    /// <param name="centers"></param>
    public void AddCenterPoint(List<Vertex_center> centers)
    {
        center = new Vertex_triangleCenter(this);
        centers.Add(center);
    }
    
    /// <summary>
    /// Subdivise triangle
    /// </summary>
    /// <param name="subQuads"></param>
    public void Subdivide(List<SubQuad> subQuads)
    {
        SubQuad quad_a = new SubQuad(a, ab.mid, center, ca.mid);
        SubQuad quad_b = new SubQuad(b, bc.mid, center, ab.mid);
        SubQuad quad_c = new SubQuad(c, ca.mid, center, bc.mid);
        subQuads.Add(quad_a);
        subQuads.Add(quad_b);
        subQuads.Add(quad_c);
    }
}

