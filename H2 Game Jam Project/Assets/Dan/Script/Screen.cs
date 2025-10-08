using UnityEngine;

public class Screen : MonoBehaviour
{
    public Canvas canvas;
    public string screenName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    virtual public void OnStart()
    {
        if(canvas == null)  canvas = GetComponentInChildren<Canvas>();
    }

    // Update is called once per frame
    virtual public void OnUpdate()
    {
        
    }

}
