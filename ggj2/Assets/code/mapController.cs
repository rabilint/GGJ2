using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mapController : MonoBehaviour
{
    public static string[] citiesName = {"Solitude", "Marthal", "Dawnstar"};
    // public int pathToCities = {"0", "7", "15"}; 
    // public GameObject[] citiesButton;
    public GameObject StartButton;
    public static int cityIndex;

    public static int money = 50;
    public Text moneyDisplay;
    
    void Start()
    {
        StartButton.SetActive(false);
        moneyDisplay.text = money.ToString();
    }

    public void ChooseCityToJourney(int index)
    {
        Debug.Log("We go to " + index);
        StartButton.SetActive(true);
        cityIndex = index;

        //activate other UI
    }

    public void StartJourney()
    {
        //activate some UI;
        money -= 10;
        SceneManager.LoadScene("RunGame");
    }
}
