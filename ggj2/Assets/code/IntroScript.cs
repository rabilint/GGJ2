using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroScript : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject videoCanvas;
    public GameObject textIntroduction;
    
    [Space]
    public string[] mainText;
    private bool printing = false;
    public Text textObj;
    public float delay = 0.025f;

    private float startTime;
    private int strIndex = 0;

    public AudioSource skyrimAwake;
    private bool VideoSceneIsActive;

    private int textLength = 0;

    private void Start()
    {
        videoPlayer.loopPointReached += EndReached;
        VideoSceneIsActive = true;
        skyrimAwake.Play();

        // printing = true;
        // textObj.text = "";
        // StartCoroutine(PrintText());
        // VideoSceneIsActive = false;
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        // SceneManager.LoadScene("Map");
        videoCanvas.SetActive(false);
        textIntroduction.SetActive(true);
        VideoSceneIsActive = false;
        
        printing = true;
        textObj.text = "";
        StartCoroutine(PrintText());
    }

    IEnumerator PrintText()
    {
        if(printing)
        {
            for(int i = 0; i < mainText[strIndex].Length; i++)
            {
                // textObj.text = mainText[strIndex].Substring(0, i + 1);
                textObj.text += mainText[strIndex][i];
                textLength++;

                if(mainText[strIndex][i] == ',')
                    yield return new WaitForSeconds(0.25f);
                else 
                    yield return new WaitForSeconds(delay);
            }
            textObj.text += "\n";
            printing = false;
        } else {
            //ждать клика...
        }
    }
    
    void Update()
    {
        if(strIndex == mainText.Length && Input.GetMouseButtonDown(0))
        {
            strIndex += strIndex;
            StartCoroutine(DeleteText());
        }

        if(Input.GetMouseButtonDown(0) && !printing && !VideoSceneIsActive)
        {
            Debug.Log("Click");
            
            printing = true;
            strIndex++;

            StartCoroutine(PrintText());
        }

        if (Input.GetKeyDown(KeyCode.F11))
        {
            if(Screen.fullScreen)
                Screen.SetResolution(1280, 720, false);
            else 
                Screen.SetResolution(1920, 1080, true);
        }
    }

    IEnumerator DeleteText()
    {
        for(int i = 0; i < textLength; i++)
        {
            textObj.text = textObj.text.Substring(0, textLength - i);
            yield return new WaitForSeconds(0.001f);
        }
        textObj.text = "";

        SceneManager.LoadScene("Map");
    }
}
