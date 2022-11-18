using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quad 
{
    public readonly Vertex_hex a;
    public readonly Vertex_hex b;
    public readonly Vertex_hex c;
    public readonly Vertex_hex d;

    public readonly Edge ab;
    public readonly Edge bc;
    public readonly Edge cd;
    public readonly Edge da;

    public Vertex_quadCenter center;

    public Quad(Vertex_hex a, Vertex_hex b, Vertex_hex c, Vertex_hex d, List<Edge> edges, List<Quad> quads)
    {
        this.a = a;
        this.b = b;
        this.c = c;
        this.d = d;
        ab = Edge.FindEdge(a,b,edges);
        bc = Edge.FindEdge(b,c,edges);
        cd = Edge.FindEdge(c,d,edges);
        da = Edge.FindEdge(a,d,edges);
        quads.Add(this);
        center = new Vertex_quadCenter(this);
    }

    /// <summary>
    /// Add center point to quad
    /// </summary>
    /// <param name="centers"></param>
    public void AddCenter(List<Vertex_center> centers)
    {
        centers.Add(new Vertex_quadCenter(this));
    }

    /// <summary>
    /// Subdivise quad
    /// </summary>
    /// <param name="subQuads"></param>
    public void Subdivide(List<SubQuad> subQuads)
    {
        SubQuad quad_a = new SubQuad(a, ab.mid, center, da.mid);
        SubQuad quad_b = new SubQuad(b, bc.mid, center, ab.mid);
        SubQuad quad_c = new SubQuad(c, cd.mid, center, bc.mid);
        SubQuad quad_d = new SubQuad(d, da.mid, center, cd.mid);
        subQuads.Add(quad_a);
        subQuads.Add(quad_b);
        subQuads.Add(quad_c);
        subQuads.Add(quad_d);
    }
}
