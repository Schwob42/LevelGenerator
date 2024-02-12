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

    [SerializeField, Range(0, 10), Tooltip("Minimale Länge eines Korridors, bis eine Kreuzung oder Kurve kommen darf. Enden sind von dieser Einstellung nicht betroffen")]
    int minLengthForCorridor;

    [Header("Wahrscheinlichkeiten für die einzelnen Bauteile")]
    [SerializeField, Range(0f, 1f)] float probabilityCorridor;
    [SerializeField, Range(0f, 1f)] float probabiliyCorner;
    [SerializeField, Range(0f, 1f)] float probabilityEnd;
    [SerializeField, Range(0f, 1f)] float probabilityTCrossing;
    [SerializeField, Range(0f, 1f)] float probabilityXCrossing;

    [Header("Einstellungen für Räume")]
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
        float sum = probabilityCorridor + probabiliyCorner + probabilityEnd + probabilityTCrossing + probabilityXCrossing;
        if (sum != 1f)
        {
            Debug.LogError("Summe muss 1 ergeben, but is " + sum);
        }

        if(minRoomSize > maxRoomSize)
        {
            Debug.LogError("Es ergibt wohl nur wenig Sinn, wenn Minimum größer Maximum sein soll ;)");
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
}
