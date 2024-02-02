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

	void Awake()
	{
		maze = new Maze(mazeSize);

		new GenerateMazeJob
		{
			maze = maze,
			seed = seed != 0 ? seed : Random.Range(1, int.MaxValue)
		}.Schedule().Complete();

		visualization.Visualize(maze);
	}

	void OnDestroy()
	{
		maze.Dispose();
	}
}
