using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell
{
    private MazeCellObject mazeCellObject;
    private bool isEmpty;
    private MazeFlags mazeFlags;
    
    public MazeCell()
    {
        mazeCellObject = null;
        isEmpty = true;
        mazeFlags = MazeFlags.Empty;
    }

    public void ChangeMazeCellObject(MazeCellObject mazeCellObject)
    {
        this.mazeCellObject = mazeCellObject;

        isEmpty = mazeCellObject != null ? false : true;
    }

    public void ChangeMazeCellObject(MazeCellObject mazeCellObject, MazeFlags mazeFlags)
    {
        ChangeMazeCellObject(mazeCellObject);
        mazeFlags = mazeFlags;
    }

    public void SetFlag(MazeFlags flag)
    {
        mazeFlags = flag;
    }


    public bool IsEmpty()
    {
        return isEmpty;
    }
}
