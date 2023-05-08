using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuScript : MonoBehaviour
{
    public GameObject creators;

    public void Play()
    {
        SceneManager.LoadScene("Map");
    }

    public void Creators()
    {
        creators.SetActive(!creators.activeSelf);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
