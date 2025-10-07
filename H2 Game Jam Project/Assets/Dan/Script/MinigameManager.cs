using NUnit.Framework;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartManager()
    {
        
    }

    // Update is called once per frame
    public void UpdateManager()
    {
        
    }

    public void StartMinigame(string minigameName)
    {
        switch (minigameName) 
        {
            case "Cha":
                ScreenManager.instance.ChangeScreen(3);
                break;
            case "Chopsticks":
                ScreenManager.instance.ChangeScreen("Chopsticks");
                break;
            default:
                Debug.Log("Unknown game state.");
                break;
        }
    }

    public void LeaveManager()
    {

    }
}
