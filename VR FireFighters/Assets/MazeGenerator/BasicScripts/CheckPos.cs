using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CheckPos : MonoBehaviour
{
    [SerializeField]
    private GameObject pos_Corridor;
    [SerializeField]
    private GameObject pos_Corner;
    [SerializeField]
    private GameObject pos_T_Crossing;
    [SerializeField]
    private GameObject pos_X_Crossing;
    [SerializeField]
    private GameObject pos_End;
    
    [SerializeField]
    private GameObject generatorButton;
    [SerializeField]
    private GameObject valueField;



    public void CheckSum()
    {
        float sum = pos_Corridor.GetComponent<Slider>().value +
        pos_Corner.GetComponent<Slider>().value +
        pos_X_Crossing.GetComponent<Slider>().value +
        pos_T_Crossing.GetComponent<Slider>().value +
        pos_End.GetComponent<Slider>().value;

        if(sum == 100)
        {
            generatorButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            generatorButton.GetComponent<Button>().interactable = false;
        }

        valueField.GetComponent<TMP_Text>().text = "Hinweis: Die Angaben erfolgen in %." +
            "Die Summe aller Wahrscheinlichkeiten muss 100 % ergeben. (Aktuell: " +
            sum + ")";
    }
}
