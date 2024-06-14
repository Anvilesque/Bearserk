using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public static bool tutorialMode;
    public List<GameObject> slides;
    private int index;
    private PauseManager pauser;
    private CardCollector cc;

    // Start is called before the first frame update
    void Start()
    {
        cc = FindObjectOfType<CardCollector>().gameObject.GetComponent<CardCollector>();
        if (cc.firstTime == true) tutorialMode = true;
        else
        {
            tutorialMode = false;
            GetComponent<SpriteRenderer>().enabled = false;
        }
        foreach (GameObject obj in slides)
        {
            obj.SetActive(false);
        }
        if (tutorialMode)
        {
            slides[0].SetActive(true);
            index = 1;
            pauser = FindObjectOfType<PauseManager>().GetComponent<PauseManager>();
            if (pauser.isPaused == false) pauser.ForcePause();
        }
        
    }

    public void NextSlide(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!tutorialMode) return;
        if (index <= slides.Count - 1)
        {
            slides[index - 1].SetActive(false);
            slides[index].SetActive(true);
            index++;
        }
        else
        {
            slides[index - 1].SetActive(false);
            pauser.ForceUnpause();
            tutorialMode = false;
            GetComponent<SpriteRenderer>().enabled = false;
            FindObjectOfType<SoundManager>().gameObject.GetComponent<SoundManager>().Play(FindObjectOfType<SoundManager>().gameObject.GetComponent<SoundManager>().songs[1]);
            cc.firstTime = false;
        }
    }
}
