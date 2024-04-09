using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class MazeSettings : ScriptableObject
{
    [SerializeField, Range(0, 50)]
    int mazeSizeX, mazeSizeY;

    [SerializeField, Range(0, 50)]
    int startPositionX, startPositionY;

    [SerializeField, Range(0, 10), Tooltip("Minimale L�nge eines Korridors, bis eine Kreuzung oder Kurve kommen darf. Enden sind von dieser Einstellung nicht betroffen")]
    int minLengthForCorridor;

    [Header("Wahrscheinlichkeiten f�r die einzelnen Bauteile.")]
    [SerializeField, Range(0.1f, 1f)] float probabilityCorridor;
    [SerializeField, Range(0f, 1f)] float probabilityCorner;
    [SerializeField, Range(0f, 1f)] float probabilityEnd;
    [SerializeField, Range(0f, 1f)] float probabilityTCrossing;
    [SerializeField, Range(0f, 1f)] float probabilityXCrossing;

    [Header("Einstellungen f�r R�ume")]
    [SerializeField] bool thereShouldBeRooms;
    [SerializeField, Tooltip("The size in number of cells per room"), Range(1, 10)] int minRoomSize, maxRoomSize;


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

    public (int,int) GetStartPosition()
    {
        return (startPositionX,startPositionY);
    }

    public (int, int) GetMazeSize()
    {
        return (mazeSizeX, mazeSizeY);
    }

    public int GetMinCorridorLength()
    {
        return minLengthForCorridor;
    }

    public (int minRoomSize, int maxRoomSize) GetRoomSettings(){
        return (minRoomSize, maxRoomSize);
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
}
