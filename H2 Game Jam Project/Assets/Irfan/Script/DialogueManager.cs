using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public GameObject journal;

    [Header("Dialogue Parts")]
    public List<DialogueSO> dialogueSOs = new List<DialogueSO>();
    public TMP_Text dialogueText;
    public TMP_Text nameText;

    [Header("Character Sprites")]
    public Sprite maleCharacterSprite;
    public Sprite femaleCharacterSprite;

    [Header("Player Details")]
    private string playerName;
    private Sprite characterSprite;

    [Header("Misc Variables")]
    private int currentDialogue;
    private bool pickingCharacter;
    private bool pickingName;
    private bool running;
    private bool manualStart;
    private float timer = 0;
    private string dialogueWName;
    private string MCNAME = "[MC Name]";

    public void StartManager()
    {
        currentDialogue = 0;
        pickingCharacter = false;
        pickingName = false;
        running = false;
        dialogueWName = "";

        if (characterSelection != null) characterSelection.SetActive(false);
        if (nameInput != null) nameInput.SetActive(false);
        if (journal != null) journal.SetActive(false);

        foreach (DialogueSO SO in dialogueSOs)
        {
            if (SO.flags.reset) SO.ResetVariables(); 
        }
    }

    public void UpdateManager()
    {
        if (pickingCharacter || pickingName) return;

        //if running == true, dialogue is currently being written on the screen
        if (running == false)
        {
            if (Input.GetMouseButtonDown(0) || manualStart)
            {
                if (dialogueSOs[currentDialogue].dialogue.Contains(MCNAME)) dialogueWName = InsertName(dialogueSOs[currentDialogue].dialogue);
                StartCoroutine(DisplayText(dialogueSOs[currentDialogue]));
            }

            if (manualStart) manualStart = false;
        }
        else if (running == true)
        {
            //Interrupts dialogue writing to skip
            if (!Input.GetMouseButtonDown(0)) return;

            StopAllCoroutines();
            if (dialogueWName == "") dialogueText.text = dialogueSOs[currentDialogue].dialogue;
            else dialogueText.text = dialogueWName;
            

            dialogueWName = "";
            running = false;
            currentDialogue++;
        }

        if (currentDialogue == 0) return;

        if (dialogueSOs[currentDialogue - 1].flags.changeScreen)
        {
            if (running) return;
            timer += Time.deltaTime;

            if (timer >= 2)
            {
                timer = 0;
                dialogueText.text = "";
                nameText.text = "";

                StateManager.instance.ChangeState(StateManager.GAMESTATE.TRANSITION);
                ScreenManager.instance.FindScreen(dialogueSOs[currentDialogue - 1].nextScreen);
                ScreenManager.instance.SetNextState(dialogueSOs[currentDialogue - 1].nextState);
            }
        }
        else if (dialogueSOs[currentDialogue - 1].flags.changeState)
        {
            
        }
        else if (dialogueSOs[currentDialogue - 1].flags.selectCharacter && pickingCharacter == false)
        {
            characterSelection.SetActive(true);
            pickingCharacter = true;
            dialogueSOs[currentDialogue - 1].flags.selectCharacter = false;
        }
        else if (dialogueSOs[currentDialogue - 1].flags.inputName && pickingName == false)
        {
            nameInput.SetActive(true);
            pickingName = true;
            dialogueSOs[currentDialogue - 1].flags.inputName = false;
        }
        else if (dialogueSOs[currentDialogue - 1].flags.getJournal)
        {
            journal.SetActive(true);
        }
        else if (dialogueSOs[currentDialogue - 1].flags.scribbleJournal)
        {
            StateManager.instance.ChangeState(StateManager.GAMESTATE.TRANSITION);
            ScreenManager.instance.journal = true;
        }
    }

    private IEnumerator DisplayText(DialogueSO so)
    {
        running = true;
        int count = 0;
        string newDialogue = "";
        dialogueText.text = "";
        if (so.characterName != "") nameText.text = so.characterName;
        else nameText.text = playerName;

        if (so.dialogue.Contains(MCNAME))
        {
            newDialogue = InsertName(so.dialogue);
        }

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
        if (index == 0)
        {
            characterSprite = maleCharacterSprite;
        }
        else
        {
            characterSprite = femaleCharacterSprite;
        }

        characterSelection.SetActive(false);
        pickingCharacter = false;
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
    public void test()
    {
        gameObject.SetActive(false);
    }
    private string InsertName(string dialogue)
    {
        int index = dialogue.IndexOf('[');
        string b4 = dialogue.Substring(0, index - 1);
        string aft = dialogue.Substring(index + MCNAME.Length, dialogue.Length - MCNAME.Length - b4.Length - 1);
        return b4 + " " + playerName + aft;
    }
}




//string temp = "[MC Name]";
//int index = so.dialogue.IndexOf('[');
//string b4 = so.dialogue.Substring(0, index - 1);
//string aft = so.dialogue.Substring(index + temp.Length, so.dialogue.Length - temp.Length - b4.Length - 1);
//newDialogue = b4 + " " + playerName + aft;