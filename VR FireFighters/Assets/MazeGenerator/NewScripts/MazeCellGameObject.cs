using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCellGameObject : MonoBehaviour
{
    bool faceNorth, faceEast, faceSouth, faceWest;

    bool hasDoor;

    /*
     * Rotation im Uhrzeigersinn
     * Norden wird Osten
     * Osten wird Süden
     * usw.
     */
    public void RotateObject(int degrees)
    {
        bool north = faceNorth;
        bool east = faceEast;
        bool south = faceSouth;
        bool west = faceWest;

        switch (degrees)
        {
            case 90:
                north = faceWest; 
                east = faceNorth;
                south = faceEast; 
                west = faceSouth;
                gameObject.transform.Rotate(new Vector3(0, 90, 0));
                break;
            case 180:
                north = faceSouth;
                east = faceWest;
                south = faceNorth;
                west = faceEast;
                gameObject.transform.Rotate(new Vector3(0, 180, 0));
                break;
            case 270:
                north = faceEast;
                east = faceSouth;
                south = faceWest;
                west = faceNorth;
                gameObject.transform.Rotate(new Vector3(0, 270, 0));
                break;
        }
        faceNorth = north;
        faceEast = east;
        faceSouth = south;
        faceWest = west;
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
}
