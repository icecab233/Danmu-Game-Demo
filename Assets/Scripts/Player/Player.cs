using UnityEngine;
using TMPro;
using System.Collections;
using Assets.HeroEditor.Common.CharacterScripts;
using UnityEngine.UI;
using HeroEditor.Common.Enums;
using DanmuGame.events;

public partial class Player : MonoBehaviour
{
    /// <summary>
    /// ������ͣ�
    ///     1. archer��ʹ��PlayerBow����й������
    ///     2. warrior: ʹ��XXX����н�ս����
    ///     3. defender���޵з�����
    ///     4. wizard: ʹ��PlayerMage����ħ�������
    ///     5. gunner: ʹ��PlayerGun�����ӵ����
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

    public int uid;
    public string playerName;
    public int level;
    public int hp;
    public int hpMax;
    public int attack;
    // speedΪ��ʾ��ֵ��timeΪʵ�ʼ��
    public int attackSpeed;
    public float attackTime = 2.0f;

    // ������ݻٶ����Ӻ�ʱ��
    public float dieTime = 2.0f;

    public int exp;

    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI levelText;

    [SerializeField] Slider HPSlider;
    [SerializeField] Slider EXPSlider;
    [SerializeField] Image levelFlagImage;
    [SerializeField] PlayerLevelData playerLevelData;

    private Character character;

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

    private void Init()
    {
        character = GetComponent<Character>();
        exp = 0;
        level = 0;
        time = Time.time;

        // TO DO Ӧ�����ⲿ����״̬
        playerStatus = PlayerStatus.attack;

        if (playerType != PlayerType.defender)
        {
            UpdateSkill();
            UpdateEquip();
        }

        switch (playerType)
        {
            case PlayerType.archer:
                playerWeapon = GetComponent<PlayerBow>();
                displayText();
                break;
            case PlayerType.warrior:
                break;
            case PlayerType.defender:
                // ��д�޵з����ߵ�����
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

    private void UpdateSkill()
    {
        hpMax = PlayerData.hpMaxOfLevel[level];
        hp = hpMax;
        attack = PlayerData.attackOfLevel[level];
        attackSpeed = PlayerData.attackSpeedOfLevel[level];
        attackTime = PlayerData.attackTimeOfLevel[level];
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

    private void levelUp()
    {
        level++;

        // �����������ǿ��...
        UpdateSkill();

        UpdateEquip();

        // FX
        levelUpFX.Play();

        OnPlayerLevelUpEvent.Raise(this);
    }

    // �ⲿ���ã�������ɫexp����Ҫ����ȼ�������
    public void addExp(int newExp)
    {
        if (playerType == PlayerType.defender) return;

        exp += newExp;

        // level up
        if (level + 1 <= PlayerData.maxLevel && exp >= PlayerData.expOfLevel[level + 1])
        {
            levelUp();
        }

        displayText();
    }

    // �ⲿ���ã���ɫ����
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

    // ��ɫ����
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

    // �������︽��������(�ȼ����ȼ�ͼ��)
    private void displayText()
    {
        // level
        levelText.text = level.ToString();
        levelFlagImage.sprite = playerLevelData.getLevelSprites(level);
        //levelFlagImage.sprite = levelFlags[level];

        // ���ݰٷֱȣ��༭exptext����ʾ��������
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

        // ����ֵ
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
