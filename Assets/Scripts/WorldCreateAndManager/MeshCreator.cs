using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshCreator : MonoBehaviour
{

    #region Create Mesh

    MeshRenderer _MeshRenderer;
    Mesh mesh;
    MeshCollider meshCollider;
    MeshFilter meshfilter;


    [SerializeField] [Range(-5f,0f)] int LimteMin;
    [SerializeField] [Range( 0f, 5f)] int LimteMax;
    [SerializeField] Material _DefaulMaterial;

    Color[] Colors;

    //Publics
    public Gradient _gradiant;
    public int _xSize = 20;
    public int _zSize = 20;
    public Vector3[] _vertices;
    public int[] _triangles;

    #endregion

    private void Start()
    {
        //CreatePoint();
        mesh = new Mesh();
        _MeshRenderer = GetComponent<MeshRenderer>();
        GetComponent<MeshFilter>().mesh = mesh;
        meshCollider = GetComponent<MeshCollider>();
        meshfilter = GetComponent<MeshFilter>();
        CreateShape();
        UpdateMesh();

    }
    void CreateShape()
    {
        _vertices = new Vector3[(_xSize + 1) * (_zSize + 1)];

        for (int i = 0 , z = 0; z <= _zSize; z++)
        {
            for (int x = 0; x <= _xSize; x++)
            {
                _vertices[i] = new Vector3(x,0,z);
                i++;
            }
        }


        _triangles = new int[_xSize*_zSize*6];
        int vert = 0;
        int tris = 0;

        for (int z = 0; z < _zSize; z++)
        {
            for (int x = 0; x < _xSize; x++)
            {
                _triangles[tris + 0] = vert + 0;
                _triangles[tris + 1] = vert + _xSize + 1;
                _triangles[tris + 2] = vert + 1;
                _triangles[tris + 3] = vert + 1;
                _triangles[tris + 4] = vert + _xSize + 1;
                _triangles[tris + 5] = vert + _xSize + 2;
                vert++;
                tris += 6;
                meshCollider.sharedMesh = meshfilter.mesh;
            }
            vert++;
        }

        Colors = new Color[_vertices.Length];
        for (int i = 0, z = 0; z <= _zSize; z++)
        {
            for (int x = 0; x <= _xSize; x++)
            {
                float height = Mathf.InverseLerp(LimteMin,LimteMax, _vertices[i].y);
                Colors[i] = _gradiant.Evaluate(height);

                i++;
            }
        }
    }
    public void UpdateMesh()
    {
        _MeshRenderer.material = _DefaulMaterial;
        mesh.Clear();        
        mesh.vertices = _vertices;
        mesh.triangles = _triangles;
        mesh.colors = Colors;
        mesh.RecalculateNormals();
        meshCollider.sharedMesh = meshfilter.mesh;
    }  

    public Vector3 GetCenterVerticePosition(Vector3 position)
    {
        for (int i = 0; i < _vertices.Length; i++)
        {
            if (Vector3.Distance(_vertices[i], position) < 0.5f)
            {              
                return _vertices[i];
            }
        }
        return new Vector3 (0,100,0);
    }

    public List<int> GetVerticesId(Vector3 position,float area = 1)
    {
        List<int> Ids = new List<int>();
        for (int i = 0; i < _vertices.Length; i++)
        {
            //Debug.Log("Distance :"+Vector3.Distance(_vertices[i], position)) ;

            if (Vector3.Distance(_vertices[i], position) <= area)
                {                    
                    Ids.Add(i);                
                }
        }
        return Ids;
    }

    public void ManipuleMesh ( Vector3 position ,List<int> idMesh,float force)
    {
        for (int i = 0; i < idMesh.Count; i++)
        {            
            _vertices[idMesh[i]].y += force * Time.deltaTime;

            if (_vertices[idMesh[i]].y >= LimteMax)
            {
                _vertices[idMesh[i]].y = LimteMax;
            }
            if (_vertices[idMesh[i]].y <= LimteMin)
            {
                _vertices[idMesh[i]].y = LimteMin;
            }
        }

        UpdateMesh();
    }

    public Vector3 getCenterPosition()
    {
        return new Vector3 (_xSize/2,0,_zSize/2);
    }
    public void ResetWorld()
    {
        for (int i = 0; i < _vertices.Length; i++)
        {
            _vertices[i] = new Vector3(_vertices[i].x,0, _vertices[i].z);
        }
        UpdateMesh();
    }   
    public void CreateNewWorld(int value)
    {
        _xSize = value;
        _zSize = value;
        mesh = new Mesh();
        _MeshRenderer = GetComponent<MeshRenderer>();
        GetComponent<MeshFilter>().mesh = mesh;
        meshCollider = GetComponent<MeshCollider>();
        meshfilter = GetComponent<MeshFilter>();
        CreateShape();
        UpdateMesh();
    }    

    public void LoadWorld(Vector3[] vertice, int[] triangles, int xSize, int ZSize)
    {
        _vertices = vertice;
        _triangles = triangles;
        _xSize = xSize;
        _zSize = ZSize;

        UpdateMesh();
    }
}
