using Assets.FantasyMonsters.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MonsterBehavior : MonoBehaviour
{
    public float health = 20.0f;
    public float maxHealth = 20.0f;
    public float walkSpeed = 0.3f;
    public bool dead = false;

    private Monster monster;

    public TextMeshProUGUI healthText;

    void Start()
    {
        monster = GetComponent<Monster>();
        // set animation to walk
        monster.SetState(MonsterState.Walk);

        // set default health text
        healthText.text = "HP: " + health + " / " + maxHealth;
    }

    void Update()
    {
        if (!dead)
        {
            // Auto Move
            transform.Translate(new Vector3(-walkSpeed * Time.deltaTime, 0, 0));
        }
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
