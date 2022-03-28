using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    MeshCreator mesh;
    Vector3 InicialPos;
    public Camera cam;

    private void Start()
    {
        mesh = GameObject.FindObjectOfType<MeshCreator>();
        InicialPos = transform.position;
        transform.position =  mesh.getCenterPosition()+InicialPos;
        cam = GetComponent<Camera>();
        UpdatePosition();
    }
    public void UpdatePosition()
    {
        transform.position = mesh.getCenterPosition() + InicialPos;
        transform.LookAt (mesh.getCenterPosition());
    }
}
