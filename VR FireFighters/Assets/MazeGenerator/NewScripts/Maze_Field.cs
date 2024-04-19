using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze_Field
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
            //Debug.Log((x,y) + " existiert nicht");
            return false;
        }
        //Debug.Log((x,y) + " existiert");
        return true;
    }



    public void SetCellDefault(int x, int y)
    {

        //Ecken
        if (x == 0 && y == 0) cells[y, x] = new Maze_Cell(true, true, false, false, x, y);                                //linke untere Ecke
        else if (x == 0 && y == mazeSizeY-1) cells[y, x] = new Maze_Cell(false, true, true, false, x, y);                 //linke obere Ecke
        else if (x == mazeSizeX-1 && y == mazeSizeY-1) cells[y, x] = new Maze_Cell(false, false, true, true, x, y);       //rechte obere Ecke
        else if (x == mazeSizeX-1 && y == 0) cells[y, x] = new Maze_Cell(true, false, false, true, x, y);                 //rechte untere Ecke

        //R�nder
        else if (x == 0 && y != 0) cells[y, x] = new Maze_Cell(true, true, true, false, x, y);                            //linker Rand
        else if (x != 0 && y == mazeSizeY-1) cells[y, x] = new Maze_Cell(false, true, true, true, x, y);                  //oberer Rand
        else if (x == mazeSizeX-1 && y != mazeSizeY-1) cells[y, x] = new Maze_Cell(true, false, true, true, x, y);        //rechter Rand
        else if (x != 0 && y == 0) cells[y, x] = new Maze_Cell(true, true, false, true, x, y);                            //unterer Rand

        //Mitte
        else cells[y, x] = new Maze_Cell(true, true, true, true, x, y);                                                   //irgendwo mittendrin

    }

    private MazeCellGameObject CreateCopyOfGameObject(MazeCellGameObject go)
    {
        MazeCellGameObject mzo = UnityEngine.MonoBehaviour.Instantiate(go);
        mzo.gameObject.SetActive(false);
        return mzo;
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
        //Debug.Log("I've got an " + mazeCellGameObject + " for " + (x, y, rotation));
        MazeCellGameObject mzo = CreateCopyOfGameObject(mazeCellGameObject);
        mzo.RotateObject(rotation);

        if (!CheckObjectFacing(x, y, mzo))
        {
            UnityEngine.MonoBehaviour.Destroy(mzo.gameObject, 0f);
            return false;
        }

        if (!cells[y, x].SetMazeCellPathObject(rotation, mzo))
        {
            UnityEngine.MonoBehaviour.Destroy(mzo.gameObject, 0f);
            return false;
        }

        mzo.transform.position = new Vector3(x*2, 1, y*2);
        mzo.gameObject.SetActive(true);
        //mzo.transform.rotation = Quaternion.Euler(new Vector3(0,rotation,0));

        return true;
    }

    public bool ReplaceCellObject(int x, int y, MazeCellGameObjects pathGameObjects){
        Maze_Cell cell = GetCellAt(x,y);
        if(cell.GetMazeCellGameObject() == null) return false;
        
        MazeCellGameObjectType type = cell.GetMazeCellGameObject().GetMazeCellGameObjectType();
        int rotation = cell.GetMazeCellGameObject().GetRotation();

        
        cell.RemoveCellObject();
        switch (type){
            case MazeCellGameObjectType.Corridor:
                return SetGameObjectIntoCell(x, y, rotation, pathGameObjects.Corridor);
            case MazeCellGameObjectType.Corner:
                return SetGameObjectIntoCell(x, y, rotation, pathGameObjects.Corner);
            case MazeCellGameObjectType.T_Crossing:
                return SetGameObjectIntoCell(x, y, rotation, pathGameObjects.T_Crossing);
            case MazeCellGameObjectType.End:
                return SetGameObjectIntoCell(x, y, rotation, pathGameObjects.End);
        }       

        return false;
    }


    public void ReservateCellForRoom(int x, int y)
    {
        cells[y, x].ReservateForRoom();
    }

    public bool SetRoomObjectIntoCell(Maze_Cell cell, int rotation, MazeCellGameObject roomObject){
        //Debug.Log("I've got an " + roomObject + " for " + (x, y, rotation));
        MazeCellGameObject mzo = CreateCopyOfGameObject(roomObject);
        mzo.RotateObject(rotation);

        if (!CheckRoomFacing(cell.x , cell.y, mzo))
        {
            Debug.Log("Wrong room facing");
            UnityEngine.MonoBehaviour.Destroy(mzo.gameObject, 0f);
            return false;
        }

        if (!cell.SetMazeCellRoomObject(rotation, mzo))
        {
            Debug.Log("Couldn't place room");
            UnityEngine.MonoBehaviour.Destroy(mzo.gameObject, 0f);
            return false;
        }

        mzo.transform.position = new Vector3(cell.x*2, 1, cell.y*2);
        mzo.gameObject.SetActive(true);
        //mzo.transform.rotation = Quaternion.Euler(new Vector3(0,rotation,0));

        return true;
    }

    private bool CheckRoomFacing(int x, int y, MazeCellGameObject obj){
        if (obj == null) return true;

        
        
        //Debug.Log("All good");
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
        if (obj == null) return true;
        //Debug.Log(obj + " " +obj.GetFaceWest());
        // 
        //Debug.Log("Orientations (" + 9 + "," + 8 + "):" + GetCellAt(9,8).GetPassageNorth() + "," + GetCellAt(9, 8).GetPassageEast() + "," + GetCellAt(9, 8).GetPassageSouth() + "," + GetCellAt(9, 8).GetPassageWest() + ",");
        
        if (CellExists(x,y-1) && GetCellAt(x,y-1).GetPassageNorth() && !(GetCellAt(x,y-1).GetMazeCellState() == MazeCellState.Empty))
        {
            // Passage zur unteren Zelle verbaut?
            if (!obj.GetFaceSouth()) return false;
        }
        if (CellExists(x-1, y) && GetCellAt(x - 1, y).GetPassageEast() && !(GetCellAt(x - 1, y).GetMazeCellState() == MazeCellState.Empty))
        {
            // Passage zur linken Zelle verbaut?
            if (!obj.GetFaceWest()) return false;
        }
        if (CellExists(x, y+1) && GetCellAt(x, y + 1).GetPassageSouth() && !(GetCellAt(x, y + 1).GetMazeCellState() == MazeCellState.Empty))
        {
            // Passage zur oberen Zelle verbaut?
            if (!obj.GetFaceNorth()) return false;
        }
        if (CellExists(x+1, y))
        {
            // Passage zur rechten Zelle verbaut?
            if (GetCellAt(x + 1, y).GetPassageWest() && !(GetCellAt(x + 1, y).GetMazeCellState() == MazeCellState.Empty) && !obj.GetFaceEast()) return false;
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
        //Debug.Log((x,y));
        if(x < 0 || x >= mazeSizeX || y<0 || y>= mazeSizeY){
            return null;
        }
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
