using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public GameObject creators;

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Creators()
    {
        if(creators.activeSelf)
            creators.SetActive(false);
        else
            creators.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
