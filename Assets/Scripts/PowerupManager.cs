using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    public List<PowerupCard> collectedCards;

    private Powerups pwup;

    public void StartPowerupManager()
    {
        pwup = FindObjectOfType<Powerups>().GetComponent<Powerups>();
        ActivateAllPowerups();
    }

    void AddPowerupCard(PowerupCard card)
    {
        collectedCards.Add(card);
    }

    void ActivatePowerup(PowerupCard card)
    {
        pwup.AddPowerup(card.data, card.time, card.functionName, card.duration);
    }

    void ActivateAllPowerups()
    {
        foreach (PowerupCard card in collectedCards)
        {
            ActivatePowerup(card);
        }
    }

}
