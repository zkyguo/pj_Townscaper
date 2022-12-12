using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class which contains basic data of an module
/// </summary>
public class Module
{
    public string name;
    public Mesh mesh;
    public int rotation;
    public bool flip;

    public Module(string name, Mesh mesh, int rotation, bool flip)
    {
        this.name = name;
        this.mesh = mesh;
        this.rotation = rotation;
        this.flip = flip;
    }
}
