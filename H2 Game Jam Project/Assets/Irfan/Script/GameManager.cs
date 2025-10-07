using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        StateManager.instance.StartManager();
        DialogueManager.instance.StartManager();
        ScreenManager.instance.StartManager();
        MinigameManager.instance.StartManager();
    }
    private void Update()
    {
        if (StateManager.instance.state == StateManager.GAMESTATE.CONVO)
        {
            DialogueManager.instance.UpdateManager();
        }
        else if (StateManager.instance.state == StateManager.GAMESTATE.WANDER)
        {

        }
        else if (StateManager.instance.state == StateManager.GAMESTATE.GAME)
        {
            MinigameManager.instance.UpdateManager();
        }
    }
}
