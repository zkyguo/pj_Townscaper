using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid 
{
    public static int radius;
    public readonly List<Vertex_hex> AllHex = new List<Vertex_hex>();
    public static float cellSize;
    public readonly List<Triangle> triangles = new List<Triangle>();    

    public Grid(int radius, float cellSize)
    {
        Grid.radius = radius;
        Grid.cellSize = cellSize;
        Vertex_hex.Hex(AllHex);
        Triangle.Triangles_Hex(AllHex, triangles);
    }


}
