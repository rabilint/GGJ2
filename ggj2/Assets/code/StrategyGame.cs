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
    public GameObject notificationObj;
    public Text newOfferObj;

    public Text[] resCountObj;
    // public int[] resCount;
    public byte playerLevel = 1;


    private void Start()
    {
        // Debug.Log();
        startTime = new float[cost.Length];
        resoursesCount = new int[cost.Length];

        for(int i = 0; i < cost.Length; i++)
        {
            startTime[i] = Time.time;
            costObj[i].text = cost[i].ToString();
        }
        notificationObj.SetActive(false);

        // RiseLevel(0);
        level[0] = 1;
        septims = 100;
        septimsObj.text = "100";
    }

    public void RiseLevel(int index)
    {
        if (septims >= cost[index] && level[index] < 99)
        {
            if(level[index]==0) startTime[index] = Time.time;
            septims -= cost[index];

            level[index]++;

            // pricesPerIteration[index] = Convert.ToInt32(Math.Ceiling(pricesPerIteration[index] * Random.Range(1f, 1.35f)));

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
            float progress = (deliveryEnd - Time.time) / deliveryDuration;
            deliveryProgressBarObj.fillAmount = Mathf.Clamp01(progress);

            remainingTimeObj.text = "Left time";
            remTimeNumObj.text = (Math.Round(deliveryEnd - Time.time)).ToString();
            if(progress <= 0) 
            {
                deliveryActive = false;
                deliveryStatusObj.text = "Markarth";
                ClearUselessText();
                if(!sendResButton.activeSelf)
                {
                    transportInfoObj.text = "";
                    trInfoNumObj.text = "";
                    Debug.Log("clear txt");
                }
                StartCoroutine(FIllPrBar());
            }
        }
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
        icon.SetActive(false);
    }

    public void ActivateSendMenu(int city)
    {
        SendMenuUi.SetActive(true);
        ClearUselessText();
        // Debug.Log(city);
    }
    public void CloseMenu()
    {
        SendMenuUi.SetActive(false);
    }
    public void CloseNotification()
    {
        notificationObj.SetActive(false);
    }
    public GameObject ResoursesMenuObj;
    public GameObject ResMenuContButton;
    public void ResoursesMenuContoller()
    {
        ResoursesMenuObj.SetActive(!ResoursesMenuObj.activeSelf);
        ResMenuContButton.SetActive(!ResMenuContButton.activeSelf);
    }

    void ClearUselessText()
    {
        profitNumObj.text = "";
        profitObj.text = "";
        remTimeNumObj.text = "";
        remainingTimeObj.text = "";
    }

    private string offerRes = null;
    private int offerCount;
    private int offerID;
    public string[] resoursesName;
    public int[] profitScale;
    
    public Image progresLogo;
    public Sprite[] ResoursesSprites;
    void CreateRequest()
    {
        if(Random.Range(0,100) <= 20 && offerRes == null)// && timer >= 2 && timeAfterLastOffer >= 2)
        {
            notificationObj.SetActive(true);

            offerID = Random.Range(0, Mathf.Clamp(playerLevel + 1, 2, 7));
            offerRes = resoursesName[offerID];
            newOfferObj.text = "New offer in Markarth!";
            offerCount = Random.Range(1,26);

            transportInfoObj.text = offerRes;
            trInfoNumObj.text = offerCount.ToString();
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
        trInfoNumObj.text = "";
        transportInfoObj.text = "";
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
        icon.SetActive(true);

        progresLogo.sprite = ResoursesSprites[index];

        Debug.Log($"{offerCount}\n {profitScale[index]}\n {profitScale[index] * offerCount}");
        resoursesCount[index] -= offerCount;

        deliveryEnd = Convert.ToInt32(Time.time + deliveryDuration);
        deliveryStatusObj.text = "Delivery in progress";
        septims -= 10; //снятие за повозку
        septimsObj.text = septims.ToString();

        profitObj.text = "Profit";
        transportInfoObj.text = offerRes;
        trInfoNumObj.text = offerCount.ToString();
        

        yield return new WaitForSeconds(deliveryDuration);

        
        septims += profitScale[index] * offerCount;
        septimsObj.text = septims.ToString();
        timeAfterLastOffer = 0;
        offerRes = null;
    }
}
