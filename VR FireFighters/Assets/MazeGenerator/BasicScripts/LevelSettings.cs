using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSettings : MonoBehaviour
{
    [SerializeField]
    private GameObject StartPos_X;

    [SerializeField]
    private GameObject StartPos_Y;
    
    [SerializeField]
    private GameObject Level_Width;

    [SerializeField]
    private GameObject Level_Height;

    private void Start()
    {
        SetMaxPositionX();
        SetMaxPositionY();
    }

    public void SetMaxPositionX()
    {
        StartPos_X.GetComponent<Slider>().maxValue = Level_Width.GetComponent<Slider>().value;
    }

    public void SetMaxPositionY()
    {
        StartPos_Y.GetComponent<Slider>().maxValue = Level_Height.GetComponent<Slider>().value;
    }
}
