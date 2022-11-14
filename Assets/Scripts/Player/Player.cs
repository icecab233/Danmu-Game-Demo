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
    /// ������ͣ�
    ///     1. archer��ʹ��PlayerBow����й������
    ///     2. warrior: ʹ��XXX����н�ս����
    ///     3. defender��ֻ�����������й���
    /// </summary>
    public enum PlayerType
    {
        archer,
        warrior,
        defender
    }
    public PlayerType playerType;

    /// <summary>
    /// ���״̬��
    ///     1. idle������
    ///     2. attack����ʼ�Զ�����
    ///     3. rage�����Զ�����
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
    // speedΪ��ʾ��ֵ��timeΪʵ�ʼ��
    public int attackSpeed;
    public float attackTime;

    // ������ݻٶ����Ӻ�ʱ��
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

    public PlayerEvent PlayerDieEvent;

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
                // ��һ��ʱ�������Զ�����
                if (Time.time - time >= attackTime)
                {
                    time = Time.time;
                    switch (playerStatus)
                    {
                        case PlayerStatus.idle:
                            break;
                        case PlayerStatus.attack:
                            playerBow.BowAttack();
                            break;
                        case PlayerStatus.rage:
                            playerBow.DoubleBowAttack();
                            break;
                    }
                }
                break;

        }
    }

    private void Init()
    {
        exp = 0;
        level = 0;
        time = Time.time;

        // TO DO Ӧ�����ⲿ����״̬
        playerStatus = PlayerStatus.attack;

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

    // ��ȡս��
    // ս�����㷽ʽ��1������ƽ������˺���
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

    // �ⲿ���ã�������ɫexp����Ҫ����ȼ�������
    public void addExp(int newExp)
    {
        if (playerType == PlayerType.defender) return;

        exp += newExp;

        // level up
        if (level + 1 <= PlayerData.maxLevel && exp >= PlayerData.expOfLevel[level + 1])
        {
            level++;

            // �����������ǿ��...
            attack = PlayerData.attackOfLevel[level];
            attackSpeed = PlayerData.attackSpeedOfLevel[level];
            attackTime = PlayerData.attackTimeOfLevel[level];
            hpMax = PlayerData.hpMaxOfLevel[level];
            hp = hpMax;
            // ����
            character.Equip(character.SpriteCollection.Bow[PlayerData.bowIdOfLevel[level]], EquipmentPart.Bow);
            // FX
            levelUpFX.Play();
        }

        displayText();
    }

    // �ⲿ���ã���ɫ�����
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

    // �ⲿ���ã���ɫ����
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

    // ��ɫ����
    private void Die()
    {
        character.Animator.SetBool("Ready", false);
        character.Animator.SetInteger("State", 6);
        PlayerDieEvent.Raise(this);
        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine()
    {
        yield return new WaitForSeconds(dieTime);
        Destroy(gameObject);
    }

    // �������︽��������
    private void displayText()
    {
        // level
        levelText.text = "LV. " + level;

        // ���ݰٷֱȣ��༭exptext����ʾ��������
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

        // �������͹���
        atkText.text = "ATK\n" + attack;
        asText.text = "AS\n" + attackSpeed;

        // ����ֵ
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

    // ���ⲿ���ã���ʼrageģʽ
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
