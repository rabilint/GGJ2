using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.Collections;
using UnityEngine.Video;

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
    // private int deliveryEnd;
    // public int deliveryDuration = 6;

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
        delActive = new bool[cost.Length];

        JarlPower = Random.Range(48, 67);
        totalPowerObj.text = totalPower.ToString();
        workerPowerObj.text = "0";
        priceObj.text = "</>";
        shopUi.SetActive(false);
        militaryPower.text = $"Military Power: {JarlPower}";

        addCities();
        // ChangePosition(5);

        for(int i = 0; i < cost.Length; i++)
        {
            startTime[i] = Time.time;
            delActive[i] = false;
            costObj[i].text = cost[i].ToString();
        }
        notificationObj.SetActive(false);

        // RiseLevel(0);
        level[0] = 1;
        resoursesLevel[0].text = "1";
        septims = 20;
        septimsObj.text = "20";
    }

    public int[] upgradeCost;
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

            upgradeCost[index] += Convert.ToInt32(Math.Ceiling(level[index] * 1.2f * 1.1f));


            // if (level[index] % 10 == 0) pricesPerIteration[index] += Convert.ToInt32(Math.Ceiling(pricesPerIteration[index] * Random.Range(1f, 2f)));
            septimsObj.text = septims.ToString();
            resoursesLevel[index].text = (level[index]).ToString();
            // resoursesProfit[index].text = pricesPerIteration[index].ToString();
            costObj[index].text = upgradeCost[index].ToString();
        }
    }

    private int timer = 0;
    private void FixedUpdate()
    {

        if(Time.time >= timer)
        {
            if(Random.Range(0,100) < 40)
                CreateRequest(Random.Range(0, 8));
            timer++;
        }

        // if(timer % 30 == 0) JarlPower++;
        ResourceProduction();
    }

    private void ResourceProduction()
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
    /* public void ActivateSendMenu(int city)
    {
        // SendMenuUi.SetActive(true);
        cityID = city;
        ChangePosition(city);
        SendMenuElements.SetActive(cities[city].offerResName != null); //(cityRequsetName != null && cityRequsetName == cities[city].name);
        
        transportInfoObj.text = cities[city].offerResName;
        trInfoNumObj.text = cities[city].offerResCount.ToString();
        profitNumObj.text = cities[city].offerResProfit;
        profitObj.text = "Profit";

        deliveryStatusObj.text = cities[city].name;
        notEnoughtRes.SetActive(false);
    } */
    public void CloseMenu()
    {
        SendMenuUi.SetActive(false);
        icon.SetActive(false);
    }
    public void CloseNotification()
    {
        notificationObj.SetActive(false);
        activateFromNotif = true;
        Debug.Log(G_indexOfChosenCity);
        ActivateSendMenu(G_indexOfChosenCity);
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
        activateShopUi.SetActive(!activateShopUi.activeSelf);
    }
    public void ShopMenuController()
    {
        shopUi.SetActive(!shopUi.activeSelf);
        activateShopUi.SetActive(!activateShopUi.activeSelf);
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
    /* void CreateRequest()
    {
        cityID = Random.Range(0, cities.Length);
        // Debug.Log(cityID);
        // Debug.Log(cities[cityID].offerResName == null);
        int rnd = Random.Range(0,100);
        Debug.Log(rnd);
        if(rnd <= 20 && cities[cityID].offerResName == null && timeAfterLastOffer >= 1  && Time.time >= 1f)// && !text.instructionIsActive)
        {
            notificationObj.SetActive(true);

            offerID = Random.Range(0, Mathf.Clamp(playerLevel - 1, 0, 7));
            if(Random.Range(0,100) < 15) offerID = Mathf.Clamp(playerLevel, 1, 7);
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

            cities[cityID].offerResName = offerRes;
            cities[cityID].offerResCount = offerCount;
            cities[cityID].offerResProfit = profitNum;
        }
    } */

    public class City
    {
        public string name{get; set;}
        public bool haveOffer{get; set;}
        public string offerResName{get; set;}
        public int offerResCount{get; set;}
        public int offerResProfit{get; set;}

        public float deliveryDuration {get; set;}
        public float deliveryEnd {get; set;}
        public bool delIsActive {get; set;}

        public City(string Name, string resName = "", int resCount = -1, int resProfit = -1, bool offerStatus = false, float delDur = 5f, float endDel = 0f, bool delStatus = false)
        {
            name = Name;
            offerResName = resName;
            offerResCount = resCount;
            offerResProfit = resProfit;
            haveOffer = offerStatus;

            deliveryDuration = delDur;
            deliveryEnd = endDel;
            delIsActive = delStatus;
        } 
    }

    void PrBarFillAmount(int cityId)
    {
        float progress = Mathf.Clamp01((cities[cityId].deliveryEnd - Time.time) / cities[cityId].deliveryDuration);
        deliveryProgressBarObj.fillAmount = progress;
        remTimeNumObj.text = (Math.Round(cities[cityId].deliveryEnd - Time.time)).ToString();

        if(progress == 1f) remainingTimeObj.text = "Left time";
        if(progress == 1f) Debug.Log("Start Delivety"); //
                    
        if(progress <= 0) //end of delivery
        {
            Debug.Log("progress <= 0 /299");
            G_indexOfChosenCity = -1;
            delActive[cityId] = false;
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

    void Update()
    {
        
        // if(cities[??].haveOffer) FIllPrBar()
        // if(G_indexOfChosenCity != -1) PrBarFillAmount(G_indexOfChosenCity);

        for (int i = 0; i < cost.Length; i++)
        {
            if(delActive[i])
                PrBarFillAmount(i);
        }
    }
    
    public Text notifTextObj;
    void CreateRequest(int cityId)
    {
        if(!cities[cityId].haveOffer)
        {
            //write values
            int res = Mathf.Clamp(Random.Range(0, playerLevel), 0, 6);
            cities[cityId].offerResName = resoursesName[res];
            cities[cityId].offerResCount = Random.Range(3, 31);
            cities[cityId].offerResProfit = cities[cityId].offerResCount * profitScale[res];
            cities[cityId].haveOffer = true;

            //Call Notification
            G_indexOfChosenCity = cityId;         
            
            notificationObj.SetActive(true);
            notifTextObj.text = $"New offer in {citiesNames[cityId]}";
        }
    }

    public void ActivateSendMenu(int cityId)
    {
        ChangePosition(cityId);
        SendMenuUi.SetActive(false);
        SendMenuUi.SetActive(true);

        title_obj.text = citiesNames[cityId];

        ChangeTextInSendMenuInfo(cityId);
        //FIllPrBar();
    }

    [Header("Offer UI")]
    public Text title_obj;
    public Text offerResName_obj;
    public Text offerResCount_obj;
    public Text offerResProfit_obj;
    public Text offerProfitObj;

    void ChangeTextInSendMenuInfo(int cityId)
    {
        if(cities[cityId].haveOffer)
        {
                //change text in game
            offerResName_obj.text = cities[cityId].offerResName;
            offerResCount_obj.text = cities[cityId].offerResCount.ToString();
            offerResProfit_obj.text = cities[cityId].offerResProfit.ToString();
            offerProfitObj.text = "Profit";
            title_obj.text = citiesNames[cityId];
                G_indexOfChosenCity = cityId; //run progressbar filling in Update()
        } else {
            Debug.Log("_379");
            G_indexOfChosenCity = -1;
            deliveryProgressBarObj.fillAmount = 1f;
        }
    }
    private int G_indexOfChosenCity = -1;
    private bool[] delActive;
    
    public void SendResourses_2()
    {
        StartCoroutine(sssssss(G_indexOfChosenCity));
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
        cities[cityID].offerResName = null;
        ClearUselessText();
        SendMenuUi.SetActive(false);
        notEnoughtRes.SetActive(false);
        notificationObj.SetActive(false);
    }

    IEnumerator sssssss(int index)
    {
        // cityId = G_indexOfChosenCity;
        Debug.Log($"{index} \n{resoursesCount[index]} \n{resoursesCount[index]} \n{!delActive[index]}");
        if(resoursesCount[index] <= cities[index].offerResCount && !delActive[index])
        {
            // cities[cityId].delIsActive = true; //run progress bar filling in Update()
            // resoursesCount[cityId] -= cities[cityId].offerResCount;
            // //prBa filling
            // yield return new WaitForSeconds(cities[cityId].deliveryDuration);
            // Debug.Log("end of delivery");
            // cities[cityId].delIsActive = true;
            deliveryActive = true;
            progresLogo.sprite = ResoursesSprites[2];
            sendResButton.SetActive(false);
            notEnoughtRes.SetActive(false);
            notificationObj.SetActive(false);
            refuseButton.SetActive(false);
            icon.SetActive(true);


            cities[index].offerResCount -= offerCount;
            resCountObj[index].text = cities[index].offerResCount.ToString();

            cities[index].deliveryEnd = Convert.ToInt32(Time.time + cities[index].deliveryDuration);
            deliveryStatusObj.text = "Delivery in progress";

            // profitObj.text = "Profit";
            // profitNumObj.text = (profitScale[index] * offerCount - 10).ToString();
            // Debug.Log(profitScale[offerID] * offerCount - 10);
            
            transportInfoObj.text = cities[index].offerResName;
            trInfoNumObj.text = offerCount.ToString();
            

            septims -= 10; //снятие за повозку
            septimsObj.text = septims.ToString();
            yield return new WaitForSeconds(cities[index].deliveryDuration);

            Debug.Log($"spetim: {profitScale[index]}");
            septims += profitScale[index] * offerCount;
            septimsObj.text = septims.ToString();
            PlayerLevelProgress();
        } else notEnoughtRes.SetActive(true);
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
        cities[index].offerResCount -= cities[index].offerResCount;
        resCountObj[index].text = cities[index].offerResCount.ToString();

        cities[index].deliveryEnd = Convert.ToInt32(Time.time + cities[index].deliveryDuration);
        deliveryStatusObj.text = "Delivery in progress";
        septims -= 10; //снятие за повозку
        septimsObj.text = septims.ToString();

        // profitObj.text = "Profit";
        // profitNumObj.text = (profitScale[index] * offerCount - 10).ToString();
        // Debug.Log(profitScale[offerID] * offerCount - 10);
        
        transportInfoObj.text = cities[index].offerResName;
        trInfoNumObj.text = offerCount.ToString();
        

        yield return new WaitForSeconds(cities[index].deliveryDuration);

        Debug.Log($"septim: {profitScale[index]}");
        septims += profitScale[index] * cities[index].offerResCount;
        septimsObj.text = septims.ToString();
        PlayerLevelProgress();
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
            cities[i] = new City(citiesNames[i]);
        cities[1].deliveryDuration = 4;
        cities[2].deliveryDuration = 7;
        cities[6].deliveryDuration = 8;
    }


    public GameObject mainSendResMenu;
    private bool activateFromNotif = false;
    void ChangePosition(int index)
    {
        if(true || !SendMenuUi.activeSelf || activateFromNotif)
        {
            float posX = citiesInfoUiSpawnpointObj[index].position.x;
            float posY = citiesInfoUiSpawnpointObj[index].position.y;

            mainSendResMenu.transform.position = new Vector2(posX, posY);

            /* if(SendMenuUi.activeSelf)
            {
                SendMenuUi.SetActive(false);
                mainSendResMenu.transform.position = new Vector2(posX, posY);
                SendMenuUi.SetActive(true);
            } else 
                mainSendResMenu.transform.position = new Vector2(posX, posY); */
            
            activateFromNotif = false;
        }
    }

    public GameObject[] knightBg;
    private int workerIndex;
    public void chooseKnight(int index)
    {
        workerIndex = index;
                
        priceObj.text = workerPrices[workerIndex].ToString();
        workerPowerObj.text = $"+{workerPower[workerIndex]}";

        for(int i = 0; i < knightBg.Length; i++)
        {
            if(i == index)
            {
                knightBg[i].SetActive(!knightBg[i].activeSelf);
                if(knightBg[i].activeSelf == false)
                {
                    workerPowerObj.text = "0";
                    workerIndex = -1;
                }
            }
            else knightBg[i].SetActive(false);
        }
    }

    public Text priceObj;
    public int[] workerPrices;
    public int[] workerPower;
    public Text workerPowerObj;
    private int totalPower = 0;
    public Text totalPowerObj;
    
    public GameObject shopUi;
    public GameObject activateShopUi;
    

    public void Employ()
    {
        if(septims >= workerPrices[workerIndex] && workerIndex != -1)
        {
            septims-= workerPrices[workerIndex];
            septimsObj.text = septims.ToString();

            for(int i = 0; i < knightBg.Length; i++)
                knightBg[i].SetActive(false);
            totalPower += workerPower[workerIndex];
            totalPowerObj.text = totalPower.ToString();

            priceObj.text = "</>";
            workerPowerObj.text = "0";
            workerIndex = -1;
        }
    }

    [Space][Header("fight elements")]
    public GameObject cityInfoObj;
    // public VideoPlayer runInWhiteRun;

    public void WhInfoController()
    {
        // StartCoroutine(RunToWhiteRun());

        cityInfoObj.SetActive(!cityInfoObj.activeSelf);
        if(cityInfoObj.activeSelf == true)
        {
            battleIsActive = false;
            fightProgresBar.fillAmount = 0;
            battleStatus.text = "";
            endBatlleTextObj.text = "";
        }
    }

    /* public GameObject runInWhiteRunObj;
    IEnumerator RunToWhiteRun()
    {
        runInWhiteRunObj.SetActive(true);
        runInWhiteRun.Play();

        yield return new WaitForSeconds((float)runInWhiteRun.length);
        Destroy(runInWhiteRunObj);
        runInWhiteRunObj.SetActive(false);
    } */

    public float JarlPower;
    public void Fight(int JarlId)
    {
        if(playerWhiterunJarl)
        {
            endBatlleTextObj.fontSize = 44;
            StartCoroutine(SimpleTextOutputer("Throw yourself put of power?"));
        } else
        if(!battleIsActive)
        {
            battleIsActive = true;
            CloseCityInfoButton.SetActive(false);
            if(Mathf.Pow(JarlPower, (float)Random.Range(103, 109) / 100) <= totalPower)
                StartCoroutine(Battle(true));
            else StartCoroutine(Battle(false));
        }
    }

    public Image fightProgresBar;
    public GameObject BalgrufSprite;
    public GameObject DovakginSprite;
    public Text whJarlObj;
    public GameObject battleStatusObj;
    public Text battleStatus;
    private bool battleIsActive = false;
    public Text militaryPower;
    IEnumerator Battle(bool playerWin)
    {
        int battleDuration = 4;//Random.Range(3, 10);
        float fightStartTime = Time.time;

        while(fightProgresBar.fillAmount < 1f)
        {
            yield return new WaitForSeconds(0.007f);
            fightProgresBar.fillAmount = (Time.time - fightStartTime) / battleDuration;
        }
        
        if(playerWin)
        {
            BalgrufSprite.SetActive(false);
            whJarlObj.text = "Jarl: Shadow Fiend";
            playerWhiterunJarl = true;

            totalPower -=  Convert.ToInt32(JarlPower * 0.8f);
            totalPowerObj.text = totalPower.ToString();
            battleStatus.text = "Victory";
            battleStatusObj.SetActive(true);
            yield return new WaitForSeconds(delayyy);

            StartCoroutine(SimpleTextOutputer("Whiterun has a new Jarl!"));

            ShowNewJarlButton.SetActive(true);
        } else {
            battleStatus.text = "Loss";
            battleStatusObj.SetActive(true);
            StartCoroutine(SimpleTextOutputer("Jarl remained in power"));
            JarlPower -= Convert.ToInt32(totalPower * 0.8f);
            totalPower = 0;
            totalPowerObj.text = "0";
        }
        CloseCityInfoButton.SetActive(true);
    }
    IEnumerator SimpleTextOutputer(string str)
    {
        for(int i = 0; i < str.Length; i++)
        {
            endBatlleTextObj.text = str.Substring(0, i + 1);
            yield return new WaitForSeconds(0.015f);
        }
    }
    public Text endBatlleTextObj;
    public GameObject ShowNewJarlButton;
    public float delayyy = 1.4f;
    public GameObject CloseCityInfoButton;
    private bool playerWhiterunJarl = false;
    public void ShowNewJarl()
    {
        // ShowNewJarlButton.SetActive(false);
        DovakginSprite.SetActive(true);
    }
    public void CloseCityInfo()
    {
        cityInfoObj.SetActive(false);
    }

    public void MoneyCheat()
    {
        septims += Random.Range(300, 600);
        septimsObj.text = septims.ToString();
    }
}
