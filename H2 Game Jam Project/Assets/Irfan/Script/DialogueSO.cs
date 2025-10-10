using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "DialogueSO")]
public class DialogueSO : ScriptableObject
{
    public string characterName;
    public Sprite characterSprite;   
    public string dialogue;
    public float delay;

    public Flags flags;
    public StateManager.GAMESTATE nextState;
    public string nextScreen;

    public void ResetVariables()
    {
        if (flags.resetCharacter) flags.selectCharacter = true;
        if (flags.resetName) flags.inputName = true;
        if (flags.resetScribbleJournal) flags.scribbleJournal = true;
        if (flags.resetCountryEraser) flags.countryEraser = true;
        if (flags.resetEndDayOne) flags.endDayOne = true;
        if (flags.resetEndDayTwo) flags.endDayTwo = true;
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
    public bool countryEraser;
    public bool resetCountryEraser;
    public bool endDayOne;
    public bool resetEndDayOne;
    public bool endDayTwo;
    public bool resetEndDayTwo;
}