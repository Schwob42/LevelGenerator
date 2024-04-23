using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class MazeSettings : ScriptableObject
{
    [SerializeField, Range(0, 50)]
    private int mazeSizeX;
    [SerializeField, Range(0, 50)]
    private int mazeSizeY;

    [SerializeField, Range(0, 50)]
    private int startPositionX, startPositionY;

    [SerializeField, Range(0, 10), Tooltip("Minimale L�nge eines Korridors, bis eine Kreuzung oder Kurve kommen darf. Enden sind von dieser Einstellung nicht betroffen")]
    private int minLengthForCorridor;

    [SerializeField]
    private int seed;

    [SerializeField]
    private bool generatePath;
    private bool generateRoom;

    [Header("Wahrscheinlichkeiten f�r die einzelnen Bauteile.")]
    [SerializeField, Range(0.1f, 1f)] private float probabilityCorridor;
    [SerializeField, Range(0f, 1f)] private float probabilityCorner;
    [SerializeField, Range(0f, 1f)] private float probabilityEnd;
    [SerializeField, Range(0f, 1f)] private float probabilityTCrossing;
    [SerializeField, Range(0f, 1f)] private float probabilityXCrossing;

    [Header("Einstellungen f�r R�ume")]
    [SerializeField] bool thereShouldBeRooms;
    [SerializeField, Range (0,1)] float roomPossibility;
    [SerializeField, Tooltip("The size in number of cells per room"), Range(1, 10)] private int minRoomSize, maxRoomSize;
    [SerializeField, Tooltip("The size in number of cells per room"), Range(3, 10)] private int roomWidth;
    [SerializeField, Tooltip("The size in number of cells per room"), Range(3, 10)] private int roomHeight;


    /**
     * Checkt (teilweise) die Eingaben. 
     * 
     * 
     * TODO: Fehler in UI anzeigen und Start des Generator verhindern. Bspw. durch das Deaktivieren eines Generate Buttons
     */
    private void OnValidate()
    {
        float sum = probabilityCorridor + probabilityCorner + probabilityEnd + probabilityTCrossing + probabilityXCrossing;
        if (sum != 1f)
        {
            Debug.LogError("Summe muss 1 ergeben, but is " + sum);
        }

        if(minRoomSize > maxRoomSize)
        {
            Debug.LogError("Es ergibt wohl nur wenig Sinn, wenn Minimum gr��er Maximum sein soll ;)");
        }

        if (startPositionX > mazeSizeX - 1) Debug.LogError("StartPositionX sollte < mazeSizeX sein");
        if (startPositionY > mazeSizeY - 1) Debug.LogError("StartPositionY sollte < mazeSizeY sein");
    }

    public (int x,int y) GetStartPosition()
    {
        return (startPositionX,startPositionY);
    }

    public void SetStartPositionX(int x)
    {
        startPositionX = x;
    }
    public void SetStartPositionY(int y)
    {
        startPositionY = y;
    }

    public (int mazeSizeX, int mazeSizeY) GetMazeSize()
    {
        Debug.Log(this.mazeSizeX + " " + this.mazeSizeY);
        return (this.mazeSizeX, this.mazeSizeY);
    }

    public void SetMazeSizeX(int mazeSizeX)
    {
        this.mazeSizeX = mazeSizeX;
    }
    
    public void SetMazeSizeY(int mazeSizeY)
    {
        this.mazeSizeY = mazeSizeY;
    }

    public int GetMinCorridorLength()
    {
        return minLengthForCorridor;
    }

    public (int minRoomSize, int maxRoomSize) GetRoomSettings(){
        return (minRoomSize, maxRoomSize);
    }

    public float GetRoomPossibility()
    {
        return roomPossibility;
    }
    
    public (int roomWidth, int roomHeight) GetRoomSize(){
        return (roomWidth, roomHeight);
    }

    /**
     * Returns the posibilitys of 
     * probabilityCorner
     * probabilityEnd
     * probabilityTCrossing
     * probabilityXCrossing
     * 
     * When you want to change this order, you also have to change the order in the Maze Generator!!
     */
    public (float, float, float, float, float) GetProbabilities()
    {
        return (probabilityCorridor, probabilityCorner, probabilityEnd, probabilityTCrossing, probabilityXCrossing);
    }

    public int GetSeed()
    {
        return this.seed;
    }

    public void SetSeed(int seed)
    {
        this.seed = seed;
    }

    public bool GetGenerateRoom()
    {
        return generateRoom;
    }
    public void SetGenerateRoom(bool state)
    {
        this.generateRoom = state;
    }

    public bool GetGeneratePath()
    {
        return generatePath;
    }

    public void SetGeneratePath(bool state)
    {
        this.generatePath = state;
    }
}
