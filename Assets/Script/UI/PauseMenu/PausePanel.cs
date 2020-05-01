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
    public float ActiveSceond = 0.5f;

    Player player;

    void Start()
    {
        player = GameManager.Instance.player;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EnterPanel();
            ExitPanel();
        }
    }

    public void EnterPanel()
    {
        if (!isActivation && !isControllable)
        {
            player.PlayerPause();
            isActivation = true;
            pausedPanel.SetActivation(true);
            backPack.SetActivation(true);

            StartCoroutine(Negate(isControllable));
        }
    }

    public void ExitPanel()
    {
        if (isActivation && isControllable)
        {
            player.PlayerQuitPause();
            isControllable = false;
            pausedPanel.SetActivation(false);
            backPack.SetActivation(false);

            StartCoroutine(Negate(isActivation));
        }
    }

    /// <summary>
    /// 取反两个布尔控制参数
    /// </summary>
    /// <param name="isExit"></param>
    /// <returns></returns>
    IEnumerator Negate(bool isExit)
    {
        yield return new WaitForSeconds(ActiveSceond);

        if (isExit) { isActivation = false; }//完成退场
        else { isControllable = true; }      //完成入场
    }
}
