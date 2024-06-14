using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClawnchUIFill : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Image img;
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        img.fillAmount = 1 - Mathf.Min(player.GetComponent<Clawnch>().clawnchTimer / player.GetComponent<Clawnch>().clawnchCooldown, 1);
    }
}
