using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SmokePlacement : MonoBehaviour
{
    [SerializeField]
    private GameObject dropdown;

    [SerializeField]
    private MazeSettings settings;

    private GameObject smoke;

    private RoomObject room;

    // Start is called before the first frame update
    void Start()
    {
        smoke = Resources.Load<GameObject>("Smoke2");
        room = null;
    }

    public void PlaceSmoke()
    {
        string roomName = dropdown.GetComponent<TMP_Dropdown>().options[dropdown.GetComponent<TMP_Dropdown>().value].text;

        room = settings.TryGetRoom(roomName);

        if (room == null) return;
        foreach(Maze_Cell cell in room.GetRoomCells())
        {
            GameObject fire_GO = Instantiate(smoke);
            if (!cell.AddAdditionalPrefabSmoke(fire_GO))
            {
                Destroy(fire_GO, 0f);
            }                
        }
    }
}
