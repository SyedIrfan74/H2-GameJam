using UnityEngine;
using UnityEngine.UI;

public class ChaptehFoot : MonoBehaviour
{
    public Slider foot;

    public float minX;
    public float maxX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(foot.value,
                                              transform.localPosition.y,
                                              0);
    }

    public void ResetFoot()
    {
        foot.value = 0;
    }
}
