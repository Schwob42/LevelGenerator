using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour
{
    public void NavigateToMain()
    {
        SceneManager.LoadScene(0);
    }

    public void NavigateToGenerator()
    {
        SceneManager.LoadScene(1);
    }
}
