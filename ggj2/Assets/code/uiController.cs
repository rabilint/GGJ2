using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiController : MonoBehaviour
{
    public GameObject TargetObj;
    private spawnRoad _actionTarget;


    void Start()
    {
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
    
}
