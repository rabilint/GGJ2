using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class uiController : MonoBehaviour
{
    public GameObject TargetObj;
    private spawnRoad _actionTarget;

    int activeSceneIndex;


    void Start()
    {
        activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        _actionTarget = TargetObj.GetComponent<spawnRoad>();
    }

    public void Restart()
    {
        _actionTarget.RestartTheGame();
    }

    public void Out()
    {
        _actionTarget.OutTheGame();
    }

    public void NextLvl()
    {
        SceneManager.LoadScene(activeSceneIndex + 1);
    }
    
}
