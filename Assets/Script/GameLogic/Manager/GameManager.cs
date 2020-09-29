using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public Player playerPrefab;
    public Level levelPrefab;
    public Camera myCamera;

    [HideInInspector]
    public Player player;
    [HideInInspector]
    public Level level;

    [Header("测试模式")]
    public bool isUseTestMod;
    public RoomLayout startRoomTestSample;
    public RoomLayout normalRoomTestSample;
    public RoomLayout bossRoomTestSample;
    public RoomLayout treasureRoomTestSample;

    private void Start()
    {
        LoadNewGame();
    }

    void LoadNewGame()
    {
        player = Instantiate(playerPrefab);
        level = Instantiate(levelPrefab);
    }

    public void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void SwitchScene(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }
    public void OverloadScene()
    {
        QuitPauseGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        Time.fixedDeltaTime = 0;
    }
    public void QuitPauseGame()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;
    }
}
