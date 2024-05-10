using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class RoomDropdown : MonoBehaviour
{
    [SerializeField]
    private GameObject settings;
    [SerializeField]
    private GameObject dropdown;

    private List<string> roomNames;

    public void ReloadRooms()
    {
        roomNames = settings.GetComponent<Settings>().GetRoomNames();
        dropdown.GetComponent<TMP_Dropdown>().options.Clear();
        foreach (string roomName in roomNames)
        {
            dropdown.GetComponent<TMP_Dropdown>().options.Add(new TMP_Dropdown.OptionData(roomName));
        }
    }
}
