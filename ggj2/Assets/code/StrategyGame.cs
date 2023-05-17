using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.Collections;

public class StrategyGame : MonoBehaviour
{
    public int septims = 50;
    public Text septimsObj;
    public float[] iteration;
    private float[] startTime;

    public Text[] resoursesLevel;
    public Text[] resoursesProfit;
    public int[] cost;
    public Text[] costObj;
    public byte[] level;
    public int[] pricesPerIteration;

    public Image[] progresBar;

    public int[] resoursesCount;
    [Space]
    private int deliveryEnd;
    public int deliveryDuration = 6;

    public Text deliveryStatusObj;

    public Text trInfoNumObj;
    public Text transportInfoObj;
    public Text profitNumObj;
    public Text profitObj;
    public Text remTimeNumObj;
    public Text remainingTimeObj;
    public Image deliveryProgressBarObj;
    private bool deliveryActive = false;

    public GameObject sendResButton;
    public GameObject icon;

    public GameObject SendMenuUi;
    public Transform SendMenuUiTransform;
    public GameObject notificationObj;
    public Text newOfferObj;

    public Text[] resCountObj;
    // public int[] resCount;
    public byte playerLevel = 1;
    public GameObject refuseButton;

    
    
    private void Start()
    {
        startTime = new float[cost.Length];
        resoursesCount = new int[cost.Length];

        addCities();
        // ChangePosition(5);

        for(int i = 0; i < cost.Length; i++)
        {
            startTime[i] = Time.time;
            costObj[i].text = cost[i].ToString();
        }
        notificationObj.SetActive(false);

        // RiseLevel(0);
        level[0] = 1;
        resoursesLevel[0].text = "1";
        septims = 20;
        septimsObj.text = "20";
    }

    public void RiseLevel(int index)
    {
        // if(iteration[index] <= 1.1f) Debug.Log();
        if (septims >= cost[index] && level[index] < 99)
        {
            if(level[index]==0) startTime[index] = Time.time;
            septims -= cost[index];

            level[index]++;

            // pricesPerIteration[index] = Convert.ToInt32(Math.Ceiling(pricesPerIteration[index] * Random.Range(1f, 1.35f)));
            iteration[index] = Mathf.Pow(iteration[index], 0.92f);

            cost[index] += Convert.ToInt32(Math.Ceiling(level[index] * 1.2f * 1.1f));


            // if (level[index] % 10 == 0) pricesPerIteration[index] += Convert.ToInt32(Math.Ceiling(pricesPerIteration[index] * Random.Range(1f, 2f)));
            septimsObj.text = septims.ToString();
            resoursesLevel[index].text = (level[index]).ToString();
            // resoursesProfit[index].text = pricesPerIteration[index].ToString();
            costObj[index].text = cost[index].ToString();
        }
    }

    private int timer = 0;
    private int timeAfterLastOffer = 0;
    private void FixedUpdate()
    {
        if(Time.time >= timer)
        {
            timer++;
            timeAfterLastOffer++;
            CreateRequest();
        }
        GetMoney();
    }

    private void GetMoney()
    {
        for(int i = 0; i < cost.Length; i++)
        {
            if(level[i] != 0)
            {
                if (Time.time >= startTime[i] + iteration[i])
                {
                    startTime[i] = Time.time;
                    resoursesCount[i]++;
                    // septims += pricesPerIteration[i];
                    resCountObj[i].text = resoursesCount[i].ToString();
                }

                float progress = (Time.time - startTime[i]) / iteration[i];
                progresBar[i].fillAmount = Mathf.Clamp01(progress);
            } else progresBar[i].fillAmount = 0f;
        }
    }

    void Update()
    {
        if(deliveryActive)
        {
            deliveryDuration = cities[cityID].deliveryTime;
            float progress = (deliveryEnd - Time.time) / deliveryDuration;
            deliveryProgressBarObj.fillAmount = Mathf.Clamp01(progress);

            remainingTimeObj.text = "Left time";
            remTimeNumObj.text = (Math.Round(deliveryEnd - Time.time)).ToString();
            
                //end of delivery
            if(progress <= 0) 
            {
                deliveryActive = false;
                deliveryStatusObj.text = cities[cityID].name;
                ClearUselessText();
                /* if(!sendResButton.activeSelf)
                {
                    transportInfoObj.text = "";
                    trInfoNumObj.text = "";
                    Debug.Log("clear txt");
                } */
                icon.SetActive(false);
                StartCoroutine(FIllPrBar());
            }
        }
    }
    void ClearUselessText()
    {
        profitNumObj.text = "";
        profitObj.text = "";
        remTimeNumObj.text = "";
        remainingTimeObj.text = "";
        transportInfoObj.text = "";
        trInfoNumObj.text = "";
    }

    IEnumerator FIllPrBar()
    {
        for (float i = 0f; i < 1f; i += 0.025f)
        {
            deliveryProgressBarObj.fillAmount = i;
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.025f);

        sendResButton.SetActive(true);
        refuseButton.SetActive(true);
    }

    public GameObject SendMenuElements;
    public void ActivateSendMenu(int city)
    {
        // SendMenuUi.SetActive(true);
        ChangePosition(city);
        SendMenuElements.SetActive(cityRequsetName != null && cityRequsetName == cities[city].name);
        deliveryStatusObj.text = cities[city].name;
        notEnoughtRes.SetActive(false);
    }
    public void CloseMenu()
    {
        SendMenuUi.SetActive(false);
        icon.SetActive(false);
    }
    public void CloseNotification()
    {
        notificationObj.SetActive(false);
        activateFromNotif = true;
        ActivateSendMenu(cityID);
    }
    public GameObject ResoursesMenuObj;
    public GameObject ResMenuContButton;
    public Animator resoursesAnim;
    public void ResoursesMenuContoller()
    {
        ResoursesMenuObj.SetActive(!ResoursesMenuObj.activeSelf);
        // if(!ResoursesMenuObj.activeSelf) resoursesAnim.SetBool("IsResMenuActive", true);
        // else resoursesAnim.SetBool("IsResMenuActive", false);
        ResMenuContButton.SetActive(!ResMenuContButton.activeSelf);
    }

    private string offerRes = null;
    private int offerCount;
    private int offerID;
    public string[] resoursesName;
    public int[] profitScale;
    
    public Image progresLogo;
    public Sprite[] ResoursesSprites;
    private string profitNum;
    private int cityID = 2;
    private string cityRequsetName;
    void CreateRequest()
    {
        if(Random.Range(0,100) <= 20 && offerRes == null && timeAfterLastOffer >= 2  && timer >= 7)
        {
            notificationObj.SetActive(true);

            offerID = Random.Range(0, Mathf.Clamp(playerLevel - 1, 0, 7));
            if(Random.Range(0,100) < 20) offerID = Mathf.Clamp(playerLevel, 1, 7);
            cityID = Random.Range(0, cities.Length);
            offerRes = resoursesName[offerID];
            cityRequsetName = citiesNames[cityID];
            newOfferObj.text = $"New offer in {cities[cityID].name}!";
            offerCount = Convert.ToInt32(Random.Range(offerMinValue, offerLimit) / (offerID + 1) + 1);
            // if(playerLevel == 1 && offerID == 0) offerCount = Mathf.Clamp(offerCount, 7, offerLimit);

            transportInfoObj.text = offerRes;
            trInfoNumObj.text = offerCount.ToString();

            deliveryStatusObj.text = cities[cityID].name;
            ChangePosition(cityID);

            profitObj.text = "Profit";
            profitNum = (profitScale[offerID] * offerCount - 10).ToString();
            profitNumObj.text = profitNum;
        }
    }

    public GameObject notEnoughtRes;
    public void SendResourses()
    {
        // Debug.Log($"\n{offerCount} \n {resoursesCount[offerID]} \n {offerRes} \n {!deliveryActive}");
        if(!deliveryActive && offerRes != null)
        {
            if(resoursesCount[offerID] == 0 || offerCount > resoursesCount[offerID]) notEnoughtRes.SetActive(true);
            else StartCoroutine(TransportResourses(offerID));
        }
    }
    public void refuseOffer()
    {
        deliveryActive = false;
        offerRes = null;
        ClearUselessText();
        SendMenuUi.SetActive(false);
        notEnoughtRes.SetActive(false);
        notificationObj.SetActive(false);
    }

    IEnumerator TransportResourses(int index)
    {
        deliveryActive = true;

        sendResButton.SetActive(false);
        notEnoughtRes.SetActive(false);
        notificationObj.SetActive(false);
        refuseButton.SetActive(false);
        icon.SetActive(true);

        progresLogo.sprite = ResoursesSprites[index];

        // Debug.Log($"{offerCount}\n {profitScale[index]}\n {profitScale[index] * offerCount}");
        resoursesCount[index] -= offerCount;
        resCountObj[index].text = resoursesCount[index].ToString();

        deliveryEnd = Convert.ToInt32(Time.time + deliveryDuration);
        deliveryStatusObj.text = "Delivery in progress";
        septims -= 10; //снятие за повозку
        septimsObj.text = septims.ToString();

        // profitObj.text = "Profit";
        // profitNumObj.text = (profitScale[index] * offerCount - 10).ToString();
        // Debug.Log(profitScale[offerID] * offerCount - 10);
        
        transportInfoObj.text = offerRes;
        trInfoNumObj.text = offerCount.ToString();
        

        yield return new WaitForSeconds(deliveryDuration);

        
        septims += profitScale[index] * offerCount;
        septimsObj.text = septims.ToString();
        PlayerLevelProgress();
        timeAfterLastOffer = 0;
        offerRes = null;
    }

    public float levelPoints = 0f;
    public float expToGetNewLvl = 2f;
    public Text levelDisplayNum;
    public Image levelProgresBar;
    private int offerLimit = 32;
    private int offerMinValue = 7;
    void PlayerLevelProgress()
    {
        levelPoints += (float)offerCount / 50f;
        levelProgresBar.fillAmount = Mathf.Clamp01(levelPoints);
        if(levelPoints >= expToGetNewLvl) //Level Up
        {
            levelPoints -= expToGetNewLvl;
            playerLevel++;
            expToGetNewLvl = Mathf.Pow(expToGetNewLvl, 1.4f);
            
            offerLimit = Convert.ToInt32(Math.Pow(offerLimit, 1.1f));
            offerMinValue = Convert.ToInt32(Math.Pow(offerMinValue, 1.1f));

            levelDisplayNum.text = playerLevel.ToString();
            levelProgresBar.fillAmount = levelPoints;
        }
    }

    public class City
    {
        public string name {get;set;}
        public bool worldSide {get; set;}
        public int deliveryTime {get; set;}

        public City(string Name, bool WorldSide, int delTime)
        {
            name = Name;
            worldSide = WorldSide;
            deliveryTime = delTime;
        }
    }
    
    public City[] cities;
    public Transform[] citiesInfoUiSpawnpointObj;
    string[] citiesNames = 
    {
        "Solitude", "Morthal", "Markarth", "Falkreath", //west
        "Winterhold", "Windhelm", "Riften", "Dawnstar" //east
    };

    void addCities()
    {

        cities = new City[citiesNames.Length];
        
        for(int i = 0; i < citiesNames.Length; i++)
            cities[i] = new City(citiesNames[i], i >= 4, 5);
        cities[1].deliveryTime = 4;
        cities[2].deliveryTime = 6;
        cities[6].deliveryTime = 7;
    }


    public GameObject mainSendResMenu;
    private bool activateFromNotif = false;
    void ChangePosition(int index)
    {
        if(true || !SendMenuUi.activeSelf || activateFromNotif)
        {
            float posX = citiesInfoUiSpawnpointObj[index].position.x;
            float posY = citiesInfoUiSpawnpointObj[index].position.y;

            SendMenuUi.SetActive(false);
            mainSendResMenu.transform.position = new Vector2(posX, posY);
            SendMenuUi.SetActive(true);
            activateFromNotif = false;
        }
    }
}
