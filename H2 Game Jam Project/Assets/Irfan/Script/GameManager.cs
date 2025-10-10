using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Image playBar;

    private void Start()
    {
        StateManager.instance.StartManager();
        DialogueManager.instance.StartManager();
        ScreenManager.instance.StartManager();
        MinigameManager.instance.StartManager();

        StartCoroutine(BarBlink());
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1)) MinigameManager.instance.StartMinigame("Cha1");
        //if (Input.GetKeyDown(KeyCode.Alpha2)) MinigameManager.instance.StartMinigame("Cha2");
        //if (Input.GetKeyDown(KeyCode.Alpha3)) MinigameManager.instance.StartMinigame("Chopsticks1");
        //if (Input.GetKeyDown(KeyCode.Alpha4)) MinigameManager.instance.StartMinigame("Chopsticks2");
        //if (Input.GetKeyDown(KeyCode.Q)) StateManager.instance.ChangeState(StateManager.GAMESTATE.GAME);

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

    private IEnumerator BarBlink()
    {
        float elapsed = 0;
        float duration = 1;
        Color initial = playBar.color;
        Color man = new Color(playBar.color.r, playBar.color.g, playBar.color.b, 0.2f);

        //lighten
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            playBar.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        elapsed = 0;
        initial = playBar.color;
        man = new Color(playBar.color.r, playBar.color.g, playBar.color.b, 0.7f);

        //Darken
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            t = t * t;
            playBar.color = Color.Lerp(initial, man, t);
            yield return null;
        }

        StartCoroutine(BarBlink());
        yield break;
    }


}
