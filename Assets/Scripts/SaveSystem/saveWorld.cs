using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class saveWorld : MonoBehaviour
{
    public string path;    
    public string FileName;    
    private DownloadHandler downloadHandler;
    
    public void OpenLoadFileExplorer()
    {
        path = EditorUtility.OpenFilePanel("Show all Worlds ()", "", "world");
        SaveSystem.LOADSAVE(path);
    }
    public void OpenSaveFileExplorer()
    {
        string _path = EditorUtility.SaveFilePanel("Show All Worlds","","Save.world", "world");
        string projectName = Path.GetFileName(_path);
     
        if (_path.Length != 0)
        {            
            Debug.Log("Save "+ projectName);
            SaveSystem.SAVE(_path, projectName);
            
            //AssetDatabase.Refresh();
        }
    }

    public string Load(string streamingAssetLocalPath)
    {
        string filePath = streamingAssetLocalPath;
        StartCoroutine(SendRequest(filePath));
        while (downloadHandler == null)
        {
            Debug.Log("waiting for " + filePath);
        }
        string data = downloadHandler.text;
        downloadHandler = null;
        return data;
    }
    private IEnumerator SendRequest(string Path)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(Path))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.LogError("Request Error: " + request.error);
            }
            else
            {
                downloadHandler = request.downloadHandler;
            }
        }
    }  
}
