using TMPro;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

using static Unity.Mathematics.math;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
	[SerializeField]
	MazeVisualization visualization;

	[SerializeField, Tooltip("Use zero for random seed. Tutorial will use 123")]
	int seed;

	[SerializeField]
	int2 mazeSize = int2(20, 20);

	Maze maze;

	//TODO: Change 10 to a variable value
	[Range(3,10)]
	int minimumCorridorLength = 3;

	[SerializeField, Tooltip("You should use a point on the edge of the maze like (0,y) or (x,0)")]
	int2 startPosition = int2(0,0);

	void Awake()
	{
		if(mazeSize.x<=0 || mazeSize.y <=0 || startPosition.x < 0 || startPosition.y < 0 || minimumCorridorLength <= 0)
        {
			// TODO: This should open an error message in the editor and NOT exit the game.
			Debug.LogError("Your config values of the maze are stupid!!!!");
			return;
        }


		maze = new Maze(mazeSize);

		new GenerateField(maze);

		visualization.Visualize(maze, minimumCorridorLength, startPosition);
	}
}
