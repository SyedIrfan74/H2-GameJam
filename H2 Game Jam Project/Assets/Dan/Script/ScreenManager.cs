using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
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
    public bool countryEraser;
    public bool stamp;
    public bool sticker;
    public bool endDayOne;
    public bool endDayTwo;
    public bool endDayThree;
    public bool transitioning;

    public bool chopsticksTutorial;
    public bool chaTutorial;

    public Image bookClosed;
    public Image bookOpened;
    public Image bookWriting;

    public Image bookWritingEndDayOne;
    public Image bookEndDayOne;

    public Image bookWritingEndDayTwo;
    public Image bookEndDayTwo;

    public Image bookWritingEndDayThree;
    public Image bookEndDayThree;

    public Image countryEraserImage;
    public Image stampImage;
    public Image stickerImage;

    public Image chopsticksTutorialImage;
    public Image chaTutorialImage;

    public GameObject dialogueGO;

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
        //if (currScreen != null) Debug.Log(currScreen.screenName);
        //if (targetScreen != null) Debug.Log(targetScreen.screenName);

        if (nextState != StateManager.GAMESTATE.NOSTATE && !transitioning) StartCoroutine(ScreenTransition());

        if (journal && !transitioning) StartCoroutine(RevealJournal());
        if (countryEraser && !transitioning) StartCoroutine(RevealCountryEraser());
        if (stamp && !transitioning) StartCoroutine(RevealStamp());
        if (sticker && !transitioning) StartCoroutine(RevealSticker());

        if (endDayOne && !transitioning) StartCoroutine(EndDayOne());
        if (endDayTwo && !transitioning) StartCoroutine(EndDayTwo());
        if (endDayThree && !transitioning) StartCoroutine(EndDayThree());

        if (chopsticksTutorial && !transitioning) StartCoroutine(RevealChopsticks());
        if (chaTutorial && !transitioning) StartCoroutine(RevealCha());
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
    public void FinishBobbyChopsticks()
    {
        StateManager.instance.ChangeState(StateManager.GAMESTATE.TRANSITION);
        ScreenManager.instance.FindScreen("Canteen");
        ScreenManager.instance.SetNextState(StateManager.GAMESTATE.CONVO);
    }
    private IEnumerator EndDayOne()
    {
        transitioning = true;

        float elapsed = 0;
        float duration = 2;
        Color initial = currScreen.fadeBlack.color;
        Color man = new Color(currScreen.fadeBlack.color.r, currScreen.fadeBlack.color.g, currScreen.fadeBlack.color.b, 1);

        //Darken BG
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            currScreen.fadeBlack.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        dialogueGO.SetActive(false);

        elapsed = 0;
        initial = bookEndDayOne.color;
        man = new Color(bookEndDayOne.color.r, bookEndDayOne.color.g, bookEndDayOne.color.b, 1);

        //Reveal Book 
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            bookEndDayOne.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        elapsed = 0;
        initial = bookWritingEndDayOne.color;
        man = new Color(bookWritingEndDayOne.color.r, bookWritingEndDayOne.color.g, bookWritingEndDayOne.color.b, 1);

        //Reveal writing
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            bookWritingEndDayOne.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        yield return new WaitForSeconds(1);

        elapsed = 0;
        initial = bookWritingEndDayOne.color;
        man = new Color(bookWritingEndDayOne.color.r, bookWritingEndDayOne.color.g, bookWritingEndDayOne.color.b, 0);

        Color initial2 = bookEndDayOne.color;
        Color man2 = new Color(bookEndDayOne.color.r, bookEndDayOne.color.g, bookEndDayOne.color.b, 0);

        //Fade out      
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            bookWritingEndDayOne.color = Color.Lerp(initial, man, t);
            bookEndDayOne.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        endDayOne = false;
        transitioning = false;

        StateManager.instance.ChangeState(StateManager.GAMESTATE.TRANSITION);
        nextState = StateManager.GAMESTATE.CONVO;
        ScreenManager.instance.FindScreen("Canteen");
        yield break;
    }
    private IEnumerator EndDayTwo()
    {
        transitioning = true;

        float elapsed = 0;
        float duration = 2;
        Color initial = currScreen.fadeBlack.color;
        Color man = new Color(currScreen.fadeBlack.color.r, currScreen.fadeBlack.color.g, currScreen.fadeBlack.color.b, 1);

        //Darken BG
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            currScreen.fadeBlack.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        dialogueGO.SetActive(false);

        elapsed = 0;
        initial = bookEndDayTwo.color;
        man = new Color(bookEndDayTwo.color.r, bookEndDayTwo.color.g, bookEndDayTwo.color.b, 1);

        //Reveal Book 
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            bookEndDayTwo.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        elapsed = 0;
        initial = bookWritingEndDayTwo.color;
        man = new Color(bookWritingEndDayTwo.color.r, bookWritingEndDayTwo.color.g, bookWritingEndDayTwo.color.b, 1);

        //Reveal writing
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            bookWritingEndDayTwo.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        yield return new WaitForSeconds(1);

        elapsed = 0;
        initial = bookWritingEndDayTwo.color;
        man = new Color(bookWritingEndDayTwo.color.r, bookWritingEndDayTwo.color.g, bookWritingEndDayTwo.color.b, 0);

        Color initial2 = bookEndDayTwo.color;
        Color man2 = new Color(bookEndDayTwo.color.r, bookEndDayTwo.color.g, bookEndDayTwo.color.b, 0);

        //Reveal writing      
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            bookWritingEndDayTwo.color = Color.Lerp(initial, man, t);
            bookEndDayTwo.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        endDayTwo = false;
        transitioning = false;

        StateManager.instance.ChangeState(StateManager.GAMESTATE.TRANSITION);
        nextState = StateManager.GAMESTATE.CONVO;
        ScreenManager.instance.FindScreen("Canteen");
        yield break;
    }
    private IEnumerator EndDayThree()
    {
        transitioning = true;

        float elapsed = 0;
        float duration = 2;
        Color initial = currScreen.fadeBlack.color;
        Color man = new Color(currScreen.fadeBlack.color.r, currScreen.fadeBlack.color.g, currScreen.fadeBlack.color.b, 1);

        //Darken BG
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            currScreen.fadeBlack.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        dialogueGO.SetActive(false);

        elapsed = 0;
        initial = bookEndDayThree.color;
        man = new Color(bookEndDayThree.color.r, bookEndDayThree.color.g, bookEndDayThree.color.b, 1);

        //Reveal Book 
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            bookEndDayThree.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        elapsed = 0;
        initial = bookWritingEndDayThree.color;
        man = new Color(bookWritingEndDayThree.color.r, bookWritingEndDayThree.color.g, bookWritingEndDayThree.color.b, 1);

        //Reveal writing
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            bookWritingEndDayThree.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        yield return new WaitForSeconds(1);

        elapsed = 0;
        initial = bookWritingEndDayThree.color;
        man = new Color(bookWritingEndDayThree.color.r, bookWritingEndDayThree.color.g, bookWritingEndDayThree.color.b, 0);

        Color initial2 = bookEndDayThree.color;
        Color man2 = new Color(bookEndDayThree.color.r, bookEndDayThree.color.g, bookEndDayThree.color.b, 0);

        //Reveal writing      
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            bookWritingEndDayThree.color = Color.Lerp(initial, man, t);
            bookEndDayThree.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        endDayThree = false;
        transitioning = false;

        //StartCoroutine(Ending());
        yield break;
    }
    private IEnumerator RevealJournal()
    {
        transitioning = true;

        float elapsed = 0;
        float duration = 2;
        Color initial = currScreen.fadeBlack.color;
        Color man = new Color(currScreen.fadeBlack.color.r, currScreen.fadeBlack.color.g, currScreen.fadeBlack.color.b, .9f);

        //Darken Background
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            currScreen.fadeBlack.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        dialogueGO.SetActive(false);

        elapsed = 0;
        initial = bookClosed.color;
        man = new Color(bookClosed.color.r, bookClosed.color.g, bookClosed.color.b, 1);

        //closed cover fade in
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            bookClosed.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        elapsed = 0;
        initial = bookOpened.color;
        man = new Color(bookOpened.color.r, bookOpened.color.g, bookOpened.color.b, 1);

        Color initial2 = bookClosed.color;
        Color man2 = new Color(bookClosed.color.r, bookClosed.color.g, bookClosed.color.b, 0);
        
        //Open fade in + Closed Fade Out
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            bookOpened.color = Color.Lerp(initial, man, t);
            bookClosed.color = Color.Lerp(initial2, man2, t);
            yield return null;
        }

        elapsed = 0;
        initial = bookWriting.color;
        man = new Color(bookWriting.color.r, bookWriting.color.g, bookWriting.color.b, 1);

        //Writing Fade In
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            bookWriting.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        elapsed = 0;
        initial = bookWriting.color;
        man = new Color(bookWriting.color.r, bookWriting.color.g, bookWriting.color.b, 0);

        initial2 = bookOpened.color;
        man2 = new Color(bookOpened.color.r, bookOpened.color.g, bookOpened.color.b, 0);

        Color initial3 = currScreen.fadeBlack.color;
        Color man3 = new Color(currScreen.fadeBlack.color.r, currScreen.fadeBlack.color.g, currScreen.fadeBlack.color.b, 0.0f);

        //Everything Fade Out
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            bookWriting.color = Color.Lerp(initial, man, t);
            bookOpened.color = Color.Lerp(initial2, man2, t);
            currScreen.fadeBlack.color = Color.Lerp(initial3, man3, t);
            yield return null;
        }

        yield return new WaitForSeconds(1);

        journal = false;
        transitioning = false;

        dialogueGO.SetActive(true);
        StateManager.instance.ChangeState(StateManager.GAMESTATE.CONVO);
        DialogueManager.instance.ManualStart();
        nextState = StateManager.GAMESTATE.NOSTATE;

        yield break;
    }
    private IEnumerator ScreenTransition()
    {
        transitioning = true;
        currScreen.canvas.gameObject.SetActive(false);

        float elapsed = 0;
        float duration = 2;
        Color initial = currScreen.fadeBlack.color;
        Color man = new Color(currScreen.fadeBlack.color.r, currScreen.fadeBlack.color.g, currScreen.fadeBlack.color.b, 1);

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            currScreen.fadeBlack.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        targetScreen.fadeBlack.color = new Color(targetScreen.fadeBlack.color.r, targetScreen.fadeBlack.color.g, targetScreen.fadeBlack.color.b, 1);
        currScreen = targetScreen;
        MoveCamera();

        elapsed = 0;
        initial = currScreen.fadeBlack.color;
        man = new Color(currScreen.fadeBlack.color.r, currScreen.fadeBlack.color.g, currScreen.fadeBlack.color.b, 0);

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            currScreen.fadeBlack.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        transitioning = false;

        targetScreen = null;

        currScreen.canvas.gameObject.SetActive(true);
        if (nextState == StateManager.GAMESTATE.CONVO) DialogueManager.instance.ManualStart();
        StateManager.instance.ChangeState(nextState);
        nextState = StateManager.GAMESTATE.NOSTATE;

        yield break;
    }
    private IEnumerator RevealCountryEraser()
    {
        transitioning = true;

        float elapsed = 0;
        float duration = 2;
        Color initial = currScreen.fadeBlack.color;
        Color man = new Color(currScreen.fadeBlack.color.r, currScreen.fadeBlack.color.g, currScreen.fadeBlack.color.b, .9f);

        //Darken Background
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            currScreen.fadeBlack.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        elapsed = 0;
        initial = countryEraserImage.color;
        man = new Color(countryEraserImage.color.r, countryEraserImage.color.g, countryEraserImage.color.b, 1);

        //Reveal Country Eraser
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            countryEraserImage.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        elapsed = 0;
        initial = countryEraserImage.color;
        man = new Color(countryEraserImage.color.r, countryEraserImage.color.g, countryEraserImage.color.b, 0);

        //Hide Country Eraser
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            countryEraserImage.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        elapsed = 0;
        initial = currScreen.fadeBlack.color;
        man = new Color(currScreen.fadeBlack.color.r, currScreen.fadeBlack.color.g, currScreen.fadeBlack.color.b, 0);

        //Brighten Background
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            currScreen.fadeBlack.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        transitioning = false;
        countryEraser = false;

        StateManager.instance.ChangeState(StateManager.GAMESTATE.CONVO);
        DialogueManager.instance.ManualStart();
        nextState = StateManager.GAMESTATE.NOSTATE;

        yield break;
    }
    private IEnumerator RevealStamp()
    {
        transitioning = true;

        float elapsed = 0;
        float duration = 2;
        Color initial = currScreen.fadeBlack.color;
        Color man = new Color(currScreen.fadeBlack.color.r, currScreen.fadeBlack.color.g, currScreen.fadeBlack.color.b, .9f);

        //Darken Background
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            currScreen.fadeBlack.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        elapsed = 0;
        initial = stampImage.color;
        man = new Color(stampImage.color.r, stampImage.color.g, stampImage.color.b, 1);

        //Reveal Country Eraser
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            stampImage.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        elapsed = 0;
        initial = stampImage.color;
        man = new Color(stampImage.color.r, stampImage.color.g, stampImage.color.b, 0);

        //Hide Country Eraser
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            stampImage.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        elapsed = 0;
        initial = currScreen.fadeBlack.color;
        man = new Color(currScreen.fadeBlack.color.r, currScreen.fadeBlack.color.g, currScreen.fadeBlack.color.b, 0);

        //Brighten Background
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            currScreen.fadeBlack.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        transitioning = false;
        stamp = false;

        StateManager.instance.ChangeState(StateManager.GAMESTATE.CONVO);
        DialogueManager.instance.ManualStart();
        nextState = StateManager.GAMESTATE.NOSTATE;

        yield break;
    }
    private IEnumerator RevealSticker()
    {
        transitioning = true;

        float elapsed = 0;
        float duration = 2;
        Color initial = currScreen.fadeBlack.color;
        Color man = new Color(currScreen.fadeBlack.color.r, currScreen.fadeBlack.color.g, currScreen.fadeBlack.color.b, .9f);

        //Darken Background
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            currScreen.fadeBlack.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        elapsed = 0;
        initial = stickerImage.color;
        man = new Color(stickerImage.color.r, stickerImage.color.g, stickerImage.color.b, 1);

        //Reveal Country Eraser
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            stickerImage.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        elapsed = 0;
        initial = stickerImage.color;
        man = new Color(stickerImage.color.r, stickerImage.color.g, stickerImage.color.b, 0);

        //Hide Country Eraser
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            stickerImage.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        elapsed = 0;
        initial = currScreen.fadeBlack.color;
        man = new Color(currScreen.fadeBlack.color.r, currScreen.fadeBlack.color.g, currScreen.fadeBlack.color.b, 0);

        //Brighten Background
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            currScreen.fadeBlack.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        transitioning = false;
        sticker = false;

        StateManager.instance.ChangeState(StateManager.GAMESTATE.CONVO);
        DialogueManager.instance.ManualStart();
        nextState = StateManager.GAMESTATE.NOSTATE;

        yield break;
    }
    private IEnumerator RevealChopsticks()
    {
        Debug.Log("Chopsticks COROUTINE");
        transitioning = true;

        float elapsed = 0;
        float duration = 2;
        Color initial = currScreen.fadeBlack.color;
        Color man = new Color(currScreen.fadeBlack.color.r, currScreen.fadeBlack.color.g, currScreen.fadeBlack.color.b, .9f);

        //Darken Background
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            currScreen.fadeBlack.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        elapsed = 0;
        initial = chopsticksTutorialImage.color;
        man = new Color(chopsticksTutorialImage.color.r, chopsticksTutorialImage.color.g, chopsticksTutorialImage.color.b, 1);

        //Reveal Country Eraser
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            chopsticksTutorialImage.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        elapsed = 0;
        initial = chopsticksTutorialImage.color;
        man = new Color(chopsticksTutorialImage.color.r, chopsticksTutorialImage.color.g, chopsticksTutorialImage.color.b, 0);

        //Hide Country Eraser
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            chopsticksTutorialImage.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        elapsed = 0;
        initial = currScreen.fadeBlack.color;
        man = new Color(currScreen.fadeBlack.color.r, currScreen.fadeBlack.color.g, currScreen.fadeBlack.color.b, 0);

        //Brighten Background
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            currScreen.fadeBlack.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        transitioning = false;
        chopsticksTutorial = false;

        StateManager.instance.ChangeState(StateManager.GAMESTATE.CONVO);
        DialogueManager.instance.ManualStart();
        nextState = StateManager.GAMESTATE.NOSTATE;

        yield break;
    }
    private IEnumerator RevealCha()
    {
        Debug.Log("Cha COROUTINE");
        transitioning = true;

        float elapsed = 0;
        float duration = 2;
        Color initial = currScreen.fadeBlack.color;
        Color man = new Color(currScreen.fadeBlack.color.r, currScreen.fadeBlack.color.g, currScreen.fadeBlack.color.b, .9f);

        //Darken Background
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            currScreen.fadeBlack.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        elapsed = 0;
        initial = chaTutorialImage.color;
        man = new Color(chaTutorialImage.color.r, chaTutorialImage.color.g, chaTutorialImage.color.b, 1);

        //Reveal Country Eraser
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            chaTutorialImage.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        elapsed = 0;
        initial = chaTutorialImage.color;
        man = new Color(chaTutorialImage.color.r, chaTutorialImage.color.g, chaTutorialImage.color.b, 0);

        //Hide Country Eraser
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            chaTutorialImage.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        elapsed = 0;
        initial = currScreen.fadeBlack.color;
        man = new Color(currScreen.fadeBlack.color.r, currScreen.fadeBlack.color.g, currScreen.fadeBlack.color.b, 0);

        //Brighten Background
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            currScreen.fadeBlack.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        transitioning = false;
        chaTutorial = false;

        StateManager.instance.ChangeState(StateManager.GAMESTATE.CONVO);
        DialogueManager.instance.ManualStart();
        nextState = StateManager.GAMESTATE.NOSTATE;

        yield break;
    }
    //Edits by: Irfan
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

//if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeScreen("Cha");
//if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeScreen("Chopsticks");


//private void Scr2ScrTransition()
//{
//    currScreen.canvas.gameObject.SetActive(false);

//    if (currScreen.fadeBlack.color.a < 1 && fadeOut == false)
//    {
//        a = Mathf.Lerp(a, 1, Time.deltaTime * transition1Speed);
//        currScreen.fadeBlack.color = new Color(currScreen.fadeBlack.color.r, currScreen.fadeBlack.color.g, currScreen.fadeBlack.color.b, a);

//        if (a > 0.99f)
//        {
//            a = 1;
//            currScreen.fadeBlack.color = new Color(currScreen.fadeBlack.color.r, currScreen.fadeBlack.color.g, currScreen.fadeBlack.color.b, a);
//        }

//        return;
//    }

//    fadeOut = true;
//    if (targetScreen != null)
//    {
//        targetScreen.fadeBlack.color = new Color(targetScreen.fadeBlack.color.r, targetScreen.fadeBlack.color.g, targetScreen.fadeBlack.color.b, 1);
//        currScreen = targetScreen;
//        MoveCamera();
//    }

//    targetScreen = null;

//    if (transitionPause > 0)
//    {
//        transitionPause -= Time.deltaTime;
//        return;
//    }


//    if (currScreen.fadeBlack.color.a > 0 && fadeIn == false)
//    {
//        a = Mathf.Lerp(a, 0, Time.deltaTime * transition2Speed);
//        currScreen.fadeBlack.color = new Color(currScreen.fadeBlack.color.r, currScreen.fadeBlack.color.g, currScreen.fadeBlack.color.b, a);

//        if (a < 0.1f)
//        {
//            a = 0;
//            currScreen.fadeBlack.color = new Color(currScreen.fadeBlack.color.r, currScreen.fadeBlack.color.g, currScreen.fadeBlack.color.b, a);
//        }

//        return;
//    }

//    a = 0;
//    fadeIn = false;
//    fadeOut = false;
//    targetScreen = null;

//    currScreen.canvas.gameObject.SetActive(true);
//    StateManager.instance.ChangeState(nextState);
//    if (nextState == StateManager.GAMESTATE.CONVO) DialogueManager.instance.ManualStart();
//    nextState = StateManager.GAMESTATE.NOSTATE;
//    return;
//}