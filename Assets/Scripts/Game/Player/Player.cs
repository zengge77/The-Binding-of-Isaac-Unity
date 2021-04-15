using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Player : MonoBehaviour, IAttackable
{
    //血量
    private int maxHealth;
    public int MaxHealth
    {
        get { return maxHealth; }
        private set { maxHealth = Mathf.Clamp(value, 0, 24); }
    }
    private int health;
    public int Health
    {
        get { return health; }
        private set { health = Mathf.Clamp(value, 0, maxHealth); }
    }
    private int soulHealth;
    public int SoulHealth
    {
        get { return soulHealth; }
        private set { soulHealth = value > 0 ? value : 0; }
    }
    public enum HealthType { Normal, Soul }

    //移速
    public float Speed { get; set; }
    public float SpeedMultiple { get; set; }

    //射程Range
    private float range;
    public float Range
    {
        get { return range; }
        set { range = value > 5f ? value : 5f; }
    }

    //射速Tears
    //用于计算射击延迟,一般道具的射速效果作用于此
    public float Tears { get; set; }
    //射击延迟,用于计算射击间隔,某些道具作用于Multiple（倍数）和Added（额外）
    public int TearsDelay
    {
        get
        {
            int temp;
            if (Tears >= 0)
            {
                temp = (int)(16 - 6 * Mathf.Sqrt(1.3f * Tears + 1));
            }
            else if (Tears >= -10f / 13f)
            {
                temp = (int)((16 - 6 * Mathf.Sqrt(1.3f * Tears + 1)) - 6 * Tears);
            }
            else
            {
                temp = (int)(16 - 6 * Tears);
            }

            temp = temp > 5 ? temp : 5;
            temp = temp * TearsDelayMultiple + TearsDelayAdded;
            return temp > 1 ? temp : 1;
        }
    }
    public int TearsDelayMultiple { get; set; }
    public int TearsDelayAdded { get; set; }
    //射击间隔,用于实际的发射计算
    private float ShotCD
    {
        get { return 1f / (30f / (TearsDelay + 1)); }
    }
    //射击计时
    private float shotTiming;

    //弹速ShotSpeed
    private float shotSpeed;
    public float ShotSpeed
    {
        get { return shotSpeed; }
        set { shotSpeed = value > 0.6f ? value : 0.6f; }
    }

    //击退
    public float Knockback { get; set; }

    //伤害  
    public float Damage
    {
        //伤害 = 3.5 * 伤害倍数 * √(基础伤害 * 1.2f + 1f)+ 额外伤害
        get { return (float)Math.Round(3.5f * DamageMultiple * Mathf.Sqrt((DamageBase * 1.2f + 1f)) + DamageAdded, 2); }
    }
    public float DamageMultiple { get; set; }
    public float DamageBase { get; set; }
    public float DamageAdded { get; set; }

    //幸运
    public int Luck { get; set; }

    //身上的道具
    List<ItemInformation> itemInformationList;
    public int CoinNum { get; set; }
    public int KeyNum { get; set; }
    public int BombNum { get; set; }

    //状态
    [HideInInspector]
    public bool isLive;
    bool isControllable;
    bool isInvincible;
    Vector2 moveInput;

    //能与子弹发生互动的对象类列表
    [HideInInspector]
    public List<string> TagThatDefaultByBullet;
    [HideInInspector]
    public List<Type> TypeThatDefaultByBullet;
    [HideInInspector]
    public List<Type> TypeThatCanBeAttackedByBullet;
    [HideInInspector]
    public List<Type> TypeThatCanBeDestroyedByBullet;
    //穿透能力
    [HideInInspector]
    public bool penetrating = false;

    [Header("自身")]
    public Transform head;
    public Transform body;
    Rigidbody2D myRigidbody;
    SpriteRenderer bodyRanderer;
    SpriteRenderer headRanderer;
    Animator wholeAnimation;
    Animator headAnimation;
    Animator bodyAnimation;

    [Header("其他")]
    public BulletPools bulletPool;
    public Transform bulletContainer;
    public TheBomb bombPrefab;
    UIManager UI;
    Level level;

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        headRanderer = head.GetComponent<SpriteRenderer>();
        bodyRanderer = body.GetComponent<SpriteRenderer>();
        wholeAnimation = GetComponent<Animator>();
        headAnimation = head.GetComponent<Animator>();
        bodyAnimation = body.GetComponent<Animator>();
        itemInformationList = new List<ItemInformation>();
    }

    void Start()
    {
        level = GameManager.Instance.level;
        UI = UIManager.Instance;

        PlayerInitialize();
    }

    void Update()
    {
        //死亡及复活测试
        if (Input.GetKey(KeyCode.O))
        {
            PlayerInitialize();
        }
        if (Input.GetKey(KeyCode.P))
        {
            PlayerDeath();
        }

        if (!isControllable) { return; }
        UpdateControl();
        UpdateMovement();
        UpdateAnimator();
    }

    /// <summary>
    /// 控制输入
    /// </summary>
    void UpdateControl()
    {
        shotTiming += Time.deltaTime;
        if (shotTiming >= ShotCD)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                LaunchBullet(KeyCode.UpArrow);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                LaunchBullet(KeyCode.DownArrow);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                LaunchBullet(KeyCode.LeftArrow);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                LaunchBullet(KeyCode.RightArrow);
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            GenerateBomb();
        }
    }

    /// <summary>
    /// 移动
    /// </summary>
    void UpdateMovement()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        moveInput = h * Vector2.right + v * Vector2.up;
        //归一化，取值0 - 1,斜着走的速度不会超过移动速度。
        if (moveInput.magnitude > 1f)
        {
            moveInput.Normalize();
        }
        //myRigidbody.velocity = moveInput * Speed * SpeedMultiple * 1.7f;
        myRigidbody.velocity = moveInput * (0.5f + 0.5f * Speed) * SpeedMultiple * 1.7f;
    }

    /// <summary>
    /// 更新动画
    /// </summary>
    void UpdateAnimator()
    {
        if (moveInput.x < 0) { bodyRanderer.flipX = true; }
        if (moveInput.x > 0) { bodyRanderer.flipX = false; }
        bodyAnimation.SetFloat("Up&Down", Mathf.Abs(moveInput.y));
        bodyAnimation.SetFloat("Left&Right", Mathf.Abs(moveInput.x));
    }

    /// <summary>
    /// 按下按键发射子弹
    /// </summary>
    void LaunchBullet(KeyCode key)
    {
        int force = 120;//施加给子弹主方向的力
        int mainCorrect;//子弹主方向修正值
        int minorCorrect = 50;//子弹次方向修正值
        GameObject bullet = bulletPool.Take();
        Rigidbody2D rigidbody = bullet.GetComponent<Rigidbody2D>();
        bullet.GetComponent<Bullet>().Initialization();
        bullet.transform.position = transform.position + new Vector3(0, 0.0055f * Range - 0.13f, 0);

        if (key == KeyCode.UpArrow)
        {
            headAnimation.Play("Up");
            //根据发射方向与人物当前移动方向赋予子弹初始一个力
            //以下面的公式为例，赋予子弹的力量大小为：new vector2(次方向上的力，主方向上的力)
            //写了注释感觉更晕了，自己体会吧
            mainCorrect = 15;
            rigidbody.AddForce(new Vector2(moveInput.x * minorCorrect, moveInput.y * mainCorrect + force) * ShotSpeed);
        }
        else if (key == KeyCode.DownArrow)
        {
            headAnimation.Play("Down");
            mainCorrect = 15;
            rigidbody.AddForce(new Vector2(moveInput.x * minorCorrect, moveInput.y * mainCorrect - force) * ShotSpeed);
        }
        else if (key == KeyCode.LeftArrow)
        {
            headAnimation.Play("Left");
            //修改mainCorrect使得发射方向与人物移动方向: 同方向时推力更大，反方向时阻力更小
            mainCorrect = moveInput.x >= 0 ? 5 : 40;
            rigidbody.AddForce(new Vector2(moveInput.x * mainCorrect - force, moveInput.y * minorCorrect) * ShotSpeed);
        }
        else if (key == KeyCode.RightArrow)
        {
            headAnimation.Play("Right");
            mainCorrect = moveInput.x >= 0 ? 40 : 5;
            rigidbody.AddForce(new Vector2(moveInput.x * mainCorrect + force, moveInput.y * minorCorrect) * ShotSpeed);
        }

        shotTiming = 0;
    }

    /// <summary>
    /// 设置炸弹
    /// </summary>
    void GenerateBomb()
    {
        if (BombNum >= 1)
        {
            BombNum--;
            Vector2 pos = transform.position + new Vector3(0, -0.15f);
            Instantiate<TheBomb>(bombPrefab, pos, Quaternion.identity);
            UI.attributes.UpDateAttributes();
        }
    }

    /// <summary>
    /// 受到攻击
    /// </summary>
    /// <param name="damage"></param>
    public void BeAttacked(float damage, Vector2 direction, float forceMultiple = 1)
    {
        if (isInvincible || !isLive) { return; }
        ReduceHealth((int)damage);
        if (isLive)
        {
            StartCoroutine(knockBackCoroutine(direction * forceMultiple));
            StartCoroutine(Invincible());
        }
    }

    /// <summary>
    /// 被击退效果
    /// </summary>
    /// <param name="force"></param>
    /// <returns></returns>
    IEnumerator knockBackCoroutine(Vector2 force)
    {
        //降低输入操作带来的移动量
        SpeedMultiple = 0.5f;

        float length = 0.3f;
        float overTime = 0.1f;
        float timeleft = overTime;
        while (timeleft > 0)
        {
            //overTime时间内移动direction * length的距离
            transform.Translate(force * length * Time.deltaTime / overTime);
            timeleft -= Time.deltaTime;
            yield return null;
        }

        //还原
        SpeedMultiple = 1;
    }
    /// <summary>
    /// 进入无敌状态并闪烁
    /// </summary>
    IEnumerator Invincible()
    {
        isInvincible = true;
        Color red = new Color(1, 0.2f, 0.2f, 1);

        float time = 0;//计时
        float flashCD = 0;//闪烁计时

        while (time < 1f)
        {
            time += Time.deltaTime;
            flashCD += Time.deltaTime;
            if (flashCD > 0)
            {
                if (bodyRanderer.color == Color.white)
                {
                    bodyRanderer.color = red;
                    headRanderer.color = red;
                }
                else if (bodyRanderer.color == red)
                {
                    bodyRanderer.color = Color.white;
                    headRanderer.color = Color.white;
                }
                flashCD -= 0.13f;
            }
            yield return null;
        }
        isInvincible = false;
    }

    /// <summary>
    /// 加血
    /// </summary>
    /// <param name="health"></param>
    /// <param name="type"></param>
    /// <param name="maxHealth"></param>
    public void AddHealth(int health, HealthType type, int maxHealth = 0)
    {
        this.MaxHealth += maxHealth;
        switch (type)
        {
            case HealthType.Normal:
                Health += health;
                break;
            case HealthType.Soul:
                SoulHealth += health;
                break;
            default:
                break;
        }

        UI.hp.UpdateHP();
    }
    /// <summary>
    /// 扣血
    /// </summary>
    /// <param name="damage"></param>
    public void ReduceHealth(int damage)
    {
        int tempHealth;

        //若以后新增血量类型，只需再写多一层计算即可
        tempHealth = SoulHealth;
        SoulHealth -= damage;
        damage = damage - tempHealth > 0 ? damage - tempHealth : 0;

        Health -= damage;
        UI.hp.UpdateHP();
        if (Health == 0) { PlayerDeath(); }
    }

    /// <summary>
    /// 状态初始化
    /// </summary>
    public void PlayerInitialize()
    {
        MaxHealth = 6;
        Health = MaxHealth;
        SoulHealth = 0;

        Speed = 1f;
        SpeedMultiple = 1f;

        Range = 23.75f;
        Tears = 0;
        TearsDelayMultiple = 1;
        TearsDelayAdded = 0;
        shotTiming = 0;
        ShotSpeed = 1;
        Knockback = 1;

        DamageMultiple = 1f;
        DamageBase = 0f;
        DamageAdded = 0f;

        Luck = 0;

        CoinNum = 10;
        KeyNum = 1;
        BombNum = 10;

        isLive = true;
        isControllable = true;
        isInvincible = false;
        bodyRanderer.enabled = true;
        headRanderer.enabled = true;
        wholeAnimation.SetBool("isLive", true);
        myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        //接触后触发眼泪的自我消除，不触发其他方法的：岩石，铁块
        TypeThatDefaultByBullet = new List<Type>() { typeof(Rock), typeof(MetalBlock) };
        //同上，因为没有类所以使用tag来判断：墙壁等
        TagThatDefaultByBullet = new List<string>() { "Walls", "MoveCollider" };
        //接触后触发碰撞物的被击方法：怪物，火堆，屎堆
        TypeThatCanBeAttackedByBullet = new List<Type>() { typeof(Monster), typeof(Poop), typeof(Fireplace) };
        //接触后触发碰撞物的被毁方法：默认为空
        TypeThatCanBeDestroyedByBullet = new List<Type>() { };

        UI.PlayerUIInitialize();
    }
    public void PlayerDeath()
    {
        isLive = false;
        isControllable = false;
        bodyRanderer.enabled = false;
        headRanderer.enabled = false;
        wholeAnimation.SetBool("isLive", false);
        myRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    public void PlayerPause()
    {
        isControllable = false;
        myRigidbody.velocity = Vector2.zero;
        headAnimation.speed = 0;
        bodyAnimation.speed = 0;
    }
    public void PlayerQuitPause()
    {
        isControllable = true;
        headAnimation.speed = 1;
        bodyAnimation.speed = 1;
    }

    /// <summary>
    /// 添加道具信息到玩家身上
    /// </summary>
    /// <param name="item"></param>
    public void AddItemInformation(Item item)
    {
        ItemInformation newItemInfo = item.itemInformation;
        itemInformationList.Add(newItemInfo);
        UI.pausePanel.backPack.AddBackpackCell(newItemInfo);
    }

    /// <summary>
    /// 查询玩家身上是否已拥有该道具
    /// </summary>
    /// <param name="itemIDList"></param>
    /// <returns></returns>
    public bool IsThisItemExist(params int[] itemIDList)
    {
        for (int i = 0; i < itemIDList.Length; i++)
        {
            int num = itemInformationList.FindIndex((ItemInformation go) => { return go.ID == itemIDList[i]; });
            if (num != -1)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 判断碰撞体并移动到下一个房间
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter2D(Collision2D collision)
    {
        //移动到下一个房间
        if (isControllable && collision.transform.CompareTag("MoveCollider"))
        {
            if (collision.transform.name == "Up")
            {
                level.MoveToNextRoom(Vector2.up);
            }
            else if (collision.transform.name == "Down")
            {
                level.MoveToNextRoom(Vector2.down);
            }
            else if (collision.transform.name == "Left")
            {
                level.MoveToNextRoom(Vector2.left);
            }
            else if (collision.transform.name == "Right")
            {
                level.MoveToNextRoom(Vector2.right);
            }
        }
    }
}