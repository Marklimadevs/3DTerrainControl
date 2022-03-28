using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


[RequireComponent(typeof(MeshFilter))]
public class MeshCreator : MonoBehaviour
{

    #region Create Mesh

    MeshRenderer _MeshRenderer;
    Mesh mesh;
    MeshCollider meshCollider;
    MeshFilter meshfilter;


    [SerializeField] [Range(-20f,0f)] int LimteMin;
    [SerializeField] [Range( 0f, 20f)] int LimteMax;
    [SerializeField] Material _DefaulMaterial;

    Color[] Colors;

    //Publics
    public Gradient _gradiant;
    public int _xSize = 20;
    public int _zSize = 20;
    public Vector3[] _vertices;
    public int[] _triangles;


    [SerializeField] TextMeshProUGUI Text_XSize;
    [SerializeField] TextMeshProUGUI Text_ZSize;
    [SerializeField] TextMeshProUGUI Text_MinY;
    [SerializeField] TextMeshProUGUI Text_MaxY;
    #endregion

    private void Start()
    {
        //get components
        mesh = new Mesh();
        _MeshRenderer = GetComponent<MeshRenderer>();
        GetComponent<MeshFilter>().mesh = mesh;
        meshCollider = GetComponent<MeshCollider>();
        meshfilter = GetComponent<MeshFilter>();

        //Create Mash
        StartCoroutine(CreateShape());
        UpdateMesh();

        //UpdateTexts
        if (Text_MaxY != null) Text_MaxY.SetText(LimteMax.ToString());
        if (Text_MinY != null) Text_MinY.SetText(LimteMin.ToString());
        if (Text_ZSize != null) Text_ZSize.SetText(_zSize.ToString());
        if (Text_XSize != null) Text_XSize.SetText(_xSize.ToString());
    }
    IEnumerator CreateShape()
    {
        _vertices = new Vector3[(_xSize + 1) * (_zSize + 1)];

        for (int i = 0 , z = 0; z <= _zSize; z++)
        {
            for (int x = 0; x <= _xSize; x++)
            {
                _vertices[i] = new Vector3(x,0,z);
                i++;
                //yield return new WaitForEndOfFrame();
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
            //yield return new WaitForEndOfFrame();
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
       yield return new WaitForEndOfFrame();
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
        CreateNewWorld(_xSize,_zSize);
        UpdateMesh();
        CameraScript cam = GameObject.FindObjectOfType<CameraScript>();
        if (cam != null)
        {
            cam.UpdatePosition();
        }
        else
        {
            Debug.LogWarning("Camera not found to set position");
        }
    }   
    public void CreateNewWorld(int Xsize,int Zsize)
    {
        _xSize = Xsize;
        _zSize = Zsize;
        mesh = new Mesh();
        _MeshRenderer = GetComponent<MeshRenderer>();
        GetComponent<MeshFilter>().mesh = mesh;
        meshCollider = GetComponent<MeshCollider>();
        meshfilter = GetComponent<MeshFilter>();
        StartCoroutine(CreateShape());
        UpdateMesh();
        Debug.Log("Novo mundo Criado");
    }    

    public void LoadWorld(Vector3[] vertice, int[] triangles, int xSize, int ZSize)
    {
        _vertices = vertice;
        _triangles = triangles;
        _xSize = xSize;
        _zSize = ZSize;

        UpdateMesh();
    }

    public void SetWightSizeWorld(float value)
    {
        _xSize = (int)value;
        if(Text_XSize!=null) Text_XSize.SetText(value.ToString());
    }
    public void SetheightSizeWorld(float value)
    {
        _zSize = (int)value;
        if (Text_ZSize != null) Text_ZSize.SetText(value.ToString());
    }

    public void SetMinY(float value)
    {
        LimteMin = (int) value;
        if (Text_MinY != null) Text_MinY.SetText(value.ToString());
    }
    public void SetMaxY(float value)

    {
        LimteMax = (int)value;
        if (Text_MaxY != null) Text_MaxY.SetText(value.ToString());
    }
}
