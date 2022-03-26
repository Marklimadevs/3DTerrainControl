using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{

    public Vector3[] vertices;
    public int[] triangles;
    public int xSize = 20;
    public int zSize = 20;

    public SaveData()
    {
        MeshCreator mesh = GameObject.FindObjectOfType<MeshCreator>();
        vertices = mesh._vertices;
        triangles = mesh._triangles;
        xSize = mesh._xSize;
        zSize = mesh._zSize;

    }
}
