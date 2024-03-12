using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.IO;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    MazeSettings generationSettings;

    [SerializeField] 
    MazeCellGameObjects pathGameObjects;
    
    
    private Maze_Field level;

    private int levelSizeX, levelSizeY;

    private float probabilityCorridor;
    private float probabilityCorner;
    private float probabilityEnd;
    private float probabilityTCrossing;
    private float probabilityXCrossing;

    /**
     * TODO: Remove Start() and add another way to start the generator. UI with a button and so on
     * 
     */
    void Start ()
    {
        GenerateField();
    }

   

    public void GenerateField ()
    {
        (int, int) size = generationSettings.GetMazeSize();
        levelSizeX = size.Item1;
        levelSizeY = size.Item2;

        (float, float, float, float, float) probs = generationSettings.GetProbabilities();
        probabilityCorridor = probs.Item1;
        probabilityCorner = probs.Item2;
        probabilityEnd = probs.Item3;
        probabilityTCrossing = probs.Item4;
        probabilityXCrossing = probs.Item5;

        level = new Maze_Field(levelSizeY, levelSizeY);
        SetEveryCellToDefault();
        
        StartPathGeneration();
    }

    /**
    * Sets every cell in the level to default. This means, that every cell gets its orientations, depending on their position
    */
    private void SetEveryCellToDefault ()
    {
        for(int y = 0; y<levelSizeY; y++)
        {
            for(int x = 0; x<levelSizeX; x++)
            {
                level.SetCellDefault(x,y);
            }
        }
    }
    

    private void StartRoomGeneration (){
        //TODO
    }

    private void StartPathGeneration ()
    {
        
        (int, int) pos = generationSettings.GetStartPosition();
        int corridorMinLength = generationSettings.GetMinCorridorLength();

        level.SetGameObjectIntoCell(pos.Item1, pos.Item2, 0, pathGameObjects.Start);

        SetNextCell(pos.Item1, pos.Item2, corridorMinLength, null);
    }



    private void SetNextCell (int x_Position, int y_Position, int corridorMinLength, Maze_Cell previousCell){
        // The last setted cell. So the cell that was buildet by the previousCell
        Maze_Cell lastCell = level.GetCellAt(x_Position, y_Position);

        if(                                                                         // Nach Norden bauen
            lastCell.GetPassageNorth() &&
            level.GetCellAt(x_Position, y_Position + 1).GetPassageSouth() &&
            level.GetCellAt(x_Position, y_Position + 1).GetIsEmpty() &&
            level.GetCellAt(x_Position, y_Position + 1) != previousCell
        ){
            

            if(corridorMinLength > 0){
                // Only corridors and end are allowed
                if(SetCorridorFor(x_Position, y_Position + 1, Orientation.North)){
                    SetNextCell(x_Position, y_Position + 1, corridorMinLength - 1, lastCell);
                }
                else {
                    // recursion stops here, because of a set end ore something went wrong (collision with neighbor)
                }
            }
            else {
                // Every pathGameObject is allowed
                if(SetRandomPathObjectToCell(x_Position, y_Position + 1)){
                    if(level.GetCellAt(x_Position, y_Position + 1).GetMazeCellGameObject().GetMazeCellGameObjectType() == MazeCellGameObjectType.Corridor){
                        // Extra corridor for the corridor length
                        SetNextCell(x_Position, y_Position + 1, 0, lastCell);
                    }
                    else
                    {
                        // Now we need corridor(s) again (depending on the settings)
                        SetNextCell(x_Position, y_Position + 1, generationSettings.GetMinCorridorLength(), lastCell);
                    }
                    
                }
            }
        }
        if(                                                                    // Nach Osten bauen
            lastCell.GetPassageEast() &&
            level.GetCellAt(x_Position + 1, y_Position).GetPassageWest() &&
            level.GetCellAt(x_Position + 1, y_Position).GetIsEmpty() &&
            level.GetCellAt(x_Position + 1, y_Position) != previousCell
        ){
            if(corridorMinLength > 0){
                // Only corridors and end are allowed
                if(SetCorridorFor(x_Position + 1, y_Position, Orientation.East)){
                    SetNextCell(x_Position + 1, y_Position, corridorMinLength - 1, lastCell);
                }
                else {
                    // recursion stops here, because of a set end
                }
            }
            else {
                // Every pathGameObject is allowed
                if(SetRandomPathObjectToCell(x_Position + 1, y_Position)){
                    if(level.GetCellAt(x_Position + 1, y_Position).GetMazeCellGameObject().GetMazeCellGameObjectType() == MazeCellGameObjectType.Corridor){
                        // Extra corridor for the corridor length
                        SetNextCell(x_Position + 1, y_Position, 0, lastCell);
                    }
                    else
                    {
                        // Now we need corridor(s) again (depending on the settings)
                        SetNextCell(x_Position + 1, y_Position, generationSettings.GetMinCorridorLength(), lastCell);
                    }
                    
                }
            }
        }
        if(                                                                     // Nach Süden bauen
            lastCell.GetPassageSouth() &&
            level.GetCellAt(x_Position, y_Position - 1).GetPassageNorth() &&
            level.GetCellAt(x_Position, y_Position - 1).GetIsEmpty() &&
            level.GetCellAt(x_Position, y_Position - 1) != previousCell
        ){
            

            if(corridorMinLength > 0){
                // Only corridors and end are allowed
                if(SetCorridorFor(x_Position, y_Position - 1, Orientation.South)){
                    SetNextCell(x_Position, y_Position - 1, corridorMinLength - 1, lastCell);
                }
                else {
                    // recursion stops here, because of a set end
                }
            }
            else {
                // Every pathGameObject is allowed
                if(SetRandomPathObjectToCell(x_Position, y_Position - 1)){
                    if(level.GetCellAt(x_Position, y_Position - 1).GetMazeCellGameObject().GetMazeCellGameObjectType() == MazeCellGameObjectType.Corridor){
                        // Extra corridor for the corridor length
                        SetNextCell(x_Position, y_Position - 1, 0, lastCell);
                    }
                    else
                    {
                        // Now we need corridor(s) again (depending on the settings)
                        SetNextCell(x_Position, y_Position - 1, generationSettings.GetMinCorridorLength(), lastCell);
                    }
                    
                }
            }
        }
        if(                                                                    // Nach West bauen
            lastCell.GetPassageWest() &&
            level.GetCellAt(x_Position - 1, y_Position).GetPassageEast() &&
            level.GetCellAt(x_Position - 1, y_Position).GetIsEmpty() &&
            level.GetCellAt(x_Position - 1, y_Position) != previousCell
        ){
            if(corridorMinLength > 0){
                // Only corridors and end are allowed
                if(SetCorridorFor(x_Position - 1, y_Position, Orientation.West)){
                    SetNextCell(x_Position - 1, y_Position, corridorMinLength - 1, lastCell);
                }
                else {
                    // recursion stops here, because of a set end
                }
            }
            else {
                // Every pathGameObject is allowed
                if(SetRandomPathObjectToCell(x_Position - 1, y_Position)){
                    if(level.GetCellAt(x_Position - 1, y_Position).GetMazeCellGameObject().GetMazeCellGameObjectType() == MazeCellGameObjectType.Corridor){
                        // Extra corridor for the corridor length
                        SetNextCell(x_Position - 1, y_Position, 0, lastCell);
                    }
                    else
                    {
                        // Now we need corridor(s) again (depending on the settings)
                        SetNextCell(x_Position - 1, y_Position, generationSettings.GetMinCorridorLength(), lastCell);
                    }
                    
                }
            }
        }
    }

    /**
    * Tries to set a corridor to the given cell. If not possible, the method tries by replacing the next cell in direction (extra method) to set the corridor. 
    * If this also not works, the method places an end.
    *  
    * Returns True if the Corridor could set and recursion is possible.
    * Returns False if the Corridor could not set. So an End or nothing was set and the recursion ends here.




    TODO: Überprüfe die Angrenzenten Zellen vom nächsten Feld, ob ein Corridor/Ende überhaupt möglich ist. Es kann durchaus sein, dass trotz der minimalen Länge 
    Eine Kurve oder Kreuzung gesetzt werden MUSS!!!!
    (s.h. Aufzeichnungen)
    */
    private bool SetCorridorFor (int x, int y, Orientation orientation){
        Maze_Cell mc = level.GetCellAt(x, y);
        switch (orientation){  
            case Orientation.North:              
                if(y < levelSizeY-1 && mc.GetPassageNorth() && mc.GetPassageSouth() && level.GetCellAt(x, y + 1).GetPassageSouth()){
                    return level.SetGameObjectIntoCell(x, y, 0, pathGameObjects.Corridor);
                }
                else if (y < levelSizeY-1){
                    if( RemoveAndReplace(x, y+1, orientation)){
                        return level.SetGameObjectIntoCell(x, y, 0, pathGameObjects.Corridor);
                    }
                    else {
                        level.SetGameObjectIntoCell(x, y, 180, pathGameObjects.End);
                        return false;
                    }
                }
                else {
                    level.SetGameObjectIntoCell(x, y, 180, pathGameObjects.End);
                        return false;
                }

            case Orientation.East:
                if(x < levelSizeX-1 && mc.GetPassageEast() && mc.GetPassageWest() && level.GetCellAt(x + 1, y).GetPassageWest()){
                    return level.SetGameObjectIntoCell(x, y, 90, pathGameObjects.Corridor);
                }
                else if (x < levelSizeX-1){
                    if( RemoveAndReplace(x + 1, y, orientation)){
                        return level.SetGameObjectIntoCell(x, y, 90, pathGameObjects.Corridor);
                    }
                    else {
                        level.SetGameObjectIntoCell(x, y, 270, pathGameObjects.End);
                        return false;
                    }
                }
                else {
                    level.SetGameObjectIntoCell(x, y, 270, pathGameObjects.End);
                    return false;
                }

            case Orientation.South:
                if(y > 0 && mc.GetPassageNorth() && mc.GetPassageSouth() && level.GetCellAt(x, y - 1).GetPassageNorth()){
                    return level.SetGameObjectIntoCell(x, y, 0, pathGameObjects.Corridor);
                }
                else if (y > 0){
                    if( RemoveAndReplace(x, y - 1, orientation)){
                        return level.SetGameObjectIntoCell(x, y, 0, pathGameObjects.Corridor);
                    }
                    else {
                        level.SetGameObjectIntoCell(x, y, 0, pathGameObjects.End);
                        return false;
                    }
                }
                else {
                    level.SetGameObjectIntoCell(x, y, 0, pathGameObjects.End);
                    return false;
                }

            case Orientation.West:
                if(x > 0 && mc.GetPassageEast() && mc.GetPassageWest() && level.GetCellAt(x - 1, y).GetPassageEast()){
                    level.SetGameObjectIntoCell(x, y, 90, pathGameObjects.Corridor);
                    return true;
                }
                else if (x > 0){
                    if( RemoveAndReplace(x - 1, y, orientation)){
                        return level.SetGameObjectIntoCell(x, y, 90, pathGameObjects.Corridor);
                    }
                    else {
                        return level.SetGameObjectIntoCell(x, y, 90, pathGameObjects.End);
                    }
                }
                else {
                    level.SetGameObjectIntoCell(x, y, 90, pathGameObjects.End);
                    return false;
                }
        }
        return false;
    }


    /**
    Tries to remove the MazeCellGameObject in the given cell position und replace it with 
    a fitting GameObject with more open passages. As Example: T_Crossing get replaces with X_Crossing

    */
    private bool RemoveAndReplace (int x, int y, Orientation orientation){
        Maze_Cell mc = level.GetCellAt(x,y);

        switch (mc.GetMazeCellGameObject().GetMazeCellGameObjectType()){
            case MazeCellGameObjectType.T_Crossing:
                mc.RemoveCellObject();
                level.SetGameObjectIntoCell(x, y, 0, pathGameObjects.X_Crossing);
                return true;


            case MazeCellGameObjectType.Start:
                // The start can't be replaced by anything else.
                return false;


            case MazeCellGameObjectType.Corridor:
                if(mc.GetPassageNorth()){   // Then it's also South because it's a corridor
                    mc.RemoveCellObject();
                    if(orientation == Orientation.East){
                        level.SetGameObjectIntoCell(x, y, 180, pathGameObjects.T_Crossing);
                        return true;
                    }
                    else if (orientation == Orientation.West){
                        level.SetGameObjectIntoCell(x, y, 0, pathGameObjects.T_Crossing);
                        return true;
                    }
                    return false;   // Then something doesn't work
                }
                else if (mc.GetPassageEast()){   // Then it's also West because it's a corridor
                    mc.RemoveCellObject();
                    if(orientation == Orientation.North){
                        level.SetGameObjectIntoCell(x, y, 90, pathGameObjects.T_Crossing);
                        return true;
                    }
                    else if (orientation == Orientation.South){
                        level.SetGameObjectIntoCell(x, y, -90, pathGameObjects.T_Crossing);
                        return true;
                    }
                    return false;   // Then something doesn't work
                }
                return false; // Then also something doesn't work
            

            case MazeCellGameObjectType.Corner:
                if(mc.GetPassageNorth() && mc.GetPassageEast()){   // Then it's a corner without rotation
                    mc.RemoveCellObject();
                    if(orientation == Orientation.North){
                        level.SetGameObjectIntoCell(x, y, 0, pathGameObjects.T_Crossing);
                        return true;
                    }
                    else if (orientation == Orientation.East){
                        level.SetGameObjectIntoCell(x, y, 270, pathGameObjects.T_Crossing);
                        return true;
                    }
                    return false;   // Then something doesn't work
                }
                else if(mc.GetPassageEast() && mc.GetPassageSouth()){   // Then it's a corner with a 90 degree (clockwise) rotation
                    mc.RemoveCellObject();
                    if(orientation == Orientation.South){
                        level.SetGameObjectIntoCell(x, y, 0, pathGameObjects.T_Crossing);
                        return true;
                    }
                    else if (orientation == Orientation.East){
                        level.SetGameObjectIntoCell(x, y, 90, pathGameObjects.T_Crossing);
                        return true;
                    }
                    return false;   // Then something doesn't work
                }
                else if(mc.GetPassageSouth() && mc.GetPassageWest()){   // Then it's a corner with a 180 degree (clockwise) rotation
                    mc.RemoveCellObject();
                    if(orientation == Orientation.South){
                        level.SetGameObjectIntoCell(x, y, 180, pathGameObjects.T_Crossing);
                        return true;
                    }
                    else if (orientation == Orientation.West){
                        level.SetGameObjectIntoCell(x, y, 90, pathGameObjects.T_Crossing);
                        return true;
                    }
                    return false;   // Then something doesn't work
                }
                else if(mc.GetPassageWest() && mc.GetPassageNorth()){   // Then it's a corner with a 90 degree (clockwise) rotation
                    mc.RemoveCellObject();
                    if(orientation == Orientation.North){
                        level.SetGameObjectIntoCell(x, y, 180, pathGameObjects.T_Crossing);
                        return true;
                    }
                    else if (orientation == Orientation.West){
                        level.SetGameObjectIntoCell(x, y, -90, pathGameObjects.T_Crossing);
                        return true;
                    }
                    return false;   // Then something doesn't work
                }
                return false; // Then something doesn't work
        }

        return false;
    }

    /**
    Sets a random (depending on the settings in LevelSettings) PathGameObject to the given cell 

    Returns false if an end was set.
    Returns true else
    */
    private bool SetRandomPathObjectToCell (int x, int y){

        Maze_Cell cellToFill = level.GetCellAt(x, y);
        
        bool northFree = cellToFill.GetPassageNorth() && level.GetCellAt(x,y+1).GetPassageSouth();
        bool eastFree = cellToFill.GetPassageEast() && level.GetCellAt(x+1,y).GetPassageWest();
        bool southFree = cellToFill.GetPassageSouth() && level.GetCellAt(x,y-1).GetPassageNorth();
        bool westFree = cellToFill.GetPassageWest() && level.GetCellAt(x-1,y).GetPassageEast();

        float maxRange = 0;
        //Debug.Log("Orientations (" + x + "," + y +"):"  + cellToFill.GetPassageNorth() + "," + cellToFill.GetPassageEast() + "," + cellToFill.GetPassageSouth() + "," + cellToFill.GetPassageWest() + ",");

        // MazeCellObjects that could fit in. But they need not to fit. 
        // For Example when you need a T_Crossin, a Corridor is also possible but will not fit in.
        List<(MazeCellGameObject, float)> mco = new List<(MazeCellGameObject, float)>();





        switch (northFree, eastFree, southFree, westFree)
        {
            case (false, false, false, false):
                // Should be impossible but better save than sorry
                Debug.LogError("So a glumb!");
                break;
            case (true, true, true, true):
                // X-Crossing, T-Crossing, Corridor, Corner and End are possible
                if(probabilityXCrossing > 0 ) mco.Add((pathGameObjects.X_Crossing, probabilityXCrossing));
                if(probabilityTCrossing > 0 ) mco.Add((pathGameObjects.T_Crossing, probabilityTCrossing));
                if(probabilityCorridor > 0 ) mco.Add((pathGameObjects.Corridor, probabilityCorridor));
                if(probabilityCorner > 0 ) mco.Add((pathGameObjects.Corner, probabilityCorner));
                if(probabilityEnd > 0 ) mco.Add((pathGameObjects.End, probabilityEnd));
                maxRange = probabilityXCrossing + probabilityTCrossing + probabilityCorridor + probabilityCorner + probabilityEnd;
                break;

            case (false, true, true, true):
            case (true, false, true, true):
            case (true, true, false, true):
            case (true, true, true, false):
                // T-Crossing, Corridor, Corner and End are possible
                if(probabilityTCrossing > 0 ) mco.Add((pathGameObjects.T_Crossing, probabilityTCrossing));
                if(probabilityCorridor > 0 ) mco.Add((pathGameObjects.Corridor, probabilityCorridor));
                if(probabilityCorner > 0 ) mco.Add((pathGameObjects.Corner, probabilityCorner));
                if(probabilityEnd > 0 ) mco.Add((pathGameObjects.End, probabilityEnd));
                maxRange = probabilityTCrossing + probabilityCorridor + probabilityCorner + probabilityEnd;
                break;

            case (false, false, true, true):
            case (false, true, true, false):
            case (true, false, false, true):
            case (true, true, false, false):
                // Corner and End are possible
                if(probabilityCorner > 0 ) mco.Add((pathGameObjects.Corner, probabilityCorner));
                if(probabilityEnd > 0 ) mco.Add((pathGameObjects.End, probabilityEnd));
                maxRange = probabilityCorner + probabilityEnd;
                break;

            case (false, true, false, true):
            case (true, false, true, false):
                // Corridor and End are possible
                if(probabilityCorridor > 0 ) mco.Add((pathGameObjects.Corridor, probabilityCorridor));
                if(probabilityEnd > 0 ) mco.Add((pathGameObjects.End, probabilityEnd));
                maxRange = probabilityCorridor + probabilityEnd;
                break;

            case (true, false, false, false):
            case (false, true, false, false):
            case (false, false, true, false):
            case (false, false, false, true):
                // Only End is possible
                if(probabilityEnd > 0 ) mco.Add((pathGameObjects.End, probabilityEnd));
                maxRange = probabilityEnd;
                break;
        }

        float randomValue;
        float sum;

        MazeCellGameObject objToUse;
        float objPos;

        while(mco.Count > 0){
            randomValue = Random.Range(0f, maxRange);
            sum = 0;
            objToUse = null;
            objPos = 0;

            for(int i = 0; i < mco.Count; i++){
                sum += mco[i].Item2;
                if( sum >= randomValue) {
                    objToUse = mco[i].Item1;
                    objPos = mco[i].Item2;
                    break;
                }
            }
        
            if(!level.SetGameObjectIntoCell(x, y, 0, objToUse)){
                if(!level.SetGameObjectIntoCell(x, y, 90, objToUse)){
                    if(!level.SetGameObjectIntoCell(x, y, 180, objToUse)){
                        if(!level.SetGameObjectIntoCell(x, y, 270, objToUse)){
                            Debug.Log((x, y, objToUse ) + " nothing was possible ");
                            mco.Remove((objToUse, objPos));
                            maxRange -= objPos;
                        }
                        else return true;
                    }
                    else return true;
                }
                else return true;
            }
            else return true;

           
        }
        return false;
    }
}

public enum Orientation{
    North,
    East,
    South,
    West
}