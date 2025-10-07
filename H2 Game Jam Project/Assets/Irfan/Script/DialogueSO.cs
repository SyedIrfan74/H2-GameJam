using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DialogueSO")]
public class DialogueSO : ScriptableObject
{
    public string characterName;
    public string dialogue;
    public float delay;
    public bool activateFlag;
    public StateManager.GAMESTATE nextState;
    public string nextScreen;
    
}

