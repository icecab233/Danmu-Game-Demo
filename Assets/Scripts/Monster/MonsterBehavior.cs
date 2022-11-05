using Assets.FantasyMonsters.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBehavior : MonoBehaviour
{
    public float health = 20.0f;
    public float walkSpeed = 0.3f;
    public bool dead = false;

    private Monster monster;

    void Start()
    {
        monster = GetComponent<Monster>();
        // set animation to walk
        monster.SetState(MonsterState.Walk);
    }

    void Update()
    {
        transform.Translate(new Vector3(-walkSpeed * Time.deltaTime, 0, 0));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Get Hit
        if (collision.GetComponent<Projectile2D>() != null)
        {
            getHit(collision.GetComponent<Projectile2D>().damage);
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
    }
}
