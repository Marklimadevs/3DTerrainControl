using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrainManager : MonoBehaviour
{
    [SerializeField] Slider BrushForceSlider;
    [SerializeField] MousePosition MousePosition;
    [SerializeField] MeshCreator world;
    [SerializeField] List<int> listSelectsVertices;

    public float ForceBrush;
    public float BrushSize;

    private void Start()
    {
        transform.localScale = new Vector3(BrushSize, BrushSize, BrushSize);
        
    }
    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && !Input.GetKey(KeyCode.LeftControl)) // forward
        {
            BrushSize += 0.5f;
            transform.localScale = new Vector3(BrushSize, BrushSize, BrushSize);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f && !Input.GetKey(KeyCode.LeftControl)) // backwards
        {
            BrushSize -= 0.5f;
            if(BrushSize<= 0.5f)
            {
                BrushSize = 0.5f;
            }
            transform.localScale = new Vector3(BrushSize, BrushSize, BrushSize);
        }
        if(Input.GetAxis("Mouse ScrollWheel") < 0f && Input.GetKey(KeyCode.LeftControl))
        {
            CameraScript cam = GameObject.FindObjectOfType<CameraScript>();
            cam.cam.orthographicSize += 0.5f;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && Input.GetKey(KeyCode.LeftControl)) 
        {
            CameraScript cam = GameObject.FindObjectOfType<CameraScript>();
            cam.cam.orthographicSize -= 0.5f;
        }
        if (Input.GetMouseButton(0))
        {
            if (MousePosition.InGrid)
            {
                listSelectsVertices = new List<int>();
                listSelectsVertices = world.GetVerticesId(MousePosition.GetWorldPosiiton(), BrushSize);
                if (listSelectsVertices.Count != 0)
                {
                    world.ManipuleMesh(MousePosition.GetWorldPosiiton(), listSelectsVertices, ForceBrush);
                }
            }
        }
        if (Input.GetMouseButton(1))
        {
            if (MousePosition.InGrid)
            {
                listSelectsVertices = new List<int>();
                listSelectsVertices = world.GetVerticesId(MousePosition.GetWorldPosiiton(), BrushSize);
                if (listSelectsVertices.Count != 0)
                {
                    world.ManipuleMesh(MousePosition.GetWorldPosiiton(), listSelectsVertices, -ForceBrush);
                }
            }
        }

    }

    public void SetbrushForce(float value)
    {
        ForceBrush = value;
    }

}
