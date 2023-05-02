using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class uiController : MonoBehaviour
{
    public GameObject TargetObj;
    private spawnRoad _actionTarget;

    public GameObject GameoverMenu;
    public GameObject SettingsMenu;

    int activeSceneIndex;


    void Start()
    {
        activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        _actionTarget = TargetObj.GetComponent<spawnRoad>();
    }

    public void Restart()
    {
        _actionTarget.RestartTheGame(playerWon: _actionTarget.currentHp > 0);
    }

    // public void Out()
    // {
    //     _actionTarget.OutTheGame();
    // }

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
