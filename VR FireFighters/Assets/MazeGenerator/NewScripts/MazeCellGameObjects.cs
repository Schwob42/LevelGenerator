using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCellGameObjects : ScriptableObject
{
    [SerializeField] // Just add more when you need it. For example a circle 
    MazeCellGameObject End, Corridor, CorridorWithDoor, Corner, T_Crossing, X_Crossing, RoomEmpty;
}
