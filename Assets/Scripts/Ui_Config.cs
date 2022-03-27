using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ui_Config : MonoBehaviour
{

    public void SizeMap(int value)
    {
        MeshCreator mesh = GameObject.FindObjectOfType<MeshCreator>();
        if (mesh != null)
        {
            mesh.CreateNewWorld(value);
        }
        else
        {
            Debug.LogWarning("The Mesh Creator is not Founded!");
        }
        
    }
    
}
