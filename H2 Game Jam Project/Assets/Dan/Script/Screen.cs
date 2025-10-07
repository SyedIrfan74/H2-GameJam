using UnityEngine;

public class Screen : MonoBehaviour
{
    public Canvas canvas;
    public string screenName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnStart()
    {
        if(canvas == null)  canvas = GetComponentInChildren<Canvas>();
    }

    // Update is called once per frame
    public void OnUpdate()
    {
        
    }

    virtual public void ActivateCanvas()
    {
        canvas.enabled = !canvas.enabled;
    }
}
