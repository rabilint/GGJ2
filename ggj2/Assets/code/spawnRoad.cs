using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spawnRoad : MonoBehaviour
{
    public bool godMode = false;

    public GameObject[] roadsPrefab = new GameObject[4];
    [SerializeField]private GameObject[] arrayOfRoads = new GameObject[7];
    [SerializeField]private int[] colors = new int[7];

    [Space]
    public Text countdownText;
    private int countdownTime = 3;
    public GameObject emptyPrefab;

    private int nextRoadIndex = 1;
    
    private bool isPressedButton = false;
    public GameObject arrowsPlace;
    public Sprite[] arrowsPrefab;
    private SpriteRenderer spr;

    public Image progresBar;
    public float progress = 0.0f;
    public int totalWay = 30;
    public int passedWay = 0;


    public int maxHp = 3;
    public int currentHp;
    public Text curHp;
    public Text mxHp;

    public GameObject GameOverUI;

    [Space]
    [Header("game over menu")]
    public GameObject tryAgain;
    public GameObject nextLvl;
    public Text header;

    public string[] cities;
    public Text levelInfo;
    private int currentCity = 0;

    void Start()
    {
        InvokeRepeating("Countdown", 0f, 0.1f);
        
        spr = arrowsPlace.GetComponent<SpriteRenderer>();

        currentHp = maxHp;
        curHp.text = currentHp.ToString();
        mxHp.text = maxHp.ToString();

        levelInfo.text = $"From {cities[0]} to {cities[1]}";
    }

    // Update is called once per frame
    void Update()
    {
        MainGameplay();

        InputManager();

        MakeArrows();

        if(godMode)
            currentHp = 420;

    }

    void Countdown()
    {
        if(currentCity < cities.Length - 1)
        {
            levelInfo.text = $"From {cities[currentCity]} to {cities[currentCity + 1]}";
        } else {
            levelInfo.text = "Game successfully completed) We hope it was worth it>";
        }

        if(countdownTime > 0)
            countdownText.text = countdownTime.ToString();
        else if(countdownTime == 0)
            countdownText.text = "GO!";
        else
        {
            SpawnRoads();
            countdownText.text = "";
            CancelInvoke("Countdown");
        }

        countdownTime--;
    }

    void SpawnRoads()
    {
        float x = -4.32f;
        float y = 0.5f;
        
        arrayOfRoads[0] = Instantiate(emptyPrefab, new Vector3(-7.32f,y,0), Quaternion.identity);
        arrayOfRoads[0].name = "obj_" + 0;
        // colors[0] = 0;
        for(int i = 1; i < arrayOfRoads.Length; i++)
        {
            int num = Random.Range(0,4);
            colors[i] = num;
            // Debug.Log(colors[i]);

            arrayOfRoads[i] = Instantiate(roadsPrefab[num], new Vector3(x,y,0), Quaternion.identity);
            arrayOfRoads[i].name = "road_" + i;
            x += 3f;
        }
    }

    void MainGameplay()
    {
        /* if(countdownTime <= 0 && Input.GetMouseButtonDown(0)) 
            MoveRoads(); */
    }

    void MoveRoads()
    {
        for(int i = 0; i < arrayOfRoads.Length; i++)
        {
            Vector2 newPosition = arrayOfRoads[i].transform.position;
            newPosition.x -= 3f;
            arrayOfRoads[i].transform.position = newPosition;

            if(arrayOfRoads[i].transform.position.x <= -10.32f)
            {
                moveR(i: i, x: 10.68f);
            }
        }
    }
    void moveR(int i, float x)
    {
        Destroy(arrayOfRoads[i]);

        int num = Random.Range(0,4);
        colors[i] = num;

        arrayOfRoads[i] = Instantiate(roadsPrefab[num], new Vector2(x, 0.5f), Quaternion.identity);
        arrayOfRoads[i].name = "road_" + i;
    }

    void InputManager()
    {
        // if(Input.GetAxis("Horizontal") == 0)
        //     isPressedButton = false;
     
        if (countdownTime <= 0)
        {
            if(Input.GetKeyDown(KeyCode.D))
                Comprarison(0);
            if(Input.GetKeyDown(KeyCode.A))
                Comprarison(1);
            if(Input.GetKeyDown(KeyCode.W))
                Comprarison(2);
            if(Input.GetKeyDown(KeyCode.S))
                Comprarison(3);
        }

        void Comprarison(int color)
        {
            if(colors[nextRoadIndex] == color)
            {
                Debug.Log("true" + nextRoadIndex);
                nextRoadIndex++;
                passedWay++;
                FillProgressBar();
                MoveRoads();
            }
            else 
            {
                currentHp--;
                curHp.text = currentHp.ToString();

                if(currentHp <= 0 && !GameOverUI.activeSelf)
                    ActivateGameOverUI();
            }

            if(nextRoadIndex == 7)
                nextRoadIndex = 0;
            

        if(passedWay == totalWay)
        {
            currentCity++;
            Debug.Log(currentCity);
            ActivateGameOverUI();
        }
        }
        /* 
        0 == green
        1 == red
        2 == yellow
        3 == white
         */
    }

    void MakeArrows()
    {
        if(currentHp > 0 && colors[nextRoadIndex] != null)
        {
            switch (colors[nextRoadIndex])
            {
                case 0:
                    spr.sprite = arrowsPrefab[0];
                    break;
                case 1:
                    spr.sprite = arrowsPrefab[1];
                    break;
                case 2:
                    spr.sprite = arrowsPrefab[2];
                    break;
                case 3:
                    spr.sprite = arrowsPrefab[3];
                    break;
                    
                default:
                    break;
            }
        }
    }

    void FillProgressBar()
    {
        progress = (float)passedWay / totalWay;
        progresBar.fillAmount = progress;
    }

    public void RestartTheGame()
    {
        // Time.timeScale = 1f;

        currentHp = maxHp;
        passedWay = 0;
        progresBar.fillAmount = 0;
        nextRoadIndex = 1;

        countdownTime = 3;
        InvokeRepeating("Countdown", 0f, 1f);

        curHp.text = currentHp.ToString();
        
        GameOverUI.SetActive(false);
    }

    public void OutTheGame()
    {
        Debug.Log("Out");
    }

    void ActivateGameOverUI()
    {
        for(int i = 0; i < arrayOfRoads.Length; i++)
            Destroy(arrayOfRoads[i]);

        GameOverUI.SetActive(true);

        if(currentHp <= 0)
        {
            tryAgain.SetActive(true);
            header.text = "GAME OVER";
        } else {
            nextLvl.SetActive(true);
            header.text = "YOUR WIN!";
        }
    }
}
