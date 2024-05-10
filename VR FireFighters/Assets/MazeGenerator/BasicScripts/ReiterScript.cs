using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReiterScript : MonoBehaviour
{
    [SerializeField]
    private GameObject settings;
    
    [SerializeField]
    private GameObject prefabs;
    
    [SerializeField]
    private GameObject roomSettings;
    
    [SerializeField]
    private GameObject pathSettings;
    
    [SerializeField]
    private GameObject generalSettings;

    [SerializeField]
    private GameObject fire;
    
    [SerializeField]
    private GameObject smoke;
    
    [SerializeField]
    private GameObject person;
    
    

    private void Start()
    {
        ChangeContent(-1);
        ChangeContent(2);
    }

    public void ChangeContent(int content)
    {
        switch (content)
        {
            case -1:
                settings.SetActive(true);
                prefabs.SetActive(false);
                break;
            case -2:
                settings.SetActive(false);
                prefabs.SetActive(true);
                break;

            case 0:
                if (!settings.active) return;
                roomSettings.SetActive(false);
                pathSettings.SetActive(true);
                generalSettings.SetActive(false);
                break;
            case 1:
                if (!settings.active) return;
                roomSettings.SetActive(true);
                pathSettings.SetActive(false);
                generalSettings.SetActive(false);    
                break;
            case 2:
                if (!settings.active) return;
                roomSettings.SetActive(false);
                pathSettings.SetActive(false);
                generalSettings.SetActive(true);
                break;
            case 3:
                if (!prefabs.active) return;
                fire.SetActive(true);
                smoke.SetActive(false);
                person.SetActive(false);
                fire.GetComponent<RoomDropdown>().ReloadRooms();
                break;
            case 4:
                if (!prefabs.active) return;
                fire.SetActive(false);
                smoke.SetActive(true);
                person.SetActive(false);
                fire.GetComponent<RoomDropdown>().ReloadRooms();
                break;
            case 5:
                if (!prefabs.active) return;
                fire.SetActive(false);
                smoke.SetActive(false);
                person.SetActive(true);
                fire.GetComponent<RoomDropdown>().ReloadRooms();
                break;

        }
    }
}
