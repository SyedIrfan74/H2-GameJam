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
        START,
        CONVO,
        WANDER,
        GAME,
        NOSTATE
    }

    public GAMESTATE currState;

    public void StartManager()
    {
        currState = GAMESTATE.START;
    }
    public void ChangeState(GAMESTATE nextState)
    {
        currState = nextState;
    }
    public void ChangeState2(int nextState)
    {
        currState = (GAMESTATE)nextState;
    }
}
