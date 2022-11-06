using UnityEngine;
using TMPro;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField]
    private string playerName;
    public int level;
    public int hp;
    public int hpMax;
    public int attack;
    // speedΪ��ʾ��ֵ��timeΪʵ�ʼ��
    public int attackSpeed;
    public float attackTime;

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

    private PlayerBow playerBow;

    private float time;

    private void Start()
    {
        Init();

        playerBow = GetComponent<PlayerBow>();
    }

    private void Update()
    {
        // ��һ��ʱ�������Զ�����
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

        displayWhenLevelUp();
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

    // �ⲿ���ã�������ɫexp����Ҫ����ȼ�������
    public void addExp(int newExp)
    {
        exp += newExp;

        // level up
        if (level + 1 <= PlayerData.maxLevel && exp >= PlayerData.expOfLevel[level + 1])
        {
            level++;

            // �����������ǿ��...
            attack = PlayerData.attackOfLevel[level];
            attackSpeed = PlayerData.attackSpeedOfLevel[level];
            attackTime = PlayerData.attackTimeOfLevel[level];
        }

        displayWhenLevelUp();
    }

    // �������󣬸������︽��������
    private void displayWhenLevelUp()
    {
        // level
        levelText.text = "LV. " + level;

        // ���ݰٷֱȣ��༭exptext����ʾ��������
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

        // �������͹���
        atkText.text = "ATK\n" + attack;
        asText.text = "AS\n" + attackSpeed;
    }
}
