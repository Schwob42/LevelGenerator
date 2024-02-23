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
    private void Start()
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

        maze = new Maze_Field(mazeSizeY, mazeSizeY);
        
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

        //bool openPaths = true;

        maze.SetGameObjectIntoCell(pos.Item1, pos.Item2, 0, mazeGameObjects.Start);

        SetNextCell(pos.Item1, pos.Item2, corridorMinLength);
    }

    /**
     * Methode für den rekursiven Aufruf zur Pfadgenerierung. 
     * Übergeben wird die Position der letzten Zelle und die noch zu erreichende gewünschte (Ende ist immer erlaubt) minimale Korridorlänge;
     * 
     * 
     */
    private void SetNextCell(int x, int y, int corridorMinLength)
    {
        Maze_Cell mc = maze.GetCellAt(x, y);

        Debug.Log("Looking for " + (x, y) + " " + mc.GetIsEmpty());

        if (corridorMinLength > 0)   //Nur Passage oder Ende möglich damit sind nur Orientierungen in entgegengesetzte Richtungen möglich und eine der Richtungen ist bereits belegt (irgendwo kommen wir ja her)
        {
            if (mc.GetPassageNorth())
            {
                if (maze.SetGameObjectIntoCell(x, y + 1, 0, mazeGameObjects.Corridor))
                {
                    SetNextCell(x, y + 1, corridorMinLength - 1);
                }
                else
                {
                    maze.SetGameObjectIntoCell(x, y + 1, 180, mazeGameObjects.End);
                }
            }
            if (mc.GetPassageSouth())
            {
                if (maze.SetGameObjectIntoCell(x, y - 1, 0, mazeGameObjects.Corridor))
                {
                    SetNextCell(x, y - 1, corridorMinLength - 1);
                }
                else
                {
                    maze.SetGameObjectIntoCell(x, y - 1, 0, mazeGameObjects.End);
                }
            }
            if (mc.GetPassageEast())
            {
                if (maze.SetGameObjectIntoCell(x + 1, y, 90, mazeGameObjects.Corridor))
                {
                    SetNextCell(x + 1, y, corridorMinLength - 1);
                }
                else
                {
                    maze.SetGameObjectIntoCell(x + 1, y, -90, mazeGameObjects.End);
                }
            }
            if (mc.GetPassageWest())
            {
                if (maze.SetGameObjectIntoCell(x - 1, y, 90, mazeGameObjects.Corridor))
                {
                    SetNextCell(x - 1, y, corridorMinLength - 1);
                }
                else
                {
                    maze.SetGameObjectIntoCell(x - 1, y, 90, mazeGameObjects.End);
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
         * Könnte zu 
         * ----x---- (soll eine Kreuzung sein) 
         *     |     
         *     |
         *  
         *  werden
         */
        else   // random object is possible 
        {
            if (mc.GetPassageNorth() && maze.GetCellAt(x, y + 1).GetIsEmpty())
            {
                // Nach Norden weiterbauen
                if (FindObjectForCellAt(x, y + 1))
                {
                    SetNextCell(x, y + 1, mazeSettings.GetMinCorridorLength());
                }
                
            }
            if (mc.GetPassageEast() && maze.GetCellAt(x + 1, y).GetIsEmpty())
            {
                // Nach Osten weiterbauen
                if (FindObjectForCellAt(x+1, y))
                {
                    SetNextCell(x+1, y, mazeSettings.GetMinCorridorLength());
                }
                    
            }
            if (mc.GetPassageSouth() && maze.GetCellAt(x, y - 1).GetIsEmpty())
            {
                // Nach Süden weiterbauen
                if (FindObjectForCellAt(x, y - 1))
                {
                    SetNextCell(x, y - 1, mazeSettings.GetMinCorridorLength());
                }
            }
            if (mc.GetPassageWest() && maze.GetCellAt(x - 1, y).GetIsEmpty())
            {
                // Nach Westen weiterbauen
                if (FindObjectForCellAt(x - 1, y))
                {
                    SetNextCell(x - 1, y, mazeSettings.GetMinCorridorLength());
                }
            }
        }
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

        foreach ((MazeCellGameObject mcgo, float f) in mco)
        {
            sum += f;
            if (randomValue <= sum)
            {
                //Debug.Log("Choice: " + mcgo);
                objToUse = mcgo;
                break;
            }
        }

        if (!maze.SetGameObjectIntoCell(x,y,0,objToUse))
        {
            if (!maze.SetGameObjectIntoCell(x, y, 90, objToUse))
            {
                if (!maze.SetGameObjectIntoCell(x, y, 180, objToUse))
                {
                    if(!maze.SetGameObjectIntoCell(x, y, 270, objToUse))
                    {
                        Debug.Log("Nothing was possible " + (x, y, objToUse) + maze.GetCellAt(x, y));
                    }
                }
            }
        }

        if (objToUse.Equals(mazeGameObjects.End))
        {
            Debug.Log("This is the end " + mazeGameObjects);
            return false;
        }

        

        return true;
    }
}
