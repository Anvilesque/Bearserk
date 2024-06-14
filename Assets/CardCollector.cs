using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCollector : MonoBehaviour
{
    private Powerups pwups;
    private Health health;

    private List<float> sprintCards;
    private List<float> horizSpeedCards;
    private List<float> jumpSpeedCards;
    private List<float> jumpTotalCards;
    private List<float> bzCDCards;
    private List<float> bzForceCards;
    private List<float> clawnchCDCards;
    private List<float> clawnchSpeedCards;
    private List<float> clawnchDurationCards;
    public List<int> myCards;
    public List<List<float>> cardValues;
    public float maxHealthIncr;
    public static CardCollector instance;

    public bool firstTime;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        sprintCards = new List<float>() {1.2f, 1.5f, 1.7f, 2.0f, 2.5f, 4f};
        horizSpeedCards = new List<float>() {15, 20, 25, 30, 40, 80};
        jumpSpeedCards = new List<float>() {20, 25, 30, 35, 40, 60};
        jumpTotalCards = new List<float>() {1, 2, 3, 4, 5, 10};
        bzCDCards = new List<float>() {5, 3.5f, 2, 1.5f, 1, 0.1f};
        bzForceCards = new List<float>() {10, 20, 30, 40, 50, 400};
        clawnchCDCards = new List<float>() {9.0f, 6.5f, 4.5f, 3, 2, 0.3f};
        clawnchSpeedCards = new List<float>() {8, 10, 15, 25, 30, 90};
        clawnchDurationCards = new List<float>() {0.3f, 0.7f, 1.5f, 2.5f, 4, 10};
        maxHealthIncr = 10;
        myCards = new List<int>();
        for (int i = 0; i < 10; i++) myCards.Add(0);
        cardValues = new List<List<float>>();
        cardValues.Add(sprintCards);
        cardValues.Add(horizSpeedCards);
        cardValues.Add(jumpSpeedCards);
        cardValues.Add(jumpTotalCards);
        cardValues.Add(bzCDCards);
        cardValues.Add(bzForceCards);
        cardValues.Add(clawnchCDCards);
        cardValues.Add(clawnchSpeedCards);
        cardValues.Add(clawnchDurationCards);

        firstTime = true;
    }

    public void PrepareNextRound()
    {
        pwups = FindObjectOfType<Powerups>().gameObject.GetComponent<Powerups>();
        health = FindObjectOfType<Health>().gameObject.GetComponent<Health>();
        pwups.HardReset();
        pwups.AddPowerup(cardValues[0][myCards[0]], 11, "IncreaseSprintMultiplier", 10);
        pwups.AddPowerup(cardValues[1][myCards[1]], 11, "IncreaseMaxHorizSpeed", 10);
        pwups.AddPowerup(cardValues[2][myCards[2]], 11, "IncreaseJumpSpeed", 10);
        pwups.AddPowerup(cardValues[3][myCards[3]], 11, "IncreaseJumpTotal", 10);
        pwups.AddPowerup(cardValues[4][myCards[4]], 11, "DecreaseBearzookaCD", 10);
        pwups.AddPowerup(cardValues[5][myCards[5]], 11, "IncreaseBearzookaForce", 10);
        pwups.AddPowerup(cardValues[6][myCards[6]], 11, "DecreaseClawnchCD", 10);
        pwups.AddPowerup(cardValues[7][myCards[7]], 11, "IncreaseClawnchSpeed", 10);
        pwups.AddPowerup(cardValues[8][myCards[8]], 11, "IncreaseClawnchDuration", 10);
        health.maxHealth = 30 + myCards[9] * maxHealthIncr;
        health.health = health.maxHealth;
    }

    
}