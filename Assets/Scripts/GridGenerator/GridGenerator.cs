using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField]
    private int radius;
    [SerializeField]
    private int cellsize;

    private Grid grid;
    private void Awake()
    {
        grid = new Grid(radius, cellsize);
    }

    private void OnDrawGizmos()
    {
        if(grid != null)
        {
            foreach (Vertex_hex vertex in grid.AllHex)
            {
                Gizmos.DrawSphere(vertex.coord.worldPosition, 0.3f);
            }

        }
        
    }
}
