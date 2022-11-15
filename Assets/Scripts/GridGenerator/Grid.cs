using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid 
{
    public static int radius;
    public readonly List<Vertex_hex> AllHex = new List<Vertex_hex>();
    public static int cellSize;

    public Grid(int radius, int cellSize)
    {
        Grid.radius = radius;
        Grid.cellSize = cellSize;
        Vertex_hex.Hex(AllHex);
    }


}