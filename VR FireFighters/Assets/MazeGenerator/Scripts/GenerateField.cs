using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

[BurstCompile]
public class GenerateField: MonoBehaviour
{
	Maze maze;

	public GenerateField(Maze maze)
    {
		this.maze = maze;

		BuildField();
    }

	public void BuildField()
	{
		int2 mazeSize = maze.GetSize();

		
		for (int y = 0; y < mazeSize.y; y++)
        {
			for (int x=0; x < mazeSize.x; x++)
            {
				maze.SetMazeCell(x, y, new MazeCell());
				MazeFlags flag = MazeFlags.PassageAll;					//No walls only passages

				if (y == 0) flag &= ~MazeFlags.PassageW;				//Wall on the left
				if (y == mazeSize.y-1) flag &= ~MazeFlags.PassageE;		//Wall on the right
				if (x == 0) flag &= ~MazeFlags.PassageS;				//Wall on the bottom
				if (x == mazeSize.x-1) flag &= ~MazeFlags.PassageN;     //Wall on the top

				/*Again. The flags are like a binary number.
				 * So 0b1111 & ~0b1000 means
				 * 0b1111 & 0b0111 = 0b0111
				 * and this means, there is a (possible) passage to the North, East and South but not to the West.
				 * This makes pretty sense because there is a wall or better the edge of the field.
				 */


				maze.GetMazeCell(x, y).SetFlag(flag);
            }
        }
	}

	/*
	 * So after this code, every cell in the field has a flag which represent the direction of (possible) passages an walls.
	 */
}