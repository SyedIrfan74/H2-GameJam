using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "DialogueSO")]
public class DialogueSO : ScriptableObject
{
    public string characterName;
    public string dialogue;
    public float delay;

    public Flags flags;
    public StateManager.GAMESTATE nextState;
    public string nextScreen;

    public void ResetVariables()
    {
        if (flags.resetCharacter) flags.selectCharacter = true;
        if (flags.resetName) flags.inputName = true;
    }
}

[Serializable]
public class Flags
{
    public bool reset;
    public bool changeScreen;
    public bool changeState;
    public bool selectCharacter;
    public bool resetCharacter;
    public bool inputName;
    public bool resetName;
    public bool getJournal;
    public bool scribbleJournal;
    public bool resetScribbleJournal;
}