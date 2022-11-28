using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubQuad 
{
    public readonly Vertex_hex a;
    public readonly Vertex_mid b;
    public readonly Vertex_center c;
    public readonly Vertex_mid d;
    public List<SubQuad_Cube> cubes = new List<SubQuad_Cube>();
    public Vector3 CenterPosition = new Vector3();

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
        CenterPosition = center;

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

/// <summary>
/// Cube create from subquad
/// </summary>
public class SubQuad_Cube
{
    public readonly SubQuad subQuad;
    public readonly int y;
    public readonly Vertex_Y[] vertexYs = new Vertex_Y[8];
    public string bits = "00000000";
    public Vector3 CenterPosition;
    public SubQuad_Cube(SubQuad subQuad, int y)
    {
        this.subQuad = subQuad;
        this.y = y;

        CenterPosition = subQuad.CenterPosition + Vector3.up * Grid.cellHeight * ( y + 0.5f);

        vertexYs[0] = subQuad.a.verticesY[y + 1];
        vertexYs[1] = subQuad.b.verticesY[y + 1];
        vertexYs[2] = subQuad.c.verticesY[y + 1];
        vertexYs[3] = subQuad.d.verticesY[y + 1];

        vertexYs[4] = subQuad.a.verticesY[y];
        vertexYs[5] = subQuad.b.verticesY[y];
        vertexYs[6] = subQuad.c.verticesY[y];
        vertexYs[7] = subQuad.d.verticesY[y];

        UpdateBit();
    }

    public void UpdateBit()
    {
        string result = "";
        if (vertexYs[0].isActive) result += "1";
        else result += "0";
        if (vertexYs[1].isActive) result += "1";
        else result += "0";
        if (vertexYs[2].isActive) result += "1";
        else result += "0";
        if (vertexYs[3].isActive) result += "1";
        else result += "0";
        if (vertexYs[4].isActive) result += "1";
        else result += "0";
        if (vertexYs[5].isActive) result += "1";
        else result += "0";
        if (vertexYs[6].isActive) result += "1";
        else result += "0";
        if (vertexYs[7].isActive) result += "1";
        else result += "0";

        bits = result;

    }
}
