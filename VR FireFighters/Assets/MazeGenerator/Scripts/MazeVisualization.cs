using UnityEngine;

[CreateAssetMenu]
public class MazeVisualization : ScriptableObject
{
	[SerializeField] // Just add more when you need it. For example a circle 
	MazeCellObject End, Corridor, Corner, T_Crossing, X_Crossing;

	public void Visualize(Maze maze)
	{
		for (int i = 0; i < maze.Length(); i++)
		{
			MazeCellObject instance = GetPrefab((MazeFlags)(i % 16)).GetInstance();
			instance.transform.localPosition = maze.IndexToWorldPosition(i);
		}
	}

	MazeCellObject GetPrefab(MazeFlags flags) => flags switch
	{
		MazeFlags.PassageN => End,
		MazeFlags.PassageE => End,
		MazeFlags.PassageS => End,
		MazeFlags.PassageW => End,

		MazeFlags.PassageN | MazeFlags.PassageS => Corridor,
		MazeFlags.PassageE | MazeFlags.PassageW => Corridor,

		MazeFlags.PassageN | MazeFlags.PassageE => Corner,
		MazeFlags.PassageE | MazeFlags.PassageS => Corner,
		MazeFlags.PassageS | MazeFlags.PassageW => Corner,
		MazeFlags.PassageW | MazeFlags.PassageN => Corner,

		MazeFlags.PassageAll & ~MazeFlags.PassageW => T_Crossing,
		MazeFlags.PassageAll & ~MazeFlags.PassageN => T_Crossing,
		MazeFlags.PassageAll & ~MazeFlags.PassageE => T_Crossing,
		MazeFlags.PassageAll & ~MazeFlags.PassageS => T_Crossing,

		_ => X_Crossing
	};
}