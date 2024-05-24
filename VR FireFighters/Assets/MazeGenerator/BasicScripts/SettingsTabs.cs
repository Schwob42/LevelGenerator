using UnityEngine;

/// <summary>
/// The script manages the switching between the different tabs in the settings
/// There are references to the various contents of the tabs.
/// </summary>
public class SettingsTabs: MonoBehaviour
{
    /////////// Main tabs

    /// <summary>
    /// The game object that contains the content of the settings.
    /// </summary>
    [SerializeField]
    private GameObject settings;

    /// <summary>
    /// The game object that contains the content of the prefabs.
    /// </summary>
    [SerializeField]
    private GameObject prefabs;

    //////////// Secondary Tabs (settings)
    /// <summary>
    /// The game object that contains the content of the room settings.
    /// </summary>
    [SerializeField]
    private GameObject roomSettings;

    /// <summary>
    /// The game object that contains the content of the path settings.
    /// </summary>
    [SerializeField]
    private GameObject pathSettings;

    /// <summary>
    /// The game object that contains the content of the general settings.
    /// </summary>
    [SerializeField]
    private GameObject generalSettings;


    //////////// Secondary Tabs (prefabs)
    /// <summary>
    /// The game object that contains the content of the fire settings.
    /// </summary>
    [SerializeField]
    private GameObject fire;

    /// <summary>
    /// The game object that contains the content of the smoke settings.
    /// </summary>
    [SerializeField]
    private GameObject smoke;

    /// <summary>
    /// The game object that contains the content of the person settings.
    /// </summary>
    [SerializeField]
    private GameObject person;    

    private void Start()
    {
        ChangeContent(-1);
        ChangeContent(2);
    }

    /// <summary>
    /// Changes the tab depending on the given input as referenc to the wished content.
    /// </summary>
    /// <param name="content">A number that represents a specific content.</param>
    public void ChangeContent(int content)
    {
        switch (content)
        {
            /// Main tabs
            case -1:    // Settings
                settings.SetActive(true);
                prefabs.SetActive(false);
                break;
            case -2:    // Prefabs
                settings.SetActive(false);
                prefabs.SetActive(true);
                break;

            /// Secondary Tabs (Settings)
            case 0:     // Path settings
                if (!settings.active) return;
                roomSettings.SetActive(false);
                pathSettings.SetActive(true);
                generalSettings.SetActive(false);
                break;
            case 1:     // Room settings
                if (!settings.active) return;
                roomSettings.SetActive(true);
                pathSettings.SetActive(false);
                generalSettings.SetActive(false);    
                break;
            case 2:     // General settings
                if (!settings.active) return;
                roomSettings.SetActive(false);
                pathSettings.SetActive(false);
                generalSettings.SetActive(true);
                break;

            /// Secondary Tabs (Prefabs)
            case 3:     // Fire settings
                if (!prefabs.active) return;
                fire.SetActive(true);
                smoke.SetActive(false);
                person.SetActive(false);
                fire.GetComponent<RoomDropdown>().ReloadRooms();
                break;
            case 4:     // Smoke settings
                if (!prefabs.active) return;
                fire.SetActive(false);
                smoke.SetActive(true);
                person.SetActive(false);
                smoke.GetComponent<RoomDropdown>().ReloadRooms();
                break;
            case 5:     // (missed) Person settings
                if (!prefabs.active) return;
                fire.SetActive(false);
                smoke.SetActive(false);
                person.SetActive(true);
                person.GetComponent<RoomDropdown>().ReloadRooms();
                break;

        }
    }
}
