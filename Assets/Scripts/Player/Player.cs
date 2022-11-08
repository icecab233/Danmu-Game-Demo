using UnityEngine;
using TMPro;
using System.Collections;
using Assets.HeroEditor.Common.CharacterScripts;

public class Player : MonoBehaviour
{
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
    private TextMeshProUGUI expText;
    [SerializeField]
    private TextMeshProUGUI atkText;
    [SerializeField]
    private TextMeshProUGUI asText;
    [SerializeField]
    private TextMeshProUGUI healthText;

    private PlayerBow playerBow;
    private Character character;

    private float time;

    private void Awake()
    {
        playerBow = GetComponent<PlayerBow>();
        character = GetComponent<Character>();

        Init();
    }

    private void Update()
    {
        // 在一定时间间隔内自动攻击
        if (Time.time - time >= attackTime)
        {
            time = Time.time;
            playerBow.BowAttack();
        }
    }

    private void Init()
    {
        exp = 0;
        level = 0;
        hp = 100;
        hpMax = 100;
        time = Time.time;

        attack = PlayerData.attackOfLevel[0];
        attackSpeed = PlayerData.attackSpeedOfLevel[0];
        attackTime = PlayerData.attackTimeOfLevel[0];

        character.Equip(character.SpriteCollection.Bow[PlayerData.bowIdOfLevel[0]], HeroEditor.Common.Enums.EquipmentPart.Bow);

        displayText();
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
        }

        displayText();
    }

    // 外部调用，角色受伤
    public void Attacked(int damage)
    {
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

        PlayerManager.playerDie(gameObject);
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
            expText.text = "NEXT: 100%";
            return;
        } else
        {
            int denominator = PlayerData.expOfLevel[level + 1] - PlayerData.expOfLevel[level];
            int numerator = exp - PlayerData.expOfLevel[level];
            double percent = (numerator * 1.0) / (denominator * 1.0) * 100;
            expText.text = "NEXT: " + (int)percent + "%";
        }

        // 攻击力和攻速
        atkText.text = "ATK\n" + attack;
        asText.text = "AS\n" + attackSpeed;

        // 生命值
        healthText.text = "HP: " + hp + "/" + hpMax;
    }
}
