using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChristmasIntroManagerScript : MonoBehaviour
{
    public GameObject sceneLoaderPrefab;


    protected void Awake()
    {
        if (DateTime.Now > new DateTime(2020, 2, 1))
        {
            SceneManager.LoadScene("3DMXR-Menu");
        }
        else
        {
            Instantiate(sceneLoaderPrefab);
        }
    }
}
