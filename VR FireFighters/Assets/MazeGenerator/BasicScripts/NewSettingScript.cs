using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class NewSettingScript : MonoBehaviour
{
    

    private int mazeLength;
    private int mazeHeight;

    public static NewSettingScript CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<NewSettingScript>(jsonString);
    }

    public bool GetDataFromSaveFile(string saveFile)
    {
        if (!File.Exists(saveFile)) return false;

        string fileContent = File.ReadAllText(saveFile);

        NewSettingScript nss = JsonUtility.FromJson<NewSettingScript>(fileContent);
        
        this.mazeHeight = nss.mazeHeight;
        this.mazeLength = nss.mazeLength;

        return true;
    }
}
