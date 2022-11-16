using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Vertex 
{
    



}

public class Vertex_hex : Vertex
{
    public readonly Coord coord;

    public Vertex_hex(Coord coord)
    {
        this.coord = coord;
    }

    /// <summary>
    /// Initiate all hex coord vertices inside of radius
    /// </summary>
    /// <param name="vertices"></param>
    /// <param name="radius"></param>
    public static void Hex(List<Vertex_hex> vertices)
    {
        foreach (Coord coord in Coord.Coord_Hex(Grid.radius))
        {
            vertices.Add(new Vertex_hex(coord));
        }
    }

    /// <summary>
    /// find all vertex of ring of radius
    /// </summary>
    /// <param name="radius"></param>
    /// <param name="vertices"></param>
    /// <returns></returns>
    public static List<Vertex_hex> GrabRing(int radius, List<Vertex_hex> vertices)
    {
        if (radius == 0) return vertices.GetRange(0, 1);
        //all vertices = first vertices of ring + radius * 6
        return vertices.GetRange(3 *(radius - 1) * radius  + 1, radius * 6);

    }
}

public class Coord
{
    private readonly int q;
    private readonly int r;
    private readonly int s;

    public readonly Vector3 worldPosition;
    public Coord(int q, int r, int s)
    {
        this.q = q;
        this.r = r;
        this.s = s;
        worldPosition = ToWorldPosition();
    }

    /// <summary>
    /// Convert q,r,s to world Position x,y,z
    /// </summary>
    /// <param name="q"></param>
    /// <param name="r"></param>
    /// <param name="s"></param>
    /// <returns></returns>
    public Vector3 ToWorldPosition()
    {
        return new Vector3(q*MathF.Sqrt(3)/2, 0, -(float)r - ((float)q/2)) * 2 * Grid.cellSize;
    }

    /// <summary>
    /// Default creation direction of grid
    /// </summary>
    static public Coord[] directions = new Coord[]
    {
        //Direction of creation is clockwise
        new Coord(0,1,-1),
        new Coord(-1,1,0),
        new Coord(-1,0,1),
        new Coord(0,-1,1),
        new Coord(1,-1,0),
        new Coord(1,0,-1)

    };

    /// <summary>
    /// Get specify direction
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    static public Coord Direction(int direction)
    {
        return directions[direction];   
    }

    /// <summary>
    /// Add two coord 
    /// </summary>
    /// <param name="coord"></param>
    /// <returns></returns>
    public Coord Add(Coord coord)
    {
        return new Coord(q+coord.q,r+coord.r, s+coord.s);
    }

    /// <summary>
    /// Find the neighbor of coord by direction
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public Coord Neighbor(int direction)
    {
        return Add(Direction(direction));
    }

    /// <summary>
    /// Find the ring(list of coord) by radius
    /// </summary>
    /// <param name="radius"></param>
    /// <returns></returns>
    public static List<Coord> Coord_Ring(int radius)
    {
        List<Coord> result = new List<Coord>();
        if(radius == 0)
        {
            result.Add(new Coord(0, 0, 0));
            return result;
        }
        else
        {
            //Set the frist Hex of ring in direction 4 by radius 
            Coord coord = Coord.Direction(4).Scale(radius);

            //foreach direction of Hex
            for(int i = 0; i < 6; i++)
            {
                //Add hex unit on edge( according to radius)
                for(int j = 0; j < radius; j++)
                {
                    result.Add(coord);
                    //Move to next coord of direction i
                    coord = coord.Neighbor(i);
                }
            }
        }
        return result;
    }

    /// <summary>
    /// Find all hex within radius
    /// </summary>
    /// <returns></returns>
    public static List<Coord> Coord_Hex(int radius)
    {
        List<Coord> result = new List<Coord>();
        //for all radius inside
        for(int i = 0; i <= radius; i++)
        {
            //add all hex of radius i
            result.AddRange(Coord_Ring(i));
        }
        return result;
    }

    /// <summary>
    /// Find the hex by factor
    /// </summary>
    /// <param name="factor"></param>
    /// <returns></returns>
    private Coord Scale(int factor)
    {
        return new Coord(q*factor,r*factor,s*factor);
    }
}