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

    void Awake()
    {
        myAnimator = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();
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
            Invoke("DelayActivation", pausePanel.ActiveSceond);
        }

        if (!value)
        {
            myAnimator.Play("Exit");
            canvasGroup.interactable = false;
        }
    }

    void DelayActivation()
    {
        canvasGroup.interactable = true;
    }
}
