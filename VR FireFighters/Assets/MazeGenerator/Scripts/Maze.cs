using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

using static Unity.Mathematics.math;

public struct Maze
{
	int2 size;  //size with x and y values

	[NativeDisableParallelForRestriction] NativeArray<MazeFlags> cells;

	public MazeFlags this[int index]
	{
		get => cells[index];
		set => cells[index] = value;
	}

	public Maze(int2 size)
	{
		this.size = size;
		cells = new NativeArray<MazeFlags>(size.x * size.y, Allocator.Persistent);
	}

	public MazeFlags Set(int index, MazeFlags mask) =>
		cells[index] = cells[index].With(mask);

	public MazeFlags Unset(int index, MazeFlags mask) =>
		cells[index] = cells[index].Without(mask);

	public int2 IndexToCoordinates(int index)
	{
		int2 coordinates;
		coordinates.y = index / size.x;
		coordinates.x = index - size.x * coordinates.y;
		return coordinates;
	}
	/**
	 * Für ein 8x9 (x,y) Maze ergibt damit: (mit der Methode Visualize() aus MazeVisualization)
	 * y0 = 0/8 = 0
	 * x0 = 0 - 8*0 = 0
	 * 
	 * Anschließend
	 * y1 = 1/8 
	 * x1 = 1 - 8*1/8 = 0
	 * 
	 * y2 = 2/8 
	 * x2 = 2 - 8*2/8 = 0
	 * 
	 * usw.
	 */

	/***
	 * Returns the length of the maze (x value * y value)
	 */
	public int Length()
    {
		return size.x * size.y;
    }

	public Vector3 CoordinatesToWorldPosition(int2 coordinates, float y = 0f) =>
		new Vector3(
			2f * coordinates.x + 1f - size.x,
			y,
			2f * coordinates.y + 1f - size.y
		);
	/**
	 * Für obiges Bspw ergibt sich: 
	 * y0 = 0/8 = 0
	 * x0 = 0-8*0 = 0
	 * => (2*0+1-8, 0, 2*0+1-9) = (-7,0,-8) 
	 * 
	 * Anschließend
	 * y1 = 1/8 
	 * x1 = 1 - 8*1/8 = 0
	 * => (2*0+1-8, 0, 2*1/8+1-9) = (-7,0,-7,75)
	 * 
	 * y2 = 2/8 
	 * x2 = 2 - 8*2/8 = 0
	 * => (2*0+1-8, 0, 2*2/8+1-9) = (-7,0,-7,5)
	 * 
	 * usw.
	 */

	public Vector3 IndexToWorldPosition(int index, float y = 0f) =>
		CoordinatesToWorldPosition(IndexToCoordinates(index), y);
	
	public void Dispose()
	{
		if (cells.IsCreated)
		{
			cells.Dispose();
		}
	}

	public int SizeEW => size.x;

	public int SizeNS => size.y;

	public int StepN => size.x;

	public int StepE => 1;

	public int StepS => -size.x;

	public int StepW => -1;


}