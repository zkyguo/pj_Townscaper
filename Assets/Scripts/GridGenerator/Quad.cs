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

    }
}
