using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PersonPlacement : MonoBehaviour
{
    [SerializeField]
    private GameObject dropdown_Room;

    [SerializeField]
    private GameObject dropdown_Person;

    [SerializeField]
    private MazeSettings settings;

    private GameObject person;

    private RoomObject room;

    // Start is called before the first frame update
    void Start()
    {
        //person = Resources.Load<GameObject>("Person");
        room = null;
    }

    public void PlacePerson()
    {

        string type = dropdown_Person.GetComponent<TMP_Dropdown>().options[dropdown_Person.GetComponent<TMP_Dropdown>().value].text;
        string roomName = dropdown_Room.GetComponent<TMP_Dropdown>().options[dropdown_Room.GetComponent<TMP_Dropdown>().value].text;

        person = Resources.Load<GameObject>(type);

        room = settings.TryGetRoom(roomName);

        if (room == null) return;
        Maze_Cell cell = room.GetRoomCells()[Random.Range(0, room.GetRoomCells().Count - 1)];
        GameObject person_GO = Instantiate(person);
        if (!cell.AddAdditionalPrefabPerson(person_GO))
        {
            Destroy(person_GO, 0f);
        }
    }
}
