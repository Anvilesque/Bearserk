using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public static bool tutorialModeEnabled;
    public List<GameObject> slides;
    private int index;
    private CardCollector cardCollector;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        cardCollector = FindObjectOfType<CardCollector>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        tutorialModeEnabled = cardCollector.firstTime;

        UndisplaySlidesAll();

        if (tutorialModeEnabled)
        {
            index = 0;
            DisplaySlide(index);
            if (!PauseManager.instance.isPaused) PauseManager.instance.Pause();
        }
        else DisableSlides();
        
        

    }

    public void NextSlide(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!tutorialModeEnabled) return;
        if (index < slides.Count - 2)
        {
            UndisplaySlide(index);
            index++;
            DisplaySlide(index);
        }
        else
        {
            UndisplaySlide(index);
            PauseManager.instance.Pause();
            tutorialModeEnabled = false;
            DisableSlides();
            SoundManager soundManager = FindObjectOfType<SoundManager>();
            soundManager.Play(soundManager.songs[1]);
            cardCollector.firstTime = false;
        }
    }

    private void DisplaySlide(int slideIndex)
    {
        slides[slideIndex].SetActive(true);
    }

    private void UndisplaySlide(int slideIndex)
    {
        slides[slideIndex].SetActive(false);
    }

    private void UndisplaySlidesAll()
    {
        foreach (GameObject obj in slides)
        {
            obj.SetActive(false);
        }
    }

    private void DisableSlides()
    {
        spriteRenderer.enabled = false;
    }
}
