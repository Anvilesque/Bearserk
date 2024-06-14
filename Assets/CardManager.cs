using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    private CardCollector cc;
    private int upgradesLeft;
    private int devClicks;
    private List<string> description;
    [SerializeField] private TMPro.TextMeshProUGUI upgradeText;
    [SerializeField] private GameObject buttons;

    // Start is called before the first frame update
    void Start()
    {
        description = new List<string>();
        description.Add("Sprint Multiplier");
        description.Add("Walking Speed");
        description.Add("Jump Speed");
        description.Add("Number of Jumps");
        description.Add("Bearzooka Cooldown");
        description.Add("Bearzooka Force");
        description.Add("Clawnch Cooldown");
        description.Add("Clawnch Speed");
        description.Add("Clawnch Duration");
        description.Add("MaxHealth");
        upgradesLeft = 2;
        devClicks = 0;
        upgradeText.text = "Upgrades Left: " + upgradesLeft;
        cc = FindObjectOfType<CardCollector>().GetComponent<CardCollector>();

        for (int i = 0; i < 9; i++)
        {
            buttons.transform.GetChild(i).Find("Description").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = description[i] + "\nCurrent Value: " + cc.cardValues[i][cc.myCards[i]] + "\nUpgrades Left: " + (5 - cc.myCards[i]);
        }
        buttons.transform.GetChild(9).Find("Description").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = description[9] + "\nCurrent Value: " + (30 + cc.maxHealthIncr * cc.myCards[9]);
    }

    public void UpdateCard()
    {
        int buttonNum = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        if (upgradesLeft > 0 && buttonNum == 9)
        {
            upgradesLeft -= 1;
            upgradeText.text = "Upgrades left: " + upgradesLeft;
            cc.myCards[9] += 1;
            buttons.transform.GetChild(9).Find("Description").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = description[9] + "\nCurrent Value: " + (30 + cc.maxHealthIncr * cc.myCards[9]);
        }
        else if (upgradesLeft > 0 && cc.myCards[buttonNum] < 5)
        {
            upgradesLeft -= 1;
            upgradeText.text = "Upgrades Left: " + upgradesLeft;
            cc.myCards[buttonNum] += 1;
            buttons.transform.GetChild(buttonNum).Find("Description").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = description[buttonNum] + "\nCurrent Value: " + cc.cardValues[buttonNum][cc.myCards[buttonNum]] + "\nUpgrades Left: " + (5 - cc.myCards[buttonNum]);
        }
    }

    public void Continue()
    {   
        SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Single);
        FindObjectOfType<SoundManager>().gameObject.GetComponent<SoundManager>().Play(FindObjectOfType<SoundManager>().gameObject.GetComponent<SoundManager>().songs[1]);
    }

    public void DevUpgrades()
    {
        devClicks++;
        if (devClicks >= 10)
        {
            upgradesLeft = 99;
            upgradeText.text = "Upgrades Left: " + upgradesLeft;
        }
    }
}
