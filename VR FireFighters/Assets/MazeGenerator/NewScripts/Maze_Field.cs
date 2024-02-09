using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze_Field : MonoBehaviour
{
    private Maze_Cell[,] cells;

    private int mazeSizeX, mazeSizeY;

    public bool SetCell(int x, int y, MazeCellGameObject mazeCellGameObject)
    {
        if (!CheckObjectFacing(x, y, mazeCellGameObject)) return false;

        if(!cells[y, x].SetMazeCellObject(mazeCellGameObject)) return false;

        return true;
    }


    /**
     * Die Methode prüft, ob ein übergebenes Objekt in eine leere Zelle passt ohne, dass es Widersprüche zu Nachbarzellen gibt.
     * Sollte es in die jeweilige Richtung keine Zelle geben, wird False ausgegeben. 
     * TODO: Drehung veranlassen! (Im Generator)
     * 
     * Es wird erst geprüft, ob es vom Objekt aus eine Orientierung nach Norden, Osten, Süden oder Westen gibt und wenn ja
     * wird überprüft, ob die Zelle dort ebenfalls eine offene Ausrichtung hat. 
     * 
     * Returns TRUE when everything is fine
     * FALSE else.
     * 
     */
    private bool CheckObjectFacing(int x, int y, MazeCellGameObject obj)
    {
        if (obj.GetFaceNorth())
        {
            if (y == mazeSizeY - 1) return false;   //out of maze
            if (obj.GetFaceNorth() != cells[y+1,x].GetPassageNorth()) return false;
        }
        if (obj.GetFaceEast())
        {
            if (x == mazeSizeX - 1) return false;   //out of maze
            if (obj.GetFaceEast() != cells[y, x+1].GetPassageEast()) return false;
        }
        if (obj.GetFaceSouth())
        {
            if (y == 0) return false;   //out of maze
            if (obj.GetFaceSouth() != cells[y - 1, x].GetPassageSouth()) return false;
        }
        if (obj.GetFaceWest())
        {
            if (x == 0) return false;   //out of maze
            if (obj.GetFaceWest() != cells[y, x-1].GetPassageWest()) return false;
        }
        return true;
    }
}
