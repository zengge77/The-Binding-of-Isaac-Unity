using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public PausePanel pausePanel;
    public Button newRun;
    public Button resumeGame;
    public Button exitGame;

    Animator myAnimator;
    CanvasGroup canvasGroup;

    float activeSceond;

    void Awake()
    {
        myAnimator = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();
        activeSceond = pausePanel.activeSceond;
    }

    private void Start()
    {
        newRun.onClick.AddListener(GameManager.Instance.OverloadScene);
        resumeGame.onClick.AddListener(pausePanel.ExitPanel);
        exitGame.onClick.AddListener(Application.Quit);
    }

    public void SetActivation(bool value)
    {
        if (value)
        {
            myAnimator.Play("Enter");
            newRun.Select();
            StartCoroutine(DelayActivation(activeSceond));
        }

        if (!value)
        {
            myAnimator.Play("Exit");
            canvasGroup.interactable = false;
        }
    }

    IEnumerator DelayActivation(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        canvasGroup.interactable = true;
    }
}
