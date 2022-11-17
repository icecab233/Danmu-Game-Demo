using System.Linq;
using System;
using Assets.HeroEditor.Common.CharacterScripts;
using UnityEngine;
using System.Collections;

public class PlayerMage : MonoBehaviour
{
    private Character character;
    public Transform FireTransform;
    public GameObject MegaPrefab;
    public float speed = 30f;
    public float missleDelay = 0.3f;

    private void Start()
    {
        character = GetComponent<Character>();
        character.GetReady();
        character.Animator.SetInteger("WeaponType", 0);
    }

    // 创造一个默认的拉弓射箭行为
    public void Attack()
    {
        StartCoroutine(AttackCoroutine());
    }

    IEnumerator AttackCoroutine()
    {
        character.Animator.SetTrigger("Slash");
        yield return new WaitForSeconds(missleDelay);
        CreateMega(Vector3.zero);
        yield break;
    }

    private void CreateMega(Vector3 increment)
    {
        var mega = Instantiate(MegaPrefab, FireTransform);

/*        // 将玩家信息存储在弓箭的Projectile2D组件中
        mega.GetComponent<Projectile2D>().player = GetComponent<Player>();
        mega.GetComponent<Projectile2D>().damage = GetComponent<Player>().attack;*/

        var rb = mega.GetComponent<Rigidbody2D>();

        mega.transform.localPosition = Vector3.zero + increment;
        mega.transform.localRotation = Quaternion.identity;
        mega.transform.SetParent(FireTransform);
        rb.velocity = speed * FireTransform.right * Mathf.Sign(character.transform.lossyScale.x) * 1f;
    }
}
