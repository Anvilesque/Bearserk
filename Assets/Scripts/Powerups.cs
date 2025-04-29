using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerups : MonoBehaviour
{
    private Rigidbody2D rb;
    private Movement mvmt;
    private Bearzooka bz;
    private Clawnch clawnch;
    [SerializeField] GameObject clock;
    private ClockManager clockManager;
    // [SerializeField] PowerupManager pwupMngr;
    private CardCollector cc;

    private List<string> PowerupName;
    private List<List<float>> PowerupData;
    private List<List<int>> PowerupTime;
    public delegate void MethodDelegate(float data);
    private List<MethodDelegate> PowerupFunction;
    private List<float> PowerupDefault;

    // private bool wonkySprintEnabled = false;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mvmt = GetComponent<Movement>();
        bz = GetComponent<Bearzooka>();
        clawnch = GetComponent<Clawnch>();
        clockManager = clock.GetComponent<ClockManager>();
        clockManager.OnClockTick += HandlePowerups;
        clockManager.OnClockBackToZero += BackToZero;
        cc = FindObjectOfType<CardCollector>().gameObject.GetComponent<CardCollector>();

        PowerupName = new List<string>();
        PowerupData = new List<List<float>>();
        PowerupTime = new List<List<int>>();
        PowerupFunction = new List<MethodDelegate>();
        float[] defaults = {mvmt.defaults["sprintMultiplier"], 
                            mvmt.defaults["maxHorizSpeed"],
                            mvmt.defaults["jumpSpeed"],
                            mvmt.defaults["jumpsTotal"],
                            mvmt.defaults["shootCooldown"],
                            mvmt.defaults["shootForce"],
                            mvmt.defaults["clawnchCooldown"],
                            mvmt.defaults["clawnchSpeed"],
                            mvmt.defaults["clawnchDuration"]};
        PowerupDefault = new List<float>(defaults);

        #region AddFunctions
        MethodDelegate iSprintM = IncreaseSprintMultiplier;
        RegisterPowerup("IncreaseSprintMultiplier", iSprintM);

        MethodDelegate iMaxHorizS = IncreaseMaxHorizSpeed;
        RegisterPowerup("IncreaseMaxHorizSpeed", iMaxHorizS);

        MethodDelegate iJumpS = IncreaseJumpSpeed;
        RegisterPowerup("IncreaseJumpSpeed", iJumpS);
        
        MethodDelegate iJumpT = IncreaseJumpTotal;
        RegisterPowerup("IncreaseJumpTotal", iJumpT);
        // AddPowerup(1, 5, iJumpT);

        MethodDelegate dBearzookaCD = DecreaseBearzookaCD;
        RegisterPowerup("DecreaseBearzookaCD", dBearzookaCD);

        MethodDelegate iBearzookaForce = IncreaseBearzookaForce;
        RegisterPowerup("IncreaseBearzookaForce", iBearzookaForce);

        MethodDelegate dClawnchCD = DecreaseClawnchCD;
        RegisterPowerup("DecreaseClawnchCD", dClawnchCD);

        MethodDelegate iClawnchSpeed = IncreaseClawnchSpeed;
        RegisterPowerup("IncreaseClawnchSpeed", iClawnchSpeed);

        MethodDelegate iClawnchDuration = IncreaseClawnchDuration;
        RegisterPowerup("IncreaseClawnchDuration", iClawnchDuration);

        MethodDelegate sHealth = SetMaxHealth;
        RegisterPowerup("SetMaxHealth", sHealth);

        // MethodDelegate wonkySprint = WonkySprint;
        // RegisterPowerup("WonkySprint", wonkySprint);

        #endregion

        // pwupMngr.StartPowerupManager();
        cc.PrepareNextRound();
    }

    void RegisterPowerup(string name, MethodDelegate dele)
    {
        PowerupName.Add(name);
        PowerupTime.Add(new List<int>());
        PowerupData.Add(new List<float>());
        PowerupFunction.Add(dele);
    }

    void HandlePowerups(object sender, ClockManager.ClockArgs e)
    {
        for (int i = 0; i < PowerupTime.Count; i++)
        {
            for (int j = 0; j < PowerupTime[i].Count; j++)
            {
                if (PowerupTime[i][j] == e.time)
                {
                    PowerupFunction[i](PowerupData[i][j]);
                }
            }
        }
        // WonkySprintHelper(e.time);
    }
    public void AddPowerup(float data, int time, string funcName, int duration)
    {
        int i = PowerupName.IndexOf(funcName);
        PowerupData[i].Add(data);
        PowerupTime[i].Add(time);
        PowerupData[i].Add(PowerupDefault[i]);
        PowerupTime[i].Add((time + duration) % 20);
    }

    #region PowerupFunctions

    void IncreaseSprintMultiplier(float multiplier)
    {
        mvmt.sprintMultiplier = multiplier;
    }

    void IncreaseMaxHorizSpeed(float speed)
    {
        mvmt.maxHorizSpeed = speed;
    }

    void IncreaseJumpSpeed(float speed)
    {
        mvmt.jumpSpeed = speed;
    }

    void IncreaseJumpTotal(float numJumps)
    {
        mvmt.jumpsTotal = (int)numJumps;
        // mvmt.jumpsAvailable = mvmt.jumpsTotal;
    }

    void DecreaseBearzookaCD(float cd)
    {
        bz.shootCooldown = cd;
    }

    void IncreaseBearzookaForce(float force)
    {
        bz.shootForce = force;
    }

    void DecreaseClawnchCD(float cd)
    {
        clawnch.clawnchCooldown = cd;
    }

    void IncreaseClawnchSpeed(float speed)
    {
        clawnch.clawnchSpeed = speed;
    }

    void IncreaseClawnchDuration(float duration)
    {
        clawnch.clawnchDuration = duration;
    }

    void SetMaxHealth(float maxHealth)
    {
        GetComponent<Health>().maxHealth = maxHealth;
    }

    #endregion

    void BackToZero(object sender, EventArgs e)
    {
        SoftReset();
    } 

    void SoftReset()
    {
        mvmt.sprintMultiplier = mvmt.defaults["sprintMultiplier"];
        mvmt.maxHorizSpeed = mvmt.defaults["maxHorizSpeed"];
        mvmt.jumpSpeed = mvmt.defaults["jumpSpeed"];
        mvmt.jumpsTotal = (int)mvmt.defaults["jumpsTotal"];

        bz.shootCooldown = mvmt.defaults["shootCooldown"];
        bz.shootForce = mvmt.defaults["shootForce"];

        clawnch.clawnchCooldown = mvmt.defaults["clawnchCooldown"];
        clawnch.clawnchSpeed = mvmt.defaults["clawnchSpeed"];
        clawnch.clawnchDuration = mvmt.defaults["clawnchDuration"];
    }

    public void HardReset()
    {
        for (int i = 0; i < PowerupTime.Count; i++)
        {
            PowerupData[i].Clear();
            PowerupTime[i].Clear();
        }
        GetComponent<Health>().maxHealth = 60f;

        SoftReset();
    }
}
