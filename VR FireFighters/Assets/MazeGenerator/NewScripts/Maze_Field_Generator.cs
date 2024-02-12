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
    private bool SetNextCell(int x, int y, int corridorMinLength)
    {
        Maze_Cell mc = maze.GetCellAt(x, y);
        //Debug.Log(mc.GetPassageNorth());
        
        //if(corridorMinLength > 0)   //Nur Passage oder Ende möglich damit sind nur Orientierungen in entgegengesetzte Richtungen möglich und eine der Richtungen ist bereits belegt (irgendwo kommen wir ja her)
        //{
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
        else if (mc.GetPassageSouth())
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
        else if (mc.GetPassageEast())
        {
            if (maze.SetGameObjectIntoCell(x + 1, y, 90, mazeGameObjects.Corridor))
            {
                SetNextCell(x+1, y, corridorMinLength - 1);
            }
            else
            {
                maze.SetGameObjectIntoCell(x + 1, y, -90, mazeGameObjects.End);
            }
        }
        else if (mc.GetPassageWest())
        {
            if (maze.SetGameObjectIntoCell(x - 1, y, 90, mazeGameObjects.Corridor))
            {
                SetNextCell(x + 1, y, corridorMinLength - 1);
            }
            else
            {
                maze.SetGameObjectIntoCell(x - 1, y, 90, mazeGameObjects.End);
            }
        }
        //}


        return true;
    }
}
