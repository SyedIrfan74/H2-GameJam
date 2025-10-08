using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

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

    public string currentMinigame = "None";

    public void StartManager()
    {
        
    }

    public void UpdateManager()
    {
        if (currentMinigame == "Chopsticks") UpdateChopsticks();
    }

    public void StartMinigame(string minigameName)
    {
        switch (minigameName) 
        {
            case "Cha":
                currentMinigame = "Cha";
                break;
            case "Chopsticks":
                currentMinigame = "Chopsticks";
                StartChopsticks();
                break;
            default:
                Debug.Log("Jialat.");
                break;
        }
        ScreenManager.instance.ChangeScreen(currentMinigame);

    }

    #region Cha
    #endregion

    #region Chopsticks
    #region Chopsticks Variables
    [Header("Chopsticks General")]
    public bool isPlayerTurn;
    public enum ChopsticksGameStates
    {
        MidGame,
        PlayerWin,
        PlayerLose,
        None
    }

    public ChopsticksGameStates currChopstickState;

    public List<Sprite> chopstickHands = new List<Sprite>();

    public TMP_Text winStateText;

    [Header("Chopsticks Player")]
    public bool isAttacking;

    // 1 for left, 2 for right
    public int selectedPlayerHand;

    // values of the players hands
    public int leftHand = 1;
    public int rightHand = 1;

    public GameObject leftHandGO;
    public GameObject rightHandGO;

    public Image leftHandSprite;
    public Image rightHandSprite;

    [Header("Chopsticks Enemy")]
    // enemy thing
    public bool isEnemyAttacking;

    // values for the enemys hands
    public int enemySelectedHand;
    public int enemyLeftHand = 1;
    public int enemyRightHand = 1;

    public GameObject enemyLeftHandGO;
    public GameObject enemyRightHandGO;

    public Image enemyLeftHandSprite;
    public Image enemyRightHandSprite;

    public bool isEnemySelecting;

    // debug text on screen
    public TMP_Text chopsticksDebugText;
    #endregion

    void StartChopsticks()
    {
        ResetChopsticks();
    }
    /// <summary>
    /// Update loop for chopsticks minigame
    /// </summary>
    void UpdateChopsticks()
    {
        if (currChopstickState == ChopsticksGameStates.MidGame)
        {
            if (!isPlayerTurn && !isEnemyAttacking)
            {
                StartCoroutine(ChopsticksEnemyTurn());
            }
        }
    }

    public void PlayerSelectHand(int i)
    {
        if (isPlayerTurn)
        {
            selectedPlayerHand = i;
            isAttacking = true;
        }
    }

    // attached to enemy hands with i being the side that is being attacked
    // 1 is for enemy's left hand, 2 is for their right hand
    public void PlayerAttack(int i)
    {
        if (isPlayerTurn && isAttacking)
        {
            HandAttackAnimation(selectedPlayerHand, i, true);
            switch (selectedPlayerHand)
            {
                case 0:
                    break;
                // attacking with the left hand
                case 1:
                    // attacking the enemy left hand
                    if (i == 1)
                    {
                        enemyLeftHand += leftHand;
                    }
                    // attacking the enemy right hand
                    else
                    {
                        enemyRightHand += leftHand;
                    }
                    break;
                // attacking with the right hand
                case 2:
                    // attacking the enemy left hand
                    if (i == 1)
                    {
                        enemyLeftHand += rightHand;
                    }
                    // attacking the enemy right hand
                    else
                    {
                        enemyRightHand += rightHand;
                    }
                    break;
            }

            enemyLeftHand = Mathf.Clamp(enemyLeftHand, 1, 5);
            enemyRightHand = Mathf.Clamp(enemyRightHand, 1, 5);


            isPlayerTurn = false;
            isAttacking = false;

            ChopsticksEndOfTurn();

        }
    }

    void HandAttackAnimation(int player, int enemy, bool isPLayerAttacking)
    {
        // player hand animation
        if (isPLayerAttacking)
        {
            // player attacks with left
            if (player == 1)
            {
                // enemy hand is left? play left hits left animation, else play left hits right animation
                //enemy == 1 ? leftHandAnimator.Play("HitLeft") : leftHandAnimator.Play("HitRight");
                Debug.Log(enemy == 1 ? "left hits left" : "left hits right");
            }
            // player attacks with right
            else
            {
                // enemy hand is left? play right hits left animation, else play right hits right animation
                //enemy == 1 ? rightHandAnimator.Play("HitLeft") : rightHandAnimator.Play("HitRight");
                Debug.Log(enemy == 1 ? "right hits left" : "right hits right");

            }
        }
        // enemy hand animation
        else if (!isPLayerAttacking)
        {
            // player attacks with left
            if (player == 1)
            {
                // enemy hand is left? play left hits left animation, else play left hits right animation
                //enemy == 1 ? leftHandAnimator.Play("HitLeft") : leftHandAnimator.Play("HitRight");
                Debug.Log(enemy == 1 ? "enemy left hits left" : "enemy left hits right");
            }
            // player attacks with right
            else
            {
                // enemy hand is left? play right hits left animation, else play right hits right animation
                //enemy == 1 ? rightHandAnimator.Play("HitLeft") : rightHandAnimator.Play("HitRight");
                Debug.Log(enemy == 1 ? "enemy right hits left" : "enemy right hits right");

            }
        }
    }
    void ChopsticksDebug()
    {
        //chopsticksDebugText.text = isPlayerTurn.ToString() + " : " + isAttacking.ToString() + "\n" +
        //                 enemyRightHand.ToString() + " : " + enemyLeftHand.ToString() + "\n" +
        //                 leftHand.ToString() + " : " + rightHand.ToString();
    }

    IEnumerator ChopsticksEnemyTurn()
    {
        Debug.Log("Enemy turn now");
        isEnemyAttacking = true;
        isEnemySelecting = true;

        yield return new WaitForSecondsRealtime(2);

        // randomly generates attacking hand
        while (isEnemySelecting)
        {
            enemySelectedHand = UnityEngine.Random.Range(0, 2) + 1;

            // enemy attacking with left hand
            if (enemySelectedHand == 1)
            {
                // if hand still alive, break this loop
                if (enemyLeftHand < 5)
                {
                    Debug.Log("Enemy Left Hand is alive");
                    isEnemySelecting = false;
                }
            }
            // enemy attacking with right hand
            else if (enemySelectedHand == 2)
            {
                // if hand still alive, break this loop
                if (enemyRightHand < 5)
                {
                    Debug.Log("Enemy Right Hand is alive");
                    isEnemySelecting = false;
                }
            }

            Debug.Log("Rerolling enemy hand");
        }
        yield return null;

        // reset bool
        isEnemySelecting = true;
        // int value for player hand to attack
        int i = 1; 

        // randomly generates hand to attack
        while (isEnemySelecting)
        {
            i = UnityEngine.Random.Range(0, 2) + 1;

            // enemy attacking player left hand
            if (i == 1)
            {
                if (leftHand < 5)
                {
                    Debug.Log("Player Left Hand is alive");
                    isEnemySelecting = false;
                }
            }
            // enemy attacking player right hand
            else if (i == 2)
            {
                if (rightHand < 5)
                {
                    Debug.Log("Player Right Hand is alive");
                    isEnemySelecting = false;
                }
            }

            Debug.Log("Rerolling player hand");
        }
        yield return null;

        // enemy attacks
        HandAttackAnimation(i , enemySelectedHand, false);
        switch (i)
        {
            case 0:
                break;
            // enemy attacking with the left hand
            case 1:
                // attacking the player left hand
                if (enemySelectedHand == 1)
                {
                    leftHand += enemyLeftHand;
                    Debug.Log("enemy hit left");
                }
                // attacking the player right hand
                else
                {
                    leftHand += enemyRightHand;
                }
                break;
            // enemy attacking with the right hand
            case 2:
                // attacking the player left hand
                if (enemySelectedHand == 1)
                {
                    rightHand += enemyLeftHand;
                }
                // attacking the player right hand
                else
                {
                    rightHand += enemyRightHand;
                }
                break;
        }

        rightHand = Mathf.Clamp(rightHand, 1, 5);
        leftHand = Mathf.Clamp(leftHand, 1, 5);

        ChopsticksEndOfTurn();

        isEnemyAttacking = false;

        isPlayerTurn = true;

    }

    void ChopsticksEndOfTurn()
    {
        ChopsticksDebug();
        ChopsticksHandStates();
        ChopsticksWinStates();
    }

    void ChopsticksHandStates()
    {
        // changing the hand sprites based on the value
        enemyLeftHandSprite.sprite = chopstickHands[enemyLeftHand - 1];
        enemyRightHandSprite.sprite = chopstickHands[enemyRightHand - 1];

        leftHandSprite.sprite = chopstickHands[leftHand - 1];
        rightHandSprite.sprite = chopstickHands[rightHand - 1];

        // disable respective hand go based on the value
        rightHandGO.SetActive(rightHand < 5);
        leftHandGO.SetActive(leftHand < 5);
        enemyRightHandGO.SetActive(enemyRightHand < 5);
        enemyLeftHandGO.SetActive(enemyLeftHand < 5);
    }

    void ChopsticksWinStates()
    {
        
        if (!rightHandGO.activeSelf && !leftHandGO.activeSelf)
        {
            currChopstickState = ChopsticksGameStates.PlayerLose;
            winStateText.text = "YOU LOSE!";
            winStateText.gameObject.transform.parent.gameObject.SetActive(true);

            return;
        }
        if (!enemyRightHandGO.activeSelf && !enemyLeftHandGO.activeSelf)
        {
            currChopstickState = ChopsticksGameStates.PlayerWin;
            winStateText.text = "YOU WIN!";
            winStateText.gameObject.transform.parent.gameObject.SetActive(true);

            return;
        }
    }

    void ResetChopsticks()
    {
        leftHand = 1; 
        rightHand = 1;
        enemyLeftHand = 1;
        enemyRightHand = 1;

        currChopstickState = ChopsticksGameStates.MidGame;

        isPlayerTurn = true;

        winStateText.gameObject.transform.parent.gameObject.SetActive(false);

    }

    #endregion

    #region Chapteh
    #endregion

    #region Bubble Blowing
    #endregion
}

