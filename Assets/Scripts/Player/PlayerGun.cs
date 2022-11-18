using UnityEngine;
using System.Collections;
using Assets.HeroEditor.Common.CharacterScripts.Firearms;
using Assets.HeroEditor.Common.CharacterScripts;
using HeroEditor.Common.Enums;
using System;

public class PlayerGun : PlayerWeaponBase
{
    public float delay = 0.1f;
    public Firearm firearm;
    public Transform ArmR;

    private int attackCount;

    private new void Start()
    {
        base.Start();
        attackCount = 0;
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
        firearm.Fire.FireButtonDown = true;
        // 每次攻击，开枪和装弹交替进行
        if (attackCount++ % 2 == 0) CreateBullet(Vector3.zero);
        yield return new WaitForSeconds(delay);
        firearm.Fire.FireButtonDown = false;
        yield break;
    }

    private void CreateBullet(Vector3 increment)
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

    /// <summary>
    /// Called each frame update, weapon to mouse rotation example.
    /// </summary>
    public void LateUpdate()
    {

        Transform arm;
        Transform weapon;

        arm = ArmR;
        weapon = firearm.FireTransform;

        if (character.IsReady())
        {
            RotateArm(arm, weapon, arm.position + 1000 * Vector3.right, -40, 40);
        }
    }

    /// <summary>
    /// Selected arm to position (world space) rotation, with limits.
    /// </summary>
    public void RotateArm(Transform arm, Transform weapon, Vector2 target, float angleMin, float angleMax) // TODO: Very hard to understand logic.
    {
        target = arm.transform.InverseTransformPoint(target);

        var angleToTarget = Vector2.SignedAngle(Vector2.right, target);
        var angleToArm = Vector2.SignedAngle(weapon.right, arm.transform.right) * Math.Sign(weapon.lossyScale.x);
        var fix = weapon.InverseTransformPoint(arm.transform.position).y / target.magnitude;

        if (fix < -1) fix = -1;
        else if (fix > 1) fix = 1;

        var angleFix = Mathf.Asin(fix) * Mathf.Rad2Deg;
        var angle = angleToTarget + angleFix + arm.transform.localEulerAngles.z;

        angle = NormalizeAngle(angle);

        if (angle > angleMax)
        {
            angle = angleMax;
        }
        else if (angle < angleMin)
        {
            angle = angleMin;
        }

        if (float.IsNaN(angle))
        {
            Debug.LogWarning(angle);
        }

        arm.transform.localEulerAngles = new Vector3(0, 0, angle + angleToArm);
    }

    private static float NormalizeAngle(float angle)
    {
        while (angle > 180) angle -= 360;
        while (angle < -180) angle += 360;

        return angle;
    }

}
