using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// The script is used to check the sum of the probabilities of each path element (prefab).
/// </summary>
public class CheckPos : MonoBehaviour
{
    /// <summary>
    /// The game object that contains the slider to set the possibility of the Corridor element.
    /// </summary>
    [SerializeField]
    private GameObject probability_Corridor;

    /// <summary>
    /// The game object that contains the slider to set the possibility of the Corner element.
    /// </summary>
    [SerializeField]
    private GameObject probability_Corner;

    /// <summary>
    /// The game object that contains the slider to set the possibility of the T-Crossing element.
    /// </summary>
    [SerializeField]
    private GameObject probability_T_Crossing;

    /// <summary>
    /// The game object that contains the slider to set the possibility of the X-Crossing element.
    /// </summary>
    [SerializeField]
    private GameObject probability_X_Crossing;

    /// <summary>
    /// The game object that contains the slider to set the possibility of the End (dead end) element.
    /// </summary>
    [SerializeField]
    private GameObject probability_End;

    /// <summary>
    /// The game object that the button to trigger the level generation.
    /// </summary>
    [SerializeField]
    private GameObject generatorButton;

    /// <summary>
    /// The game object, which contains a text field showing the sum of all possibilities and a hint.
    /// </summary>
    [SerializeField]
    private GameObject valueField;

    /// <summary>
    /// Checks the sum of the set probabilities of each path element.
    /// The sum of all probabilities should be 100 (percent).
    /// In case that the sum is 100, the generatorButton is activated.
    /// If the sum is greater or less than 100, the generatorButton is deactivated.
    /// </summary>
    public void CheckSum()
    {
        float sum = probability_Corridor.GetComponent<Slider>().value +
            probability_Corner.GetComponent<Slider>().value +
            probability_X_Crossing.GetComponent<Slider>().value +
            probability_T_Crossing.GetComponent<Slider>().value +
            probability_End.GetComponent<Slider>().value;

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
