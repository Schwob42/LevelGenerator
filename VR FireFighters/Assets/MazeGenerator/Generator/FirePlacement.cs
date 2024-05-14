using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FirePlacement : MonoBehaviour
{
    [SerializeField]
    private GameObject dropdown;

    [SerializeField]
    private MazeSettings settings;

    private GameObject fire;

    private RoomObject room;

    // Start is called before the first frame update
    void Start()
    {
        fire = Resources.Load<GameObject>("Fire");
        room = null;
    }

    public void PlaceFire()
    {
        string roomName = dropdown.GetComponent<TMP_Dropdown>().options[dropdown.GetComponent<TMP_Dropdown>().value].text;

        room = settings.TryGetRoom(roomName);

        if (room == null) return;
        foreach(Maze_Cell cell in room.GetRoomCells())
        {
            if (Random.Range(0f, 1f) > 0.5f) continue;  //TODO let choose in settings
            GameObject fire_GO = Instantiate(fire);
            cell.AddAdditionalPrefabFire(fire_GO);
        }
    }
}
