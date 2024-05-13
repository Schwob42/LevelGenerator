using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObject : ScriptableObject
{
    /// <summary>
    /// The name of the room. This name is only set once per generation.
    /// </summary>
    private string name;

    /// <summary>
    /// A list of prefabs with room elements.
    /// </summary>
    private List<Maze_Cell> roomPrefabs;

    /// <summary>
    /// The game object that contains the user interface with the label.
    /// This game object is loaded in the constructor.
    /// You should not assign it yourself.
    /// </summary>
    private GameObject UI;


    public RoomObject(int number, List<Maze_Cell> roomPrefabs)
    {
        this.roomPrefabs = roomPrefabs;
        UI = Resources.Load<GameObject>("UI");
        SetName(number);
        AddUI();
    }

    /// <summary>
    /// Sets the name of the room.
    /// </summary>
    /// <param name="number">The wished name if the room.</param>
    private void SetName(int number)
    {
        name = "Raum " + number;
    }

    /// <summary>
    /// Places the UI element with the label (room name) in the scene.
    /// </summary>
    private void AddUI()
    {
        GameObject UI_Name = Instantiate(UI);
        UI_Name.transform.SetParent(roomPrefabs[0].GetMazeCellGameObject().gameObject.transform);
        UI_Name.transform.localPosition = new Vector3(2, 1, 0);
        UI_Name.GetComponent<UI_Name>().SetName(name);
    } 

    public string GetRoomName()
    {
        return name;
    }
}
