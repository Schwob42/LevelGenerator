using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    [SerializeField]
    private GameObject sliderValueTextField;
    [SerializeField]
    private Slider slider;

    private void Start()
    {
        ChangeValue();
    }

    public void ChangeValue()
    {
        float value = slider.value;
        sliderValueTextField.GetComponent<TMP_Text>().text = value.ToString();
    }
}
