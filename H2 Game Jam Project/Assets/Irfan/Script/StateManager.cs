using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager instance;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(instance);
        else instance = this;
    }
    public void StartManager()
    {
        state = GAMESTATE.CONVO;
    }
    public enum GAMESTATE
    {
        CONVO,
        WANDER,
        GAME
    }

    public GAMESTATE state;
}
