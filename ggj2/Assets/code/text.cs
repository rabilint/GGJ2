using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class text : MonoBehaviour
{
    private bool printing = false;
    public Text textObj;
    private int textLength = 0;
    public string[] mainText;
    private int strIndex = 0;
    public float delay = 0.025f;

    private void Start()
    {
        printing = true;
        StartCoroutine(PrintText());
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !printing )
        {
            Debug.Log("Click");
            textObj.text = "";
            printing = true;
            strIndex++;

            StartCoroutine(PrintText());
        }
    }
    IEnumerator PrintText()
    {
        if (printing)
        {
            for (int i = 0; i < mainText[strIndex].Length; i++)
            {
                // textObj.text = mainText[strIndex].Substring(0, i + 1);
                textObj.text += mainText[strIndex][i];
                textLength++;

                if (mainText[strIndex][i] == ',')
                    yield return new WaitForSeconds(0.25f);
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
}

