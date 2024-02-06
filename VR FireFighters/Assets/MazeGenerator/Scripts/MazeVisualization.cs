using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class MazeVisualization : ScriptableObject
{
	[SerializeField] // Just add more when you need it. For example a circle 
	MazeCellObject End, Corridor, CorridorWithDoor, Corner, T_Crossing, X_Crossing, RoomEmpty;

	[SerializeField, Range(0.00f,1.00f)]
	float ProbabilityForARoom;

	[SerializeField, Range(0,10)]
	int minSizeRoom, maxSizeRoom;

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
			(MazeCellObject, int) prefabWithRotation = GetPrefab(maze[i], true);
			
			//Wenn ein Corridor mit Tür gewählt wurde, soll ein Raum dahinter generiert werden. Ist das nicht möglich, soll der Corridor nicht mit Tür gebaut werden

			if (prefabWithRotation.Item1.gameObject.GetComponent<MazeCellObject>().GetDoor()){
				int skip = GenerateRoom(i, maze);
				i += skip;
			}

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
	 * 
	 * MazePrefabs: True für Prefabs aus dem Maze
	 *				False für Prefabs aus dem Raum
	 */
	(MazeCellObject, int) GetPrefab(MazeFlags flags, bool MazePrefabs)
    {
        if (MazePrefabs) { 
			float random = Random.Range(0, 101);	// increase or decrease for each added or removed corridor prefab
		
			return flags switch
			{
				MazeFlags.PassageN => (End, 0),
				MazeFlags.PassageE => (End, 1),
				MazeFlags.PassageS => (End, 2),
				MazeFlags.PassageW => (End, 3),

			

				MazeFlags.PassageN | MazeFlags.PassageS => (random<ProbabilityForARoom*100) ? (CorridorWithDoor, 0) : (Corridor, 0),
				MazeFlags.PassageE | MazeFlags.PassageW => (random<ProbabilityForARoom*100) ? (CorridorWithDoor, 1) : (Corridor, 1),

				MazeFlags.PassageN | MazeFlags.PassageE => (Corner, 0),
				MazeFlags.PassageE | MazeFlags.PassageS => (Corner, 1),
				MazeFlags.PassageS | MazeFlags.PassageW => (Corner, 2),
				MazeFlags.PassageW | MazeFlags.PassageN => (Corner, 3),

				MazeFlags.PassageAll & ~MazeFlags.PassageW => (T_Crossing, 0),
				MazeFlags.PassageAll & ~MazeFlags.PassageN => (T_Crossing, 1),
				MazeFlags.PassageAll & ~MazeFlags.PassageE => (T_Crossing, 2),
				MazeFlags.PassageAll & ~MazeFlags.PassageS => (T_Crossing, 3),

				_ => (X_Crossing, 0)
			};
		} else
        {
			return (RoomEmpty, 0);
        }
    }

	


	private int GenerateRoom(int mazePosition, Maze maze)
    {
		List<MazeCellObject> room = new List<MazeCellObject>();
		bool keepRoom = false;
		
		for(int i = mazePosition+1; i<maze.Length(); i++)
        {
			room.Add(RoomEmpty);
			Debug.Log(i);
			if (i == mazePosition + minSizeRoom)
            {
				Debug.Log("Logged");
				keepRoom = true;
            }
			if (i == mazePosition + maxSizeRoom)
            {
				Debug.Log("Ended");
				break;
            }
        }

        // if not erfolgreich
        if (!keepRoom)
        {
			Debug.Log("Nop");
			room.Clear();
			
        }
		else
        {
			Debug.Log("Yes");
			// Put into maze
			int i = mazePosition;
			foreach(MazeCellObject r in room)
            {				
				r.transform.SetPositionAndRotation(maze.IndexToWorldPosition(i), rotations[0]);
				Debug.Log(r.transform);
			}
		}
		return room.Count;
	}
}