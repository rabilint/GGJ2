using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class text : MonoBehaviour
{
    private bool printing = false;
    public Text textObj;
    private int textLength = 0;
    public string[] mainText;
    private int strIndex = 0;
    public float delay = 0.025f;
    public float pause = 0.08f;

    private bool intr = true;

    private void Start()
    {
        if(intr)
        {
            printing = true;
            StartCoroutine(PrintText());
            intr = false;
        }
    }
    public GameObject[] characters;
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !printing )
        {
            textObj.text = "";
            printing = true;
            strIndex++;

            if(strIndex < mainText.Length)
                StartCoroutine(PrintText());

            if(strIndex >= mainText.Length)
            {
                instructionIsActive = false;
                characters[0].SetActive(false);
                characters[2].SetActive(false);
            }
        }
    }
    public static bool instructionIsActive = true;
    IEnumerator PrintText()
    {
        if (printing)
        {
            if(strIndex == 0) yield return new WaitForSeconds(1f);
            for (int i = 0; i < mainText[strIndex].Length; i++)
            {
                // textObj.text = mainText[strIndex].Substring(0, i + 1);
                textObj.text += mainText[strIndex][i];
                textLength++;

                if (mainText[strIndex][i] == ',')
                    yield return new WaitForSeconds(0.25f);
                else if(mainText[strIndex][i] == '.' || mainText[strIndex][i] == '!')
                    yield return new WaitForSeconds(pause);
                else
                    yield return new WaitForSeconds(delay);
            }
            textObj.text += "\n";
            printing = false;
        }
        else
        {
            //ждать клика...
        }
    }
    public GameObject runInWhiteRunObj;
    public VideoPlayer runInWhiteRun;

    public void GoToWhiteRun()
    {
        if(!instructionIsActive)
        {
            Debug.Log("Run to Whiterun");
            StartCoroutine(RunToWhiterun());
        }
    }
    
    public GameObject whButton;
    IEnumerator RunToWhiterun()
    {
        runInWhiteRun.Play();

        yield return new WaitForSeconds((float)runInWhiteRun.length);
        
        whButton.SetActive(false);
        runInWhiteRunObj.SetActive(false);

        
        StartCoroutine(BalgrufMonolog());
    }

    public GameObject strategyGameObj;
    IEnumerator BalgrufMonolog()
    {
        characters[1].SetActive(true);
        characters[2].SetActive(true);
        string BalgText = "It's good that you're here. Whiterun lacks a skilled delivery man";
        for(int i = 0; i < BalgText.Length; i++)
        {
            textObj.text = BalgText.Substring(0, i+1);
            
            if (BalgText[i] == '.')
                yield return new WaitForSeconds(1f);
            else
                yield return new WaitForSeconds(0.015f);
        }
        yield return new WaitForSeconds(3.5f);

        characters[1].SetActive(false);
        characters[2].SetActive(false);
        characters[3].SetActive(false);

        strategyGameObj.SetActive(true);
    }
}

