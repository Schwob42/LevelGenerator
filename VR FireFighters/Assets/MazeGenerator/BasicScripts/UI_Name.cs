using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Name : MonoBehaviour
{
    [SerializeField]
    private GameObject label;

    public void SetName(string name)
    {
        label.GetComponent<TMP_Text>().text = name;
    }
}
