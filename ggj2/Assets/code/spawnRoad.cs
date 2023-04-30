using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spawnRoad : MonoBehaviour
{
    public GameObject[] roadsPrefab = new GameObject[4];
    private GameObject[] arrayOfRoads = new GameObject[6];

    // public GameObject[] countdownNums = GameObject[4];
    public Text countdownText;
    private int countdownTime = 3;

    void Start()
    {
        InvokeRepeating("Countdown", 0.35f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Countdown()
    {
        if(countdownTime > 0)
            countdownText.text = countdownTime.ToString();
        else if(countdownTime == 0)
            countdownText.text = "GO!";
        else
        {
            Destroy(countdownText);
            CancelInvoke("Countdown");
        }

        countdownTime--;
    }
}
