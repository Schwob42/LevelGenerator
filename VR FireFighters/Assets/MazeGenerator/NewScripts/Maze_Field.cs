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

    public bool CellExists(int x, int y)
    {
        if(x<0 || x>= mazeSizeX || y<0 || y >= mazeSizeY)
        {
            return false;
        }
        return true;
    }

    public void SetCellDefault(int x, int y)
    {

        //Ecken
        if (x == 0 && y == 0) cells[y, x] = new Maze_Cell(true, true, false, false);                            //linke untere Ecke
        else if (x == 0 && y == mazeSizeY) cells[y, x] = new Maze_Cell(false, true, true, false);               //linke obere Ecke
        else if (x == mazeSizeX && y == mazeSizeY) cells[y, x] = new Maze_Cell(false, false, true, true);       //rechte obere Ecke
        else if (x == mazeSizeX && y != 0) cells[y, x] = new Maze_Cell(true, false, false, true);               //rechte untere Ecke

        //Ränder
        else if (x == 0 && y != 0) cells[y, x] = new Maze_Cell(true, true, true, false);                        //linker Rand
        else if (x != 0 && y == mazeSizeY) cells[y, x] = new Maze_Cell(false, true, true, true);                //oberer Rand
        else if (x == mazeSizeX && y != mazeSizeY) cells[y, x] = new Maze_Cell(true, false, true, true);        //rechter Rand
        else if (x != 0 && y == 0) cells[y, x] = new Maze_Cell(true, true, false, true);                        //unterer Rand

        //Mitte
        else cells[y, x] = new Maze_Cell(true, true, true, true);                                               //irgendwo mittendrin

    }

    private MazeCellGameObject CreateCopyOfGameObject(MazeCellGameObject go)
    {
        MazeCellGameObject mzo = Instantiate(go);
        mzo.gameObject.SetActive(false);
        return mzo;
    }

    /**
     * Versucht ein übergebenes Object dem Maze hinzuzufügen.
     * 
     * Gibt True zurück, wenn es funktioniert hat,
     * False, wenn nicht.
     * 
     */
    public bool SetGameObjectIntoCell(int x, int y, int rotation, MazeCellGameObject mazeCellGameObject)
    {
        //Debug.Log("I've got an " + mazeCellGameObject + " for " + (x, y, rotation));
        MazeCellGameObject mzo = CreateCopyOfGameObject(mazeCellGameObject);
        mzo.RotateObject(rotation);

        if (!CheckObjectFacing(x, y, mzo))
        {
            Destroy(mzo.gameObject);
            return false;
        }

        if (!cells[y, x].SetMazeCellObject(rotation, mzo))
        {
            Destroy(mzo.gameObject);
            return false;
        }

        mzo.transform.position = new Vector3(x*2, 1, y*2);
        mzo.gameObject.SetActive(true);
        //mzo.transform.rotation = Quaternion.Euler(new Vector3(0,rotation,0));

        return true;
    }


    /**
     * Die Methode prüft, ob ein übergebenes Objekt in eine leere Zelle passt ohne, dass es Widersprüche zu Nachbarzellen gibt.
     * Sollte es in die jeweilige Richtung keine Zelle geben, wird False ausgegeben. 
     * TODO: Drehung veranlassen! (Im Generator)
     * 
     * Es wird erst geprüft, ob es vom Objekt aus eine Orientierung nach Norden, Osten, Süden oder Westen gibt und wenn ja
     * wird überprüft, ob die Zelle dort eine entgegengesetzt Öffnung hat.
     * Sprich, wir wollen bspw. einen Corridor (ohne Drehung) einfügen, dann schauen wir in der Zelle darüber, ob sie eine Passage nach Süden hat und in der Zelle darunter, 
     * ob sie eine Passage nach Norden hat. 
     * TODO: Bild dazu machen. Vllt fällt da außer mir noch jemand drauf rein ^^ 
     * 
     * Returns TRUE when everything is fine
     * FALSE else. 
     */
    private bool CheckObjectFacing(int x, int y, MazeCellGameObject obj)
    {
        //Debug.Log((x, y));
        if (obj == null) return true;
        //Debug.Log(obj + " " +obj.GetFaceWest());
        // 
        if (CellExists(x,y-1) && GetCellAt(x,y-1).GetPassageNorth() && !GetCellAt(x,y-1).GetIsEmpty())
        {
            // Passage zur unteren Zelle verbaut?
            if (!obj.GetFaceSouth()) return false;
        }
        if (CellExists(x-1, y) && GetCellAt(x - 1, y).GetPassageEast() && !GetCellAt(x - 1, y).GetIsEmpty())
        {
            // Passage zur linken Zelle verbaut?
            if (!obj.GetFaceWest()) return false;
        }
        if (CellExists(x, y+1) && GetCellAt(x, y + 1).GetPassageSouth() && !GetCellAt(x, y + 1).GetIsEmpty())
        {
            // Passage zur oberen Zelle verbaut?
            if (!obj.GetFaceNorth()) return false;
        }
        if (CellExists(x+1, y) && GetCellAt(x + 1, y).GetPassageWest() && !GetCellAt(x + 1, y).GetIsEmpty())
        {
            // Passage zur rechten Zelle verbaut?
            if (!obj.GetFaceEast()) return false;
        }

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

        if (obj.GetFaceSouth())     // Wenn das Objekt eine Passage nach Süden hat
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
