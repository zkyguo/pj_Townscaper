using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;

[CreateAssetMenu(menuName ="ScriptableObject/ModuleLibrary")]
public class ModuleLibrary : ScriptableObject
{
    [SerializeField]
    private GameObject importedModules;
    private Dictionary<string, List<Module>> moduleLibrary = new Dictionary<string, List<Module>>();

    private void Awake()
    {
        ImportedModule();
    }

    public void ImportedModule()
    {
        for (int i = 0; i < 256; i++)
        {
            moduleLibrary.Add(Convert.ToString(i, 2).PadLeft(8, '0'), new List<Module>());
        }
        foreach (Transform child in importedModules.transform)
        {
            Mesh mesh = child.GetComponent<MeshFilter>().sharedMesh;
            string name = child.name;
            moduleLibrary[name].Add(new Module(name, mesh, 0, false));
            //Find rotated variant of mesh
            if(!RightAngleRotateCheck(name))
            {
                moduleLibrary[rotateName(name, 1)].Add(new Module(rotateName(name, 1), mesh, 1, false));
                if(!StraightAngleRotateCheck(name))
                {
                    moduleLibrary[rotateName(name,2)].Add(new Module(rotateName(name, 2), mesh, 2, false));
                    moduleLibrary[rotateName(name,3)].Add(new Module(rotateName(name, 3), mesh, 3, false));
                    if(!SymetricEqualCheck(name))
                    {
                        moduleLibrary[flipName(name)].Add(new Module(flipName(name), mesh, 0, true));
                        moduleLibrary[rotateName(flipName(name), 1)].Add(new Module(rotateName(flipName(name), 1), mesh, 1, true));
                        moduleLibrary[rotateName(flipName(name), 2)].Add(new Module(rotateName(flipName(name), 2), mesh, 2, true));
                        moduleLibrary[rotateName(flipName(name), 3)].Add(new Module(rotateName(flipName(name), 3), mesh, 3, true));
                    }
                }
            }
        }
    }

    public List<Module> getModules(string name)
    {
        return moduleLibrary[name];
    }

    /// <summary>
    /// Rotate mesh name to perrform rotation check
    /// </summary>
    /// <param name="name"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    private string rotateName(string name, int time)
    {
        string result = name;
        for (int i = 0; i < time; i++)
        {
            result = result[3] + result.Substring(0, 3) + result[7] + result.Substring(4, 3);
        }
        return result;
    }
        
    private string flipName(string name)
    {
        return name[3].ToString() + name[2] + name[1] + name[0] + name[7] + name[6] + name[5] + name[4]; 
    }

    /// <summary>
    /// Check if module is equal to himself after rotate 90 degree
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private bool RightAngleRotateCheck(string name)
    {
        //check if 4 first and 4 last is equal
        return name[0] == name[1] && name[1] == name[2] && name[2] == name[3] && name[4] == name[5] && name[5] == name[6] && name[6] == name[7];
    }

    /// <summary>
    /// Check if module is equal to himself after rotate 180 degree
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private bool StraightAngleRotateCheck(string name)
    {
        //check if 4 first and 4 last is equal
        return name[0] == name[2] && name[1] == name[3] && name[4] == name[6] && name[5] == name[7];
    }

    /// <summary>
    /// Check if all symetrique of module has an mirror
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private bool SymetricEqualCheck(string name)
    {
        string sysmmetric_vertical = name[3].ToString() + name[2] + name[1] + name[0] + name[7] + name[6] + name[5] + name[4];
        string sysmmetric_horizontal = name[1].ToString() + name[0] + name[3] + name[2] + name[5] + name[4] + name[7] + name[6];
        string sysmmetric_diagonalUp = name[0].ToString() + name[3] + name[2] + name[1] + name[4] + name[7] + name[6] + name[5];
        string sysmmetric_diagonaDown = name[2].ToString() + name[1] + name[0] + name[3] + name[6] + name[5] + name[4] + name[7];

        return name == sysmmetric_vertical || name == sysmmetric_horizontal || name == sysmmetric_diagonalUp || name == sysmmetric_diagonaDown;
    }
}
