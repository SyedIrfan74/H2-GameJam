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
        if (flags.resetStamp) flags.stamp = true;
        if (flags.resetSticker) flags.sticker = true;
        if (flags.resetEndDayOne) flags.endDayOne = true;
        if (flags.resetEndDayTwo) flags.endDayTwo = true;
        if (flags.resetEndDayThree) flags.endDayThree = true;

        if (flags.resetChopsticksTutorial) flags.chopsticksTutorial = true;
        if (flags.resetChaTutorial) flags.chaTutorial = true;
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

    public bool chopsticksTutorial;
    public bool resetChopsticksTutorial;

    public bool stamp;
    public bool resetStamp;

    public bool chaTutorial;
    public bool resetChaTutorial;

    public bool sticker;
    public bool resetSticker;

    public bool endDayOne;
    public bool resetEndDayOne;

    public bool endDayTwo;
    public bool resetEndDayTwo;

    public bool endDayThree;
    public bool resetEndDayThree;
}