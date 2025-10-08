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
        START,      //0
        CONVO,      //1
        WANDER,     //2
        GAME,       //3
        TRANSITION, //4
        NOSTATE     //5
    }

    public GAMESTATE currState;

    public void StartManager()
    {
        currState = GAMESTATE.GAME;
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
