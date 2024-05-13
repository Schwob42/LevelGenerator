using UnityEngine;
using UnityEngine.UI;

/**
 * The script manages the local settings for the start position.
 */
public class MaxPosition: MonoBehaviour
{
    /**
     * The game object that contains the slider to set the x coordinate of the start position.
     */
    [SerializeField]
    private GameObject StartPos_X;

    /**
     * The game object that contains the slider to set the y coordinate of the start position.
     */
    [SerializeField]
    private GameObject StartPos_Y;

    /**
     * The game object that contains the slider to set the width of the level.
     */
    [SerializeField]
    private GameObject Level_Width;

    /**
     * The game object that contains the slider to set the height of the level.
     */
    [SerializeField]
    private GameObject Level_Height;

    private void Start()
    {
        SetMaxPositionX();
        SetMaxPositionY();
    }

    /**
     * Calculates the maximal possible value for the x coordinate.
     * Sets the max value of the slider to the calculated value.
     */
    public void SetMaxPositionX()
    {
        StartPos_X.GetComponent<Slider>().maxValue = Level_Width.GetComponent<Slider>().value-1;
    }

    /**
     * Calculates the maximal possible value for the y coordinate.
     * Sets the max value of the slider to the calculated value.
     */
    public void SetMaxPositionY()
    {
        StartPos_Y.GetComponent<Slider>().maxValue = Level_Height.GetComponent<Slider>().value-1;
    }
}
