using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze_Field_Generator : MonoBehaviour
{
    [SerializeField]
    MazeSettings mazeSettings;

    [SerializeField] 
    MazeCellGameObjects mazeGameObjects;
    
    
    private Maze_Field maze;

    private int mazeSizeX, mazeSizeY;

    private float probabilityCorridor;
    private float probabilityCorner;
    private float probabilityEnd;
    private float probabilityTCrossing;
    private float probabilityXCrossing;

    /**
     * TODO: Remove Start() and add another way to start the generator. UI with a button and so on
     * 
     */
    void Start()
    {
        GenerateField();
    }

   

    public void GenerateField()
    {
        (int, int) size = mazeSettings.GetMazeSize();
        mazeSizeX = size.Item1;
        mazeSizeY = size.Item2;

        (float, float, float, float, float) probs = mazeSettings.GetProbabilities();
        probabilityCorridor = probs.Item1;
        probabilityCorner = probs.Item2;
        probabilityEnd = probs.Item3;
        probabilityTCrossing = probs.Item4;
        probabilityXCrossing = probs.Item5;

        maze = new Maze_Field(mazeSizeY, mazeSizeY, null);
        SetEveryCellToDefault();
        
        GeneratePath();
    }

    private void SetEveryCellToDefault()
    {
        for(int y = 0; y<mazeSizeY; y++)
        {
            for(int x = 0; x<mazeSizeX; x++)
            {
                maze.SetCellDefault(x,y);
            }
        }
    }


    


    private void GeneratePath()
    {
        
        (int, int) pos = mazeSettings.GetStartPosition();
        int corridorMinLength = mazeSettings.GetMinCorridorLength();

        maze.SetGameObjectIntoCell(pos.Item1, pos.Item2, 0, mazeGameObjects.Start);

        SetNextCell(null, pos.Item1, pos.Item2, corridorMinLength);
    }


    private void GenerateRoom(){
        //TODO
    }

    /**
    * This method will set only one Cell (at the given position)
    * The method searchs (no random) for a fitting CellGameObject like T_Crossing or X_Crossing
    *
    * @parameters 
    */
    private void SetNextCellOnly(int x, int y){
        // The next cell to set
        Maze_Cell nextCellToSet = maze.GetCellAt(x,y);

        if( nextCellToSet.GetPassageNorth() && maze.GetCellAt(x,y+1).GetPassageSouth() &&   //Passage to the cell in the north is needed
            nextCellToSet.GetPassageEast() && maze.GetCellAt(x+1,y).GetPassageWest() &&     //Passage to the cell in the east is needed
            nextCellToSet.GetPassageSouth() && maze.GetCellAt(x,y-1).GetPassageNorth() &&     //Passage to the cell in the south is needed
            nextCellToSet.GetPassageWest() && maze.GetCellAt(x-1,y).GetPassageEast()     //Passage to the cell in the west is needed
        ){
            // All directions needed => X_Crossing is needed
            nextCellToSet.SetMazeCellPathObject(0, mazeGameObjects.X_Crossing);
        }
        else if(
            nextCellToSet.GetPassageNorth() && maze.GetCellAt(x,y+1).GetPassageSouth() &&   //Passage to the cell in the north is needed
            nextCellToSet.GetPassageEast() && maze.GetCellAt(x+1,y).GetPassageWest() &&     //Passage to the cell in the east is needed
            nextCellToSet.GetPassageSouth() && maze.GetCellAt(x,y-1).GetPassageNorth()     //Passage to the cell in the south is needed
        ){
            // All directions except West needed T_Crossing is needed
            nextCellToSet.SetMazeCellPathObject(0, mazeGameObjects.X_Crossing);
        }
        else if(
            nextCellToSet.GetPassageSouth() && maze.GetCellAt(x,y-1).GetPassageNorth() &&   //Passage to the cell in the north is needed
            nextCellToSet.GetPassageEast() && maze.GetCellAt(x+1,y).GetPassageWest() &&     //Passage to the cell in the east is needed
            nextCellToSet.GetPassageWest() && maze.GetCellAt(x-1,y).GetPassageEast()     //Passage to the cell in the west is needed
        ){
            // All directions except North needed T_Crossing is needed
            nextCellToSet.SetMazeCellPathObject(90, mazeGameObjects.X_Crossing);
        }
        else if( 
            nextCellToSet.GetPassageNorth() && maze.GetCellAt(x,y+1).GetPassageSouth() &&   //Passage to the cell in the north is needed
            nextCellToSet.GetPassageSouth() && maze.GetCellAt(x,y-1).GetPassageNorth() &&     //Passage to the cell in the south is needed
            nextCellToSet.GetPassageWest() && maze.GetCellAt(x-1,y).GetPassageEast()     //Passage to the cell in the west is needed
        ){
            // All directions except East needed T_Crossing is needed
            nextCellToSet.SetMazeCellPathObject(180, mazeGameObjects.T_Crossing);
        }
        else if( 
            nextCellToSet.GetPassageNorth() && maze.GetCellAt(x,y+1).GetPassageSouth() &&   //Passage to the cell in the north is needed
            nextCellToSet.GetPassageEast() && maze.GetCellAt(x+1,y).GetPassageWest() &&     //Passage to the cell in the east is needed
            nextCellToSet.GetPassageWest() && maze.GetCellAt(x-1,y).GetPassageEast()     //Passage to the cell in the west is needed
        ){
            // All directions except South needed T_Crossing is needed
            nextCellToSet.SetMazeCellPathObject(270, mazeGameObjects.X_Crossing);
        }
        else if( 
            nextCellToSet.GetPassageNorth() && maze.GetCellAt(x,y+1).GetPassageSouth() &&   //Passage to the cell in the north is needed
            nextCellToSet.GetPassageSouth() && maze.GetCellAt(x,y-1).GetPassageNorth()     //Passage to the cell in the south is needed
        ){
            // Corridor is needed
            nextCellToSet.SetMazeCellPathObject(0, mazeGameObjects.Corridor);
        }
        else if( 
            nextCellToSet.GetPassageEast() && maze.GetCellAt(x+1,y).GetPassageWest() &&     //Passage to the cell in the east is needed
            nextCellToSet.GetPassageWest() && maze.GetCellAt(x-1,y).GetPassageEast()     //Passage to the cell in the west is needed
        ){
            // Corridor is needed
            nextCellToSet.SetMazeCellPathObject(90, mazeGameObjects.Corridor);
        }
        else if( 
            nextCellToSet.GetPassageNorth() && maze.GetCellAt(x,y+1).GetPassageSouth() &&   //Passage to the cell in the north is needed
            nextCellToSet.GetPassageEast() && maze.GetCellAt(x+1,y).GetPassageWest()     //Passage to the cell in the east is needed
        ){
            // Corner is needed
            nextCellToSet.SetMazeCellPathObject(0, mazeGameObjects.Corner);
        }
        else if(
            nextCellToSet.GetPassageEast() && maze.GetCellAt(x+1,y).GetPassageWest() &&     //Passage to the cell in the east is needed
            nextCellToSet.GetPassageSouth() && maze.GetCellAt(x,y-1).GetPassageNorth()     //Passage to the cell in the south is needed
        ){
            // Corner is needed
            nextCellToSet.SetMazeCellPathObject(90, mazeGameObjects.Corner);
        }
        else if(
            nextCellToSet.GetPassageSouth() && maze.GetCellAt(x,y-1).GetPassageNorth() &&     //Passage to the cell in the south is needed
            nextCellToSet.GetPassageWest() && maze.GetCellAt(x-1,y).GetPassageEast()     //Passage to the cell in the west is needed
        ){
            // Corner is needed
            nextCellToSet.SetMazeCellPathObject(180, mazeGameObjects.Corner);
        }
        else if( 
            nextCellToSet.GetPassageNorth() && maze.GetCellAt(x,y+1).GetPassageSouth() &&   //Passage to the cell in the north is needed
            nextCellToSet.GetPassageWest() && maze.GetCellAt(x-1,y).GetPassageEast()     //Passage to the cell in the west is needed
        ){
            // Corner is needed
            nextCellToSet.SetMazeCellPathObject(270, mazeGameObjects.Corner);
        }
        else if( nextCellToSet.GetPassageNorth() && maze.GetCellAt(x,y+1).GetPassageSouth())    //Passage to the cell in the north is needed
        {
            nextCellToSet.SetMazeCellPathObject(0, mazeGameObjects.End);
        }
        else if( nextCellToSet.GetPassageEast() && maze.GetCellAt(x+1,y).GetPassageWest())      //Passage to the cell in the east is needed
        {
            nextCellToSet.SetMazeCellPathObject(90, mazeGameObjects.End);
        }
        else if ( nextCellToSet.GetPassageSouth() && maze.GetCellAt(x,y-1).GetPassageNorth())     //Passage to the cell in the south is needed
        {
            nextCellToSet.SetMazeCellPathObject(180, mazeGameObjects.End);
        }
        else if( nextCellToSet.GetPassageWest() && maze.GetCellAt(x-1,y).GetPassageEast())      //Passage to the cell in the west is needed
        {
            nextCellToSet.SetMazeCellPathObject(270, mazeGameObjects.End);
        }
    }

    /**
    * This method will set only one Cell (at the given position)
    * The method searchs for the best a fitting CellGameObject Corridor OR End
    * The best will is to find a orientation for a corridor. The End will be placed in case there is no other option.
    *
    * @parameters 
    */
    private void SetCorridorOrEndNextCellOnly(int x, int y){

    }


    /**
     * Methode für den **rekursiven** Aufruf zur Pfadgenerierung. 
     * übergeben wird die Position der letzten Zelle und die noch zu erreichende gewünschte (Ende ist immer erlaubt) minimale Korridorlänge;
     * 
     * previousCell ist die Zelle, aus der die aktuelle Zelle erzeugt wurde. 
     */
    private void SetNextCell(Maze_Cell previousCell, int x, int y, int corridorMinLength)
    {
        // Last changed cell on this path
        Maze_Cell currentCell = maze.GetCellAt(x, y);

        //Debug.Log(maze);


        //Debug.Log("Looking for " + (x, y) + " " + mc.GetIsEmpty());



        if (corridorMinLength > 0)   //Nur Passage oder Ende m�glich damit sind nur Orientierungen in entgegengesetzte Richtungen m�glich und eine der Richtungen ist bereits belegt (irgendwo kommen wir ja her)
        {
            
            if (currentCell.GetPassageNorth() && maze.GetCellAt(x, y + 1) != previousCell)
            {
                //Debug.Log(previousCell+ " und " + maze.GetCellAt(x, y + 1) + " sind gleich:" + (maze.GetCellAt(x, y + 1) == previousCell));
                if (maze.SetGameObjectIntoCell(x, y + 1, 0, mazeGameObjects.Corridor))
                {
                    SetNextCell(currentCell, x, y + 1, corridorMinLength - 1);
                }
                else
                {
                    if ((y + 2 < mazeSizeY) && maze.GetCellAt(x, y + 2).GetMazeCellState() == MazeCellState.Path)
                    {
                        ChangeZellGameObjectAt(0, x, y + 2);
                        maze.SetGameObjectIntoCell(x, y + 1, 0, mazeGameObjects.Corridor);
                        SetNextCell(currentCell, x, y + 1, 0);
                    }
                    maze.SetGameObjectIntoCell(x, y + 1, 180, mazeGameObjects.End);
                }
            }
            if (currentCell.GetPassageSouth() && maze.GetCellAt(x, y - 1) != previousCell)
            {
                //Debug.Log(previousCell + " und " + maze.GetCellAt(x, y + 1) + " sind gleich:" + (maze.GetCellAt(x, y - 1) == previousCell));
                //Debug.Log("Orientations (" + x + "," + y + "):" + mc.GetPassageNorth() + "," + mc.GetPassageEast() + "," + mc.GetPassageSouth() + "," + mc.GetPassageWest() + ",");
                if (maze.SetGameObjectIntoCell(x, y - 1, 0, mazeGameObjects.Corridor))
                {                    
                    SetNextCell(currentCell, x, y - 1, corridorMinLength - 1);
                }
                else
                {
                    if ((y - 2 >= 0) && maze.GetCellAt(x, y - 2).GetMazeCellState() == MazeCellState.Path)
                    {
                        ChangeZellGameObjectAt(0, x, y - 2);
                        maze.SetGameObjectIntoCell(x, y - 1, 0, mazeGameObjects.Corridor);
                        SetNextCell(currentCell, x, y - 1, 0);
                    }
                    else
                    {
                        maze.SetGameObjectIntoCell(x, y - 1, 0, mazeGameObjects.End);
                    }
                }
            }
            if (currentCell.GetPassageEast() && maze.GetCellAt(x+1, y) != previousCell)
            {
                if (maze.SetGameObjectIntoCell(x + 1, y, 90, mazeGameObjects.Corridor))
                {
                    SetNextCell(currentCell, x + 1, y, corridorMinLength - 1);
                }
                else
                {
                    if ((x + 2 < mazeSizeX) && maze.GetCellAt(x + 2, y).GetMazeCellState() == MazeCellState.Path)
                    {
                        ChangeZellGameObjectAt(0, x + 2, y);
                        maze.SetGameObjectIntoCell(x + 1, y, 90, mazeGameObjects.Corridor);
                        SetNextCell(currentCell, x + 1, y, 0);
                    }
                    else
                    {
                        maze.SetGameObjectIntoCell(x + 1, y, -90, mazeGameObjects.End);
                    }
                }
            }
            if (currentCell.GetPassageWest() && maze.GetCellAt(x-1, y) != previousCell)
            {
                if (maze.SetGameObjectIntoCell(x - 1, y, 90, mazeGameObjects.Corridor))
                {
                    SetNextCell(currentCell, x - 1, y, corridorMinLength - 1);
                }
                else
                {
                    if ((x - 2 >= 0) && maze.GetCellAt(x - 2, y).GetMazeCellState() == MazeCellState.Path)
                    {
                        ChangeZellGameObjectAt(0, x - 2, y);
                        maze.SetGameObjectIntoCell(x - 1, y, 90, mazeGameObjects.Corridor);
                        SetNextCell(currentCell, x - 1, y, 0);
                    }
                    else
                    {
                        maze.SetGameObjectIntoCell(x - 1, y, 90, mazeGameObjects.End);
                    }
                }
            }
        }
        /*
         * TODO:
         * Im Else sollte es noch den Fall <mc.GetPassage<Richtung> && maze.GetCellAt(x, y) is not empty> geben. In diesem soll geschaut werden, ob
         * das CellGameObject in der belegten Zelle so angepasst werden kann, dass ein Ende oder eine Kurve vermieden werden kann. 
         * Bspw sowas 
         * 
         * ---[ ]----
         *     |     
         *     |
         *     
         * K�nnte zu 
         * ----x---- (soll eine Kreuzung sein) 
         *     |     
         *     |
         *  
         *  werden
         */
        else   // random object is possible 
        {
            // Norden
            if (currentCell.GetPassageNorth() && maze.GetCellAt(x, y + 1).GetMazeCellState() == MazeCellState.Empty)
            {
                // Nach Norden weiterbauen
                if (FindObjectForCellAt(x, y + 1))
                {
                    SetNextCell(currentCell, x, y + 1, mazeSettings.GetMinCorridorLength());
                }
                
            }
            // Zelle von Zellenobjekt belegt => Umbauen, um ein unn�tiges Ende zu verhindern
            else if (currentCell.GetPassageNorth() && (maze.GetCellAt(x, y + 1).GetMazeCellState() == MazeCellState.Path) && maze.GetCellAt(x, y + 1) != previousCell)
            {
                ChangeZellGameObjectAt(90, x, y+2);
                FindObjectForCellAt(x,y+1);
                SetNextCellOnly(x,y+2);
            }
            
            // Osten
            if (currentCell.GetPassageEast() && maze.GetCellAt(x + 1, y).GetMazeCellState() == MazeCellState.Empty)
            {
                // Nach Osten weiterbauen
                if (FindObjectForCellAt(x+1, y))
                {
                    SetNextCell(currentCell, x+1, y, mazeSettings.GetMinCorridorLength());
                }
                    
            }
            // Zelle von Zellenobjekt belegt => Umbauen, um ein unn�tiges Ende zu verhindern
            else if (currentCell.GetPassageEast() && (maze.GetCellAt(x + 1, y).GetMazeCellState() == MazeCellState.Path) && maze.GetCellAt(x + 1, y) != previousCell)
            {
                ChangeZellGameObjectAt(90, x+2, y);
                // Nach Osten weiterbauen
                FindObjectForCellAt(x + 1, y);
                SetNextCellOnly(x+2,y);
            }

            // S�den
            if (currentCell.GetPassageSouth() && maze.GetCellAt(x, y - 1).GetMazeCellState() == MazeCellState.Empty)
            {
                // Nach S�den weiterbauen
                if (FindObjectForCellAt(x, y - 1))
                {
                    SetNextCell(currentCell, x, y - 1, mazeSettings.GetMinCorridorLength());
                }
            }

            // Zelle von Zellenobjekt belegt => Umbauen, um ein unn�tiges Ende zu verhindern
            else if (currentCell.GetPassageSouth() && (maze.GetCellAt(x, y - 1).GetMazeCellState() == MazeCellState.Path) && maze.GetCellAt(x, y - 1) != previousCell)
            {
                 ChangeZellGameObjectAt(180, x, y-2);
                // Nach S�den weiterbauen
                FindObjectForCellAt(x, y - 1);
                SetNextCellOnly(x,y-2);
            }

            // Westen
            if (currentCell.GetPassageWest() && maze.GetCellAt(x - 1, y).GetMazeCellState() == MazeCellState.Empty)
            {
                // Nach Westen weiterbauen
                if (FindObjectForCellAt(x - 1, y))
                {
                    SetNextCell(currentCell, x - 1, y, mazeSettings.GetMinCorridorLength());
                }
            }
            // Zelle von Zellenobjekt belegt => Umbauen, um ein unn�tiges Ende zu verhindern
            else if (currentCell.GetPassageWest() && (maze.GetCellAt(x - 1, y).GetMazeCellState() == MazeCellState.Path) && maze.GetCellAt(x - 1, y) != previousCell)
            {
                 ChangeZellGameObjectAt(90, x-2, y);
                // Nach Westen weiterbauen
                FindObjectForCellAt(x - 1, y);
                SetNextCellOnly(x-2,y);
            }
        }
    }

    /**
    * Ver�ndert das GameObject in der Zelle zum n�chst "hoheren" (Au�er wenn es ein Ende, Start oder X-Kreuzung ist)
    * Coridor und Kurve werden zur T-Kreuzung
    * T-Kreuzung wird zur X-Kreuzung
    */
    private void ChangeZellGameObjectAt(int rotation, int x_NextCell, int y_NextCell)
    {
        Maze_Cell mc = maze.GetCellAt(x_NextCell, y_NextCell);

        mc.RemoveCellObject();
       /* switch (mc.GetMazeCellGameObject().GetMazeCellGameObjectType())
        {
            case MazeCellGameObjectType.Corridor:
            case MazeCellGameObjectType.Corner:
                mc.RemoveCellObject();
                //mc.SetMazeCellObject(rotation, mazeGameObjects.X_Crossing);
                break;
            case MazeCellGameObjectType.T_Crossing:
                mc.RemoveCellObject();
                //mc.SetMazeCellObject(0, mazeGameObjects.X_Crossing);
                break;
        }*/
    }


    /**
     * 
     * 
     * Returns false, when the way ends with the chosen object
     * Returns true, when the way goes on
     */
    private bool FindObjectForCellAt(int x, int y)
    {
        Maze_Cell cellToFill = maze.GetCellAt(x, y);
        
        float maxRange = 0;
        //Debug.Log("Orientations (" + x + "," + y +"):"  + cellToFill.GetPassageNorth() + "," + cellToFill.GetPassageEast() + "," + cellToFill.GetPassageSouth() + "," + cellToFill.GetPassageWest() + ",");

        // MazeCellObjects that could fit in. But they need not to fit. 
        // For Example when you need a T_Crossin, a Corridor is also possible but will not fit in.
        List<(MazeCellGameObject, float)> mco = new List<(MazeCellGameObject, float)>();

        switch (cellToFill.GetPassageNorth(), cellToFill.GetPassageEast(), cellToFill.GetPassageSouth(), cellToFill.GetPassageWest())
        {
            case (false, false, false, false):
                // Should be impossible but better save than sorry
                Debug.LogError("So a glumb!");
                break;
            case (true, true, true, true):
                // X-Crossing, T-Crossing, Corridor, Corner and End are possible
                mco.Add((mazeGameObjects.X_Crossing, probabilityXCrossing));
                mco.Add((mazeGameObjects.T_Crossing, probabilityTCrossing));
                mco.Add((mazeGameObjects.Corridor, probabilityCorridor));
                mco.Add((mazeGameObjects.Corner, probabilityCorner));
                mco.Add((mazeGameObjects.End, probabilityEnd));
                maxRange = probabilityXCrossing + probabilityTCrossing + probabilityCorridor + probabilityCorner + probabilityEnd;
                break;

            case (false, true, true, true):
            case (true, false, true, true):
            case (true, true, false, true):
            case (true, true, true, false):
                // T-Crossing, Corridor, Corner and End are possible
                mco.Add((mazeGameObjects.T_Crossing, probabilityTCrossing));
                mco.Add((mazeGameObjects.Corridor, probabilityCorridor));
                mco.Add((mazeGameObjects.Corner, probabilityCorner));
                mco.Add((mazeGameObjects.End, probabilityEnd));
                maxRange = probabilityTCrossing + probabilityCorridor + probabilityCorner + probabilityEnd;
                break;

            case (false, false, true, true):
            case (false, true, true, false):
            case (true, false, false, true):
            case (true, true, false, false):
                // Corner and End are possible
                mco.Add((mazeGameObjects.Corner, probabilityCorner));
                mco.Add((mazeGameObjects.End, probabilityEnd));
                maxRange = probabilityCorner + probabilityEnd;
                break;

            case (false, true, false, true):
            case (true, false, true, false):
                // Corridor and End are possible
                mco.Add((mazeGameObjects.Corridor, probabilityCorridor));
                mco.Add((mazeGameObjects.End, probabilityEnd));
                maxRange = probabilityCorridor + probabilityEnd;
                break;

            case (true, false, false, false):
            case (false, true, false, false):
            case (false, false, true, false):
            case (false, false, false, true):
                // Only End is possible
                mco.Add((mazeGameObjects.End, probabilityEnd));
                maxRange = probabilityEnd;
                break;
        }


        //Debug.Log("Max Range: " + maxRange);

        float randomValue = Random.Range(0f, maxRange);
        float sum = 0;

        MazeCellGameObject objToUse = null;
        float posiOfObj = 0;

        while (mco.Count > 0) { 
            for (int i = 0; i < mco.Count; i++)
            {
                if (mco[i].Item2 == 0)
                {
                    // TODO: Besser machen und oben in die switch Case integrieren 
                    mco.Remove(mco[i]);
                    i--;
                    continue;
                }
                sum += mco[i].Item2;
                if (randomValue <= sum)
                {
                    //Debug.Log("Choice: " + mco[i].Item1);
                    objToUse = mco[i].Item1;
                    posiOfObj = mco[i].Item2;
                    break;
                }
            }

            if (!maze.SetGameObjectIntoCell(x, y, 0, objToUse))
            {
                if (!maze.SetGameObjectIntoCell(x, y, 90, objToUse))
                {
                    if (!maze.SetGameObjectIntoCell(x, y, 180, objToUse))
                    {
                        if(!maze.SetGameObjectIntoCell(x, y, 270, objToUse))
                        {
                            Debug.Log((x, y, objToUse) + " was not possible ");
                            mco.Remove((objToUse, posiOfObj));
                            continue;
                        }
                    }
                }
            }
            break;
        }

        if (objToUse.Equals(mazeGameObjects.End))
        {
            Debug.Log("This is the end " + mazeGameObjects);
            return false;
        }

        

        return true;
    }
}
