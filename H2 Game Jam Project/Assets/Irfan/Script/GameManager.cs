using UnityEngine;

/// <summary>
/// 
/// </summary>

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
        if (Input.GetKeyDown(KeyCode.Alpha1)) MinigameManager.instance.StartMinigame("Cha");
        if (Input.GetKeyDown(KeyCode.Alpha2)) MinigameManager.instance.StartMinigame("Chopsticks");

        if (StateManager.instance.currState == StateManager.GAMESTATE.CONVO)
        {
            DialogueManager.instance.UpdateManager();
        }
        else if (StateManager.instance.currState == StateManager.GAMESTATE.WANDER)
        {

        }
        else if (StateManager.instance.currState == StateManager.GAMESTATE.GAME)
        {
            MinigameManager.instance.UpdateManager();
        }
        else if (StateManager.instance.currState == StateManager.GAMESTATE.TRANSITION)
        {
            ScreenManager.instance.UpdateManager();
        }
    }
}
