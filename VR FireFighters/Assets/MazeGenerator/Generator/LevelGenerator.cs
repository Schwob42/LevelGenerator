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

    [SerializeField]
    GameObject levelGameobject; // wo die Objekte eingefügt werden

    // List of coordinates of cells that can't get filled.
    private List<(int x, int y)> redos = new List<(int x,  int y)>();

    //private List</*rooms*/> rooms = new List<T>();
    
    
    private Maze_Field level;
    private List<RoomObject> rooms;

    private int levelSizeX, levelSizeY;

    private float probabilityCorridor;
    private float probabilityCorner;
    private float probabilityEnd;
    private float probabilityTCrossing;
    private float probabilityXCrossing;

    private float roomPossibility;

    /**
     * TODO: Remove Start() and add another way to start the generator. UI with a button and so on
     * 
     */
    void Start ()
    {
        levelGameobject = GameObject.Find("LevelGameObject");
    }
   

    public void GenerateField ()
    {
        
        Random.InitState(generationSettings.GetSeed());

        //(int, int) size = generationSettings.GetMazeSize();
        levelSizeX = generationSettings.GetMazeSizeX();
        levelSizeY = generationSettings.GetMazeSizeY();

        // Kamera ausrichten
        this.gameObject.GetComponent<SetLevelCamera>().SetCameraPosition(levelSizeX, levelSizeY);

        (float, float, float, float, float) probs = generationSettings.GetProbabilities();
        probabilityCorridor = probs.Item1;
        probabilityCorner = probs.Item2;
        probabilityEnd = probs.Item3;
        probabilityTCrossing = probs.Item4;
        probabilityXCrossing = probs.Item5;

        roomPossibility = generationSettings.GetRoomPossibility();

        rooms = new List<RoomObject>();
        generationSettings.SetRooms(rooms);


        if(levelGameobject.transform.childCount > 0)
        {
            foreach(Transform t in levelGameobject.transform.GetComponentInChildren<Transform>())
            {
                UnityEngine.MonoBehaviour.Destroy(t.gameObject, 0f);
            }
        }

        level = new Maze_Field(levelSizeX, levelSizeY, levelGameobject);
        SetEveryCellToDefault();

        if (!generationSettings.GetGenerateRoom())
        {
            roomPossibility = 0;
            // Sonst würden Pfadelemente mit Türen gebaut werden
        }

        if (generationSettings.GetGeneratePath())
        {
            StartPathGeneration();
        }
        else
        {
            if (generationSettings.GetGenerateRoom())
            {
                StartRoomGeneration(generationSettings.GetStartPosition().x, generationSettings.GetStartPosition().y, Orientation.North);
            }            
        }

        FixRedos();
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

    private bool StartRoomGeneration (int x, int y, Orientation direction)
    {
        List<Maze_Cell> room = null;

        // TODO: Nur eine Tür plazieren, wenn dahinter schon ein Raum existiert (Raum mit mehreren Türen)
        if(direction == Orientation.North)
        {
            room = GenerateRoom(x, y + 1, direction);
        }
        else if (direction == Orientation.East)
        {
            room = GenerateRoom(x + 1, y, direction);
        }
        else if (direction == Orientation.South)
        {
            room = GenerateRoom(x, y - 1, direction);
        }
        else if (direction == Orientation.West)
        {
           room = GenerateRoom(x - 1, y, direction);
        }

        if (room != null && room.Count > 0)
        {
            InsertRoomPrefabs(room);
            RoomObject roomObj = new RoomObject(rooms.Count, room);
            rooms.Add(roomObj);
            return true;
        }
        else
        {
            RemoveRoom(x, y, room);
            return false;
        }
    }

    private List<Maze_Cell> GenerateRoom(int startX, int startY, Orientation direction)
    {
        List<Maze_Cell> room = new List<Maze_Cell>();
        GenerateRoomRekursiv(startX, startY, startX, startY, room, direction);

        return room;
    }

    private void GenerateRoomRekursiv(int startX, int startY, int currentPositionX, int currentPositionY, List<Maze_Cell> room, Orientation direction)
    {
        if(direction == Orientation.North) { 
            // Out of local space
            if (currentPositionX < startX - generationSettings.GetRoomSizeWidth() / 2 ) return;
            if (currentPositionX > startX + generationSettings.GetRoomSizeWidth() / 2) return;

            if (currentPositionY - startY > generationSettings.GetRoomSizeHeight()) return; 
            if (currentPositionY - startY < 0) return;
        }
        else if (direction == Orientation.East)
        {
            // Out of local space
            if (currentPositionX - startX > generationSettings.GetRoomSizeWidth()) return;
            if (currentPositionX - startX < 0) return;

            if (currentPositionY < startY - generationSettings.GetRoomSizeHeight() / 2) return;
            if (currentPositionY > startY + generationSettings.GetRoomSizeHeight() / 2) return;
        }
        else if (direction == Orientation.South)
        {
            // Out of local space
            if (currentPositionX < startX - generationSettings.GetRoomSizeWidth() / 2) return;
            if (currentPositionX > startX + generationSettings.GetRoomSizeWidth() / 2) return;

            if (startY - currentPositionY > generationSettings.GetRoomSizeHeight()) return;
            if (startY - currentPositionY < 0) return;
        }
        else if (direction == Orientation.West)
        {
            // Out of local space
            if (startX - currentPositionX > generationSettings.GetRoomSizeWidth()) return;
            if (startX - currentPositionX < 0) return;

            if (currentPositionY < startY - generationSettings.GetRoomSizeHeight() / 2) return;
            if (currentPositionY > startY + generationSettings.GetRoomSizeHeight() / 2) return;
        }

        // Out of global space
        if (currentPositionX >= generationSettings.GetMazeSizeX()) return;
        if (currentPositionY >= generationSettings.GetMazeSizeY()) return;
        if (currentPositionX < 0) return;
        if (currentPositionY < 0) return;

        Maze_Cell cell = level.GetCellAt(currentPositionX, currentPositionY);

        // Cell cant't be used for the room
        if (cell.GetMazeCellState() != MazeCellState.Empty) return;

        cell.ReservateForRoom();
        room.Add(cell);

        GenerateRoomRekursiv(startX, startY, currentPositionX + 1, currentPositionY, room, direction);
        GenerateRoomRekursiv(startX, startY, currentPositionX - 1, currentPositionY, room, direction);
        GenerateRoomRekursiv(startX, startY, currentPositionX, currentPositionY + 1, room, direction);
        GenerateRoomRekursiv(startX, startY, currentPositionX, currentPositionY - 1, room, direction);
    }

    /*

    /// <summary>
    /// Startmethode für die Raumgenerrierung. Ruft im weiteren Verlauf die rekursive Methode zur Raumgenerierung (<see cref="GenerateRoomRecursive"/>) auf
    /// </summary>
    /// <param name="startPosition_x">Position des Raumstarts in x Koordinate</param>
    /// <param name="startPosition_y">Position des Raumstarts in y Koordinate</param>
    /// <param name="possibleRoom">Array des möglichen Raums</param>
    /// <param name="direction">Richtung, von der Tür des Ganges aus, in die gebaut werden soll</param>
    private void GenerateRoom(int startPosition_x, int startPosition_y, (int row, int column)[,] possibleRoom, Orientation direction)   // Orientierung bedeutet, in Welche Richtung von der Tür aus gebaut wird
    {
        int mappedStart_x = -1;
        int mappedStart_y = -1;

        //only needed in case the start could not be set
        int original_startPosition_x = startPosition_x;
        int original_startPosition_y = startPosition_y;

        int rotationStart = 0;

        
        // (0,0) (0,1) .... (0,n)       n = GetLength(1)
        // (1,0) (1,1) .... (1,n)       m = GetLength(0)
        // ..... ..... .... (.,n)
        // (m,0) (m,1) .... (m,n)
        
        switch (direction)
        {
            case Orientation.North:
                mappedStart_x = (int)Mathf.Round(possibleRoom.GetLength(1) / 2);    // halbe Breite
                mappedStart_y = possibleRoom.GetLength(0) - 1;                      // untere Kante
                startPosition_y += 1;                                               // wäre sonst auf dem Gang
                rotationStart = -90;
                break;
            case Orientation.East:
                mappedStart_x = 0;                                                  // ganze Breite
                mappedStart_y = (int)Mathf.Round(possibleRoom.GetLength(0) / 2);    // halbe Höhe
                startPosition_x += 1;                                               // wäre sonst auf dem Gang
                rotationStart = 0;
                break;
            case Orientation.West:
                mappedStart_x = possibleRoom.GetLength(1) - 1;                      // linke Kante
                mappedStart_y = (int)Mathf.Round(possibleRoom.GetLength(0) / 2);    // Halbe Höhe
                startPosition_x -= 1;                                               // wäre sonst auf dem Gang
                rotationStart = 180;
                break;
            case Orientation.South:
                mappedStart_x = (int)Mathf.Round(possibleRoom.GetLength(1) / 2);    // Halbe Breite
                mappedStart_y = 0;                                                  // Untere Kante
                startPosition_y -= 1;                                               // wäre sonst auf dem Gang
                rotationStart = 90;
                break;
        }

        Debug.Log(mappedStart_x + " " + mappedStart_y);
        possibleRoom[mappedStart_y, mappedStart_x] = (startPosition_x, startPosition_y);

        if (!level.SetRoomObjectIntoCell(startPosition_x, startPosition_y, rotationStart, pathGameObjects.RoomWithDoor))
        {
            level.ReplaceCellObject(original_startPosition_x, original_startPosition_y, pathGameObjects);
            possibleRoom = null;
            return;
        }
        
        GenerateRoomRecursive(possibleRoom, (mappedStart_x, mappedStart_y+1), (mappedStart_x, mappedStart_y), (startPosition_x, startPosition_y));
        GenerateRoomRecursive(possibleRoom, (mappedStart_x+1, mappedStart_y), (mappedStart_x, mappedStart_y), (startPosition_x, startPosition_y));
        GenerateRoomRecursive(possibleRoom, (mappedStart_x, mappedStart_y-1), (mappedStart_x, mappedStart_y), (startPosition_x, startPosition_y));
        GenerateRoomRecursive(possibleRoom, (mappedStart_x-1, mappedStart_y), (mappedStart_x, mappedStart_y), (startPosition_x, startPosition_y));    
    }

    */

    /// <summary>
    /// Rekursive Methode zur Raumgenerierung. 
    /// Abbruchbedingungen sind:
    /// <list type="bullet">
    ///     <item>Koordinate x oder y liegt außerhalb der lokalen Map (Der Rahmen des zu generierenden Raums</item>
    ///     <item>Koordinate x oder y liegt außerhalb der globalen Map (Das Spielfeld an sich)</item>
    ///     <item>Die Zelle der angegebenen Koordinaten ist belegt </item>
    /// </list>
    /// </summary>
    /// <param name="possibleRoom"></param>
    /// <param name="currentLocalCell"></param>
    /// <param name="startPointLocal"></param>
    /// <param name="startPointGlobal"></param>
    /// <returns>
    /// True: Wenn zur aktuellen (übergebenen) Zelle *keine* Wand braucht <br/>
    /// False: Wenn zur aktuellen (übergebenen) Zelle eine Wand braucht
    /// </returns>
    private void GenerateRoomRecursive((int row, int column)[,] possibleRoom, (int currentPosition_x, int currentPosition_y) currentLocalCell, (int startPoint_x, int startPoint_y) startPointLocal, (int startPoint_x, int startPoint_y) startPointGlobal)
    {
        
        // Nicht innerhalb der lokalen Map
        if (currentLocalCell.currentPosition_x < 0 || currentLocalCell.currentPosition_x >= possibleRoom.GetLength(1)) return;
        if (currentLocalCell.currentPosition_y < 0 || currentLocalCell.currentPosition_y >= possibleRoom.GetLength(0)) return;

        //  X und Y transformiert auf das globale Koordinatensystem
        int globalCoordinate_x = currentLocalCell.currentPosition_x - startPointLocal.startPoint_x + startPointGlobal.startPoint_x;
        int globalCoordinate_y = currentLocalCell.currentPosition_y - startPointLocal.startPoint_y + startPointGlobal.startPoint_y;

        // Nicht innherhalb der globalen Map also dem Level
        if (globalCoordinate_x < 0 || globalCoordinate_x >= level.GetMazeSizeX()) return;
        if (globalCoordinate_y < 0 || globalCoordinate_y >= level.GetMazeSizeY()) return;

        // Zelle ist belegt
        Maze_Cell currentCell = level.GetCellAt(globalCoordinate_x, globalCoordinate_y);
        //Debug.Log(currentCell.GetMazeCellState());
        if (currentCell.GetMazeCellState() != MazeCellState.Empty) return;
        
        

        possibleRoom[currentLocalCell.currentPosition_y, currentLocalCell.currentPosition_x] = (globalCoordinate_x, globalCoordinate_y);

        level.ReservateCellForRoom(globalCoordinate_x, globalCoordinate_y);
        //Debug.Log("Reservate " + currentLocalCell);

        GenerateRoomRecursive(possibleRoom, (currentLocalCell.currentPosition_x, currentLocalCell.currentPosition_y + 1), startPointLocal, startPointGlobal);
        GenerateRoomRecursive(possibleRoom, (currentLocalCell.currentPosition_x + 1, currentLocalCell.currentPosition_y), startPointLocal, startPointGlobal);
        GenerateRoomRecursive(possibleRoom, (currentLocalCell.currentPosition_x, currentLocalCell.currentPosition_y - 1), startPointLocal, startPointGlobal);
        GenerateRoomRecursive(possibleRoom, (currentLocalCell.currentPosition_x - 1, currentLocalCell.currentPosition_y), startPointLocal, startPointGlobal);
         
    }

    private void InsertRoomPrefabs(List<Maze_Cell> room)
    {
        foreach (Maze_Cell cell in room)
        {
            // Is there a room in each direction?
            bool roomNorth = level.GetCellAt(cell.x, cell.y + 1) != null ? 
                level.GetCellAt(cell.x, cell.y + 1).GetMazeCellState() == MazeCellState.Room : false;

            bool roomEast = level.GetCellAt(cell.x + 1, cell.y) != null ?  
                level.GetCellAt(cell.x + 1, cell.y).GetMazeCellState() == MazeCellState.Room : false;

            bool roomSouth = level.GetCellAt(cell.x, cell.y - 1) != null ? 
                level.GetCellAt(cell.x, cell.y - 1).GetMazeCellState() == MazeCellState.Room : false;

            bool roomWest = level.GetCellAt(cell.x - 1, cell.y) != null ? 
                level.GetCellAt(cell.x - 1, cell.y).GetMazeCellState() == MazeCellState.Room : false;

            if(!roomNorth && !roomEast && !roomSouth)
            {
                level.SetRoomObjectIntoCell(cell, 90, pathGameObjects.RoomEnd);
            }
            else if (!roomEast && !roomSouth && !roomWest)
            {
                level.SetRoomObjectIntoCell(cell, 180, pathGameObjects.RoomEnd);
            }
            else if (!roomSouth && !roomWest && !roomNorth)
            {
                level.SetRoomObjectIntoCell(cell, -90, pathGameObjects.RoomEnd);
            }
            else if (!roomWest && !roomNorth && !roomEast)
            {
                level.SetRoomObjectIntoCell(cell, 0, pathGameObjects.RoomEnd);
            }
            else if (!roomNorth && !roomEast)    // Oben rechts
            {
                level.SetRoomObjectIntoCell(cell, 90, pathGameObjects.RoomWithCorner);
            }
            else if (!roomEast && !roomSouth)// unten rechts
            {
                level.SetRoomObjectIntoCell(cell, 180, pathGameObjects.RoomWithCorner);
            }
            else if (!roomSouth && !roomWest)// unten links
            {
                level.SetRoomObjectIntoCell(cell, -90, pathGameObjects.RoomWithCorner);
            }
            else if (!roomWest && !roomNorth)// oben links
            {
                level.SetRoomObjectIntoCell(cell, 0, pathGameObjects.RoomWithCorner);
            }
            else if (!roomNorth && !roomSouth)  // Wand oben und unten
            {
                level.SetRoomObjectIntoCell(cell, 90, pathGameObjects.RoomWallBothSides);
            }
            else if (!roomEast && !roomWest)
            {
                level.SetRoomObjectIntoCell(cell, 0, pathGameObjects.RoomWallBothSides);
            }
            else if (!roomNorth)    //oben
            {
                level.SetRoomObjectIntoCell(cell, 90, pathGameObjects.RoomWithWall);
            }
            else if (!roomEast) // rechts
            {
                level.SetRoomObjectIntoCell(cell, 180, pathGameObjects.RoomWithWall);
            }
            else if (!roomSouth)    // unten
            {
                level.SetRoomObjectIntoCell(cell, -90, pathGameObjects.RoomWithWall);
            }
            else if (!roomWest) // links
            {
                level.SetRoomObjectIntoCell(cell, 0, pathGameObjects.RoomWithWall);
            }
            else {
                level.SetRoomObjectIntoCell(cell, 0, pathGameObjects.RoomEmpty);
            }
        }
    }

    private void RemoveRoom(int startX, int startY, List<Maze_Cell> room)
    {
        if(room != null) { 
            foreach (Maze_Cell cell in room)
            {
                cell.RemoveReservationForRoom();
            }
        }

        level.ReplaceCellObject(startX, startY, pathGameObjects);
    }

    
    private void StartRoomGeneration (int x, int y){
    //private bool StartRoomGeneration (int x, int y){
        Maze_Cell cell = level.GetCellAt(x, y);
        MazeCellGameObject mco = cell.GetMazeCellGameObject();

        if (mco.GetDoorNorth())
        {
            StartRoomGeneration(x, y, Orientation.North);
        }
        if (mco.GetDoorEast())
        {
            StartRoomGeneration(x, y, Orientation.East);
        }
        if (mco.GetDoorSouth())
        {
            StartRoomGeneration (x, y, Orientation.South);
        }
        if (mco.GetDoorWest())
        {
            StartRoomGeneration (x, y, Orientation.West);
        }
    }

    /**
    X und y sind die Startposition des Raums, also die Stelle, wo man durch die Türe kommt (und schon im Raum steht).
    orientation ist die Richtung, in die der Raum erweitert werden soll
    stepsOn(X/Y)Axis sind die Anzahl an Schritten, die in die jeweilige Richtung bereits gemacht wurden. Diese darf nie größer als die maxWidth/Height) aus den Settings ein
    */
    /*
    private void TryToBuildRoom(int x, int y, List<(int x, int y)> possibleRoom, Orientation orientation, bool isStart=false) {
        Debug.Log(possibleRoom.Count);
        
        // Endbedingung für rekursives Bauen des Raums
        if(possibleRoom.Count == generationSettings.GetRoomSettings().maxRoomSize){
            return;
        }
        
        if(isStart){
            // Build Room with Door 

            switch (orientation){
                case Orientation.North:
                    if(!level.SetRoomObjectIntoCell(x, y, -90, pathGameObjects.RoomWithDoor)){
                        return;
                    }
                    possibleRoom.Add((x, y));
                    break;
                
                case Orientation.East:
                    if(!level.SetRoomObjectIntoCell(x, y, 0, pathGameObjects.RoomWithDoor)){
                        return;
                    }
                    possibleRoom.Add((x, y));
                    break;
                
                case Orientation.South:
                    if(!level.SetRoomObjectIntoCell(x, y, 90, pathGameObjects.RoomWithDoor)){
                        return;
                    }
                    possibleRoom.Add((x, y));
                    break;

                case Orientation.West:
                    if(!level.SetRoomObjectIntoCell(x, y, 180, pathGameObjects.RoomWithDoor)){
                        return;
                    }
                    possibleRoom.Add((x, y));
                    break;
            }
        }
        else {
            switch (orientation){
                case Orientation.North:
                    level.SetRoomObjectIntoCell(x, y, -90, pathGameObjects.RoomEmpty);
                    possibleRoom.Add((x, y));
                    break;
                
                case Orientation.East:
                    level.SetRoomObjectIntoCell(x, y, 0, pathGameObjects.RoomEmpty);
                    possibleRoom.Add((x, y));
                    break;
                
                case Orientation.South:
                    level.SetRoomObjectIntoCell(x, y, 90, pathGameObjects.RoomEmpty);
                    possibleRoom.Add((x, y));
                    break;

                case Orientation.West:
                    level.SetRoomObjectIntoCell(x, y, 180, pathGameObjects.RoomEmpty);
                    possibleRoom.Add((x, y));
                    break;
            } 
        }


        // Rekursive Aufrufe

        //Bauen nach Norden 
        if(level.CellExists(x, y+1) && level.GetCellAt(x, y+1).GetMazeCellState() == MazeCellState.Empty){
            TryToBuildRoom(x, y+1, possibleRoom, Orientation.North);
        }
        // Bauen nach Osten
        if(level.CellExists(x+1, y) && level.GetCellAt(x+1, y).GetMazeCellState() == MazeCellState.Empty){
            TryToBuildRoom(x+1, y, possibleRoom, Orientation.East);
        }
        // Bauen nach Süden
        if(level.CellExists(x, y-1) && level.GetCellAt(x, y-1).GetMazeCellState() == MazeCellState.Empty){
            TryToBuildRoom(x, y-1, possibleRoom, Orientation.South);
        }
        // Bauen nach Westen
        if(level.CellExists(x-1, y) && level.GetCellAt(x-1, y).GetMazeCellState() == MazeCellState.Empty){
            TryToBuildRoom(x-1, y, possibleRoom, Orientation.West);
        }
    }
    */
    private void StartPathGeneration ()
    {
        
        (int, int) pos = generationSettings.GetStartPosition();
        int corridorMinLength = generationSettings.GetMinCorridorLength();

        level.SetGameObjectIntoCell(pos.Item1, pos.Item2, 0, pathGameObjects.Start);

        SetNextCell(pos.Item1, pos.Item2, corridorMinLength, null, true);
    }

    private void FixRedos(){
        if(redos.Count == 0) return;
        foreach((int x, int y) coordinates in redos){
            SetRandomPathObjectToCell(coordinates.x, coordinates.y, false);
        }
    }


    private void SetNextCell (int x_Position, int y_Position, int corridorMinLength, Maze_Cell previousCell, bool recursive){
        // The last setted cell. So the cell that was buildet by the previousCell
        Maze_Cell lastCell = level.GetCellAt(x_Position, y_Position);

        if(                                                                         // Nach Norden bauen
            lastCell.GetPassageNorth() &&
            level.GetCellAt(x_Position, y_Position + 1).GetPassageSouth() &&
            level.GetCellAt(x_Position, y_Position + 1).GetMazeCellState() == MazeCellState.Empty &&
            level.GetCellAt(x_Position, y_Position + 1) != previousCell &&
            level.GetCellAt(x_Position, y_Position + 1).GetMazeCellState() != MazeCellState.Room 
        ){
            if(corridorMinLength > 0){
                // Only corridors and end are allowed
                if(SetCorridorFor(x_Position, y_Position + 1, Orientation.North)){
                    SetNextCell(x_Position, y_Position + 1, corridorMinLength - 1, lastCell, true);
                }
                else {
                    // recursion stops here, because of a set end ore something went wrong (collision with neighbor or room)
                }
            }
            else {
                // Every pathGameObject is allowed
                if(SetRandomPathObjectToCell(x_Position, y_Position + 1)){
                    if(level.GetCellAt(x_Position, y_Position + 1).GetMazeCellGameObject().GetMazeCellGameObjectType() == MazeCellGameObjectType.Corridor){
                        // Extra corridor for the corridor length
                        SetNextCell(x_Position, y_Position + 1, 0, lastCell, true);
                    }
                    else
                    {
                        // Now we need corridor(s) again (depending on the settings)
                        SetNextCell(x_Position, y_Position + 1, generationSettings.GetMinCorridorLength(), lastCell, true);
                    }
                }
            }
        }
        if(                                                                    // Nach Osten bauen
            lastCell.GetPassageEast() &&
            level.GetCellAt(x_Position + 1, y_Position).GetPassageWest() &&
            level.GetCellAt(x_Position + 1, y_Position).GetMazeCellState() == MazeCellState.Empty &&
            level.GetCellAt(x_Position + 1, y_Position) != previousCell &&
            level.GetCellAt(x_Position + 1, y_Position).GetMazeCellState() != MazeCellState.Room
        ){
            if(corridorMinLength > 0){
                // Only corridors and end are allowed
                if(SetCorridorFor(x_Position + 1, y_Position, Orientation.East)){
                    SetNextCell(x_Position + 1, y_Position, corridorMinLength - 1, lastCell, true);
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
                        SetNextCell(x_Position + 1, y_Position, 0, lastCell, true);
                    }
                    else
                    {
                        // Now we need corridor(s) again (depending on the settings)
                        SetNextCell(x_Position + 1, y_Position, generationSettings.GetMinCorridorLength(), lastCell, true);
                    }
                    
                }
            }
        }
        if(                                                                     // Nach Süden bauen
            lastCell.GetPassageSouth() &&
            level.GetCellAt(x_Position, y_Position - 1).GetPassageNorth() &&
            level.GetCellAt(x_Position, y_Position - 1).GetMazeCellState() == MazeCellState.Empty &&
            level.GetCellAt(x_Position, y_Position - 1) != previousCell &&
            level.GetCellAt(x_Position, y_Position - 1).GetMazeCellState() != MazeCellState.Room
        ){
            

            if(corridorMinLength > 0){
                // Only corridors and end are allowed
                if(SetCorridorFor(x_Position, y_Position - 1, Orientation.South)){
                    SetNextCell(x_Position, y_Position - 1, corridorMinLength - 1, lastCell, true);
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
                        SetNextCell(x_Position, y_Position - 1, 0, lastCell, true);
                    }
                    else
                    {
                        // Now we need corridor(s) again (depending on the settings)
                        SetNextCell(x_Position, y_Position - 1, generationSettings.GetMinCorridorLength(), lastCell, true);
                    }
                    
                }
            }
        }
        if(                                                                    // Nach West bauen
            lastCell.GetPassageWest() &&
            level.GetCellAt(x_Position - 1, y_Position).GetPassageEast() &&
            level.GetCellAt(x_Position - 1, y_Position).GetMazeCellState() == MazeCellState.Empty &&
            level.GetCellAt(x_Position - 1, y_Position) != previousCell &&
            level.GetCellAt(x_Position - 1, y_Position).GetMazeCellState() != MazeCellState.Room
        ){
            if(corridorMinLength > 0){
                // Only corridors and end are allowed
                if(SetCorridorFor(x_Position - 1, y_Position, Orientation.West)){
                    SetNextCell(x_Position - 1, y_Position, corridorMinLength - 1, lastCell, true);
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
                        SetNextCell(x_Position - 1, y_Position, 0, lastCell, true);
                    }
                    else
                    {
                        // Now we need corridor(s) again (depending on the settings)
                        SetNextCell(x_Position - 1, y_Position, generationSettings.GetMinCorridorLength(), lastCell, true);
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
    */
    private bool SetCorridorFor (int x, int y, Orientation orientation){
        Maze_Cell mc = level.GetCellAt(x, y);
        float random = Random.value;
        bool buildDoor = random <= roomPossibility;

        switch (orientation){  
            case Orientation.North:              
                if(y < levelSizeY-1 && mc.GetPassageNorth() && mc.GetPassageSouth() && level.GetCellAt(x, y + 1).GetPassageSouth()){
                    if (buildDoor && !level.SetGameObjectIntoCell(x, y, 0, pathGameObjects.CorridorWithDoor)){
                        redos.Add((x, y));
                        return false;
                    }
                    else if(!buildDoor && !level.SetGameObjectIntoCell(x, y, 0, pathGameObjects.Corridor))
                    {
                        redos.Add((x, y));
                        return false;
                    }
                    if(level.GetCellAt(x,y).GetMazeCellGameObject().objectHasDoor()){
                        StartRoomGeneration(x,y);
                    }
                    return true;
                }
                else if (y < levelSizeY-1){
                    if( RemoveAndReplace(x, y+1, orientation)){
                        return level.SetGameObjectIntoCell(x, y, 0, pathGameObjects.Corridor);
                    }
                    else {
                        if(!level.SetGameObjectIntoCell(x, y, 180, pathGameObjects.End)){
                            redos.Add((x, y));
                        }
                        return false;
                    }
                }
                else {
                    if(!level.SetGameObjectIntoCell(x, y, 180, pathGameObjects.End)){
                        redos.Add((x, y));
                    }
                    return false;
                }

            case Orientation.East:
                if(x < levelSizeX-1 && mc.GetPassageEast() && mc.GetPassageWest() && level.GetCellAt(x + 1, y).GetPassageWest()){
                    if (buildDoor && !level.SetGameObjectIntoCell(x, y, 90, pathGameObjects.CorridorWithDoor))
                    {
                        redos.Add((x, y));
                        return false;
                    }
                    else if (!buildDoor && !level.SetGameObjectIntoCell(x, y, 90, pathGameObjects.Corridor))
                    {
                        redos.Add((x, y));
                        return false;
                    }
                    if (level.GetCellAt(x, y).GetMazeCellGameObject().objectHasDoor())
                    {
                        StartRoomGeneration(x, y);
                    }
                    return true;
                }
                else if (x < levelSizeX-1){
                    if( RemoveAndReplace(x + 1, y, orientation)){
                        return level.SetGameObjectIntoCell(x, y, 90, pathGameObjects.Corridor);
                    }
                    else {
                        if(!level.SetGameObjectIntoCell(x, y, 270, pathGameObjects.End)){
                            redos.Add((x, y));
                        }
                        return false;
                    }
                }
                else {
                    if(!level.SetGameObjectIntoCell(x, y, 270, pathGameObjects.End)){
                        redos.Add((x, y));
                    }
                    return false;
                }

            case Orientation.South:
                if(y > 0 && mc.GetPassageNorth() && mc.GetPassageSouth() && level.GetCellAt(x, y - 1).GetPassageNorth()){
                    if (buildDoor && !level.SetGameObjectIntoCell(x, y, 180, pathGameObjects.CorridorWithDoor))
                    {
                        redos.Add((x, y));
                        return false;
                    }
                    else if (!buildDoor && !level.SetGameObjectIntoCell(x, y, 180, pathGameObjects.Corridor))
                    {
                        redos.Add((x, y));
                        return false;
                    }
                    if (level.GetCellAt(x, y).GetMazeCellGameObject().objectHasDoor())
                    {
                        StartRoomGeneration(x, y);
                    }
                    return true;
                }
                else if (y > 0){
                    if( RemoveAndReplace(x, y - 1, orientation)){
                        return level.SetGameObjectIntoCell(x, y, 0, pathGameObjects.Corridor);
                    }
                    else {
                        if(!level.SetGameObjectIntoCell(x, y, 0, pathGameObjects.End)){
                            redos.Add((x, y));
                        }
                        return false;
                    }
                }
                else {
                    if(!level.SetGameObjectIntoCell(x, y, 0, pathGameObjects.End)){
                        redos.Add((x, y));
                    }
                    return false;
                }

            case Orientation.West:                
                if (x > 0 && mc.GetPassageEast() && mc.GetPassageWest() && level.GetCellAt(x - 1, y).GetPassageEast()){
                    if (buildDoor && !level.SetGameObjectIntoCell(x, y, -90, pathGameObjects.CorridorWithDoor))
                    {
                        redos.Add((x, y));
                        return false;
                    }
                    else if (!buildDoor && !level.SetGameObjectIntoCell(x, y, -90, pathGameObjects.Corridor))
                    {
                        redos.Add((x, y));
                        return false;
                    }
                    if (level.GetCellAt(x, y).GetMazeCellGameObject().objectHasDoor())
                    {
                        StartRoomGeneration(x, y);
                    }
                    return true;

                }
                else if (x > 0){
                    if( RemoveAndReplace(x - 1, y, orientation)){
                        return level.SetGameObjectIntoCell(x, y, 90, pathGameObjects.Corridor);
                    }
                    else {
                        if(!level.SetGameObjectIntoCell(x, y, 90, pathGameObjects.End)){
                            redos.Add((x, y));
                        }
                        return false;
                    }
                }
                else {
                    if(!level.SetGameObjectIntoCell(x, y, 90, pathGameObjects.End)){
                        redos.Add((x, y));
                    }
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

        if(mc.GetMazeCellGameObject().objectHasDoor()) return false;

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

    @params insertToRedosIfNeeded is needed to prevent to insert the same coordinate twice (by FixRedo()). 
    **Only FixRedo() calls this method with insertToRedosIfNeeded=false!!!**

    Returns false if an end was set.
    Returns true else
    */
    private bool SetRandomPathObjectToCell (int x, int y, bool insertToRedosIfNeeded = true){

        Maze_Cell cellToFill = level.GetCellAt(x, y);

        float maxRange = 0;
        //Debug.Log("Orientations (" + x + "," + y +"):"  + cellToFill.GetPassageNorth() + "," + cellToFill.GetPassageEast() + "," + cellToFill.GetPassageSouth() + "," + cellToFill.GetPassageWest() + ",");

        // MazeCellObjects that could fit in. But they need not to fit. 
        // For Example when you need a T_Crossin, a Corridor is also possible but will not fit in.
        List<(MazeCellGameObject, float)> mco = new List<(MazeCellGameObject, float)>();   


        bool northFree = cellToFill.GetPassageNorth() && (level.GetCellAt(x,y+1) != null) ? level.GetCellAt(x,y+1).GetPassageSouth() : false;
        bool eastFree = cellToFill.GetPassageEast() && (level.GetCellAt(x+1,y) != null) ? level.GetCellAt(x+1,y).GetPassageWest() : false;
        bool southFree = cellToFill.GetPassageSouth() && (level.GetCellAt(x,y-1) != null) ? level.GetCellAt(x,y-1).GetPassageNorth() : false;
        bool westFree = cellToFill.GetPassageWest() && (level.GetCellAt(x-1,y) != null) ? level.GetCellAt(x-1,y).GetPassageEast() : false;

        if(!insertToRedosIfNeeded){
            // We only get here when we already fixing the redos. So we need to find a perfect fitting solution without open ends (there is no longer a recursion)
            // and without colliding with other passages. As we NEED paths to connect there HAS to be passages ONLY in these directions. Else we could
            // get open ends because of the random choice.

            northFree = northFree   && (level.GetCellAt(x,y+1) != null) ? level.GetCellAt(x,y+1).GetMazeCellState() == MazeCellState.Path : false;  
            eastFree = eastFree     && (level.GetCellAt(x+1,y) != null) ? level.GetCellAt(x+1,y).GetMazeCellState() == MazeCellState.Path : false; 
            southFree = southFree   && (level.GetCellAt(x,y-1) != null) ? level.GetCellAt(x,y-1).GetMazeCellState() == MazeCellState.Path : false; 
            westFree = westFree     && (level.GetCellAt(x-1,y) != null) ? level.GetCellAt(x-1,y).GetMazeCellState() == MazeCellState.Path : false; 
        }

        switch (northFree, eastFree, southFree, westFree)
        {
            case (false, false, false, false):
                // Should be impossible but better save than sorry
                Debug.Log((x,y));
                Debug.LogError("So a glumb!");
                return false;
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
            randomValue = Random.value % maxRange;
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
                            //Debug.Log((x, y, objToUse ) + " nothing was possible ");
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
        
        if(insertToRedosIfNeeded) {
            Debug.Log("Added " + (x, y));
            redos.Add((x, y));
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