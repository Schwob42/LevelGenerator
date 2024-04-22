using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReiterScript : MonoBehaviour
{
    [SerializeField]
    private GameObject roomSettings;
    
    [SerializeField]
    private GameObject pathSettings;
    
    [SerializeField]
    private GameObject generalSettings;

    private void Start()
    {
        ChangeContent(2);
    }

    public void ChangeContent(int content)
    {
        switch (content)
        {
            case 0:
                roomSettings.SetActive(false);
                pathSettings.SetActive(true);
                generalSettings.SetActive(false);
                break;
            case 1:
                roomSettings.SetActive(true);
                pathSettings.SetActive(false);
                generalSettings.SetActive(false);    
                break;
            case 2:
                roomSettings.SetActive(false);
                pathSettings.SetActive(false);
                generalSettings.SetActive(true);
                break;
        }
    }
}
