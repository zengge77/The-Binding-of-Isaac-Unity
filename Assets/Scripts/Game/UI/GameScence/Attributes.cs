using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Attributes : MonoBehaviour
{
    Player player;

    public Text Coin;
    public Text Key;
    public Text Bomb;
    public Text Speed;
    public Text Rang;
    public Text TearsDelay;
    public Text ShotSpeed;
    public Text Damage;
    public Text Luck;

    void Start()
    {
        player = GameManager.Instance.player;
    }

    public void UpDateAttributes()
    {
        Coin.text = player.CoinNum.ToString();
        Key.text = player.KeyNum.ToString();
        Bomb.text = player.BombNum.ToString();
        Speed.text = String.Format("{0:0.00}", player.Speed); 
        Rang.text = String.Format("{0:0.00}", player.Range);
        TearsDelay.text = String.Format("{0:0.00}", player.TearsDelay);
        ShotSpeed.text = String.Format("{0:0.00}", player.ShotSpeed);
        Damage.text = String.Format("{0:0.00}", player.Damage);
        Luck.text = String.Format("{0:0.00}", player.Luck);
    }
}
