using UnityEngine;
using TMPro;
using System.Collections;
using Assets.HeroEditor.Common.CharacterScripts;
using UnityEngine.UI;
using Assets.HeroEditor.Common.CommonScripts;
using HeroEditor.Common.Enums;
using DanmuGame.events;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 玩家类型：
    ///     1. archer：使用PlayerBow类进行弓箭射击
    ///     2. warrior: 使用XXX类进行近战攻击
    ///     3. defender：无敌防御者
    ///     4. wizard: 使用PlayerMage进行魔法弹射击
    ///     5. gunner: 使用PlayerGun进行子弹射击
    /// </summary>
    public enum PlayerType
    {
        archer,
        warrior,
        defender,
        wizard,
        gunner
    }
    public PlayerType playerType;

    /// <summary>
    /// 玩家状态：
    ///     1. idle，待命
    ///     2. attack，开始自动攻击
    ///     3. rage，狂暴自动攻击
    /// </summary>

    public enum PlayerStatus
    {
        idle,
        attack,
        rage
    }
    public PlayerStatus playerStatus;

    public string playerName;
    public int level;
    public int hp;
    public int hpMax;
    public int attack;
    // speed为显示数值，time为实际间隔
    public int attackSpeed;
    public float attackTime = 2.0f;

    // 死亡后摧毁对象延后时间
    public float dieTime = 2.0f;

    public int exp;

    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI levelText;

    [SerializeField] Slider HPSlider;
    [SerializeField] Slider EXPSlider;
    [SerializeField] Sprite[] levelFlags;
    [SerializeField] Image levelFlagImage;

    private Character character;
    private PlayerWeaponBase playerWeapon;

    public ParticleSystem levelUpFX;
    public ParticleSystem rageFX;

    public PlayerEvent OnPlayerDieEvent;
    public PlayerEvent OnPlayerLevelUpEvent;

    private float time;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        if (playerType == PlayerType.defender) return;
        switch (playerStatus)
        {
            case PlayerStatus.idle:
                break;
            case PlayerStatus.attack:
                if (Time.time - time >= attackTime)
                {
                    time = Time.time;
                    playerWeapon.Attack();
                }
                break;
            case PlayerStatus.rage:
                playerWeapon.DoubleAttack();
                break;
        }
    }

    private void UpdateSkill(int level)
    {
        hpMax = PlayerData.hpMaxOfLevel[level];
        hp = hpMax;
        attack = PlayerData.attackOfLevel[level];
        attackSpeed = PlayerData.attackSpeedOfLevel[level];
        attackTime = PlayerData.attackTimeOfLevel[level];
    }

    private void InitWeapon()
    {
        switch (playerType)
        {
            case PlayerType.archer:
                playerWeapon = GetComponent<PlayerBow>();
                break;
            case PlayerType.defender:
                break;
            case PlayerType.gunner:
                playerWeapon = GetComponent<PlayerGun>();
                break;
            case PlayerType.warrior:
                break;
            case PlayerType.wizard:
                playerWeapon = GetComponent<PlayerMage>();
                break;
        }
    }

    private void Init()
    {
        character = GetComponent<Character>();
        exp = 0;
        level = 0;
        time = Time.time;

        // TO DO 应该由外部控制状态
        playerStatus = PlayerStatus.attack;

        UpdateSkill(0);

        switch (playerType)
        {
            case PlayerType.archer:
                playerWeapon = GetComponent<PlayerBow>();
                character.Equip(character.SpriteCollection.Bow[PlayerData.bowIdOfLevel[0]], EquipmentPart.Bow);
                displayText();
                break;
            case PlayerType.warrior:
                break;
            case PlayerType.defender:
                // 重写无敌防御者的属性
                hpMax = 1000000;
                hp = 100000;
                attack = 0;
                attackSpeed = 0;
                attackTime = 0f;
                break;
            case PlayerType.wizard:
                playerWeapon = GetComponent<PlayerMage>();
                break;
            case PlayerType.gunner:
                playerWeapon = GetComponent<PlayerGun>();
                break;
        }


    }

    // 获取战力
    // 战力计算方式：1分钟内平均输出伤害量
    public int GetCP()
    {
        float attackInternalTime = PlayerData.bowChargeTime + attackTime;
        float cp = 60.0f / attackInternalTime * attack;
        return (int)cp;
    }

    public void changeName(string _name)
    {
        playerName = _name;

        // refresh UI name text display
        nameText.text = playerName;
    }

    // 更改职业
    public void changeType(PlayerType newPlayerType)
    {
        playerType = newPlayerType;
        InitWeapon();
    }

    // 外部调用，提升角色exp，需要计算等级提升等
    public void addExp(int newExp)
    {
        if (playerType == PlayerType.defender) return;

        exp += newExp;

        // level up
        if (level + 1 <= PlayerData.maxLevel && exp >= PlayerData.expOfLevel[level + 1])
        {
            level++;

            // 做出升级后的强化...
            attack = PlayerData.attackOfLevel[level];
            attackSpeed = PlayerData.attackSpeedOfLevel[level];
            attackTime = PlayerData.attackTimeOfLevel[level];
            hpMax = PlayerData.hpMaxOfLevel[level];
            hp = hpMax;

            // FX
            levelUpFX.Play();

            // 更换武器
            switch (playerType)
            {
                case PlayerType.archer:
                    // 换弓
                    character.Equip(character.SpriteCollection.Bow[PlayerData.bowIdOfLevel[level]], EquipmentPart.Bow);
                    break;
            }

            OnPlayerLevelUpEvent.Raise(this);
        }

        displayText();
    }

    // 外部调用，角色随机化
    public void Randomize()
    {
        Character character = GetComponent<Character>();
        character.Equip(character.SpriteCollection.Helmet.Random(), EquipmentPart.Helmet);
        character.Equip(character.SpriteCollection.Armor.Random(), EquipmentPart.Armor);
        character.SetBody(character.SpriteCollection.Hair.Random(), BodyPart.Hair, CharacterExtensions.RandomColor);
        character.SetBody(character.SpriteCollection.Eyebrows.Random(), BodyPart.Eyebrows);
        character.SetBody(character.SpriteCollection.Eyes.Random(), BodyPart.Eyes, CharacterExtensions.RandomColor);
        character.SetBody(character.SpriteCollection.Mouth.Random(), BodyPart.Mouth);
    }

    // 外部调用，角色受伤
    public void Attacked(int damage)
    {
        if (playerType == PlayerType.defender) return;

        if (hp > damage)
        {
            hp -= damage;
            displayText();
        }
        else
        {
            hp = 0;
            displayText();
            Die();
        }
    }

    // 角色死亡
    private void Die()
    {
        character.Animator.SetBool("Ready", false);
        character.Animator.SetInteger("State", 6);
        OnPlayerDieEvent.Raise(this);
        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine()
    {
        yield return new WaitForSeconds(dieTime);
        Destroy(gameObject);
    }

    // 更新人物附近的文字
    private void displayText()
    {
        // level
        levelText.text = level.ToString();
        levelFlagImage.sprite = levelFlags[level];

        // 根据百分比，编辑exptext，显示升级进度
        if (level == PlayerData.maxLevel)
        {
            EXPSlider.value = 1.0f;
            return;
        }
        else
        {
            int denominator = PlayerData.expOfLevel[level + 1] - PlayerData.expOfLevel[level];
            int numerator = exp - PlayerData.expOfLevel[level];
            float percent = (numerator * 1.0f) / (denominator * 1.0f);
            EXPSlider.value = percent;
        }

        // 生命值
        HPSlider.value = hp * 1.0f / hpMax;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (playerType)
        {
            case PlayerType.warrior:
                // attack
                break;
            case PlayerType.defender:
                if (collision.gameObject.GetComponent<MonsterBehavior>() != null)
                {
                    GetComponent<Character>().Animator.SetTrigger("Slash");
                    collision.gameObject.GetComponent<MonsterBehavior>().getHit(100000f, this);
                }
                break;
        }
    }

    // 供外部调用，开始rage模式
    public void StartRage(float time)
    {
        StartCoroutine(RageCoroutine(time));
    }

    IEnumerator RageCoroutine(float time)
    {
        rageFX.Play();
        PlayerStatus oldPlayerStatus = playerStatus;
        playerStatus = PlayerStatus.rage;

        yield return new WaitForSeconds(time);

        playerStatus = oldPlayerStatus;
        rageFX.Stop();
    }
}
