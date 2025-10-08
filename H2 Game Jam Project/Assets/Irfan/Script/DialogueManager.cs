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

    [Header("Dialogue Parts")]
    public List<DialogueSO> dialogueSOs = new List<DialogueSO>();
    public TMP_Text dialogueText;
    public TMP_Text nameText;

    public string playerName;
    public int currentDialogue;
    public bool isFlagActivated;
    public bool running;

    public void StartManager()
    {
        currentDialogue = 0;
        isFlagActivated = false;
        running = false;
        characterSelection.SetActive(false);
        nameInput.SetActive(false);
    }

    public void UpdateManager()
    {
        //if running == true, dialogue is currently being written on the screen
        if (running == false)
        {
            if (Input.GetMouseButtonDown(0) && isFlagActivated != true) StartCoroutine(DisplayText(dialogueSOs[currentDialogue]));
        }
        else if (running == true)
        {
            //Interrupts dialogue writing to skip
            if (!Input.GetMouseButtonDown(0)) return;

            StopAllCoroutines();
            dialogueText.text = dialogueSOs[currentDialogue].dialogue;
            running = false;

            if (dialogueSOs[currentDialogue].activateFlag) isFlagActivated = true;

            currentDialogue++;
        }

        //Flag to indicate a change in STATE or SCREEN
        if (isFlagActivated)
        {
            //Handles Normal Gameplay
            if (!dialogueSOs[currentDialogue - 1].starting)
            {
                if (dialogueSOs[currentDialogue - 1].nextState != StateManager.GAMESTATE.NOSTATE) StateManager.instance.ChangeState(dialogueSOs[currentDialogue - 1].nextState);
                //else if (dialogueSOs[currentDialogue - 1].nextScreen != "") ScreenManager.instance.ChangeScreen(dialogueSOs[currentDialogue - 1].nextScreen);
                //else if (dialogueSOs[currentDialogue - 1].nextScreen != "") ScreenManager.instance.StartCoroutine(ScreenManager.ChangeScene(dialogueSOs[currentDialogue - 1].nextScreen));

                return;
            }
            
            //Handles Intro
            if (dialogueSOs[currentDialogue - 1].bools[0] == true)
            {

            }  
        }            
    }

    private IEnumerator DisplayText(DialogueSO so)
    {
        running = true;
        int stringlen = so.dialogue.Length;
        int count = 0;
        dialogueText.text = "";
        if (so.characterName != "") nameText.text = so.characterName;
        else nameText.text = playerName;

        while (count < stringlen)
        {
            dialogueText.text += so.dialogue[count];
            count++;

            yield return new WaitForSeconds(so.delay);
        }

        currentDialogue++;
        running = false;

        if (so.activateFlag) isFlagActivated = true; 

        yield return null;
    }
}
