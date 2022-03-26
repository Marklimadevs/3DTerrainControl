using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class saveWorld : MonoBehaviour
{
    public string WorldName;
    public string FolderName;
    
    public void SaveWorld()
    {
        SaveSystem.SAVE(FolderName,WorldName);
    }

    public void LoadWorld()
    {
        SaveSystem.LOADSAVE(FolderName,WorldName);
    }
    public void SetSaveName(string Name)
    {
        WorldName = Name;
    }
    public void SetFolderName(string Name)
    {
        FolderName = Name;
    }
}
