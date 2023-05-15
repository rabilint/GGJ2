using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Video;

public class CharactersController : MonoBehaviour
{
    public bool playerComeFromIntro;
    public GameObject textBg;
    public Text textObj;
    public GameObject ralof;
    public Animator ralofAnimation;
    public Animator reverseAnimation;
    public AudioSource GladYourHere;
    public string[] ralofStartPhrases;

    private int phraseIndex = 0;

    private bool typing = true;
    private bool dialogIsActive = true;

    public GameObject tipBg;
    public Text tipText;

    public GameObject wRun;
    public VideoPlayer wRunVid;
    public GameObject Balgruuf;
    public string[] BalgruufTxt;


    private void Start()
    {
        ralofAnimation.enabled = false;
        textObj.text = "";
        if(playerComeFromIntro) StartCoroutine(StartDialogue());
    }

    private IEnumerator StartDialogue()
    {
        ralof.SetActive(true);
        yield return new WaitForSeconds(1.8f);
        ralofAnimation.enabled = true;
        // ralofAnimation.SetTrigger("Slide");

        yield return new WaitForSeconds(ralofAnimation.GetCurrentAnimatorStateInfo(0).length);
        ralofAnimation.enabled = false;
        
        yield return new WaitForSeconds(0.7f);
        textBg.SetActive(true);
        GladYourHere.Play();

        StartCoroutine(Dialog(ralofStartPhrases, ralof));
    }

    IEnumerator Dialog(string[] phrases, GameObject characterWhoSpeak)
    {
        // Debug.Log("length: " + phrases.Length);
        Debug.Log(phrases);
        for(int i = 0; i < phrases.Length; i++)
        {
            StartCoroutine(TextPrinter(phrases[i], phrases.Length, characterWhoSpeak));
            while(!Input.GetMouseButtonDown(0) && !typing && dialogIsActive)
            {
                yield return null;
            }
            Debug.Log("+1");
        }
    }

    IEnumerator TextPrinter(string str, int iterationController, GameObject character)
    {
        Debug.Log("start printing");
        Debug.Log($"index - {phraseIndex}\n controller - {iterationController}");

        if(phraseIndex < iterationController)
        {
            Debug.Log("InputAct");
            typing = true;
            textObj.text = "";
            for(int j = 0; j < str.Length; j++)
            {
                textObj.text += str[j];
                
                if(str[j] == ',')
                    yield return new WaitForSeconds(0.25f);
                else 
                    yield return new WaitForSeconds(0.015f);
            }
            typing = false;
        } else {
            Debug.Log("else");

            character.SetActive(false);
            textBg.SetActive(false);

            StartCoroutine(ActivateTip());
        }
        phraseIndex++;
        Debug.Log($"phIndex: {phraseIndex}");
    }

    void TextInputer(string[] phrasesArray, GameObject character)
    {
        StartCoroutine(Dialog(phrasesArray, character));
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !typing && dialogIsActive)
        {
            typing = true;
            // StartCoroutine(TextPrinter());
        }
    }

    IEnumerator ActivateTip()
    {
        tipBg.SetActive(true);
        tipText.text = "";
        string tip = "Find and go to Whiterun";
        for(int i = 0; i < tip.Length; i++)
        {
            tipText.text += tip[i];
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(5);
        
        for(int i = 0; i < tip.Length; i++)
        {
            tipText.text = tip.Substring(0, tip.Length - i);
            yield return new WaitForSeconds(0.01f);
        }
        tipText.text = "";

        tipBg.SetActive(false);
    }

    public void WhiterunJourney()
    {
        StartCoroutine(WhJr());
    }

    IEnumerator WhJr()
    {
        wRun.SetActive(true);
        yield return new WaitForSeconds((float)wRunVid.length);
        wRun.SetActive(false);
        yield return new WaitForSeconds(0.3f);

            //BalgSpeach
        Balgruuf.SetActive(true);
        textBg.SetActive(true);
        textObj.text = "";

        yield return new WaitForSeconds(0.5f);
        phraseIndex = 0;
        
        // for(int i = 0; i < BalgruufTxt.Length; i++)
        // {
        //     textObj.text += BalgruufTxt[j][i];
        //     yield return new WaitForSeconds(0.015f);
        // }
        Debug.Log("THE END");
    }
}
