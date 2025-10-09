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
        if (Input.GetKeyDown(KeyCode.Alpha1)) MinigameManager.instance.StartMinigame("Cha1");
        if (Input.GetKeyDown(KeyCode.Alpha2)) MinigameManager.instance.StartMinigame("Cha2");
        if (Input.GetKeyDown(KeyCode.Alpha3)) MinigameManager.instance.StartMinigame("Chopsticks1");
        if (Input.GetKeyDown(KeyCode.Alpha4)) MinigameManager.instance.StartMinigame("Chopsticks2");
        if (Input.GetKeyDown(KeyCode.Q)) StateManager.instance.ChangeState(StateManager.GAMESTATE.GAME);

        if (StateManager.instance.currState == StateManager.GAMESTATE.CONVO)
        {
            DialogueManager.instance.UpdateManager();
        }
        else if (StateManager.instance.currState == StateManager.GAMESTATE.GAME)
        {
            MinigameManager.instance.UpdateManager();
        }
        else if (StateManager.instance.currState == StateManager.GAMESTATE.TRANSITION)
        {
            ScreenManager.instance.UpdateManager();
        }
        else if (StateManager.instance.currState == StateManager.GAMESTATE.WANDER)
        {
            //UNUSED
        }
    }
}
