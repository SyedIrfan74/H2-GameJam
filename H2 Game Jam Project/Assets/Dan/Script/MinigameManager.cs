using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager instance;


    private void Awake()
    {
        if(instance != null && instance != this) Destroy(instance);
        else instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartManager()
    {
        
    }

    // Update is called once per frame
    public void UpdateManager()
    {
        
    }

    public void StartMinigame()
    {

    }

    public void LeaveManager()
    {

    }
}
