using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze_Cell
{
    /*
     * True when the cell is empty.
     * False else.
     */
    private bool isEmpty;

    /*
     * True when the passage is free, false if blocked.
     * Blocked by edge of field, cell walls or what ever doesn't matter.
     * The originals are used for saving the condition at the initialization. 
     */
    private bool passageNorthOriginal, passageEastOriginal, passageSouthOriginal, passageWestOriginal;  // NEVER EVER CHANGE THIS AND MAKE NO SETTER
    private bool passageNorth, passageEast, passageSouth, passageWest;

    /*
     * The object to set into the cell. Could be null.
     */
    private MazeCellGameObject mazeCellGameObject;

    public Maze_Cell(bool passageNorth, bool passageEast, bool passageSouth, bool passageWest)
    {
        isEmpty = true;

        this.passageNorthOriginal = this.passageNorth = passageNorth;
        this.passageEastOriginal = this.passageEast = passageEast;
        this.passageSouthOriginal = this.passageSouth = passageSouth;
        this.passageWestOriginal = this.passageWest = passageWest;
    }


    /**
     * Pr�ft, ob ein �bergebenes Objekt in die Zelle passt.
     * Return True, wenn m�glich
     * Return False, falls nicht m�glich
     */
    public bool CheckForMazeCellObject(int rotation, MazeCellGameObject obj)
    {
        if (obj == null) return false;
        if (!isEmpty) return false;
        if (!CheckObjectFacing(obj)) return false;


        //Debug.Log("Check True");
        return true;
    }

    /**
     * Setzt ein neues Objekt in die Zelle, sofern dies m�glich ist.
     * Return True, wenn erfolgreich
     * Return False, falls nicht m�glich, es gibt schon ein Objekt oder �bergebenes Objekt leer ist.
     */
    public bool SetMazeCellObject(int rotation, MazeCellGameObject obj)
    {
        if (!this.CheckForMazeCellObject(rotation, obj)) return false;
        
        //Debug.Log("Object to set in: " + (obj, rotation));
        passageNorth = obj.GetFaceNorth();
        passageEast = obj.GetFaceEast();
        passageSouth = obj.GetFaceSouth();
        passageWest = obj.GetFaceWest();
        mazeCellGameObject = obj;
        isEmpty = false;

        return true;
    }

    /**
     * Entfernt das GameObject in der Zelle, sofern vorhanden.
     * 
     * Gibt True zur�ck, sofern ein Objekt entfernt wurde,
     * False, wenn kein Objekt entfernt werden konnte. 
     */
    public bool RemoveCellObject()
    {
        if (mazeCellGameObject == null) return false;

        passageNorth = passageNorthOriginal;
        passageEast = passageEastOriginal;
        passageSouth = passageSouthOriginal;
        passageWest = passageWestOriginal;

        isEmpty = true;
        UnityEngine.MonoBehaviour.Destroy(mazeCellGameObject.gameObject);
        mazeCellGameObject = null;

        return true;
    }


    /**
     * Die Methode pr�ft, ob ein �bergebenes Objekt in eine leere Zelle passt ohne, dass W�nde und G�nge kollifieren.
     * 
     * Es wird erst gepr�ft, ob es vom Objekt aus eine Orientierung nach Norden, Osten, S�den oder Westen gibt und wenn ja
     * wird �berpr�ft, ob die Zelle dort ebenfalls eine offene Ausrichtung hat. 
     * Im weiteren Verlauf wird dasselbe mit den T�ren des Objekts gemacht, sofern das Objekt T�ren hat.
     * 
     * Returns TRUE when everything is fine
     * FALSE else.
     * 
     * TODO: Es m�ssten eigentlich auch noch die Nachbarzellen �berpr�ft werden -> Muss im Maze selbst passieren
     */
    private bool CheckObjectFacing(MazeCellGameObject obj)
    {
        // Passagen
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

        // T�ren
        if (!obj.objectHasDoor())   // fall das Objekt keine T�ren hat, wird der Rest �bersprungen
        {
            return true;
        }

        if (obj.GetDoorNorth())
        {
            if (obj.GetDoorNorth() != passageNorth) return false;
        }
        if (obj.GetDoorEast())
        {
            if (obj.GetDoorEast() != passageEast) return false;
        }
        if (obj.GetDoorSouth())
        {
            if (obj.GetDoorSouth() != passageSouth) return false;
        }
        if (obj.GetDoorWest())
        {
            if (obj.GetDoorWest() != passageWest) return false;
        }

        return true;
    }

    public MazeCellGameObject GetMazeCellGameObject()
    {
        return this.mazeCellGameObject;
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

    public bool GetIsEmpty()
    {
        return isEmpty;
    }
}
