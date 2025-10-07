using NUnit.Framework;
using System.Collections.Generic;
using Unity.Cinemachine;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartManager()
    {
       foreach(Screen screen in screenList) 
        { 
            screen.OnStart(); 
            screenTransforms.Add(screen.transform);
        }
    }

    // Update is called once per frame
    public void UpdateManager()
    {

    }
    
    /// <summary>
    /// Changes screen based on int n
    /// </summary>
    /// <param name="n">Index of screen in list</param>

    public void ChangeScreen(int n)
    {
        Camera.main.transform.position = new Vector3(screenTransforms[n].position.x,
                                                     screenTransforms[n].position.y,
                                                     Camera.main.transform.position.z);
    }

    /// <summary>
    /// Changes screen based on string str
    /// </summary>
    /// <param name="str">Name of screen</param>
    public void ChangeScreen(string str)
    {
        for (int i = 0; i < screenList.Count; i++)
        {
            if (screenList[i].name == str)
            {
                Camera.main.transform.position = new Vector3(screenList[i].transform.position.x,
                                                             screenList[i].transform.position.y,
                                                             Camera.main.transform.position.z);
                break;
            }
        }               
    }
}
