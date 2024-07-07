using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.IO;

/// <summary>
/// This script manages the local settings and the settings saved in the corresponding file.
/// Settings are only saved permanently once they have been saved in the relevant file.
/// </summary>
public class Settings : MonoBehaviour
{
    /// <summary>
    /// Reference to the file containing the saved settings.
    /// </summary>
    [SerializeField]
    private MazeSettings settings;

    private MazeSettings1 m_Settings;

    /// <summary>
    /// A list of all rooms in the level.
    /// </summary>
    private List<RoomObject> rooms;

    ////////// General settings
    [SerializeField]
    private Toggle generatePath;

    [SerializeField]
    private Toggle generateRoom;

    [SerializeField]
    private GameObject levelWidth;

    [SerializeField]
    private GameObject levelHeight;

    [SerializeField]
    private GameObject startPositionX;

    [SerializeField]
    private GameObject startPositionY;

    [SerializeField]
    private GameObject seed;

    ////////// Path settings (pro = probability)
    [SerializeField]
    private GameObject pro_Corridor;
    [SerializeField]
    private GameObject pro_Corner;
    [SerializeField]
    private GameObject pro_T_Crossing;
    [SerializeField]
    private GameObject pro_X_Crossing;
    [SerializeField]
    private GameObject pro_End;
    [SerializeField]
    private GameObject min_CorridorLenght;

    // Raumeinstellungen
    [SerializeField]
    private GameObject roomWidth;
    [SerializeField]
    private GameObject roomHeight;
    [SerializeField]
    private GameObject roomPossibility;


    // TESTSREGION
    NewSettingScript nss;
    readonly private string path = "./Assets/SaveFiles/";


    // Start is called before the first frame update
    void Start()
    {
        GetSettings();
    }
    
    // Nur zur Sicherheit
    private void OnDestroy()
    {
        //SaveDataToFile();
    }

    public bool GetDataFromSaveFile()
    {
        if (!File.Exists(path + "settings.json"))
        {
            Debug.LogError("File not found");
            m_Settings = new MazeSettings1();
            return false;
        }

        

        string fileContent = File.ReadAllText(path + "settings.json");

        if(fileContent == null || fileContent == "")
        {
            Debug.LogError("File is empty");
            m_Settings = new MazeSettings1();
            return true;
        }
        else
        {
            this.m_Settings = JsonUtility.FromJson<MazeSettings1>(fileContent);
            return true;
        }
    }

    public void SaveDataToFile()
    {
        //File.WriteAllText(Application.persistentDataPath + "/gamedata.json", JsonUtility.ToJson(this.settings));
        string jsonString = JsonUtility.ToJson(this.m_Settings, true);
        Debug.Log("Saving file with\n"+ jsonString);
        File.WriteAllText(path + "settings.json", jsonString);

    }

    /// <summary>
    /// Loads every saved setting from the file and sets the value of each setting game object.
    /// </summary>
    private void GetSettings()
    {
        if (!GetDataFromSaveFile())
        {
            // TODO
            // Return to start and show error messag

            return;
        }
        /*  ORIGINAL
        generatePath.GetComponent<Toggle>().isOn = settings.GetGeneratePath();
        generateRoom.GetComponent<Toggle>().isOn = settings.GetGenerateRoom();
        levelWidth.GetComponent<Slider>().value = settings.GetMazeSizeX();
        levelHeight.GetComponent<Slider>().value = settings.GetMazeSizeY();
        startPositionX.GetComponent<Slider>().value = settings.GetStartPosition().x;
        startPositionY.GetComponent<Slider>().value = settings.GetStartPosition().y;
        seed.GetComponent<TMP_InputField>().text = settings.GetSeed().ToString();
        roomHeight.GetComponent<Slider>().value = settings.GetRoomSizeHeight();
        roomWidth.GetComponent<Slider>().value = settings.GetRoomSizeWidth();
        roomPossibility.GetComponent<Slider>().value = settings.GetRoomPossibility() * 100;
        pro_Corridor.GetComponent<Slider>().value = settings.GetCorridorPossibility() * 100;
        pro_Corner.GetComponent<Slider>().value = settings.GetCornerPossibility() * 100;
        pro_X_Crossing.GetComponent<Slider>().value = settings.GetXCrossingPossibility() * 100;
        pro_T_Crossing.GetComponent<Slider>().value = settings.GetTCrossingPossibility() * 100;
        pro_End.GetComponent<Slider>().value = settings.GetEndPossibility() * 100;
        rooms = settings.GetRooms();
        */


        // TEST
        /**/
        Debug.Log(m_Settings.GetGeneratePath());
        generatePath.GetComponent<Toggle>().isOn = m_Settings.GetGeneratePath();
        generateRoom.GetComponent<Toggle>().isOn = m_Settings.GetGenerateRoom();
        levelWidth.GetComponent<Slider>().value = m_Settings.GetMazeSizeX();
        levelHeight.GetComponent<Slider>().value = m_Settings.GetMazeSizeY();
        startPositionX.GetComponent<Slider>().value = m_Settings.GetStartPosition().x;
        startPositionY.GetComponent<Slider>().value = m_Settings.GetStartPosition().y;
        seed.GetComponent<TMP_InputField>().text = m_Settings.GetSeed().ToString();
        roomHeight.GetComponent<Slider>().value = m_Settings.GetRoomSizeHeight();
        roomWidth.GetComponent<Slider>().value = m_Settings.GetRoomSizeWidth();
        roomPossibility.GetComponent<Slider>().value = m_Settings.GetRoomPossibility() * 100;
        pro_Corridor.GetComponent<Slider>().value = m_Settings.GetCorridorPossibility() * 100;
        pro_Corner.GetComponent<Slider>().value = m_Settings.GetCornerPossibility() * 100;
        pro_X_Crossing.GetComponent<Slider>().value = m_Settings.GetXCrossingPossibility() * 100;
        pro_T_Crossing.GetComponent<Slider>().value = m_Settings.GetTCrossingPossibility() * 100;
        pro_End.GetComponent<Slider>().value = m_Settings.GetEndPossibility() * 100;
        rooms = m_Settings.GetRooms();
        

    }

    

    /// <summary>
    /// Generates names (like "room 1") for each room. Keep in mind that a room or the room game object has no name to reference!
    /// The room names are generated to give the user a reference point for room selection in the UI.
    /// </summary>
    /// <returns>List of names of the rooms</returns>
    public List<string> GetRoomNames()
    {
        rooms = settings.GetRooms();
        if (rooms == null) return new List<string>();

        List<string> roomNames = new List<string>();

        foreach (RoomObject room in rooms)
        {
            roomNames.Add(room.GetRoomName());
        }

        return roomNames;
    }


    ////////////////////////////////////////
    /////// Setter methodes for all settings
    ///// General settings
    public void SetGeneratePath()
    {
        settings.SetGeneratePath(generatePath.GetComponent<Toggle>().isOn);
        m_Settings.SetGeneratePath(generatePath.GetComponent<Toggle>().isOn);
    }    
    public void SetGenerateRoom()
    {
        settings.SetGenerateRoom(generateRoom.GetComponent<Toggle>().isOn);
        m_Settings.SetGenerateRoom(generateRoom.GetComponent<Toggle>().isOn);
    }
    public void SetMazeSizeX()
    {
        settings.SetMazeSizeX((int)levelWidth.GetComponent<Slider>().value);
        m_Settings.SetMazeSizeX((int)levelWidth.GetComponent<Slider>().value);
        
    }
    public void SetMazeSizeY()
    {
        settings.SetMazeSizeY((int)levelHeight.GetComponent<Slider>().value);
        m_Settings.SetMazeSizeY((int)levelHeight.GetComponent<Slider>().value);
    }
    public void SetStartPositionX()
    {
        settings.SetStartPositionX((int)startPositionX.GetComponent<Slider>().value);
        m_Settings.SetStartPositionX((int)startPositionX.GetComponent<Slider>().value);
    }
    public void SetStartPositionY()
    {
        settings.SetStartPositionY((int)startPositionY.GetComponent<Slider>().value);
        m_Settings.SetStartPositionY((int)startPositionY.GetComponent<Slider>().value);
    }
    public void SetSeed()
    {
        settings.SetSeed(Int32.Parse(seed.GetComponent<TMP_InputField>().text));
        m_Settings.SetSeed(Int32.Parse(seed.GetComponent<TMP_InputField>().text));
    }

    ///// Path settings
    public void SetCornerPossibility()
    {
        settings.SetCornerPossibility((pro_Corner.GetComponent<Slider>().value / 100));
        m_Settings.SetCornerPossibility((pro_Corner.GetComponent<Slider>().value / 100));
    }
    public void SetCorridorPossibility()
    {
        settings.SetCorridorPossibility((pro_Corridor.GetComponent<Slider>().value / 100));
        m_Settings.SetCorridorPossibility((pro_Corridor.GetComponent<Slider>().value / 100));
    }
    public void SetXCrossingPossibility()
    {
        settings.SetXCrossingPossibility((pro_X_Crossing.GetComponent<Slider>().value / 100));
        m_Settings.SetXCrossingPossibility((pro_X_Crossing.GetComponent<Slider>().value / 100));
    }
    public void SetTCrossingPossibility()
    {
        settings.SetTCrossingPossibility((pro_T_Crossing.GetComponent<Slider>().value / 100));
        m_Settings.SetTCrossingPossibility((pro_T_Crossing.GetComponent<Slider>().value / 100));
    }
    public void SetEndPossibility()
    {
        settings.SetEndPossibility((pro_End.GetComponent<Slider>().value / 100));
        m_Settings.SetEndPossibility((pro_End.GetComponent<Slider>().value / 100));
    }
    public void SetMinCorridorLength()
    {
        settings.SetMinCorridorLength((int)min_CorridorLenght.GetComponent<Slider>().value);
        m_Settings.SetMinCorridorLength((int)min_CorridorLenght.GetComponent<Slider>().value);
    }

    ///// Room settings
    public void SetStartRoomWidth()
    {
        settings.SetRoomSizeWidth((int)roomWidth.GetComponent<Slider>().value);
        m_Settings.SetRoomSizeWidth((int)roomWidth.GetComponent<Slider>().value);
    }
    public void SetStartRoomHeight()
    {
        settings.SetRoomSizeHeight((int)roomHeight.GetComponent<Slider>().value);
        m_Settings.SetRoomSizeHeight((int)roomHeight.GetComponent<Slider>().value);
    }
    public void SetRoomPossibility()
    {
        settings.SetRoomPossibility((roomPossibility.GetComponent<Slider>().value / 100));
        m_Settings.SetRoomPossibility((roomPossibility.GetComponent<Slider>().value / 100));
    }
}
