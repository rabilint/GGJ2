using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class uiContr : MonoBehaviour
{
    public GameObject GameoverMenu;
    public GameObject SettingsMenu;

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }

    public void Settings()
    {
        SettingsMenu.SetActive(!SettingsMenu.activeSelf);
        GameoverMenu.SetActive(!GameoverMenu.activeSelf);
    }
}
