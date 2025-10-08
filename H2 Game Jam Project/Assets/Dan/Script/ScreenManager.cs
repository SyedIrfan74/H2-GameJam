using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    #region Singleton
    public static ScreenManager instance;
    private void Awake()
    {
        if (instance != null && instance != this) Destroy(instance);
        else instance = this;
    }
    #endregion
    public List<Screen> screenList = new List<Screen>();
    public List<Transform> screenTransforms = new List<Transform>();
    public string currentScreen = "Main Menu";

    public void StartManager()
    {
       foreach(Screen screen in screenList) 
       { 
            screen.OnStart(); 
            screenTransforms.Add(screen.transform);
       }

       ChangeScreen(currentScreen);
    }

    public void UpdateManager()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeScreen("Cha");
        if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeScreen("Chopsticks");
    }

    /// <summary>
    /// Changes screen based on string str
    /// </summary>
    /// <param name="str">Name of screen</param>
    public void ChangeScreen(string str)
    {
        for (int i = 0; i < screenList.Count; i++)
        {
            Debug.Log(screenList[i].name);
            // found the right screen
            if (screenList[i].name.Contains(str))
            {
                Debug.Log("FUCKU");
                Camera.main.transform.position = new Vector3(screenList[i].transform.position.x,
                                                             screenList[i].transform.position.y,
                                                             Camera.main.transform.position.z);
                screenList[i].canvas.gameObject.SetActive(true);
            }
            // everything else
            else
            {
                Debug.Log("mannnnnn");
                screenList[i].canvas.gameObject.SetActive(false);
            }
        }      
        
        currentScreen = str;
    }
}
