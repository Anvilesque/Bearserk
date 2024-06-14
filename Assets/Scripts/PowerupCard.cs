using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Powerup", menuName = "Powerup Card")]
public class PowerupCard : ScriptableObject
{
    public string cardName;
    public string description;
    public int rarity;
    public int cost;

    public string functionName;
    public float data;
    public int time;
    public int duration;
}
