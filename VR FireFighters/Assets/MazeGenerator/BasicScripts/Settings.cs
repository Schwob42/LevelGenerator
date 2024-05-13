using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

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

    // Start is called before the first frame update
    void Start()
    {
        GetSettings();
    }

    /// <summary>
    /// Loads every saved setting from the file and sets the value of each setting game object.
    /// </summary>
    private void GetSettings()
    {
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
    }    
    public void SetGenerateRoom()
    {
        settings.SetGenerateRoom(generateRoom.GetComponent<Toggle>().isOn);
    }
    public void SetMazeSizeX()
    {
        settings.SetMazeSizeX((int)levelWidth.GetComponent<Slider>().value);
    }
    public void SetMazeSizeY()
    {
        settings.SetMazeSizeY((int)levelHeight.GetComponent<Slider>().value);
    }
    public void SetStartPositionX()
    {
        settings.SetStartPositionX((int)startPositionX.GetComponent<Slider>().value);
    }
    public void SetStartPositionY()
    {
        settings.SetStartPositionY((int)startPositionY.GetComponent<Slider>().value);
    }
    public void SetSeed()
    {
        settings.SetSeed(Int32.Parse(seed.GetComponent<TMP_InputField>().text));
    }

    ///// Path settings
    public void SetCornerPossibility()
    {
        settings.SetCornerPossibility((pro_Corner.GetComponent<Slider>().value / 100));
    }
    public void SetCorridorPossibility()
    {
        settings.SetCorridorPossibility((pro_Corridor.GetComponent<Slider>().value / 100));
    }
    public void SetXCrossingPossibility()
    {
        settings.SetXCrossingPossibility((pro_X_Crossing.GetComponent<Slider>().value / 100));
    }
    public void SetTCrossingPossibility()
    {
        settings.SetTCrossingPossibility((pro_T_Crossing.GetComponent<Slider>().value / 100));
    }
    public void SetEndPossibility()
    {
        settings.SetEndPossibility((pro_End.GetComponent<Slider>().value / 100));
    }
    public void SetMinCorridorLength()
    {
        settings.SetMinCorridorLength((int)min_CorridorLenght.GetComponent<Slider>().value);
    }

    ///// Room settings
    public void SetStartRoomWidth()
    {
        settings.SetRoomSizeWidth((int)roomWidth.GetComponent<Slider>().value);
    }
    public void SetStartRoomHeight()
    {
        settings.SetRoomSizeHeight((int)roomHeight.GetComponent<Slider>().value);
    }
    public void SetRoomPossibility()
    {
        settings.SetRoomPossibility((roomPossibility.GetComponent<Slider>().value / 100));
    }
}
