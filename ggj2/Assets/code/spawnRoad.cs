using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spawnRoad : MonoBehaviour
{
    public GameObject[] roadsPrefab = new GameObject[4];
    [SerializeField]private GameObject[] arrayOfRoads = new GameObject[6];

    // public GameObject[] countdownNums = GameObject[4];
    [Space]
    public Text countdownText;
    private int countdownTime = 3;
    public GameObject emptyPrefab;

    // private GameObject roads;

    void Start()
    {
        InvokeRepeating("Countdown", 0f, 1.0f);
        SpawnRoads();
    }

    // Update is called once per frame
    void Update()
    {
        MainGameplay();
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

    void SpawnRoads()
    {
        float x = -4.32f;
        float y = 0.5f;
        
        arrayOfRoads[0] = Instantiate(emptyPrefab, new Vector3(-7.32f,y,0), Quaternion.identity);
        arrayOfRoads[0].name = "null_" + 0;
        for(int i = 1; i < arrayOfRoads.Length; i++)
        {
            int num = Random.Range(0,4);

            arrayOfRoads[i] = Instantiate(roadsPrefab[num], new Vector3(x,y,0), Quaternion.identity);
            arrayOfRoads[i].name = "road_" + i;
            x += 3;
        }
    }

    void MainGameplay()
    {
        if(countdownTime <= 0 && Input.GetMouseButtonDown(0)) 
            MoveRoads();
    }

    void MoveRoads()
    {
        for(int i = 0; i < arrayOfRoads.Length; i++)
        {
            if(arrayOfRoads[i].transform.position.x <= -7.32f)
            {
                Destroy(arrayOfRoads[i]);

                int num = Random.Range(0,4);
                arrayOfRoads[i] = Instantiate(roadsPrefab[num], new Vector2(7.68f, 0.5f), Quaternion.identity);
                // Debug.Log(i);
            } else {
                Vector3 newPosition = arrayOfRoads[i].transform.position;
                newPosition.x -= 3f;
                arrayOfRoads[i].transform.position = newPosition;
            }
        }
    }
}
