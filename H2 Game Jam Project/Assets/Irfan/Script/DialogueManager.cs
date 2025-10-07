using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(instance);
        else instance = this;
    }


    public List<DialogueSO> dialogueSOs = new List<DialogueSO>();
    public TMP_Text dialogueText;
    public TMP_Text nameText;

    public string playerName;
    public int currentDialogue;
    public bool isFlagActivated;

    public void StartManager()
    {
        currentDialogue = 0;
        isFlagActivated = false;
    }

    public void UpdateManager()
    {
        if (Input.GetMouseButtonDown(0) && isFlagActivated != true)
            StartCoroutine(DisplayText(dialogueSOs[currentDialogue]));

        if (isFlagActivated)
        {
            if (dialogueSOs[currentDialogue - 1].nextState != StateManager.GAMESTATE.NOSTATE) StateManager.instance.ChangeState(dialogueSOs[currentDialogue - 1].nextState);
            //else if (dialogueSOs[currentDialogue - 1].nextState != StateManager.GAMESTATE.NOSTATE)  
        }            
    }

    private IEnumerator DisplayText(DialogueSO so)
    {
        Debug.Log("New Line");

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
        Debug.Log("Line Completed");

        if (so.activateFlag) isFlagActivated = true; 

        yield return null;
    }
}
