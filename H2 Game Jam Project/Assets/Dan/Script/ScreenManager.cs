using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    #region Singleton
    public static ScreenManager instance;
    private void Awake()
    {
        if (instance != null && instance != this) Destroy(instance);
        else instance = this;
    }
    #endregion
    public List<Screen> screenList = new List<Screen>();
    public List<Transform> screenTransforms = new List<Transform>();
    public string currentScreen = "Main Menu";


    //Edits by: Irfan
    public Screen targetScreen;
    public Screen currScreen;
    //Edits by: Irfan
    public void StartManager()
    {
       foreach(Screen screen in screenList) 
       { 
            screen.OnStart(); 
            screenTransforms.Add(screen.transform);
       }

       ChangeScreen(currentScreen);
    }

    public void UpdateManager()
    {

    }

    /// <summary>
    /// Changes screen based on string str
    /// </summary>
    /// <param name="str">Name of screen</param>
    public void ChangeScreen(string str)
    {
        for (int i = 0; i < screenList.Count; i++)
        {
            Debug.Log(screenList[i].name);
            // found the right screen
            if (screenList[i].name.Contains(str))
            {
                Debug.Log("FUCKU");
                Camera.main.transform.position = new Vector3(screenList[i].transform.position.x,
                                                             screenList[i].transform.position.y,
                                                             Camera.main.transform.position.z);
                screenList[i].canvas.gameObject.SetActive(true);
            }
            // everything else
            else
            {
                Debug.Log("mannnnnn");
                screenList[i].canvas.gameObject.SetActive(false);
            }
        }      
        
        currentScreen = str;
    }


    //Edits by: Irfan
    public void FindScreen(string str)
    {
        for (int i = 0; i < screenList.Count; i++)
        {
            // found the right screen
            if (!screenList[i].name.Contains(str)) continue;
            
            targetScreen = screenList[i];
            return;
        }
    }

    public void ChangeScene(string str)
    {
        FindScreen(str);
        StartCoroutine(FadeToBlack(currScreen));

    }

    private IEnumerator FadeToBlack(Screen screen)
    {
        float a = 0;
        while (screen.fadeBlack.color.a != 1)
        {
            a = Mathf.Lerp(a, 1, Time.deltaTime);
            screen.fadeBlack.color = new Color(screen.fadeBlack.color.r, screen.fadeBlack.color.g, screen.fadeBlack.color.b, a);
        }

        StartCoroutine(FadeOutBlack(screen));

        yield return null;
    }

    private IEnumerator FadeOutBlack(Screen screen)
    {
        float a = 0;
        while (screen.fadeBlack.color.a != 1)
        {
            a = Mathf.Lerp(a, 1, Time.deltaTime);
            screen.fadeBlack.color = new Color(screen.fadeBlack.color.r, screen.fadeBlack.color.g, screen.fadeBlack.color.b, a);
        }

        yield return null;
    }
    //Edits by: Irfan
}
