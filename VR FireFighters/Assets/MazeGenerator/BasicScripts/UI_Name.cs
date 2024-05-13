using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// The script holds a label that shows the name of a room in the UI.
/// </summary>
public class UI_Name : MonoBehaviour
{
    [SerializeField]
    private GameObject label;

    public void SetName(string name)
    {
        label.GetComponent<TMP_Text>().text = name;
    }
}
