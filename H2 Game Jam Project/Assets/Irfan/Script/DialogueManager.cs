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

    public void StartManager()
    {
        currentDialogue = 0;
    }

    public void UpdateManager()
    {
        if (Input.GetMouseButtonDown(0))
            StartCoroutine(DisplayText(dialogueSOs[currentDialogue]));
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

        yield return null;
    }
}
