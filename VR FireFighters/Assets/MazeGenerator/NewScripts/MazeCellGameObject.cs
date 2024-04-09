using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MazeCellGameObjectType
{
    Corridor,
    Corner,
    T_Crossing,
    X_Crossing,
    End,
    Start,
    Room
}

public class MazeCellGameObject : MonoBehaviour
{
    [SerializeField]
    private bool faceNorth, faceEast, faceSouth, faceWest;

    [SerializeField]
    private bool hasDoor;

    /**
    * TRUE if there is a door in the respective direction,
    * False if not
    */
    [SerializeField]
    private bool doorToNorth, doorToEast, doorToSouth, doorToWest;

    [SerializeField]
    private MazeCellGameObjectType type;

    private int rotation = 0;

    /*
     * Rotation im Uhrzeigersinn
     * Norden wird Osten
     * Osten wird S�den
     * usw.
     */
    public void RotateObject(int degrees)
    {
        // Zwischenspeicher an boolschen Werten.
        bool north = faceNorth;
        bool east = faceEast;
        bool south = faceSouth;
        bool west = faceWest;

        bool doorNorth = doorToNorth;
        bool doorEast = doorToEast;
        bool doorSouth = doorToSouth;
        bool doorWest = doorToWest;

        rotation += degrees;

        switch (degrees)
        {
            case 90:
            case -270:          // Norden wird nach Osten gedreht
                // Passagen
                north = faceWest; 
                east = faceNorth;
                south = faceEast; 
                west = faceSouth;

                //T�ren
                doorNorth = doorToWest;
                doorEast = doorToNorth;
                doorSouth = doorToEast;
                doorWest = doorToSouth;

                gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                break;

            case 180:
            case -180:          // Norden wird nach S�den gedreht
                // Passagen
                north = faceSouth;
                east = faceWest;
                south = faceNorth;
                west = faceEast;

                // T�ren
                doorNorth = doorToSouth;
                doorEast = doorToWest;
                doorSouth = doorToNorth;
                doorWest = doorToEast;

                gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                break;

            case 270:
            case -90:          // Norden wird nach Westen gedreht
                // Passagen
                north = faceEast;
                east = faceSouth;
                south = faceWest;
                west = faceNorth;

                // T�ren
                doorNorth = doorToEast;
                doorEast = doorToSouth;
                doorSouth = doorToWest;
                doorWest = doorToNorth;

                gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
                break;
        }
        // Passagen
        faceNorth = north;
        faceEast = east;
        faceSouth = south;
        faceWest = west;

        // Door
        doorToNorth = doorNorth;
        doorToEast = doorEast;
        doorToWest = doorWest;
        doorToSouth = doorSouth;
    }

    public MazeCellGameObjectType GetMazeCellGameObjectType()
    {
        return this.type;
    }

    public bool GetFaceNorth()
    {
        return faceNorth;
    }

    public bool GetFaceEast()
    {
        return faceEast;
    }

    public bool GetFaceSouth()
    {
        return faceSouth;
    }

    public bool GetFaceWest()
    {
        return faceWest;
    }

    public bool objectHasDoor()
    {
        return hasDoor;
    }

    public bool GetDoorNorth()
    {
        return doorToNorth;
    }

    public bool GetDoorEast()
    {
        return doorToEast;
    }

    public bool GetDoorSouth()
    {
        return doorToSouth;
    }

    public bool GetDoorWest()
    {
        return doorToWest;
    }

    public int GetRotation(){
        return rotation;
    }
}
