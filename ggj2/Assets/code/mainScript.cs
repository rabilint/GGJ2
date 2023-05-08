using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mainScript : MonoBehaviour
{
    public bool godMode = false;
    [Space]

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

    [SerializeField] private AudioSource f;
    [SerializeField] private AudioSource d;

    public Image progresBar;
    public float progress = 0.0f;
    public int totalWay = 10;
    public int howMuchToAdd = 5;
    public int passedWay = 0;


    public int maxHp = 3;
    public int currentHp;
    public Text curHp;
    public Text mxHp;

    public Text totalWay_obj;
    public Text passedWay_obj;
    public bool skipTimer = false;

    [Space]
    [Header("game over menu")]
    public GameObject tryAgain;
    public GameObject nextLvl;
    public Text header;

    public GameObject skipTimerUiCheck;
    public GameObject GameOverCanvas;

    public GameObject GameoverMenuu;
    public GameObject SettingsMenuu;

    public string[] cities;
    public Text levelInfo;
    private int currentCity;

    public int[] wayToCities;
    public Text moneyDisplayer;
    public int[] pathToCities = new int[] {0, 7, 15};

    void Start()
    {
        cities = mapController.citiesName;
        currentCity = mapController.cityIndex;
        if(currentCity == 0) currentCity = 1;
        totalWay = pathToCities[currentCity];

        InvokeRepeating("Countdown", 0f, 0.5f);
        FillProgressBar();
        totalWay_obj.text = totalWay.ToString();
        
        spr = arrowsPlace.GetComponent<SpriteRenderer>();

        currentHp = maxHp;
        curHp.text = currentHp.ToString();
        mxHp.text = maxHp.ToString();
        moneyDisplayer.text = mapController.money.ToString();

        levelInfo.text = $"From {cities[currentCity - 1]} to {cities[currentCity]}";
    }

    // Update is called once per frame
    void Update()
    {
        InputManager();

        MakeArrows();

        if(godMode)
            currentHp = 420;

    }

    void Countdown()
    {
        if(currentCity < cities.Length - 1)
        {
            levelInfo.text = $"From {cities[currentCity - 1]} to {cities[currentCity]}";
        } else {
            levelInfo.text = "Game successfully completed) We hope it was worth it>";
        }

            //start countdown
        if(countdownTime > 0)
            countdownText.text = countdownTime.ToString();
        else if(countdownTime == 0)
            countdownText.text = "GO!";

        if(countdownTime < 0 || skipTimer)
        {
            SpawnRoads();
            countdownText.text = "";
            CancelInvoke("Countdown");
            countdownTime = -1;
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

            arrayOfRoads[i] = Instantiate(roadsPrefab[num], new Vector3(x,y,0), Quaternion.identity);
            arrayOfRoads[i].name = "road_" + i;
            x += 3f;
        }
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
        if (countdownTime <= 0 && !GameOverCanvas.activeSelf)
        {
            if(Input.GetKeyDown(KeyCode.D))
                Comprarison(0);
            if(Input.GetKeyDown(KeyCode.A))
                Comprarison(1);
            if(Input.GetKeyDown(KeyCode.W))
                Comprarison(2);
            if(Input.GetKeyDown(KeyCode.S))
                Comprarison(3);

            if(Input.GetKeyDown(KeyCode.W))
                Debug.Log("it work");
        }

        if(Input.GetKeyDown(KeyCode.Space) && GameOverCanvas.activeSelf) {
            RestartTheGame();
        }

        void Comprarison(int color)
        {
            if(colors[nextRoadIndex] == color)
            {
                Debug.Log("true" + nextRoadIndex);
                // d.Play();
                nextRoadIndex++;
                passedWay++;
                FillProgressBar();
                MoveRoads();
            }
            else 
            {
                currentHp--;
                // f.Play();
                curHp.text = currentHp.ToString();

                if(currentHp <= 0)
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

        passedWay_obj.text = passedWay.ToString();
    }

    public void RestartTheGame()
    {
        mapController.money -= 5;
        moneyDisplayer.text = mapController.money.ToString();

        currentHp = maxHp;
        passedWay = 0;
        progresBar.fillAmount = 0;
        passedWay_obj.text = passedWay.ToString();

            nextRoadIndex = 1; //?
            
        if (false)
        {
            totalWay += howMuchToAdd;
            howMuchToAdd++;//= (int)System.Math.Round(howMuchToAdd * 1.4f);
            totalWay_obj.text = totalWay.ToString();
        }

        countdownTime = 3;
        InvokeRepeating("Countdown", 0f, 0.5f);

        curHp.text = currentHp.ToString();
        
        GameOverCanvas.SetActive(false);
    }

    void ActivateGameOverUI()
    {
        for(int i = 0; i < arrayOfRoads.Length; i++)
            Destroy(arrayOfRoads[i]);

        GameOverCanvas.SetActive(true);
        
        GameoverMenuu.SetActive(true);
        SettingsMenuu.SetActive(false);

        if(currentHp <= 0)
        {
            header.text = "Loss...";
            tryAgain.SetActive(true);
        } else {
            header.text = "Path Complete!";
            nextLvl.SetActive(true);
        }
    }

    public void SwitchSkipMode()
    {
        skipTimer = !skipTimer;
        skipTimerUiCheck.SetActive(skipTimer);
    }
    
    public void ToMenu()
    {
        mapController.money += 20;
        SceneManager.LoadScene("Map");
    }
}

/* 
возможность вкл/выкл таймера в начале

на пробел игра перезапускается

добавить кнопку рестарта (или hotKey)

после прохождения уровня текущий путь не обновляется
 */
