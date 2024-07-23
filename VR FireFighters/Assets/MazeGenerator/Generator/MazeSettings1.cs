using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MazeSettings1
{
    [SerializeField]
    private int mazeSizeX;
    [SerializeField]
    private int mazeSizeY;

    [SerializeField]
    private int startPositionX, startPositionY;

    [SerializeField, Range(0, 10), Tooltip("Minimale Länge eines Korridors, bis eine Kreuzung oder Kurve kommen darf. Enden sind von dieser Einstellung nicht betroffen")]
    private int minLengthForCorridor;

    [SerializeField]
    private int seed;

    [SerializeField]
    private bool generatePath;
    [SerializeField]
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

    private List<RoomObject> rooms = new List<RoomObject>();

    // TODO delete
    public (int x,int y) GetStartPosition()
    {
        return (startPositionX,startPositionY);
    }

    public void SetStartPositionX(int x)
    {
        if (x < 0 || x > mazeSizeX) return;
        this.startPositionX = x;
    }
    public void SetStartPositionY(int y)
    {
        if (y < 0 || y > mazeSizeY) return;
        this.startPositionY = y;
    }

    // TODO delete
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
        if (mazeSizeX < 0) return;
        this.mazeSizeX = mazeSizeX;
    }
    
    public void SetMazeSizeY(int mazeSizeY)
    {
        if (mazeSizeY < 0) return;
        this.mazeSizeY = mazeSizeY;
    }

    public int GetMinCorridorLength()
    {
        return minLengthForCorridor;
    }
    
    public void SetMinCorridorLength(int len)
    {
        if (len < 0) return;
        this.minLengthForCorridor = len;
    }

    public float GetRoomPossibility()
    {
        return roomPossibility;
    }
    public void SetRoomPossibility(float pos)
    {
        if (pos < 0 || pos > 100) return;
        this.roomPossibility = pos;
    }
    
    public float GetCorridorPossibility()
    {
        return probabilityCorridor;
    }
    public void SetCorridorPossibility(float pos)
    {
        if (pos < 0 || pos > 100) return;
        this.probabilityCorridor = pos;
    }
    
    public float GetCornerPossibility()
    {
        return probabilityCorner;
    } 
    public void SetCornerPossibility(float pos)
    {
        if (pos < 0 || pos > 100) return;
        this.probabilityCorner = pos;
    }
    
    public float GetXCrossingPossibility()
    {
        return probabilityXCrossing;
    }    
    public void SetXCrossingPossibility(float pos)
    {
        if (pos < 0 || pos > 100) return;
        this.probabilityXCrossing = pos;
    }
    
    public float GetTCrossingPossibility()
    {
        return probabilityTCrossing;
    }
    public void SetTCrossingPossibility(float pos)
    {
        if (pos < 0 || pos > 100) return;
        this.probabilityTCrossing = pos;
    }
    
    public float GetEndPossibility()
    {
        return probabilityEnd;
    }
    public void SetEndPossibility(float pos)
    {
        if (pos < 0 || pos > 100) return;
        this.probabilityEnd = pos;
    }

    public int GetRoomSizeWidth(){
        return roomWidth;
    }
    public int GetRoomSizeHeight(){
        return roomHeight;
    }
    
    public void SetRoomSizeWidth(int width){
        if (width < 0) return;
        this.roomWidth = width;
    }
    public void SetRoomSizeHeight(int height){
        if (height < 0) return;
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
     * 
     * 
     * TODO DELETE
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

    public List<RoomObject> GetRooms()
    {
        return this.rooms;
    }

    public RoomObject TryGetRoom(string name)
    {
        if (name == null || name.Equals("")) return null;

        foreach(RoomObject room in rooms)
        {
            Debug.Log("Habe Raum " + room.GetRoomName());
            if (room.GetRoomName().Equals(name))
            {
                return room;
            }
        }
        return null;
    }

    public void SetRooms(List<RoomObject> rooms)
    {
        this.rooms = rooms;
    }
}
