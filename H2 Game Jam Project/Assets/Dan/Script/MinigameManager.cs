using System.Collections.Generic;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    #region Singleton
    public static MinigameManager instance;

    private void Awake()
    {
        if(instance != null && instance != this) Destroy(instance);
        else instance = this;
    }
    #endregion

    public List<MinigameScreen> minigameList = new List<MinigameScreen>();

    public string currentMinigame = "None";

    public void StartManager()
    {
        
    }

    public void UpdateManager()
    {
        
    }

    public void StartMinigame(string minigameName)
    {
        switch (minigameName) 
        {
            case "Cha":
                currentMinigame = "Cha";
                break;
            case "Chopsticks":
                currentMinigame = "Chopsticks";
                break;
            default:
                Debug.Log("Jialat.");
                break;
        }
        ScreenManager.instance.ChangeScreen(currentMinigame);

    }

    public void LeaveManager()
    {

    }
}
