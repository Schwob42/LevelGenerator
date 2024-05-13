using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The game object that contains the content of the room settings.
/// The script is used to navigate between the different scenes in the game. #
/// <br/>
/// If you want to change the scenes, remember to take the scene numbering in "File > Build Settings" into account!
/// </summary>
public class Navigation : MonoBehaviour
{
    /// <summary>
    /// Navigates to the main menu.
    /// </summary>
    public void NavigateToMain()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Navigates to the generator settings.
    /// </summary>
    public void NavigateToGenerator()
    {
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Navigates to the general settings.
    /// </summary>
    public void NavigateToSettings()
    {
        SceneManager.LoadScene(2);
    }

    /// <summary>
    /// Navigates to the simulation in VR.
    /// </summary>
    public void NavigateToSimulation()
    {
        SceneManager.LoadScene(3);
    }
}
