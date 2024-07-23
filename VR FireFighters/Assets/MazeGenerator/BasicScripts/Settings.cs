using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.IO;
using Newtonsoft.Json;
using System.IO;

/// <summary>
/// This script manages the local settings and the settings saved in the corresponding file.
/// Settings are only saved permanently once they have been saved in the relevant file.
/// </summary>
public class Settings : MonoBehaviour
{
    public static Settings instance;

    /// <summary>
    /// A list of all rooms in the level.
    /// </summary>
    private List<RoomObject> rooms;

    ////////// General settings
    protected class SettingsGeneral
    {
        [SerializeField]
        public Toggle generatePath;

        [SerializeField]
        public Toggle generateRoom;

        [SerializeField]
        public GameObject levelWidth;

        [SerializeField]
        public GameObject levelHeight;

        [SerializeField]
        public GameObject startPositionX;

        [SerializeField]
        public GameObject startPositionY;

        [SerializeField]
        public GameObject seed;

        ////////// Path settings (pro = probability)
        [SerializeField]
        public GameObject pro_Corridor;
        [SerializeField]
        public GameObject pro_Corner;
        [SerializeField]
        public GameObject pro_T_Crossing;
        [SerializeField]
        public GameObject pro_X_Crossing;
        [SerializeField]
        public GameObject pro_End;
        [SerializeField]
        public GameObject min_CorridorLenght;

        // Raumeinstellungen
        [SerializeField]
        public GameObject roomWidth;
        [SerializeField]
        public GameObject roomHeight;
        [SerializeField]
        public GameObject roomPossibility;

    }


    private static SettingsGeneral settingsGeneral;
    JsonSerializer JS = new JsonSerializer();

    // TESTSREGION
    NewSettingScript nss;
    readonly private string path = "./Assets/SaveFiles/";


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        GetStoredSettings();
    }
    
    // Nur zur Sicherheit
    private void OnDestroy()
    {
        //SaveDataToFile();
    }

    public void GetStoredSettings()
    {
        using (StreamReader file = File.OpenText(path + "settings_personal.json"))
        {
            settingsGeneral = (SettingsGeneral)JS.Deserialize(file, typeof(Settings));

            if (settingsGeneral == null) settingsGeneral = CatchSettingsError();
        }
    }

    private SettingsGeneral CatchSettingsError()
    {
        Debug.LogError("Settings could not be loaded. Loading default settings. Please check './Assets/Ressources/settings_personal.json'");
        using (StreamReader file = File.OpenText(path + "settings.json"))
        {
            return (SettingsGeneral)JS.Deserialize(file, typeof(Settings));
        }
    }

    public void SaveDataToFile()
    {
        //File.WriteAllText(Application.persistentDataPath + "/gamedata.json", JsonUtility.ToJson(this.settings));
        string jsonString = JsonUtility.ToJson(settingsGeneral);
        Debug.Log("Saving file with\n"+ jsonString);
        File.WriteAllText(path + "settings_personal.json", jsonString);

    }

    /// <summary>
    /// Generates names (like "room 1") for each room. Keep in mind that a room or the room game object has no name to reference!
    /// The room names are generated to give the user a reference point for room selection in the UI.
    /// </summary>
    /// <returns>List of names of the rooms</returns>
    public List<string> GetRoomNames()
    {
        if (this.rooms == null) return new List<string>();

        List<string> roomNames = new List<string>();

        foreach (RoomObject room in this.rooms)
        {
            roomNames.Add(room.GetRoomName());
        }

        return roomNames;
    }

    public List<string> SetRooms(List<RoomObject> rooms)
    {
        this.rooms = rooms;
        if (rooms == null) return new List<string>();

        List<string> roomNames = new List<string>();

        foreach (RoomObject room in rooms)
        {
            roomNames.Add(room.GetRoomName());
        }

        return roomNames;
    }

    /*
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
    */
}
