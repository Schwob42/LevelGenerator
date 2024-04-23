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

    // Pfadeinstellungen

    // Raumeinstellungen


    // Start is called before the first frame update
    void Start()
    {
        GetSettings();
    }

    private void GetSettings()
    {
        generatePath.GetComponent<Toggle>().isOn = settings.GetGeneratePath();
        generateRoom.GetComponent<Toggle>().isOn = settings.GetGenerateRoom();
        levelWidth.GetComponent<Slider>().value = settings.GetMazeSize().mazeSizeX;
        levelHeight.GetComponent<Slider>().value = settings.GetMazeSize().mazeSizeY;
        startPositionX.GetComponent<Slider>().value = settings.GetStartPosition().x;
        startPositionY.GetComponent<Slider>().value = settings.GetStartPosition().y;
        seed.GetComponent<TMP_InputField>().text = settings.GetSeed().ToString();
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
    // - Raumeinstellungen
}
