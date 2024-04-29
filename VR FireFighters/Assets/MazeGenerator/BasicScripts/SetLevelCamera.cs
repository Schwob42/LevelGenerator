using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLevelCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject Camera;

    public void SetCameraPosition(int x, int y)
    {
        Camera.GetComponent<Transform>().position = new Vector3(x, Camera.GetComponent<Transform>().position.y, y);
    }

    public void ZoomIn()
    {
        Vector3 pos = Camera.GetComponent<Transform>().position;
        pos.y -= 10;
        if (pos.y < 10) pos.y = 10;
        Camera.GetComponent<Transform>().position = pos;
    }
    
    public void ZoomOut()
    {
        Vector3 pos = Camera.GetComponent<Transform>().position;
        pos.y += 10;
        if (pos.y > 1000) pos.y = 1000;
        Camera.GetComponent<Transform>().position = pos;
    }
}
