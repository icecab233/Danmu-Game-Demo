using Assets.FantasyMonsters.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MonsterBehavior : MonoBehaviour
{
    public MonsterData monsterData;
    [HideInInspector]
    public float health;
    public float maxHealth;
    public float walkSpeed;
    public int level;
    public bool dead = false;

    private Monster monster;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI levelText;

    void Start()
    {
        Init();

        monster = GetComponent<Monster>();
        // set animation to walk
        monster.SetState(MonsterState.Walk);

        // set default text
        healthText.text = "HP: " + health + " / " + maxHealth;
        levelText.text = "LV. " + level;
    }

    void Update()
    {
        if (!dead)
        {
            // Auto Move
            transform.Translate(new Vector3(-walkSpeed * Time.deltaTime, 0, 0));
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

        // 这里计算的level是以0开始的，改为以1开始
        level++;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Get Hit
        if (collision.GetComponent<Projectile2D>() != null)
        {
            getHit(collision.GetComponent<Projectile2D>().damage);
            collision.GetComponent<Projectile2D>().BangSelf();
        }
    }

    public void getHit(float damage)
    {
        if (health > damage) health -= damage;
        else if (!dead)
        {
            health = 0;
            dead = true;
            monster.Die();
        }
        healthText.text = "HP: "+health+ " / "+maxHealth;
    }
}
