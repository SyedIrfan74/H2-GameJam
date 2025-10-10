using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    #region Singleton
    public static DialogueManager instance;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(instance);
        else instance = this;
    }
    #endregion

    [Header("Intro Screen")]
    public GameObject characterSelection;
    public GameObject nameInput;
    public TMP_InputField nameInputField;

    [Header("Dialogue Parts")]
    public List<DialogueSO> dialogueSOs = new List<DialogueSO>();
    public TMP_Text dialogueText;
    public TMP_Text nameText;
    public GameObject dialogueGO;
    public Image characterImage;

    [Header("Character Sprites")]
    public Sprite maleCharacterSprite;
    public Sprite femaleCharacterSprite;

    [Header("Player Details")]
    private string playerName;
    private Sprite characterSprite;

    [Header("Misc Variables")]
    public InputActionAsset inputActionAsset;
    public InputAction clickAction;
    public InputAction touchAction;

    private AudioData currPlaying;

    [Header("Misc Variables")]
    private int currentDialogue;
    private bool pickingCharacter;
    private bool pickingName;
    private bool running;
    private bool manualStart;
    private float timer = 0;
    private string dialogueWName;
    private string MCNAME = "[MC Name]";
    private bool waiting;

    public void StartManager()
    {
        clickAction = inputActionAsset.FindAction("Click");
        touchAction = inputActionAsset.FindAction("Touch");
        clickAction.Enable();
        touchAction.Enable();

        currentDialogue = 0;
        pickingCharacter = false;
        pickingName = false;
        running = false;
        dialogueWName = "";
        waiting = false;
        dialogueGO.SetActive(false);

        if (characterSelection != null) characterSelection.SetActive(false);
        if (nameInput != null) nameInput.SetActive(false);

        foreach (DialogueSO SO in dialogueSOs)
        {
            if (SO.flags.reset) SO.ResetVariables(); 
        }
    }

    public void UpdateManager()
    {
        if (pickingCharacter || pickingName) return;

        dialogueGO.SetActive(true);

        //if running == true, dialogue is currently being written on the screen
        if (running)
        {
            //Interrupts dialogue writing to skip
            if (Application.isMobilePlatform)
            {
                if (!touchAction.WasPressedThisFrame()) return;
            }
            else if (!Application.isMobilePlatform)
            {
                if (!clickAction.WasPressedThisFrame()) return;
            }

            StopAllCoroutines();
            if (dialogueWName == "") dialogueText.text = dialogueSOs[currentDialogue].dialogue;
            else dialogueText.text = dialogueWName;

            dialogueWName = "";
            running = false;
            currentDialogue++;
            return;
        }

        //Starts next Dialogue to be shown
        if (!running && !waiting)
        {
            if (Application.isMobilePlatform)
            {
                if ((touchAction.WasPressedThisFrame() || manualStart) && currentDialogue < dialogueSOs.Count)
                {
                    Debug.Log("MOBILE LAH FUCK");
                    if (dialogueSOs[currentDialogue].dialogue.Contains(MCNAME)) dialogueWName = InsertName(dialogueSOs[currentDialogue].dialogue);
                    StartCoroutine(DisplayText(dialogueSOs[currentDialogue]));
                    return;
                }
            }
            else if (!Application.isMobilePlatform)
            {
                if ((clickAction.WasPressedThisFrame() || manualStart) && currentDialogue < dialogueSOs.Count)
                {
                    Debug.Log("NOT MOBILE");
                    if (dialogueSOs[currentDialogue].dialogue.Contains(MCNAME)) dialogueWName = InsertName(dialogueSOs[currentDialogue].dialogue);
                    StartCoroutine(DisplayText(dialogueSOs[currentDialogue]));
                    return;
                }
            }
        }

        //Debug.Log(Mathf.Clamp(currentDialogue - 1, 0, dialogueSOs.Count));

        if (dialogueSOs[Mathf.Clamp(currentDialogue - 1, 0, dialogueSOs.Count)].flags.changeScreen && ScreenManager.instance.currScreen.screenName != dialogueSOs[Mathf.Clamp(currentDialogue - 1, 0, dialogueSOs.Count)].nextScreen)
        {
            waiting = true;

            if (running) return;
            timer += Time.deltaTime;

            if (timer >= 2)
            {
                timer = 0;
                dialogueText.text = "";
                nameText.text = "";

                if (dialogueSOs[currentDialogue - 1].nextState == StateManager.GAMESTATE.GAME)
                {
                    waiting = false;
                    AudioManager.instance.StopAudio(currPlaying);
                    MinigameManager.instance.StartMinigame2(dialogueSOs[currentDialogue - 1].nextScreen);
                    StateManager.instance.ChangeState(StateManager.GAMESTATE.TRANSITION);
                    ScreenManager.instance.FindScreen(MinigameManager.instance.currentMinigame);
                    ScreenManager.instance.SetNextState(dialogueSOs[currentDialogue - 1].nextState);
                    dialogueGO.SetActive(false);
                    return;
                }

                waiting = false;
                StateManager.instance.ChangeState(StateManager.GAMESTATE.TRANSITION);
                ScreenManager.instance.FindScreen(dialogueSOs[currentDialogue - 1].nextScreen);
                ScreenManager.instance.SetNextState(dialogueSOs[currentDialogue - 1].nextState);
                dialogueGO.SetActive(false);
            }
        }
        else if (dialogueSOs[Mathf.Clamp(currentDialogue - 1, 0, dialogueSOs.Count)].flags.selectCharacter && pickingCharacter == false)
        {
            StartCoroutine(PickCharacter());
        }
        else if (dialogueSOs[Mathf.Clamp(currentDialogue - 1, 0, dialogueSOs.Count)].flags.inputName && pickingName == false)
        {
            StartCoroutine(PickName());
        }
        else if (dialogueSOs[Mathf.Clamp(currentDialogue - 1, 0, dialogueSOs.Count)].flags.scribbleJournal)
        {
            StateManager.instance.ChangeState(StateManager.GAMESTATE.TRANSITION);
            ScreenManager.instance.journal = true;
            dialogueSOs[Mathf.Clamp(currentDialogue - 1, 0, dialogueSOs.Count)].flags.scribbleJournal = false;
        }

        //COLLECTABLES
        else if (dialogueSOs[Mathf.Clamp(currentDialogue - 1, 0, dialogueSOs.Count)].flags.countryEraser)
        {
            StateManager.instance.ChangeState(StateManager.GAMESTATE.TRANSITION);
            ScreenManager.instance.countryEraser = true;
            dialogueSOs[Mathf.Clamp(currentDialogue - 1, 0, dialogueSOs.Count)].flags.countryEraser = false;
        }
        else if (dialogueSOs[Mathf.Clamp(currentDialogue - 1, 0, dialogueSOs.Count)].flags.stamp)
        {
            StateManager.instance.ChangeState(StateManager.GAMESTATE.TRANSITION);
            ScreenManager.instance.stamp = true;
            dialogueSOs[Mathf.Clamp(currentDialogue - 1, 0, dialogueSOs.Count)].flags.stamp = false;
        }
        else if (dialogueSOs[Mathf.Clamp(currentDialogue - 1, 0, dialogueSOs.Count)].flags.sticker)
        {
            StateManager.instance.ChangeState(StateManager.GAMESTATE.TRANSITION);
            ScreenManager.instance.sticker = true;
            dialogueSOs[Mathf.Clamp(currentDialogue - 1, 0, dialogueSOs.Count)].flags.sticker = false;
        }

        //TUTORIAL SCREENS
        else if (dialogueSOs[Mathf.Clamp(currentDialogue - 1, 0, dialogueSOs.Count)].flags.chopsticksTutorial)
        {
            StateManager.instance.ChangeState(StateManager.GAMESTATE.TRANSITION);
            ScreenManager.instance.chopsticksTutorial = true;
            dialogueSOs[Mathf.Clamp(currentDialogue - 1, 0, dialogueSOs.Count)].flags.chopsticksTutorial = false;
        }
        else if (dialogueSOs[Mathf.Clamp(currentDialogue - 1, 0, dialogueSOs.Count)].flags.chaTutorial)
        {
            StateManager.instance.ChangeState(StateManager.GAMESTATE.TRANSITION);
            ScreenManager.instance.chaTutorial = true;
            dialogueSOs[Mathf.Clamp(currentDialogue - 1, 0, dialogueSOs.Count)].flags.chaTutorial = false;
        }

        //END OF DAY
        else if (dialogueSOs[Mathf.Clamp(currentDialogue - 1, 0, dialogueSOs.Count)].flags.endDayOne)
        {
            StateManager.instance.ChangeState(StateManager.GAMESTATE.TRANSITION);
            ScreenManager.instance.endDayOne = true;
            dialogueSOs[Mathf.Clamp(currentDialogue - 1, 0, dialogueSOs.Count)].flags.endDayOne = false;
            AudioManager.instance.StopAudio(currPlaying);
        }
        else if (dialogueSOs[Mathf.Clamp(currentDialogue - 1, 0, dialogueSOs.Count)].flags.endDayTwo)
        {
            StateManager.instance.ChangeState(StateManager.GAMESTATE.TRANSITION);
            ScreenManager.instance.endDayTwo = true;
            dialogueSOs[Mathf.Clamp(currentDialogue - 1, 0, dialogueSOs.Count)].flags.endDayTwo = false;
            AudioManager.instance.StopAudio(currPlaying);
        }
        else if (dialogueSOs[Mathf.Clamp(currentDialogue - 1, 0, dialogueSOs.Count)].flags.endDayThree)
        {
            StateManager.instance.ChangeState(StateManager.GAMESTATE.TRANSITION);
            ScreenManager.instance.endDayThree = true;
            dialogueSOs[Mathf.Clamp(currentDialogue - 1, 0, dialogueSOs.Count)].flags.endDayThree = false;
            AudioManager.instance.StopAudio(currPlaying);
        }
    }

    private IEnumerator PickCharacter()
    {
        pickingCharacter = true;
        float elapsed = 0;
        float duration = 2;
        Color initial = ScreenManager.instance.currScreen.fadeBlack.color;
        Color man = new Color(ScreenManager.instance.currScreen.fadeBlack.color.r, ScreenManager.instance.currScreen.fadeBlack.color.g, ScreenManager.instance.currScreen.fadeBlack.color.b, 0.8f);

        //Darken BG
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            ScreenManager.instance.currScreen.fadeBlack.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        dialogueGO.SetActive(false);
        characterSelection.SetActive(true);
        dialogueSOs[Mathf.Clamp(currentDialogue - 1, 0, dialogueSOs.Count)].flags.selectCharacter = false;
    }
    public void PickCharAfter()
    {
        StartCoroutine(PickCharacterAfter());
    }
    private IEnumerator PickCharacterAfter()
    {
        float elapsed = 0;
        float duration = 2;
        Color initial = ScreenManager.instance.currScreen.fadeBlack.color;
        Color man = new Color(ScreenManager.instance.currScreen.fadeBlack.color.r, ScreenManager.instance.currScreen.fadeBlack.color.g, ScreenManager.instance.currScreen.fadeBlack.color.b, 0f);

        //Darken BG
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            ScreenManager.instance.currScreen.fadeBlack.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        dialogueGO.SetActive(true);
        characterSelection.SetActive(false);
        pickingCharacter = false;
    }
    private IEnumerator PickName()
    {
        pickingName = true;
        float elapsed = 0;
        float duration = 2;
        Color initial = ScreenManager.instance.currScreen.fadeBlack.color;
        Color man = new Color(ScreenManager.instance.currScreen.fadeBlack.color.r, ScreenManager.instance.currScreen.fadeBlack.color.g, ScreenManager.instance.currScreen.fadeBlack.color.b, 0.8f);

        //Darken BG
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            ScreenManager.instance.currScreen.fadeBlack.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        dialogueGO.SetActive(false);
        nameInput.SetActive(true);
        dialogueSOs[Mathf.Clamp(currentDialogue - 1, 0, dialogueSOs.Count)].flags.inputName = false;
    }
    public void PickNameAfter()
    {
        StartCoroutine(PickNameAft());
    }
    private IEnumerator PickNameAft()
    {
        float elapsed = 0;
        float duration = 2;
        Color initial = ScreenManager.instance.currScreen.fadeBlack.color;
        Color man = new Color(ScreenManager.instance.currScreen.fadeBlack.color.r, ScreenManager.instance.currScreen.fadeBlack.color.g, ScreenManager.instance.currScreen.fadeBlack.color.b, 0f);

        //Lighten BG
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            ScreenManager.instance.currScreen.fadeBlack.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        dialogueGO.SetActive(true);
        nameInput.SetActive(false);
        pickingName = false;
    }
    private IEnumerator DisplayText(DialogueSO so)
    {
        if (manualStart) manualStart = false;
        running = true;
        int count = 0;
        string newDialogue = "";
        dialogueText.text = "";
        if (so.characterName == MCNAME) nameText.text = playerName;
        else if (so.characterName == "") nameText.text = "";
        else nameText.text = so.characterName;

        if ((so.audio != null && so.audio != currPlaying) || so.audio == currPlaying) 
        { 
            AudioManager.instance.StopAudio(currPlaying);
            AudioManager.instance.PlayAudio(so.audio); 
            currPlaying = so.audio;  
        }

        if (so.characterSprite != null)
        {
            if (so.characterName != "") characterImage.sprite = so.characterSprite;
            characterImage.color = new Color(1, 1, 1, 1);
        }
        else
        {
            if (so.characterName == MCNAME)
            {
                characterImage.sprite = characterSprite;
                characterImage.color = new Color(1, 1, 1, 1);
            }
            else
            {
                characterImage.sprite = null;
                characterImage.color = new Color(1, 1, 1, 0);
            }
        }

        if (so.dialogue.Contains(MCNAME)) newDialogue = InsertName(so.dialogue);

        int stringlen = 0;

        if (newDialogue != "") stringlen = newDialogue.Length;
        else stringlen = so.dialogue.Length;

        while (count < stringlen)
        {
            if (newDialogue != "")
            {
                dialogueText.text += newDialogue[count];
                count++;
            }
            else
            {
                dialogueText.text += so.dialogue[count];
                count++;
            }

            yield return new WaitForSeconds(so.delay);
        }

        currentDialogue++;
        running = false;

        yield return null;
    }
    public void SetCharacter(int index)
    {
        if (index == 0) characterSprite = maleCharacterSprite;
        else characterSprite = femaleCharacterSprite;
    }
    public void SetName()
    {
        playerName = nameInputField.text;
        nameInput.SetActive(false);
        pickingName = false;
    }
    public void ManualStart()
    {
        manualStart = true;
    }
    private string InsertName(string dialogue)
    {
        int index = dialogue.IndexOf('[');
        string b4 = dialogue.Substring(0, index - 1);
        string aft = dialogue.Substring(index + MCNAME.Length, dialogue.Length - MCNAME.Length - b4.Length - 1);
        return b4 + " " + playerName + aft;
    }
}


//if (!Input.GetMouseButtonDown(0)) return;
//if (!clickAction.WasPressedThisFrame() || !touchAction.WasPressedThisFrame()) return;

//if ((Input.GetMouseButtonDown(0) || manualStart) && currentDialogue < dialogueSOs.Count)
//{
//    if (dialogueSOs[currentDialogue].dialogue.Contains(MCNAME)) dialogueWName = InsertName(dialogueSOs[currentDialogue].dialogue);
//    StartCoroutine(DisplayText(dialogueSOs[currentDialogue]));
//    return;
//}
//if ((clickAction.WasPressedThisFrame() || touchAction.WasPressedThisFrame() || manualStart) && currentDialogue < dialogueSOs.Count)
//{
//    if (dialogueSOs[currentDialogue].dialogue.Contains(MCNAME)) dialogueWName = InsertName(dialogueSOs[currentDialogue].dialogue);
//    StartCoroutine(DisplayText(dialogueSOs[currentDialogue]));
//    return;
//}

//yield return new WaitForSeconds(1);
//dialogueSOs[Mathf.Clamp(currentDialogue - 1, 0, dialogueSOs.Count)].flags.selectCharacter = false;


//nameInput.SetActive(true);
//pickingName = true;
//dialogueSOs[Mathf.Clamp(currentDialogue - 1, 0, dialogueSOs.Count)].flags.inputName = false;
////dialogueGO.SetActive(false);

//characterSelection.SetActive(true);
//pickingCharacter = true;
//dialogueSOs[Mathf.Clamp(currentDialogue - 1, 0, dialogueSOs.Count)].flags.selectCharacter = false;
////dialogueGO.SetActive(false);

//else if (dialogueSOs[Mathf.Clamp(currentDialogue - 1, 0, dialogueSOs.Count)].flags.getJournal)
//{
//    journal.SetActive(true);
//}

//else if (dialogueSOs[Mathf.Clamp(currentDialogue - 1, 0, dialogueSOs.Count)].flags.changeState)
//{
//    //UNUSED
//}


//string temp = "[MC Name]";
//int index = so.dialogue.IndexOf('[');
//string b4 = so.dialogue.Substring(0, index - 1);
//string aft = so.dialogue.Substring(index + temp.Length, so.dialogue.Length - temp.Length - b4.Length - 1);
//newDialogue = b4 + " " + playerName + aft;


//else if (running == true)
//{
//    //Interrupts dialogue writing to skip
//    if (!Input.GetMouseButtonDown(0)) return;

//    StopAllCoroutines();
//    if (dialogueWName == "") dialogueText.text = dialogueSOs[currentDialogue].dialogue;
//    else dialogueText.text = dialogueWName;


//    dialogueWName = "";
//    running = false;
//    currentDialogue++;
//}


//if (currentDialogue == 0) return;

