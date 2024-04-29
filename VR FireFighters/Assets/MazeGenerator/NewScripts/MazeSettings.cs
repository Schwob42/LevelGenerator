using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class MazeSettings : ScriptableObject
{
    [SerializeField]
    private int mazeSizeX;
    [SerializeField]
    private int mazeSizeY;

    [SerializeField]
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
        this.startPositionX = x;
    }
    public void SetStartPositionY(int y)
    {
        this.startPositionY = y;
    }

    public (int mazeSizeX, int mazeSizeY) GetMazeSize()
    {
        Debug.Log(this.mazeSizeX + " " + this.mazeSizeY);
        return (this.mazeSizeX, this.mazeSizeY);
    }
    public int GetMazeSizeX()
    {
        return this.mazeSizeX;
    }
    public int GetMazeSizeY()
    {
        return this.mazeSizeY;
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
    
    public void SetMinCorridorLength(int len)
    {
        this.minLengthForCorridor = len;
    }

    public float GetRoomPossibility()
    {
        return roomPossibility;
    }
    public void SetRoomPossibility(float pos)
    {
        this.roomPossibility = pos;
    }
    
    public float GetCorridorPossibility()
    {
        return probabilityCorridor;
    }
    public void SetCorridorPossibility(float pos)
    {
        this.probabilityCorridor = pos;
    }
    
    public float GetCornerPossibility()
    {
        return probabilityCorner;
    } 
    public void SetCornerPossibility(float pos)
    {
        this.probabilityCorner = pos;
    }
    
    public float GetXCrossingPossibility()
    {
        return probabilityXCrossing;
    }    
    public void SetXCrossingPossibility(float pos)
    {
        this.probabilityXCrossing = pos;
    }
    
    public float GetTCrossingPossibility()
    {
        return probabilityTCrossing;
    }
    public void SetTCrossingPossibility(float pos)
    {
        this.probabilityTCrossing = pos;
    }
    
    public float GetEndPossibility()
    {
        return probabilityEnd;
    }
    public void SetEndPossibility(float pos)
    {
        this.probabilityEnd = pos;
    }


    public int GetRoomSizeWidth(){
        return roomWidth;
    }
    public int GetRoomSizeHeight(){
        return roomHeight;
    }
    
    public void SetRoomSizeWidth(int width){
        this.roomWidth = width;
    }
    public void SetRoomSizeHeight(int height){
        this.roomHeight = height;
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
