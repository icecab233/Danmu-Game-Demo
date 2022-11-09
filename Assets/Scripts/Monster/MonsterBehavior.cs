using Assets.FantasyMonsters.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.HeroEditor.Common.ExampleScripts;

public class MonsterBehavior : MonoBehaviour
{
    public MonsterData monsterData;
    [HideInInspector]
    public float health;
    public float maxHealth;
    public float walkSpeed;
    public int level;

    private Monster monster;

    /// <summary>
    /// ״̬�仯˵��
    /// 1. ��ǰ״̬��Walk
    ///     (1). ǰ��������ң�����ΪOnTrigger��������Player��GameObject
    ///          �л�ΪAttack״̬
    /// 2. ��ǰ״̬��Attack
    ///     (1). �������������Ϊ playerCollisionΪnull 
    ///          �� onTriggerExit��collisionΪplayerCollisionΪnull
    ///          �л�ΪWalk״̬
    /// </summary>
    public enum MonsterStatus
    {
        Idle,
        Walk,
        Run,
        Attack,
        Die
    }

    public MonsterStatus currentMonsterStatus;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI levelText;

    private Collider2D playerCollision = null;
    private IEnumerator attackCoroutine = null;

    void Start()
    {
        Init();

        monster = GetComponent<Monster>();
        // set animation to walk
        monster.SetState(MonsterState.Walk);
        currentMonsterStatus = MonsterStatus.Walk;

        // set default text
        healthText.text = "HP: " + health + " / " + maxHealth;
        levelText.text = "LV. " + (level+1);
    }

    void Update()
    {
        switch (currentMonsterStatus)
        {
            case MonsterStatus.Walk:
                // Auto Move
                transform.Translate(new Vector3(-walkSpeed * Time.deltaTime, 0, 0));
                break;
            case MonsterStatus.Attack:
                // ״̬�仯2.1
                if (playerCollision == null)
                {
                    ToWalkStatus();
                }
                break;
        }
    }

    // ��ʼ���ȼ�������ֵ���ٶȵȻ�������
    private void Init()
    {
        // Random level
        int randomValue = Random.Range(1, 101);
        int j = 1, sum = 0;
        for (int i = 0; i<monsterData.levelChance.Length; i++)
        {
            if (randomValue <= sum + monsterData.levelChance[i])
            {
                j = i;
                break;
            }
            sum += monsterData.levelChance[i];
        }
        level = j;

        // Init values
        maxHealth = monsterData.maxHealth[level];
        walkSpeed = monsterData.speed;
        health = maxHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ��������Զ����������
        if (collision.GetComponent<Projectile2D>() != null)
        {
            getHit(collision.GetComponent<Projectile2D>().damage, collision.GetComponent<Projectile2D>().player);
            collision.GetComponent<Projectile2D>().BangSelf();
            return;
        }
        // �������
        if (collision.GetComponent<Player>() != null)
        {
            // ״̬�仯1.1
            playerCollision = collision;
            ToAttackStatus();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // ״̬�仯2.1
        if (collision == playerCollision) ToWalkStatus();
    }

    private void ToAttackStatus()
    {
        currentMonsterStatus = MonsterStatus.Attack;
        monster.SetState(MonsterState.Ready);
        attackCoroutine = AttackCoroutine(10.0f / monsterData.attackSpeed);
        StartCoroutine(attackCoroutine);
    }

    // �Զ�������Ϊ
    IEnumerator AttackCoroutine(float coolDownTime)
    {
        while (true)
        {
            if (playerCollision != null)
            {
                monster.Attack();
                playerCollision.GetComponent<Player>().Attacked(monsterData.attack);
            }
            yield return new WaitForSeconds(coolDownTime);
        }
    }

    private void ToWalkStatus()
    {
        currentMonsterStatus = MonsterStatus.Walk;
        monster.SetState(MonsterState.Walk);
        if (attackCoroutine != null) StopCoroutine(attackCoroutine);
    }

    public void getHit(float damage, Player player)
    {
        if (health > damage)
        {
            health -= damage;
            healthText.text = "HP: " + health + " / " + maxHealth;
        }
        else if (currentMonsterStatus != MonsterStatus.Die)
        {
            Die();

            //����Ҽ��㾭��ֵ
            player.addExp(monsterData.dropExp[level]);
        }
    }

    // ����������Ϊ
    private void Die()
    {
        // ����������������������
        health = 0;
        currentMonsterStatus = MonsterStatus.Die;
        monster.Die();

        // ά���������б�
        WaveManager.livingMonsters.Remove(gameObject);
    }
}
