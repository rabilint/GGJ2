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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            if(Screen.fullScreen)
                Screen.SetResolution(1280, 720, false);
            else 
                Screen.SetResolution(1920, 1080, true);
        }
    }
}
