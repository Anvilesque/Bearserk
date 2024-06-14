using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClockVisualizer : MonoBehaviour
{
    
    private ClockManager clockManager;
    private TextMeshProUGUI text;
    private float countdownTime;

    // Start is called before the first frame update
    void Start()
    {
        clockManager = GetComponent<ClockManager>();
        text = GetComponent<TextMeshProUGUI>();
        countdownTime = Mathf.CeilToInt(clockManager.counter);
    }

    // Update is called once per frame
    void Update()
    {
        countdownTime = Mathf.CeilToInt(clockManager.counter);
        text.text = countdownTime.ToString();
    }
}
