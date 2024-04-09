using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MazeCellGameObjects : ScriptableObject
{
    [SerializeField] // Just add more when you need it. For example a circle 
    public MazeCellGameObject End, Corridor, CorridorWithDoor, Corner, T_Crossing, X_Crossing, RoomEmpty, RoomWithWall, RoomWithCorner, RoomWithDoor, Start;
}
