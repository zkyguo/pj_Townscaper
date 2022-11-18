using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubQuad 
{
    public readonly Vertex_hex a;
    public readonly Vertex_mid b;
    public readonly Vertex_center c;
    public readonly Vertex_mid d;

    public SubQuad(Vertex_hex a, Vertex_mid b, Vertex_center c, Vertex_mid d)
    {
        this.a = a;
        this.b = b;
        this.c = c;
        this.d = d;
    }
}
