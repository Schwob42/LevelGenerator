using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze_Cell : MonoBehaviour
{
    /*
     * True when the cell is empty.
     * False else.
     */
    bool isEmpty;

    /*
     * True when the passage is free, false if blocked.
     * Blocked by edge of field, cell walls or what ever doesn't matter. 
     */
    bool passageNorth, passageEast, passageSouth, passageWest;

    /*
     * The object to set into the cell. Could be null.
     */
    MazeCellGameObject mazeCellObject;

    public Maze_Cell()
    {
        isEmpty = true;
    }

    public bool SetMazeCellObject(MazeCellGameObject obj)
    {
        mazeCellObject = obj;

        if (obj == null)
        {
            isEmpty = true;
            passageNorth = passageEast = passageSouth = passageWest = true;
            return true;
        }
        else
        {
            if (!CheckObjectFacing(obj)) return false;

            passageNorth = obj.GetFaceNorth();
            passageEast = obj.GetFaceEast();
            passageSouth = obj.GetFaceSouth();
            passageWest = obj.GetFaceWest();
            return true;
        }
    }

    /**
     * Die Methode prüft, ob ein übergebenes Objekt in eine leere Zelle passt ohne, dass Wände und Gänge kollifieren.
     * 
     * Es wird erst geprüft, ob es vom Objekt aus eine Orientierung nach Norden, Osten, Süden oder Westen gibt und wenn ja
     * wird überprüft, ob die Zelle dort ebenfalls eine offene Ausrichtung hat. 
     * 
     * Returns TRUE when everything is fine
     * FALSE else.
     * 
     * TODO: Es müssten eigentlich auch noch die Nachbarzellen überprüft werden -> Muss im Maze selbst passieren
     */
    private bool CheckObjectFacing(MazeCellGameObject obj)
    {
        if (obj.GetFaceNorth())
        {
            if (obj.GetFaceNorth() != passageNorth) return false;
        }
        if (obj.GetFaceEast())
        {
            if (obj.GetFaceEast() != passageEast) return false;
        }
        if (obj.GetFaceSouth())
        {
            if (obj.GetFaceSouth() != passageSouth) return false;
        }
        if (obj.GetFaceWest())
        {
            if (obj.GetFaceWest() != passageWest) return false;
        }
        return true;
    }


    public bool GetPassageNorth()
    {
        return passageNorth;
    }

    public bool GetPassageEast()
    {
        return passageEast;
    }

    public bool GetPassageSouth()
    {
        return passageSouth;
    }

    public bool GetPassageWest()
    {
        return passageWest;
    }
}
