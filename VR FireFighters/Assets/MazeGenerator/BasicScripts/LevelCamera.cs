using UnityEngine;

public class LevelCamera : MonoBehaviour
{
    /// <summary>
    /// The game object that contains the cames for the bird view.
    /// </summary>
    [SerializeField]
    private GameObject Camera;

    /// <summary>
    /// Sets the camera to the center of the level.
    /// </summary>
    /// <param name="x">The level width</param>
    /// <param name="y">The level hight</param>
    public void SetCameraPosition(int x, int y)
    {
        Camera.GetComponent<Transform>().position = new Vector3(x, Camera.GetComponent<Transform>().position.y, y);
    }

    /// <summary>
    /// The function zooms in and sets the camera to a lower position.
    /// The minimum value for the Y-coordinate is 10.
    /// </summary>
    public void ZoomIn()
    {
        Vector3 pos = Camera.GetComponent<Transform>().position;
        pos.y -= 10;
        if (pos.y < 10) pos.y = 10;
        Camera.GetComponent<Transform>().position = pos;
    }

    /// <summary>
    /// The function zooms in and sets the camera to a lower position.
    /// The maximum value for the Y-coordinate is 1000.
    /// </summary>
    public void ZoomOut()
    {
        Vector3 pos = Camera.GetComponent<Transform>().position;
        pos.y += 10;
        if (pos.y > 1000) pos.y = 1000;
        Camera.GetComponent<Transform>().position = pos;
    }
}
