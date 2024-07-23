using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PersonController : MonoBehaviour
{
    [SerializeField]
    private GameObject Camera;

    [SerializeField]
    private float speed = .5f;


    void Update()
    {
        Move(Input.GetAxisRaw("OVR RightJoystick Y"), Input.GetAxisRaw("OVR RightJoystick X"));
    }

    public void Move(float forward, float right)
    {
        gameObject.transform.position += new Vector3(Camera.transform.forward.x, 0, Camera.transform.forward.z).normalized * forward * speed;
        gameObject.transform.position += new Vector3(Camera.transform.right.x, 0, Camera.transform.right.z).normalized * right * speed;
           // = new Vector3(pos.x+right*speed, pos.y, pos.z+forward*speed);
    }
}
