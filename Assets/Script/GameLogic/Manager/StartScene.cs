using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public Button startButton;
    public Button exitButton;

    private void OnEnable()
    {
        FillDate();
        startButton.onClick.AddListener(StartGame);
        exitButton.onClick.AddListener(ExitGame);
    }

    private void OnDisable()
    {
        startButton.onClick.RemoveListener(StartGame);
        startButton.onClick.RemoveListener(ExitGame);
    }

    void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    void ExitGame()
    {
        Application.Quit();
    }

    void FillDate()
    {
        ItemDateFromJson.InitializeItemInfoList();
    }
}
