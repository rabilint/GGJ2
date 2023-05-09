using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mapController : MonoBehaviour
{
    public static string[] citiesName = {"Solitude", "Marthal", "Dawnstar", "Whiterun", "Winterhold", "Windhelm", "Falkreath", "Markarth", "Riften"};
    // public static var allCities = new Dictionary<string, int
    // public int pathToCities = {"0", "7", "15"}; 
    // public GameObject[] citiesButton;
    public GameObject StartButton;
    public static int cityIndex;

    public static int money = 50;
    public Text moneyDisplay;
    public GameObject mapSettingsInterface;
    
    void Start()
    {
        money = PlayerPrefs.GetInt("totalMoney");

        StartButton.SetActive(false);
        moneyDisplay.text = money.ToString();
        Debug.Log(money);
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

    public void SettingsInMap()
    {
        mapSettingsInterface.SetActive(!mapSettingsInterface.activeSelf);
    }

    public void ToMianMenu()
    {
        SceneManager.LoadScene("MianMenu");
    }

    void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            SettingsInMap();

        if (Input.GetKeyDown(KeyCode.F11))
            FullscreenMode();

        moneyDisplay.text = money.ToString();
    }

    public void FullscreenMode()
    {
        if(Screen.fullScreen)
            Screen.SetResolution(1280, 720, false);
        else 
            Screen.SetResolution(1920, 1080, true);
    }

    public void RickRolled()
    {
        Application.OpenURL("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
    }
}
