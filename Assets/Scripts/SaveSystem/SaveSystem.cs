using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void SAVE( string FolderName,string WorldName)
    {
        MeshCreator mesh = GameObject.FindObjectOfType<MeshCreator>();
        SaveData save = new SaveData
        {
            triangles = mesh._triangles,
            vertices = mesh._vertices,
            xSize = mesh._xSize,
            zSize = mesh._zSize
        };
        string json = JsonUtility.ToJson(save);        
        if (!Directory.Exists(Application.dataPath + "/" + FolderName)) // SE NAO EXISTIR A PASTA, ELE CRIA UMA
        {
            Directory.CreateDirectory(Application.dataPath + "/" + FolderName);
        }
        File.WriteAllText(Application.dataPath + "/" + FolderName +"/"+ WorldName, json);
        Debug.Log("saved IN:"+ Application.dataPath + "/" + FolderName + "/" + WorldName + "\n" + json);

    }
    public static void LOADSAVE(string FolderName, string WorldName)
    {
        string path = Application.dataPath + "/" + FolderName + "/" + WorldName;
        //SE JA EXISTER SAVE
        if (File.Exists(path))
        {
            string saveString = File.ReadAllText(Application.dataPath + "/" + FolderName + "/" + WorldName);
            SaveData save = JsonUtility.FromJson<SaveData>(saveString);
            MeshCreator mesh = GameObject.FindObjectOfType<MeshCreator>();
            mesh.LoadWorld(save.vertices,save.triangles,save.xSize,save.zSize);
            Debug.Log("saved :" + "\n" + saveString);
        }
        //SE NÂO EXISTIR SAVE
        else
        {
            Debug.Log("NENHUM SAVE ENCONTRADO" + path);
            NEWSAVE(FolderName,WorldName);
        }
    }
    public static void NEWSAVE(string FolderName, string WorldName)
    {        
        SaveSystem.SAVE(FolderName,WorldName);
    }
}
