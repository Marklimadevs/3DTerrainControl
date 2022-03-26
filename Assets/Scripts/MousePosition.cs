using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MouseClass
{
   Mouse2D,Mouse3D
}
public class MousePosition : MonoBehaviour
{
    [SerializeField] Camera MainCamera;
    [SerializeField] MeshCreator _MeshCreator;
    [SerializeField] MouseClass _MouseClass;
    [SerializeField] bool GridMovig;
    [SerializeField] public bool InGrid;
    [SerializeField] GameObject CursorGO;

    public LayerMask LayerMaskIgnoreWater;
    public LayerMask layerMaskWater;

    private void Update()
    {
        if (GetWorldPosiiton().y != 100)
        {
            transform.position = GetWorldPosiiton();
        }
    }

    public Vector3 GetWorldPosiiton()
    {
        if (_MouseClass == MouseClass.Mouse2D)
        {
            Vector3 mouseWoldPostiion = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWoldPostiion.z = 0;
            return mouseWoldPostiion;
        }
        else if (_MouseClass == MouseClass.Mouse3D)
        {
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit raycastHit, 1000f, LayerMaskIgnoreWater))
            {
                Vector3 tilepos = Vector3.zero;
                Vector3 VerticePoint = _MeshCreator.GetCenterVerticePosition(raycastHit.point);
                InGrid = true;
                return VerticePoint;
            }
            else 
            {
                if(Physics.Raycast(ray, out RaycastHit _raycastHit, 1000f, layerMaskWater))
                {
                    InGrid = false;
                    return _raycastHit.point;
                }
                InGrid = false;
                return new Vector3(0, 100, 0);
            }
        }
        return MainCamera.transform.position ;
    }
}
