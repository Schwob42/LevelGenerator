using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze_Field : MonoBehaviour
{
    private Maze_Cell[,] cells;

    private int mazeSizeX, mazeSizeY;

    public Maze_Field(int mazeSizeX, int mazeSizeY)
    {
        this.mazeSizeX = mazeSizeX;
        this.mazeSizeY = mazeSizeY;
        cells = new Maze_Cell[mazeSizeY, mazeSizeY];
    }

    public void SetCellDefault(int x, int y)
    {

        //Ecken
        if (x == 0 && y == 0) cells[y, x] = new Maze_Cell(true, true, false, false);                            //linke untere Ecke
        else if (x == 0 && y == mazeSizeY) cells[y, x] = new Maze_Cell(false, true, true, false);               //linke obere Ecke
        else if (x == mazeSizeX && y == mazeSizeY) cells[y, x] = new Maze_Cell(false, false, true, true);       //rechte obere Ecke
        else if (x == mazeSizeX && y != 0) cells[y, x] = new Maze_Cell(true, false, false, true);               //rechte untere Ecke

        //R�nder
        else if (x == 0 && y != 0) cells[y, x] = new Maze_Cell(true, true, true, false);                        //linker Rand
        else if (x != 0 && y == mazeSizeY) cells[y, x] = new Maze_Cell(false, true, true, true);                //oberer Rand
        else if (x == mazeSizeX && y != mazeSizeY) cells[y, x] = new Maze_Cell(true, false, true, true);        //rechter Rand
        else if (x != 0 && y == 0) cells[y, x] = new Maze_Cell(true, true, false, true);                        //unterer Rand

        //Mitte
        else cells[y, x] = new Maze_Cell(true, true, true, true);                                               //irgendwo mittendrin

    }

    /**
     * Versucht ein �bergebenes Object dem Maze hinzuzuf�gen.
     * 
     * Gibt True zur�ck, wenn es funktioniert hat,
     * False, wenn nicht.
     * 
     */
    public bool SetGameObjectIntoCell(int x, int y, int rotation, MazeCellGameObject mazeCellGameObject)
    {
        mazeCellGameObject.RotateObject(rotation);
        if (!CheckObjectFacing(x, y, mazeCellGameObject)) return false;

        if(!cells[y, x].SetMazeCellObject(rotation, mazeCellGameObject)) return false;

        mazeCellGameObject.RotateObject(-rotation);

        MazeCellGameObject mzo = GameObject.Instantiate(mazeCellGameObject);

        mzo.transform.position = new Vector3(x*2, 1, y*2);
        mzo.transform.Rotate(new Vector3(0,rotation,0));

        return true;
    }


    /**
     * Die Methode pr�ft, ob ein �bergebenes Objekt in eine leere Zelle passt ohne, dass es Widerspr�che zu Nachbarzellen gibt.
     * Sollte es in die jeweilige Richtung keine Zelle geben, wird False ausgegeben. 
     * TODO: Drehung veranlassen! (Im Generator)
     * 
     * Es wird erst gepr�ft, ob es vom Objekt aus eine Orientierung nach Norden, Osten, S�den oder Westen gibt und wenn ja
     * wird �berpr�ft, ob die Zelle dort eine entgegengesetzt �ffnung hat.
     * Sprich, wir wollen bspw. einen Corridor (ohne Drehung) einf�gen, dann schauen wir in der Zelle dar�ber, ob sie eine Passage nach S�den hat und in der Zelle darunter, 
     * ob sie eine Passage nach Norden hat. 
     * TODO: Bild dazu machen. Vllt f�llt da au�er mir noch jemand drauf rein ^^ 
     * 
     * Returns TRUE when everything is fine
     * FALSE else. 
     */
    private bool CheckObjectFacing(int x, int y, MazeCellGameObject obj)
    {
        //Debug.Log((x, y));
        if (obj == null) return true;

        if (obj.GetFaceNorth())     // Wenn das Objekt eine Passage nach Norden hat
        {
            if (y >= mazeSizeY - 1) return false;   //out of maze
            if (obj.GetFaceNorth() != cells[y+1,x].GetPassageSouth()) return false;
            //Debug.Log("North good");
        }

        if (obj.GetFaceEast())      // Wenn das Objekt eine Passage nach Osten hat
        {
            if (x >= mazeSizeX - 1) return false;   //out of maze
            if (obj.GetFaceEast() != cells[y, x+1].GetPassageWest()) return false;
            //Debug.Log("East good");
        }

        if (obj.GetFaceSouth())     // Wenn das Objekt eine Passage nach S�den hat
        {
            if (y <= 0) return false;   //out of maze
            if (obj.GetFaceSouth() != cells[y - 1, x].GetPassageNorth()) return false;
            //Debug.Log("South good");
        }

        if (obj.GetFaceWest())      // Wenn das Objekt eine Passage nach Westen hat
        {
            if (x <= 0) return false;   //out of maze
            if (obj.GetFaceWest() != cells[y, x-1].GetPassageEast()) return false;
            //Debug.Log("West good");
        }
        //Debug.Log("All good");
        return true;
    }

    public Maze_Cell GetCellAt(int x, int y)
    {
        return cells[y, x];
    }

    public int GetMazeSizeX()
    {
        return mazeSizeX;
    }

    public int GetMazeSizeY()
    {
        return mazeSizeY;
    }
}
