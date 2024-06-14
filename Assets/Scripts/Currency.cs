using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currency : MonoBehaviour
{
    private int broinAmt;
    

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Save()
    {
        PlayerPrefs.SetInt("Broins", broinAmt);
    }

    void Load()
    {
        PlayerPrefs.GetInt("Broins", 0);
    }
}
