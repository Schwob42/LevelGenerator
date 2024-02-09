using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

using static Unity.Mathematics.math;

public struct Maze
{
	int2 size;  //size with x and y values

	MazeCell[,] cells;

	public Maze(int2 size)
	{
		this.size = size;
		cells = new MazeCell[size.y, size.x];
	}

	public MazeCell GetMazeCell(int x, int y)
    {
		return cells[y, x];
    }
	
	public void SetMazeCell(int x, int y, MazeCell mazeCell)
    {
		cells[y, x] = mazeCell;
    }

	
	public int Length()
    {
		return size.x * size.y;
    }

	/**
	 * Don't be confused. The y coordinates are the z coordinate in the real coordinate system.
	 * The x coordinate is still x and the real y is constant 0.
	 * 
	 * shift: The shift of the zero point of the maze
	 */
	public Vector3 CoordinatesToWorldPosition(int2 shift, int2 MazeCellCoordinates) =>
		new Vector3(
			2f * MazeCellCoordinates.x + shift.x,
			0,
			2f * MazeCellCoordinates.y + shift.y
		);
	
	public int2 GetSize()
    {
		return size;
    }

	public int SizeEW => size.x;

	public int SizeNS => size.y;

	public int StepN => size.x;

	public int StepE => 1;

	public int StepS => -size.x;

	public int StepW => -1;


}