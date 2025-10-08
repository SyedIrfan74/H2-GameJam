using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UI;

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
    public StateManager.GAMESTATE nextState;

    public bool fadeOut;
    public bool fadeIn;
    public float a = 0;
    public float transitionPause = 2;
    public float transition1Speed = 2;
    public float transition2Speed = 2;
    public bool journal;
    public bool transitioning;
    public Image bookClosed;
    public Image bookOpened;
    public Image bookWriting;

    //Edits by: Irfan
    public void StartManager()
    {
        foreach(Screen screen in screenList) 
        { 
            screen.OnStart(); 
            screenTransforms.Add(screen.transform);

            if (screen.screenName.Contains("Main Menu")) currScreen = screen;
        }

        ChangeScreen(currentScreen);
        //Change to new funcs later ^
        targetScreen = null;
        fadeOut = false;
        fadeIn = false;
        journal = false;
        transitioning = false;
        nextState = StateManager.GAMESTATE.NOSTATE;
    }

    public void UpdateManager()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeScreen("Cha");
        if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeScreen("Chopsticks");

        if (currScreen != null) Debug.Log(currScreen.screenName);
        if (targetScreen != null) Debug.Log(targetScreen.screenName);

        if (nextState != StateManager.GAMESTATE.NOSTATE) Scr2ScrTransition();

        if (journal && !transitioning)
        {
            StartCoroutine(RevealJournal());
        }
    }

    /// <summary>
    /// Changes screen based on string str
    /// </summary>
    /// <param name="str">Name of screen</param>
    public void ChangeScreen(string str)
    {
        for (int i = 0; i < screenList.Count; i++)
        {
            //Debug.Log(screenList[i].name);
            // found the right screen
            if (screenList[i].name.Contains(str))
            {
                //Debug.Log("FUCKU");
                Camera.main.transform.position = new Vector3(screenList[i].transform.position.x,
                                                             screenList[i].transform.position.y,
                                                             Camera.main.transform.position.z);
                screenList[i].canvas.gameObject.SetActive(true);
            }
            // everything else
            else
            {
                //Debug.Log("mannnnnn");
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
    public void SetNextState(StateManager.GAMESTATE gameState)
    {
        nextState = gameState;
    }

    public void SetNextState2(int gameState)
    {
        nextState = (StateManager.GAMESTATE)gameState;
    }

    public void MoveCamera()
    {
        Camera.main.transform.position = new Vector3(currScreen.transform.position.x, currScreen.transform.position.y, Camera.main.transform.position.z);
    }
    private IEnumerator RevealJournal()
    {
        float elapsed = 0;
        float duration = 2;
        Color initial = bookClosed.color;
        Color man = new Color(bookClosed.color.r, bookClosed.color.g, bookClosed.color.b, 1);
        transitioning = true;

        while (elapsed < duration)
        {
            Debug.Log("IUHAUVBEFCUGVTICAVGUACVUGJHK");
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            bookClosed.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        yield break;
    }

    private void Scr2ScrTransition()
    {
        currScreen.canvas.gameObject.SetActive(false);

        if (currScreen.fadeBlack.color.a < 1 && fadeOut == false)
        {
            a = Mathf.Lerp(a, 1, Time.deltaTime * transition1Speed);
            currScreen.fadeBlack.color = new Color(currScreen.fadeBlack.color.r, currScreen.fadeBlack.color.g, currScreen.fadeBlack.color.b, a);

            if (a > 0.99f)
            {
                a = 1;
                currScreen.fadeBlack.color = new Color(currScreen.fadeBlack.color.r, currScreen.fadeBlack.color.g, currScreen.fadeBlack.color.b, a);
            }

            return;
        }

        fadeOut = true;
        if (targetScreen != null)
        {
            targetScreen.fadeBlack.color = new Color(targetScreen.fadeBlack.color.r, targetScreen.fadeBlack.color.g, targetScreen.fadeBlack.color.b, 1);
            currScreen = targetScreen;
            MoveCamera();
        }

        targetScreen = null;

        if (transitionPause > 0)
        {
            transitionPause -= Time.deltaTime;
            return;
        }


        if (currScreen.fadeBlack.color.a > 0 && fadeIn == false)
        {
            a = Mathf.Lerp(a, 0, Time.deltaTime * transition2Speed);
            currScreen.fadeBlack.color = new Color(currScreen.fadeBlack.color.r, currScreen.fadeBlack.color.g, currScreen.fadeBlack.color.b, a);

            if (a < 0.1f)
            {
                a = 0;
                currScreen.fadeBlack.color = new Color(currScreen.fadeBlack.color.r, currScreen.fadeBlack.color.g, currScreen.fadeBlack.color.b, a);
            }

            return;
        }

        a = 0;
        fadeIn = false;
        fadeOut = false;
        targetScreen = null;

        currScreen.canvas.gameObject.SetActive(true);
        StateManager.instance.ChangeState(nextState);
        if (nextState == StateManager.GAMESTATE.CONVO) DialogueManager.instance.ManualStart();
        nextState = StateManager.GAMESTATE.NOSTATE;
        return;
    }

    //public void ChangeScene(string str)
    //{
    //    //If no screen found, exit early
    //    if (!FindScreen(str))
    //    {
    //        Debug.Log("No Screen Found!");
    //        return;
    //    }

    //    Debug.Log("Screen Found");
    //    StateManager.instance.ChangeState(StateManager.GAMESTATE.TRANSITION);

    //    //StartCoroutine(FadeToBlack(currScreen));
    //    currScreen = targetScreen;
    //    Camera.main.transform.position = new Vector3(currScreen.transform.position.x, currScreen.transform.position.y, Camera.main.transform.position.z);
    //    //StartCoroutine(FadeOutBlack(currScreen));
    //}

    //private IEnumerator FadeToBlack(Screen screen)
    //{
    //    float a = 0;
    //    while (screen.fadeBlack.color.a != 1)
    //    {
    //        a = Mathf.Lerp(a, 1, Time.deltaTime);
    //        screen.fadeBlack.color = new Color(screen.fadeBlack.color.r, screen.fadeBlack.color.g, screen.fadeBlack.color.b, a);
    //    }

    //    yield return null;
    //}

    //private IEnumerator FadeOutBlack(Screen screen)
    //{
    //    float a = 1;
    //    while (screen.fadeBlack.color.a != 0)
    //    {
    //        a = Mathf.Lerp(a, 0, Time.deltaTime);
    //        screen.fadeBlack.color = new Color(screen.fadeBlack.color.r, screen.fadeBlack.color.g, screen.fadeBlack.color.b, a);
    //    }

    //    yield return null;
    //}
    //Edits by: Irfan
}
