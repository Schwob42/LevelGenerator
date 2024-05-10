using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObject : ScriptableObject
{
    private string name;

    private List<Maze_Cell> roomPrefabs;

    [SerializeField]
    private GameObject UI;


    public RoomObject(int number, List<Maze_Cell> roomPrefabs)
    {
        this.roomPrefabs = roomPrefabs;
        UI = Resources.Load<GameObject>("UI");
        SetName(number);
        AddUI();
    }

    private void SetName(int number)
    {
        name = "Raum " + number;
    }

    public string GetRoomName()
    {
        return name;
    }

    private void AddUI()
    {
        GameObject UI_Name = Instantiate(UI);
        UI_Name.transform.SetParent(roomPrefabs[0].GetMazeCellGameObject().gameObject.transform);
        UI_Name.transform.localPosition = new Vector3(2, 1, 0);
        UI_Name.GetComponent<UI_Name>().SetName(name);
    } 
}
