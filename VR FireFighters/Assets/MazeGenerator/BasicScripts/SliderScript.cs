using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The script is used for showing the current value of the slider in a text field.
/// </summary>
public class SliderScript : MonoBehaviour
{
    /// <summary>
    /// The text field that is to display the value of the slider.
    /// </summary>
    [SerializeField]
    private GameObject sliderValueTextField;

    /// <summary>
    /// The slider with the value to be displayed.
    /// </summary>
    [SerializeField]
    private Slider slider;

    private void Start()
    {
        ChangeValue();
    }

    /// <summary>
    /// The method is triggered to determine the value of the slider and write it to the text field.
    /// </summary>
    public void ChangeValue()
    {
        float value = slider.value;
        sliderValueTextField.GetComponent<TMP_Text>().text = value.ToString();
    }
}
