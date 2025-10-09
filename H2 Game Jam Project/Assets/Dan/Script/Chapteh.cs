using UnityEngine;

public class Chapteh : MonoBehaviour
{
    public float chaptehSpeed = 100f;

    public float gravity = -9.81f;

    public Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Chapteh Foot"))
        {
            Debug.Log("Chapteh Hit");
            chaptehSpeed += 0.2f;
            rb.AddForce(new Vector2(Random.Range(-1,2), chaptehSpeed), ForceMode2D.Impulse);
        }
    }
        
}
