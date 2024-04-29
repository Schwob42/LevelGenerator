using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class Settings : MonoBehaviour
{
    [SerializeField]
    private MazeSettings settings;

    // Allgemeine Einstellungen
    [SerializeField]
    private Toggle generatePath;
    [SerializeField]
    private GameObject generateRoom;

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

    // Pfadeinstellungen (Pos = Possibility)
    [SerializeField]
    private GameObject pos_Corridor;
    [SerializeField]
    private GameObject pos_Corner;
    [SerializeField]
    private GameObject pos_T_Crossing;
    [SerializeField]
    private GameObject pos_X_Crossing;
    [SerializeField]
    private GameObject pos_End;
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
        pos_Corridor.GetComponent<Slider>().value = settings.GetCorridorPossibility() * 100;
        pos_Corner.GetComponent<Slider>().value = settings.GetCornerPossibility() * 100;
        pos_X_Crossing.GetComponent<Slider>().value = settings.GetXCrossingPossibility() * 100;
        pos_T_Crossing.GetComponent<Slider>().value = settings.GetTCrossingPossibility() * 100;
        pos_End.GetComponent<Slider>().value = settings.GetEndPossibility() * 100;
    }

    // Setter und Getter für alle Einstellungen
    // - Allgemeine Einstellungen
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

    // - Pfadeinstellungen
    public void SetCornerPossibility()
    {
        settings.SetCornerPossibility((pos_Corner.GetComponent<Slider>().value / 100));
    }
    public void SetCorridorPossibility()
    {
        settings.SetCorridorPossibility((pos_Corridor.GetComponent<Slider>().value / 100));
    }
    public void SetXCrossingPossibility()
    {
        settings.SetXCrossingPossibility((pos_X_Crossing.GetComponent<Slider>().value / 100));
    }
    public void SetTCrossingPossibility()
    {
        settings.SetTCrossingPossibility((pos_T_Crossing.GetComponent<Slider>().value / 100));
    }
    public void SetEndPossibility()
    {
        settings.SetEndPossibility((pos_End.GetComponent<Slider>().value / 100));
    }
    public void SetMinCorridorLength()
    {
        settings.SetMinCorridorLength((int)min_CorridorLenght.GetComponent<Slider>().value);
    }

    // - Raumeinstellungen

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
