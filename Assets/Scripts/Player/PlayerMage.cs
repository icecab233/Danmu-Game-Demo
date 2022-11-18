using UnityEngine;
using System.Collections;

public class PlayerMage : PlayerWeaponBase
{
    public float missleDelay = 0.3f;

    private new void Start()
    {
        base.Start();
        character.Animator.SetInteger("WeaponType", 0);
    }

    public override void Attack()
    {
        StartCoroutine(AttackCoroutine());
    }

    public override void DoubleAttack()
    {
        // TO DO
        Attack();
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
        var mega = Instantiate(ProjectilePrefab, FireTransform);

        // 将玩家信息存储在弓箭的Projectile2D组件中
        mega.GetComponent<BaseProjectile>().player = GetComponent<Player>();
        mega.GetComponent<BaseProjectile>().damage = GetComponent<Player>().attack;

        var rb = mega.GetComponent<Rigidbody2D>();

        mega.transform.localPosition = Vector3.zero + increment;
        mega.transform.localRotation = Quaternion.identity;
        mega.transform.SetParent(FireTransform);
        rb.velocity = speed * FireTransform.right * Mathf.Sign(character.transform.lossyScale.x) * 1f;
        mega.transform.LookAt(Vector3.right); //Sets the projectiles rotation to look at the point clicked
        rb.AddForce(mega.transform.forward * speed); //Set the speed of the projectile by applying force to the rigidbody
    }
}
