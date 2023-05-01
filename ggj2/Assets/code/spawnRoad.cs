using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spawnRoad : MonoBehaviour
{
    public GameObject[] roadsPrefab = new GameObject[4];
    [SerializeField]private GameObject[] arrayOfRoads = new GameObject[7];
    [SerializeField]private int[] colors = new int[7];

    // public GameObject[] countdownNums = GameObject[4];
    [Space]
    public Text countdownText;
    private int countdownTime = 3;
    public GameObject emptyPrefab;

    // private GameObject roads;
    private int nextRoadIndex = 1;
    
    // [Space]
    // [Header("Input Manager")]
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

    void Start()
    {
        InvokeRepeating("Countdown", 0f, 1f);
        SpawnRoads();
        spr = arrowsPlace.GetComponent<SpriteRenderer>();

        currentHp = maxHp;
        curHp.text = currentHp.ToString();
        mxHp.text = maxHp.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        MainGameplay();

        InputManager();

        MakeArrows();

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
        // colors[0] = 0;
        for(int i = 1; i < arrayOfRoads.Length; i++)
        {
            int num = Random.Range(0,4);
            colors[i] = num;
            // Debug.Log(colors[i]);

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
            if(arrayOfRoads[i].transform.position.x == -7.32f)
            {
                moveR(i, 10.68f);
            } else if(arrayOfRoads[i].transform.position.x == -10.32f) {
                moveR(i, 7.68f);
            } else          
            {
                Vector2 newPosition = arrayOfRoads[i].transform.position;
                newPosition.x -= 3f;
                arrayOfRoads[i].transform.position = newPosition;
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
     
        if(Input.GetKeyDown(KeyCode.D))
            Comprarison(0);
        if(Input.GetKeyDown(KeyCode.A))
            Comprarison(1);
        if(Input.GetKeyDown(KeyCode.W))
            Comprarison(2);
        if(Input.GetKeyDown(KeyCode.S))
            Comprarison(3);

        void Comprarison(int color)
        {
            if(colors[nextRoadIndex] == color)
                Debug.Log("true" + nextRoadIndex);
            else 
            {
                currentHp--;
                curHp.text = currentHp.ToString();
            }
                // Debug.Log("false" + nextRoadIndex);

            nextRoadIndex++;
            passedWay++;
            if(nextRoadIndex == 7)
                nextRoadIndex = 0;
            
            FillProgressBar();
            MoveRoads();
            
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
        if(colors[nextRoadIndex] != null)
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

        if(passedWay == totalWay)
        {
            Debug.Log("Level passed!");
        }
    }
}
