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

    /// <summary>
    /// Caculate Relax offset of subQuad
    /// </summary>
    public void CalculateSmoothOffset()
    {
        //Find the center of relax
        Vector3 center = (a.currentPosition + b.currentPosition + c.currentPosition + d.currentPosition) / 4;

        //We want to have a square which 4 edge are equals, so we have to find the vector with average length of all vector(vertex to center)
        Vector3 vector_a = ((a.currentPosition) + Quaternion.AngleAxis(-90, Vector3.up) * (b.currentPosition - center) + center
                                                + Quaternion.AngleAxis(-180, Vector3.up) * (c.currentPosition - center) + center
                                                + Quaternion.AngleAxis(-270, Vector3.up) * (d.currentPosition - center) + center ) /4;

        //Vector b,c,d will have the same length of a, so we just need to rotate them 
        Vector3 vector_b = Quaternion.AngleAxis(90, Vector3.up) * (vector_a - center) + center;
        Vector3 vector_c = Quaternion.AngleAxis(180, Vector3.up) * (vector_a - center) + center;
        Vector3 vector_d = Quaternion.AngleAxis(270, Vector3.up) * (vector_a - center) + center;

        //Optional : Make a offset
        a.offset += (vector_a - a.currentPosition) * 0.1f;
        b.offset += (vector_b - b.currentPosition) * 0.1f;
        c.offset += (vector_c - c.currentPosition) * 0.1f;
        d.offset += (vector_d - d.currentPosition) * 0.1f;
    }
}
