using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
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

    public AudioData bgmAudio;

    public void StartManager()
    {
        
    }

    public void UpdateManager()
    {
        if (currentMinigame == "Chopsticks") UpdateChopsticks();
        if (currentMinigame == "Cha") UpdateCha();
    }

    public void StartMinigame(string minigameName)
    {
        switch (minigameName) 
        {
            case "Cha1":
                currentMinigame = "Cha";
                StartCha(1);
                break;
            case "Cha2":
                currentMinigame = "Cha";
                StartCha(2);
                break;
            case "Chopsticks1":
                currentMinigame = "Chopsticks";
                StartChopsticks(1);
                break;
            case "Chopsticks2":
                currentMinigame = "Chopsticks";
                StartChopsticks(2);
                break;
            case "Chapteh1":
                currentMinigame = "Chapteh";
                StartChapteh(1);
                break;
            case "Chapteh2":
                currentMinigame = "Chapteh";
                StartChapteh(2);
                break;
            case "Bubble Blowing":
                currentMinigame = "Bubble Blowing";
                //StartBB();
                break;
            default:
                Debug.Log("Jialat.");
                break;
        }

        ScreenManager.instance.ChangeScreen(currentMinigame);
        AudioManager.instance.PlayAudio(bgmAudio);

    }

    public void StartMinigame2(string minigameName)
    {
        switch (minigameName)
        {
            case "Cha1":
                currentMinigame = "Cha";
                StartCha(1);
                break;
            case "Cha2":
                currentMinigame = "Cha";
                StartCha(2);
                break;
            case "Chopsticks1":
                currentMinigame = "Chopsticks";
                StartChopsticks(1);
                break;
            case "Chopsticks2":
                currentMinigame = "Chopsticks";
                StartChopsticks(2);
                break;
            case "Chapteh1":
                currentMinigame = "Chapteh";
                StartChapteh(1);
                break;
            case "Chapteh2":
                currentMinigame = "Chapteh";
                StartChapteh(2);
                break;
            case "Bubble Blowing":
                currentMinigame = "Bubble Blowing";
                //StartBB();
                break;
            default:
                Debug.Log("Jialat.");
                break;
        }
    }

    #region Chapteh
    [Header("Chapteh")]

    public Chapteh chapteh;

    public int chaptehDifficulty;

    void StartChapteh(int i)
    {
        chaptehDifficulty = i;

        chapteh.targetScore = chaptehDifficulty == 1 ? 10 : 30;

        ResetChapteh();
    }

    public void ResetChapteh()
    {
        chapteh.gameObject.SetActive(true);
        chapteh.ResetChapteh();
    }

    public void EndChapteh()
    {
        if (chapteh.score >= chapteh.targetScore)
        {
            chapteh.chaptehWinImage.gameObject.SetActive(true);
            chapteh.chaptehWinImage.sprite = chapteh.chaptehWinSprite;
        }
        else
        {
            chapteh.chaptehWinImage.gameObject.SetActive(true);
            chapteh.chaptehWinImage.sprite = chapteh.chaptehLoseSprite;
        }
    }

    #endregion

    #region Cha
    #region Cha Variables
    [Header("Cha General")]

    public ChaGameStates currChaState;

    public enum ChaGameStates
    {
        Selecting,
        Attacking,
        PlayerWin,
        PlayerLose,
        None
    }

    public enum ChaHandStates
    {
        Person,
        Gun,
        Rock,
        Dead
    }

    public List<Sprite> chaHandSprites = new List<Sprite>();

    public int chaDifficulty;

    public Image chaWinImage;

    public AudioData chaPersonAudio;
    public AudioData chaGunAudio;
    public AudioData chaRockAudio;

    [Header("Cha Player")]

    public List<ChaHandStates> playerHands = new List<ChaHandStates>(2);

    public int leftHandIndex;
    public int rightHandIndex;

    public List<Image> playerSprites = new List<Image>(2);

    [Header("Cha Enemy")]

    public List<ChaHandStates> enemyHands = new List<ChaHandStates>(2);

    public List<Image> enemyHandSprites = new List<Image>(2);

    public Image enemyHumanSprite; // the enemy sprite in the middle
    public List<Sprite> enemyHumanSprites = new List<Sprite>();
    public List<Sprite> bobbyHandSprites = new List<Sprite>();
    public List<Sprite> priyaHandSprites = new List<Sprite>();

    public List<GameObject> deadHandImageGO = new List<GameObject>();

    public bool isEnemySelectingCha;
    #endregion

    void StartCha(int i)
    {
        chaDifficulty = i;

        enemyHumanSprite.sprite = enemyHumanSprites[i - 1];
        ChaHandSprites();
        ResetCha();
    }

    void UpdateCha()
    {
        //if (Input.GetKeyDown(KeyCode.P)) { ChaHandSprites(); }
    }

    /// <summary>
    /// Attached to a button and on click, itll change the hand type 
    /// </summary>
    /// <param name="i">The hand being changed, 0 being left, 1 being right</param>
    public void ChaPlayerChangeHand(int i)
    {
        if (currChaState == ChaGameStates.Selecting)
        {
            if (i == 0) 
            {
                leftHandIndex++;
                if (leftHandIndex > 2)
                {
                    playerHands[i] = ChaHandStates.Person;
                    leftHandIndex = 0;
                }
                playerHands[i] = (ChaHandStates)leftHandIndex; 
            }
            if (i == 1)
            {
                rightHandIndex++;
                if (rightHandIndex > 2)
                {
                    playerHands[i] = ChaHandStates.Person;
                    rightHandIndex = 0;
                }
                playerHands[i] = (ChaHandStates)rightHandIndex;
            }

            ChaHandSprites();
        }
    }

    public void ChaConfirm()
    {
        if(currChaState == ChaGameStates.Selecting)
            Debug.Log("PENISSS");

        currChaState = ChaGameStates.None;
        isEnemySelectingCha = true;

        StartCoroutine(ChaEnemyTurn());
    }

    IEnumerator ChaEnemyTurn()
    {
        yield return new WaitForSecondsRealtime(1f);

        while (isEnemySelectingCha == true) 
        { 
            for (int x = 0; x < enemyHands.Count; x++)
            {
                if (enemyHands[x] == ChaHandStates.Dead)
                {
                    Debug.Log(x + "hand is dead");
                    continue;
                }

                int i = UnityEngine.Random.Range(0, 2);
                enemyHands[x] = (ChaHandStates)i;
                Debug.Log("IMPICKING" + i.ToString());

            }

            yield return new WaitForSecondsRealtime(1f);

            ChaHandSprites();
            StartCoroutine(ChaAttackPhase());

            Debug.Log("IMDONEPICKING");
            isEnemySelectingCha = false;
            yield break;
        }
    }

    IEnumerator ChaAttackPhase()
    {
        Debug.Log("IMATTACKING");
        ChaHandSprites();
        
        yield return new WaitForSecondsRealtime(1f);

        for (int i = 0; i < enemyHands.Count; i++)
        {
            //comparing the hands
            switch (i)
            {
                // players left hand
                case 0:
                    // enemy right hand alive
                    if (enemyHands[1] != ChaHandStates.Dead)
                    {
                        // player beat enemy
                        if (DidPlayerWin(playerHands[i], enemyHands[1]) == 1)
                        {
                            enemyHands[1] = ChaHandStates.Dead;
                        }
                        // enemy beat player
                        else if (DidPlayerWin(playerHands[i], enemyHands[1]) == 0)
                        {
                            playerHands[i] = ChaHandStates.Dead;
                        }
                        Debug.Log(i + " : " + DidPlayerWin(playerHands[i], enemyHands[0]).ToString());

                    }
                    // enemy right hand dead, left attack enemy left
                    else
                    {
                        // player beat enemy
                        if (DidPlayerWin(playerHands[i], enemyHands[0]) == 1)
                        {
                            enemyHands[0] = ChaHandStates.Dead;
                        }
                        // enemy beat player
                        else if (DidPlayerWin(playerHands[i], enemyHands[0]) == 0)
                        {
                            playerHands[i] = ChaHandStates.Dead;
                        }
                        Debug.Log(i + " : " + DidPlayerWin(playerHands[i], enemyHands[1]).ToString());

                    }
                    break;
                // player right hand
                case 1:
                    // enemy left hand alive
                    if (enemyHands[0] != ChaHandStates.Dead)
                    {
                        // player beat enemy
                        if (DidPlayerWin(playerHands[i], enemyHands[0]) == 1)
                        {
                            enemyHands[0] = ChaHandStates.Dead;
                        }
                        // enemy beat player
                        else if (DidPlayerWin(playerHands[i], enemyHands[0]) == 0)
                        {
                            playerHands[i] = ChaHandStates.Dead;
                        }
                        Debug.Log(i + " : " + DidPlayerWin(playerHands[i], enemyHands[0]).ToString());

                    }
                    // enemy right hand dead, left attack enemy left
                    else
                    {
                        // player beat enemy
                        if (DidPlayerWin(playerHands[i], enemyHands[1]) == 1)
                        {
                            enemyHands[1] = ChaHandStates.Dead;
                        }
                        // enemy beat player
                        else if (DidPlayerWin(playerHands[i], enemyHands[1]) == 0)
                        {
                            playerHands[i] = ChaHandStates.Dead;
                        }
                        Debug.Log(i + " : " + DidPlayerWin(playerHands[i], enemyHands[1]).ToString());

                    }
                    break;
            }
        }

        currChaState = ChaGameStates.Selecting;

        ChaHandSprites();

        yield return new WaitForSecondsRealtime(3f);

        ChaWinStates();

    }

    int DidPlayerWin(ChaHandStates player, ChaHandStates enemy)
    {
        int didPlayerWin = 2;
        switch (player)
        {
            case ChaHandStates.Dead:
                break;
            case ChaHandStates.Person:
                if (enemy == ChaHandStates.Person || enemy == ChaHandStates.Dead) break;
                if (enemy == ChaHandStates.Gun)
                {
                    Debug.Log("Player lose");

                    return didPlayerWin = 0;
                }
                if (enemy == ChaHandStates.Rock)
                {
                    Debug.Log("Player win");

                    return didPlayerWin = 1;
                }
                break;
            case ChaHandStates.Gun:
                if (enemy == ChaHandStates.Gun || enemy == ChaHandStates.Dead) break;
                if (enemy == ChaHandStates.Rock)
                {
                    Debug.Log("Player lose");

                    return didPlayerWin = 0;
                }
                if (enemy == ChaHandStates.Person)
                {
                    Debug.Log("Player win");

                    return didPlayerWin = 1;
                }
                break;
            case ChaHandStates.Rock:
                if (enemy == ChaHandStates.Rock || enemy == ChaHandStates.Dead) break;
                if (enemy == ChaHandStates.Person)
                {
                    Debug.Log("Player lose");

                    return didPlayerWin = 0;
                }
                if (enemy == ChaHandStates.Gun)
                {
                    Debug.Log("Player win");

                    return didPlayerWin = 1;
                }
                break;
        }

        return didPlayerWin;
    }

    void ChaHandSprites()
    {
        enemyHumanSprite.sprite = enemyHumanSprites[chaDifficulty - 1];

        for (int i = 0; i < enemyHandSprites.Count; i++)
        {
            if (enemyHands[i] != ChaHandStates.Dead)
            {
                enemyHandSprites[i].sprite = chaDifficulty == 1 ? bobbyHandSprites[(int)enemyHands[i]] : priyaHandSprites[(int)enemyHands[i]]; 
                //enemyHandSprites[i].sprite = chaHandSprites[(int)enemyHands[i]];
                deadHandImageGO[i].SetActive(false);
            }
            else deadHandImageGO[i].SetActive(true);


            if (playerHands[i] != ChaHandStates.Dead)
            {
                playerSprites[i].sprite = chaHandSprites[(int)playerHands[i]];
                playerSprites[i].enabled = true;
            }
            else playerSprites[i].enabled = false;
        }
    }

    public void ResetCha()
    {
        currChaState = ChaGameStates.Selecting;

        for(int i = 0; i < enemyHands.Count; i++)
        {
            playerHands[i] = ChaHandStates.Person;
            enemyHands[i] = ChaHandStates.Person;
        }

        foreach (Image sprite in playerSprites)
        {
            sprite.enabled  = true;
        }
        foreach (Image sprite in enemyHandSprites)
        {
            sprite.enabled  = true;
        }

        ChaHandSprites();
        //ChaWinStates();
    }

    void ChaWinStates()
    {
        if (enemyHands[0] == ChaHandStates.Dead && enemyHands[1] == ChaHandStates.Dead) currChaState = ChaGameStates.PlayerWin;
        else if (playerHands[0] == ChaHandStates.Dead && playerHands[1] == ChaHandStates.Dead) currChaState = ChaGameStates.PlayerLose;

        if (currChaState == ChaGameStates.PlayerWin)
        {
            chaWinImage.sprite = chapteh.chaptehWinSprite;
            chaWinImage.gameObject.SetActive(true);
            AudioManager.instance.StopAudio(bgmAudio);
        }
        else if (currChaState == ChaGameStates.PlayerLose)
        {
            chaWinImage.sprite = chapteh.chaptehWinSprite;
            chaWinImage.gameObject.SetActive(true);
            AudioManager.instance.StopAudio(bgmAudio);
        }
        else
        {
            chaWinImage.gameObject.SetActive(false);
        }
    }
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

    public int turnCounter = 1;

    public TMP_Text turnCounterText;

    public int difficulty = 1;

    public AudioData chopstickAudio;

    public Image chopstickWinImage;

    [Header("Chopsticks Player")]
    public bool isAttacking;

    // 1 for left, 2 for right
    public int selectedPlayerHand;

    // values of the players hands
    public int leftHand = 1;
    public int rightHand = 1;

    public GameObject leftHandGO;
    public GameObject rightHandGO;
    public GameObject playerClapGO;

    public Image leftHandSprite;
    public Image rightHandSprite;

    public Animator leftHandAnimator;
    public Animator rightHandAnimator;

    public GameObject clapButton;

    [Header("Chopsticks Enemy")]
    // enemy thing
    public bool isEnemyAttacking;

    // values for the enemys hands
    public int enemySelectedHand;
    public int enemyLeftHand = 1;
    public int enemyRightHand = 1;

    public GameObject enemyLeftHandGO;
    public GameObject enemyRightHandGO;
    public GameObject enemyClapGO;

    public Image enemyLeftHandSprite;
    public Image enemyRightHandSprite;

    public Animator enemyLeftHandAnimator;
    public Animator enemyRightHandAnimator;

    public bool isEnemySelecting;


    #endregion

    void StartChopsticks(int d)
    {
        difficulty = d;

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

    public void PlayerClap()
    {
        if (currChopstickState == ChopsticksGameStates.MidGame && (rightHand >= 1  && !leftHandSprite.enabled || leftHand >= 1 && !rightHandSprite.enabled))
        {

            StartCoroutine(ClapAnimation(true));

            // check if its the players turn and that the hand they selected isnt the same as the current hand
            if (isPlayerTurn)
            {
                // check to ensure that 1 < selected hand < 5
                if (rightHand >= 5 && leftHand > 1 && leftHand < 5)
                {
                    int total = leftHand;
                    leftHand = Mathf.CeilToInt(total / 2);
                    rightHand = total - leftHand;
                    Debug.Log("Left clap right");

                    isPlayerTurn = false;
                    isAttacking = false;

                    ChopsticksEndOfTurn();
                }

                else if (leftHand >= 5 && rightHand > 1 && rightHand < 5)
                {
                    int total = rightHand;
                    rightHand = Mathf.CeilToInt(total / 2);
                    leftHand = total - rightHand;
                    Debug.Log("right clap Left");

                    isPlayerTurn = false;
                    isAttacking = false;

                    ChopsticksEndOfTurn();
                }
            }
            else
            {
                return;
            }
        }
    }
    IEnumerator ClapAnimation(bool isPlayer)
    {
        if (isPlayer)
        {
            playerClapGO.SetActive(true);
            leftHandGO.SetActive(false);
            rightHandGO.SetActive(false);

            yield return new WaitForSeconds(0.1f);

            playerClapGO.SetActive(false);
            leftHandGO.SetActive(true);
            rightHandGO.SetActive(true);

            Debug.Log("CLAP");

        }
        if (!isPlayer)
        {
            enemyClapGO.SetActive(true);
            enemyLeftHandGO.SetActive(false);
            enemyRightHandGO.SetActive(false);

            yield return new WaitForSeconds(0.5f);

            enemyClapGO.SetActive(false);
            enemyLeftHandGO.SetActive(true);
            enemyRightHandGO.SetActive(true);

            Debug.Log("PLAC");
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
                Debug.Log(enemy == 1 ? "enemy left hits left" : "enemy right hits left");
            }
            // player attacks with right
            else
            {
                // enemy hand is left? play right hits left animation, else play right hits right animation
                //enemy == 1 ? rightHandAnimator.Play("HitLeft") : rightHandAnimator.Play("HitRight");
                Debug.Log(enemy == 1 ? "enemy left hits right" : "enemy right hits right");

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

        if (difficulty == 2) EnemyClap();

        if (isPlayerTurn) yield break;

        // randomly generates attacking hand
        int maxAttempts = 100; // safety counter
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            attempts++;
            enemySelectedHand = UnityEngine.Random.Range(1, 3); // 1 or 2

            // enemy attacking with left hand
            if (enemySelectedHand == 1 && enemyLeftHand < 5)
            {
                Debug.Log("Enemy Left Hand is alive");
                break;
            }
            // enemy attacking with right hand
            else if (enemySelectedHand == 2 && enemyRightHand < 5)
            {
                Debug.Log("Enemy Right Hand is alive");
                break;
            }

            Debug.Log("Rerolling enemy hand");
            yield return null; // CRITICAL: Give control back to Unity each loop iteration!
        }

        if (attempts >= maxAttempts)
        {
            Debug.LogError("Could not find valid enemy hand!");
            isEnemyAttacking = false;
            isPlayerTurn = true;
            yield break;
        }

        // int value for player hand to attack
        int i = 1;

        // randomly generates hand to attack
        attempts = 0;
        while (attempts < maxAttempts)
        {
            attempts++;
            i = UnityEngine.Random.Range(1, 3); // 1 or 2

            // enemy attacking player left hand
            if (i == 1 && leftHand < 5)
            {
                Debug.Log("Player Left Hand is alive");
                break;
            }
            // enemy attacking player right hand
            else if (i == 2 && rightHand < 5)
            {
                Debug.Log("Player Right Hand is alive");
                break;
            }

            Debug.Log("Rerolling player hand");
            yield return null; // CRITICAL: Give control back to Unity each loop iteration!
        }

        if (attempts >= maxAttempts)
        {
            Debug.LogError("Could not find valid player hand to attack!");
            isEnemyAttacking = false;
            isPlayerTurn = true;
            yield break;
        }

        // enemy attacks
        HandAttackAnimation(i, enemySelectedHand, false);

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
                else if (enemySelectedHand == 2)
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
                else if (enemySelectedHand == 2)
                {
                    rightHand += enemyRightHand;
                }
                break;
        }

        ChopsticksEndOfTurn();

        isEnemyAttacking = false;
        isPlayerTurn = true;
    }

    void EnemyClap()
    {
        if (currChopstickState == ChopsticksGameStates.MidGame && (enemyRightHand > 1 && !enemyLeftHandSprite.enabled || enemyLeftHand > 1 && !enemyRightHandSprite.enabled))
        {
            // check if its the players turn and that the hand they selected isnt the same as the current hand
            if (!isPlayerTurn)
            {
                // check to ensure that 1 < selected hand < 5
                if (enemyRightHand >= 5 && enemyLeftHand > 1 && enemyLeftHand < 5)
                {
                    int total = enemyLeftHand;
                    enemyLeftHand = Mathf.CeilToInt(total / 2);
                    enemyRightHand = total - enemyLeftHand;
                    Debug.Log("enemy Left clap right");

                    isPlayerTurn = true;
                    isEnemyAttacking = false;

                    StartCoroutine(ClapAnimation(false));

                    ChopsticksEndOfTurn();
                }

                else if (enemyLeftHand >= 5 && enemyRightHand > 1 && enemyRightHand < 5)
                {
                    int total = enemyRightHand;
                    enemyRightHand = Mathf.CeilToInt(total / 2);
                    enemyLeftHand = total - enemyRightHand;
                    Debug.Log("enemy right clap Left");

                    isPlayerTurn = true;
                    isEnemyAttacking = false;

                    StartCoroutine(ClapAnimation(false));

                    ChopsticksEndOfTurn();
                }
            }
            else
            {
                return;
            }
        }
    }

    void ChopsticksEndOfTurn()
    {
        ChopsticksDebug();
        ChopsticksHandStates();
        ChopsticksWinStates();
        ChopsticksUpdateTurnCounter();
    }

    void ChopsticksUpdateTurnCounter()
    {
        turnCounter++;
        turnCounterText.text = "Turn: " + turnCounter + "\n" + (isPlayerTurn ? "Your turn!" : "Their turn!");
    }

    void ChopsticksHandStates()
    {
        // SWITCHING the hand sprites based on the value
        enemyLeftHandSprite.sprite = chopstickHands[Mathf.Clamp(enemyLeftHand - 1, 0, 4)];
        enemyRightHandSprite.sprite = chopstickHands[Mathf.Clamp(enemyRightHand - 1, 0, 4)];

        leftHandSprite.sprite = chopstickHands[Mathf.Clamp(leftHand - 1, 0, 4)];
        rightHandSprite.sprite = chopstickHands[Mathf.Clamp(rightHand - 1, 0, 4)];

        // DISABLING the hand sprites based on the value
        enemyRightHandSprite.enabled = enemyRightHand < 5;
        enemyLeftHandSprite.enabled = enemyLeftHand < 5;

        leftHandSprite.enabled = leftHand < 5;
        rightHandSprite.enabled = rightHand < 5;

        // clap button activates when one hand is dead
        clapButton.SetActive((1 < leftHand && rightHand >= 5) || ( 1 < rightHand && leftHand >= 5));
    }

    void ChopsticksWinStates()
    {
        if (!rightHandSprite.enabled && !leftHandSprite.enabled)
        {
            currChopstickState = ChopsticksGameStates.PlayerLose;

            chopstickWinImage.gameObject.SetActive(true);
            chopstickWinImage.sprite = chapteh.chaptehLoseSprite;

            StopCoroutine(ChopsticksEnemyTurn());
            AudioManager.instance.StopAudio(bgmAudio);

            return;
        }

        if (!enemyRightHandSprite.enabled && !enemyLeftHandSprite.enabled)
        {
            currChopstickState = ChopsticksGameStates.PlayerWin;
            chopstickWinImage.gameObject.SetActive(true);
            chopstickWinImage.sprite = chapteh.chaptehWinSprite;
            StopCoroutine(ChopsticksEnemyTurn());
            AudioManager.instance.StopAudio(bgmAudio);

            return;
        }
    }

    public void ResetChopsticks()
    {
        turnCounter = 0;

        leftHand = 1; 
        rightHand = 1;
        enemyLeftHand = 1;
        enemyRightHand = 1;

        currChopstickState = ChopsticksGameStates.MidGame;

        isPlayerTurn = true;

        chopstickWinImage.gameObject.SetActive(false);

        ChopsticksEndOfTurn();
    }

    #endregion

    #region Bubble Blowing
    #endregion

}

