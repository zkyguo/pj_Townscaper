using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UIElements;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class GridGenerator : MonoBehaviour
{
    [SerializeField]
    private int radius;
    [SerializeField]
    private float cellsize;
    [SerializeField]
    private int smoothIteration;
    [SerializeField]
    public float cellHeight;
    [SerializeField]
    public int height;
    [SerializeField]
    private bool showSmooth;
    [SerializeField]
    private bool showVerticalVertices;
    [SerializeField]
    private bool showVertices;
    [SerializeField]
    ModuleLibrary moduleLibrary;
    [SerializeField]
    Material moduleMaterial;

    public Transform testSphere;


    private Grid grid;
    private void Awake()
    {
        grid = new Grid(radius, cellsize, smoothIteration, cellHeight, height);
        moduleLibrary = Instantiate(moduleLibrary);
    }

    private void Update()
    {
        if(testSphere != null && grid != null)
        {
            foreach (Vertex vertex in grid.vertices)
            {
                foreach (Vertex_Y vertexY in vertex.verticesY)
                {
                    if(vertexY.isActive && Vector3.Distance(vertexY.worldPosition, testSphere.position) > 3f)
                    {
                        vertexY.isActive = false;
                    }
                    else if(!vertexY.isActive && Vector3.Distance(vertexY.worldPosition, testSphere.position) < 3f && !vertexY.isBoundary)
                    {
                        vertexY.isActive = true;    
                    }
                }
                //Initialize each slot of cube
                foreach (SubQuad subquad in grid.subQuads)
                {
                    foreach (SubQuad_Cube cube in subquad.cubes)
                    {
                        cube.UpdateBit();
                        if(cube.LastBits != cube.Currentbits)
                        {
                            UpdateSlot(cube);
                        }
                    }
                }
            }
            
        }
    }

    private void UpdateSlot(SubQuad_Cube subQuad_Cube)
    {
        string name = "Slot_" + grid.subQuads.IndexOf(subQuad_Cube.subQuad) + "_" + subQuad_Cube.y;
        GameObject slot_gameobject = null;
        //if slot has an gameobject
        if(transform.Find(name))
        {
            slot_gameobject = transform.Find(name).gameObject;
        }
        //if not, create one
        if(slot_gameobject == null)
        {
            if(subQuad_Cube.Currentbits != "00000000" && subQuad_Cube.Currentbits != "11111111")
            {
                slot_gameobject = new GameObject(name, typeof(Slot));
                slot_gameobject.transform.parent = transform;
                slot_gameobject.transform.localPosition = subQuad_Cube.CenterPosition;
                Slot slot = slot_gameobject.GetComponent<Slot>();
                slot.Initialized(moduleLibrary, subQuad_Cube, moduleMaterial);
                slot.UpdateModule(slot.possibleModules[0]);
            }
            
        }
        else
        {
            Slot slot = slot_gameobject.GetComponent<Slot>();
            //if current bit is void 
            if(subQuad_Cube.Currentbits.Equals("00000000") || subQuad_Cube.Currentbits.Equals("11111111"))
            {
                Destroy(slot_gameobject);
                Resources.UnloadUnusedAssets();
            }
            else
            {
                slot.ResetPossibleModules(moduleLibrary);
                slot.UpdateModule(slot.possibleModules[0]);
            }
        }
    }
    private void OnDrawGizmos()
    {
        if(grid != null)
        {
            if (showVertices)
            {
                Gizmos.color = Color.blue;
                foreach (Vertex vertex in grid.vertices)
                {
                    Gizmos.DrawSphere(vertex.currentPosition, 0.15f);
                }

                return;
            }
            if (showVerticalVertices)
            {              
                /*foreach (Vertex vertex in grid.vertices)
                {
                    foreach(Vertex_Y vertexY in vertex.verticesY)
                    {
                        if (vertexY.isActive)
                        {
                            Gizmos.color = Color.red;
                        }
                        else
                        {
                            Gizmos.color = Color.grey;
                        }
                            
                        Gizmos.DrawSphere(vertexY.worldPosition, 0.15f);
                    }
                }*/
                foreach (SubQuad subquad in grid.subQuads)
                {
                    foreach (SubQuad_Cube cube in subquad.cubes)
                    {
                        cube.UpdateBit();
                        Gizmos.color = Color.gray;
                        Gizmos.DrawLine(cube.vertexYs[0].worldPosition, cube.vertexYs[1].worldPosition);
                        Gizmos.DrawLine(cube.vertexYs[1].worldPosition, cube.vertexYs[2].worldPosition);
                        Gizmos.DrawLine(cube.vertexYs[2].worldPosition, cube.vertexYs[3].worldPosition);
                        Gizmos.DrawLine(cube.vertexYs[3].worldPosition, cube.vertexYs[0].worldPosition);
                        Gizmos.DrawLine(cube.vertexYs[4].worldPosition, cube.vertexYs[5].worldPosition);
                        Gizmos.DrawLine(cube.vertexYs[5].worldPosition, cube.vertexYs[6].worldPosition);
                        Gizmos.DrawLine(cube.vertexYs[6].worldPosition, cube.vertexYs[7].worldPosition);
                        Gizmos.DrawLine(cube.vertexYs[7].worldPosition, cube.vertexYs[4].worldPosition);
                        Gizmos.DrawLine(cube.vertexYs[0].worldPosition, cube.vertexYs[4].worldPosition);
                        Gizmos.DrawLine(cube.vertexYs[1].worldPosition, cube.vertexYs[5].worldPosition);
                        Gizmos.DrawLine(cube.vertexYs[2].worldPosition, cube.vertexYs[6].worldPosition);
                        Gizmos.DrawLine(cube.vertexYs[3].worldPosition, cube.vertexYs[7].worldPosition);


                        /*Gizmos.color = Color.blue;
                        Gizmos.DrawSphere(cube.CenterPosition, 0.15f);

                        GUI.color = Color.blue;
                        Handles.Label(cube.CenterPosition, cube.Currentbits);*/

                    }
                }
                return;
            }
            if (!showSmooth)
            {
                foreach (Vertex_hex vertex in grid.allHex)
                {
                    Gizmos.DrawSphere(vertex.coord.worldPosition, 0.15f);
                }
                Gizmos.color = Color.yellow;
                foreach (Triangle triangle in grid.triangles)
                {
                    Gizmos.DrawLine(triangle.a.coord.worldPosition, triangle.b.coord.worldPosition);
                    Gizmos.DrawLine(triangle.b.coord.worldPosition, triangle.c.coord.worldPosition);
                    Gizmos.DrawLine(triangle.c.coord.worldPosition, triangle.a.coord.worldPosition);
                    Gizmos.DrawSphere((triangle.a.coord.worldPosition + triangle.b.coord.worldPosition + triangle.c.coord.worldPosition) / 3, 0.15f);
                }
                Gizmos.color = Color.white;
                foreach (Quad quad in grid.quads)
                {
                    Gizmos.DrawLine(quad.a.coord.worldPosition, quad.b.coord.worldPosition);
                    Gizmos.DrawLine(quad.b.coord.worldPosition, quad.c.coord.worldPosition);
                    Gizmos.DrawLine(quad.c.coord.worldPosition, quad.d.coord.worldPosition);
                    Gizmos.DrawLine(quad.d.coord.worldPosition, quad.a.coord.worldPosition);
                }
                Gizmos.color = Color.red;
                foreach (Vertex_mid mid in grid.mids)
                {
                    Gizmos.DrawSphere(mid.initialPosition, 0.15f);
                }
                Gizmos.color = Color.blue;
                foreach (Vertex_center center in grid.centers)
                {
                    Gizmos.DrawSphere(center.initialPosition, 0.15f);
                }
                Gizmos.color = Color.white;
                foreach (SubQuad sub in grid.subQuads)
                {
                    Gizmos.DrawLine(sub.a.initialPosition, sub.b.initialPosition);
                    Gizmos.DrawLine(sub.b.initialPosition, sub.c.initialPosition);
                    Gizmos.DrawLine(sub.c.initialPosition, sub.d.initialPosition);
                    Gizmos.DrawLine(sub.d.initialPosition, sub.a.initialPosition);
                }
                return;
            }
            if(showSmooth)
            {
                foreach (Vertex_hex vertex in grid.allHex)
                {
                    Gizmos.DrawSphere(vertex.currentPosition, 0.15f);
                }
                /*Gizmos.color = Color.yellow;
                foreach (Triangle triangle in grid.triangles)
                {
                    Gizmos.DrawLine(triangle.a.currentPosition, triangle.b.currentPosition);
                    Gizmos.DrawLine(triangle.b.currentPosition, triangle.c.currentPosition);
                    Gizmos.DrawLine(triangle.c.currentPosition, triangle.a.currentPosition);
                    Gizmos.DrawSphere((triangle.a.currentPosition + triangle.b.currentPosition + triangle.c.currentPosition) / 3, 0.15f);
                }*/
                /*Gizmos.color = Color.white;
                foreach (Quad quad in grid.quads)
                {
                    Gizmos.DrawLine(quad.a.currentPosition, quad.b.currentPosition);
                    Gizmos.DrawLine(quad.b.currentPosition, quad.c.currentPosition);
                    Gizmos.DrawLine(quad.c.currentPosition, quad.d.currentPosition);
                    Gizmos.DrawLine(quad.d.currentPosition, quad.a.currentPosition);
                }*/
                Gizmos.color = Color.red;
                foreach (Vertex_mid mid in grid.mids)
                {
                    Gizmos.DrawSphere(mid.currentPosition, 0.15f);
                }
                Gizmos.color = Color.blue;
                foreach (Vertex_center center in grid.centers)
                {
                    Gizmos.DrawSphere(center.currentPosition, 0.15f);
                }
                Gizmos.color = Color.white;
                foreach (SubQuad sub in grid.subQuads)
                {
                    Gizmos.DrawLine(sub.a.currentPosition, sub.b.currentPosition);
                    Gizmos.DrawLine(sub.b.currentPosition, sub.c.currentPosition);
                    Gizmos.DrawLine(sub.c.currentPosition, sub.d.currentPosition);
                    Gizmos.DrawLine(sub.d.currentPosition, sub.a.currentPosition);
                }

                return;
            }
 

            
        }
        
    }
}
