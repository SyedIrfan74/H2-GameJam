using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Chapteh : MonoBehaviour
{
    public float startingY;
    public float chaptehSpeed = 100f;

    public float gravity = -9.81f;

    public Rigidbody2D rb;

    public int score;
    public int targetScore;

    public float minX;
    public float maxX;

    // score board
    public TMP_Text scoreText;

    // win screen
    public Image chaptehWinImage;
    public Sprite chaptehWinSprite;
    public Sprite chaptehLoseSprite;

    public AudioData chaptehHitAudio;

    private void Start()
    {
        startingY = transform.localPosition.y;

        gameObject.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score +
                         "\nTarget: " + targetScore;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Chapteh Foot"))
        {
            Debug.Log("Chapteh Hit");
            chaptehSpeed += 0.5f;
            rb.AddForce(new Vector2(Random.Range(minX, maxX), chaptehSpeed), ForceMode2D.Impulse);

            AudioManager.instance.PlayAudio(chaptehHitAudio);

            score++;
            minX -= 0.2f;
            maxX += 0.2f;
        }
        else if (collision.gameObject.name.Contains("Floor"))
        {
            gameObject.SetActive(false);
            MinigameManager.instance.EndChapteh();
        }
        else if (collision.gameObject.name.Contains("Square"))
        {
            rb.AddForce(new Vector2(chaptehSpeed, Random.Range(minX, maxX)), ForceMode2D.Impulse);

        }

    }

    public void ResetChapteh()
    {
        chaptehWinImage.gameObject.SetActive(false);

        transform.localPosition = new Vector3(0, startingY, 0);

        score = 0;
    }
}
