using UnityEngine;

public class Screen : MonoBehaviour
{
    public Canvas canvas;
    public string screenName;

    //Edit by: Irfan
    public SpriteRenderer fadeBlack;

    virtual public void OnStart()
    {
        if(canvas == null)  canvas = GetComponentInChildren<Canvas>();

        //Edit by: Irfan
        if (fadeBlack == null) fadeBlack = GetComponentInChildren<SpriteRenderer>();
    }

    virtual public void OnUpdate()
    {
        
    }

}
