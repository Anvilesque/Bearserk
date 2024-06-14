using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public bool isPaused = false;
    public static PauseManager instance = null;
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
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0) isPaused = true;
        else isPaused = false;
    }

    public void ForcePause()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
    }

    public void ForceUnpause()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
    }

    public void Pause()
    {
        if (TutorialManager.tutorialMode) return;
        if (!isPaused)
        {
            Time.timeScale = 0;
            AudioListener.pause = true;
        }
        else
        {
            Time.timeScale = 1;
            AudioListener.pause = false;
        }
        
    }
}
