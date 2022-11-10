using UnityEngine;
using TMPro;
using System.Collections;
using Assets.HeroEditor.Common.CharacterScripts;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 玩家类型：
    ///     1. archer：使用PlayerBow类进行弓箭射击
    ///     2. warrior: 使用XXX类进行近战攻击
    ///     3. defender：只防御，不进行攻击
    /// </summary>
    public enum PlayerType
    {
        archer,
        warrior,
        defender
    }
    public PlayerType playerType;

    [SerializeField]
    private string playerName;
    public int level;
    public int hp;
    public int hpMax;
    public int attack;
    // speed为显示数值，time为实际间隔
    public int attackSpeed;
    public float attackTime;

    // 死亡后摧毁对象延后时间
    public float dieTime = 2.0f;

    private int exp;

    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private Image EXPBar;
    [SerializeField]
    private TextMeshProUGUI atkText;
    [SerializeField]
    private TextMeshProUGUI asText;
    [SerializeField]
    private Image HPBar;

    private PlayerBow playerBow;
    private Character character;

    public ParticleSystem levelUpFX;
    public ParticleSystem rageFX;

    private float time;

    private void Awake()
    {
        character = GetComponent<Character>();

        Init();
    }

    private void Update()
    {
        switch (playerType)
        {
            case PlayerType.archer:
                // 在一定时间间隔内自动攻击
                if (Time.time - time >= attackTime)
                {
                    time = Time.time;
                    playerBow.BowAttack();
                }
                break;

        }
    }

    private void Init()
    {
        exp = 0;
        level = 0;
        time = Time.time;

        switch (playerType)
        {
            case PlayerType.archer:
                hpMax = PlayerData.hpMaxOfLevel[level];
                hp = hpMax;
                attack = PlayerData.attackOfLevel[0];
                attackSpeed = PlayerData.attackSpeedOfLevel[0];
                attackTime = PlayerData.attackTimeOfLevel[0];

                playerBow = GetComponent<PlayerBow>();
                character.Equip(character.SpriteCollection.Bow[PlayerData.bowIdOfLevel[0]], HeroEditor.Common.Enums.EquipmentPart.Bow);
                displayText();
                break;
            case PlayerType.warrior:
                break;
            case PlayerType.defender:
                hpMax = 1000000;
                hp = 100000;
                attack = 0;
                attackSpeed = 0;
                attackTime = 0f;
                break;
        }

        
    }

    public void changeName(string _name)
    {
        playerName = _name;

        // refresh UI name text display
        nameText.text = playerName;
    }

    public string getName()
    {
        return playerName;
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
            // 换弓
            character.Equip(character.SpriteCollection.Bow[PlayerData.bowIdOfLevel[level]], HeroEditor.Common.Enums.EquipmentPart.Bow);
            // FX
            levelUpFX.Play();
        }

        displayText();
    }

    // 外部调用，角色受伤
    public void Attacked(int damage)
    {
        if (playerType == PlayerType.defender) return;

        if (hp > damage)
        {
            hp -= damage;
            displayText();
        } else
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
        StartCoroutine(DieCoroutine());

        PlayerManager.Instance.playerDie(gameObject);
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
        levelText.text = "LV. " + level;

        // 根据百分比，编辑exptext，显示升级进度
        if (level == PlayerData.maxLevel)
        {
            EXPBar.fillAmount = 1.0f;
            return;
        } else
        {
            int denominator = PlayerData.expOfLevel[level + 1] - PlayerData.expOfLevel[level];
            int numerator = exp - PlayerData.expOfLevel[level];
            float percent = (numerator * 1.0f) / (denominator * 1.0f);
            EXPBar.fillAmount = percent;
        }

        // 攻击力和攻速
        atkText.text = "ATK\n" + attack;
        asText.text = "AS\n" + attackSpeed;

        // 生命值
        HPBar.fillAmount = hp * 1.0f / hpMax;
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
        playerBow.rageMode = true;

        yield return new WaitForSeconds(time);

        playerBow.rageMode = false;
        rageFX.Stop();
    }
}
