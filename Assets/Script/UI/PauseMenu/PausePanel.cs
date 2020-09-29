using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : MonoBehaviour
{
    public PauseMenu pausedPanel;
    public BackPack backPack;

    bool isActivation = false;
    bool isControllable = false;
    [HideInInspector]
    public float activeSceond = 0.5f;

    Player player;

    void Start()
    {
        player = GameManager.Instance.player;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isActivation && !isControllable)
            {
                EnterPanel();
            }
            else if (isActivation && isControllable)
            {
                ExitPanel();
            }
        }
    }

    public void EnterPanel()
    {
        GameManager.Instance.PauseGame();
        player.PlayerPause();
        isActivation = true;
        pausedPanel.SetActivation(true);
        backPack.SetActivation(true);

        StartCoroutine(DelayNegate(isControllable));
    }

    public void ExitPanel()
    {

        GameManager.Instance.QuitPauseGame();
        player.PlayerQuitPause();
        isControllable = false;
        pausedPanel.SetActivation(false);
        backPack.SetActivation(false);

        StartCoroutine(DelayNegate(isActivation));
    }

    /// <summary>
    /// 取反两个布尔控制参数
    /// </summary>
    /// <param name="isExit"></param>
    /// <returns></returns>
    IEnumerator DelayNegate(bool isExit)
    {
        yield return new WaitForSecondsRealtime(activeSceond);
        if (isExit) { isActivation = false; }//完成退场
        else { isControllable = true; }      //完成入场
    }

}
