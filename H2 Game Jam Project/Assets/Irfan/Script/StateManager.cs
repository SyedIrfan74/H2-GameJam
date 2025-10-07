using System;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager instance;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(instance);
        else instance = this;
    }

    [Serializable]
    public enum GAMESTATE
    {
        CONVO,
        WANDER,
        GAME,
        NOSTATE
    }

    public GAMESTATE currState;

    public void StartManager()
    {
        currState = GAMESTATE.CONVO;
    }
    public void ChangeState(GAMESTATE nextState)
    {
        currState = nextState;
    }
}
