using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu] 
public class Scenes : ScriptableObject
{
    [SerializeField]
    private Scene mainScreen;

    [SerializeField]
    private Scene generatorScreen;

    [SerializeField] bool thereShouldBeRooms;
}
