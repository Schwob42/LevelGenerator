using UnityEngine;

[CreateAssetMenu]
public class MazeVisualization : ScriptableObject
{
	[SerializeField] // Just add more when you need it. For example a circle 
	MazeCellObject End, Corridor, Corner, T_Crossing, X_Crossing;

	static Quaternion[] rotations =
	{
		Quaternion.identity,
		Quaternion.Euler(0f, 90f, 0f),
		Quaternion.Euler(0f, 180f, 0f),
		Quaternion.Euler(0f, 270f, 0f)
	};

	public void Visualize(Maze maze)
	{
		for (int i = 0; i < maze.Length(); i++)
		{
			(MazeCellObject, int) prefabWithRotation = GetPrefab(maze[i]);
			MazeCellObject instance = prefabWithRotation.Item1.GetInstance();
			//instance.transform.localPosition = maze.IndexToWorldPosition(i);
			instance.transform.SetPositionAndRotation(
				maze.IndexToWorldPosition(i), rotations[prefabWithRotation.Item2]
			);
		}
	}

	/**
	 * Der folgende Code ist nichts anderes, als ein großes Switch-Case, abhängig von den gesetzten Flags
	 * 
	 * !!!Wichtig!!!!
	 * Es handelt sich hier noch immer um boolsche Operationen. 
	 * >> MazeFlags.PassageN | MazeFlags.PassageS heißt "Wenn die Flag für N und S gesetzt wurde, dann ist es ein Corridor"
	 * >> 0b0001 | 0b0100 = 0b0101
	 * 
	 * Selbiges gilt für MazeFlags.PassageAll & ~MazeFlags.PassageW
	 * >> MazeFlags.PassageAll & ~MazeFlags.PassageW heißt nichts anderes, als "Es gibt in alle Richtungen einen Durchgang und nicht nach Westen"
	 * >> 0b1111 & ~0b1000 = 0b0111
	 */
	(MazeCellObject, int) GetPrefab(MazeFlags flags) => flags switch
	{
		MazeFlags.PassageN => (End,0),
		MazeFlags.PassageE => (End,1),
		MazeFlags.PassageS => (End,2),
		MazeFlags.PassageW => (End,3),

		MazeFlags.PassageN | MazeFlags.PassageS => (Corridor,0),
		MazeFlags.PassageE | MazeFlags.PassageW => (Corridor,1),

		MazeFlags.PassageN | MazeFlags.PassageE => (Corner,0),
		MazeFlags.PassageE | MazeFlags.PassageS => (Corner,1),
		MazeFlags.PassageS | MazeFlags.PassageW => (Corner,2),
		MazeFlags.PassageW | MazeFlags.PassageN => (Corner,3),

		MazeFlags.PassageAll & ~MazeFlags.PassageW => (T_Crossing,0),
		MazeFlags.PassageAll & ~MazeFlags.PassageN => (T_Crossing,1),
		MazeFlags.PassageAll & ~MazeFlags.PassageE => (T_Crossing,2),
		MazeFlags.PassageAll & ~MazeFlags.PassageS => (T_Crossing,3),

		_ => (X_Crossing,0)
	};
}