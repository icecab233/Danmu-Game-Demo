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
    /// 状态变化说明
    /// 1. 当前状态：Walk
    ///     (1). 前方遇到玩家，表现为OnTrigger中碰到带Player的GameObject
    ///          切换为Attack状态
    /// 2. 当前状态：Attack
    ///     (1). 玩家死亡，表现为 playerCollision为null 
    ///          或 onTriggerExit的collision为playerCollision为null
    ///          切换为Walk状态
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
                // 状态变化2.1
                if (playerCollision == null)
                {
                    ToWalkStatus();
                }
                break;
        }
    }

    // 初始化等级、生命值、速度等基础属性
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
        // 被弓箭等远程射击物击中
        if (collision.GetComponent<Projectile2D>() != null)
        {
            getHit(collision.GetComponent<Projectile2D>().damage, collision.GetComponent<Projectile2D>().player);
            collision.GetComponent<Projectile2D>().BangSelf();
            return;
        }
        // 攻击玩家
        if (collision.GetComponent<Player>() != null)
        {
            // 状态变化1.1
            playerCollision = collision;
            ToAttackStatus();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 状态变化2.1
        if (collision == playerCollision) ToWalkStatus();
    }

    private void ToAttackStatus()
    {
        currentMonsterStatus = MonsterStatus.Attack;
        monster.SetState(MonsterState.Ready);
        attackCoroutine = AttackCoroutine(10.0f / monsterData.attackSpeed);
        StartCoroutine(attackCoroutine);
    }

    // 自动攻击行为
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

            //给玩家计算经验值
            player.addExp(monsterData.dropExp[level]);
        }
    }

    // 怪物死亡行为
    private void Die()
    {
        // 怪物死亡，播放死亡动画
        health = 0;
        currentMonsterStatus = MonsterStatus.Die;
        monster.Die();

        // 维护存活怪物列表
        WaveManager.livingMonsters.Remove(gameObject);
    }
}
