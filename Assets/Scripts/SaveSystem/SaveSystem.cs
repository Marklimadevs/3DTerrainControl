using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void SAVE(string _Path, string FileName)
    {
        string FinalFileName = _Path;
        MeshCreator mesh = GameObject.FindObjectOfType<MeshCreator>();
        SaveData save = new SaveData
        {
            triangles = mesh._triangles,
            vertices = mesh._vertices,
            xSize = mesh._xSize,
            zSize = mesh._zSize
        };
        string json = JsonUtility.ToJson(save);        
        /*if (!Directory.Exists(FinalFileName)) // SE NAO EXISTIR A PASTA, ELE CRIA UMA
        {
            Directory.CreateDirectory(FinalFileName);
        }        */
        File.WriteAllText(FinalFileName, json);
        Debug.Log("saved IN:"+ FinalFileName + "\n" + json);

    }
    public static void LOADSAVE(string Path, string jsondata="")
    { 
        //SE JA EXISTER SAVE
        if (File.Exists(Path))
        {
            string saveString = File.ReadAllText(Path);
            SaveData save = JsonUtility.FromJson<SaveData>(saveString);
            MeshCreator mesh = GameObject.FindObjectOfType<MeshCreator>();
            mesh.LoadWorld(save.vertices,save.triangles,save.xSize,save.zSize);
            Debug.Log("loaded :" + "\n" + saveString);
        }
        //SE NÂO EXISTIR SAVE
        else
        {
            Debug.Log("NENHUM SAVE ENCONTRADO" + Path);
            //NEWSAVE(FolderName,WorldName);
        }
    }
}
