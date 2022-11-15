using System.Linq;
using System;
using Assets.HeroEditor.Common.CharacterScripts;
using UnityEngine;
using System.Collections;
using Assets.HeroEditor4D.SimpleColorPicker.Scripts;

public class PlayerBow : MonoBehaviour
{
    private Character character;
    public AnimationClip ClipCharge;
    public Transform FireTransform;
    public GameObject ArrowPrefab;
    public float speed = 30f;
    public bool CreateArrows;

    [SerializeField]
    private Transform ArmL;
    private Transform weapon;

    /// <summary>
    /// Maybe should be set outside (by input manager or AI).
    /// </summary>
    [HideInInspector] public bool ChargeButtonDown;
    [HideInInspector] public bool ChargeButtonUp;

    private float _chargeTime;

    private void Start()
    {
        character = GetComponent<Character>();
        character.GetReady();
        character.Animator.SetInteger("WeaponType", 3);

        weapon = character.BowRenderers[3].transform;
    }

    /// <summary>
    /// Called each frame update, weapon to mouse rotation example.
    /// </summary>
    public void LateUpdate()
    {
        if (character.IsReady())
        {
            RotateArm(ArmL, weapon, ArmL.position + 1000 * Vector3.right, -40, 40);
        }
    }

    // 创造一个默认的拉弓射箭行为
    public void BowAttack()
    {
        StartCoroutine(BowAttackCoroutine());
    }

    // 创造一个双箭的拉弓射箭行为
    public void DoubleBowAttack()
    {
        StartCoroutine(DoubleBowAttackCoroutine());

    }

    IEnumerator BowAttackCoroutine()
    {
        StartCharge();
        yield return new WaitForSeconds(1.0f);
        endCharge(false);
        yield break;
    }

    IEnumerator DoubleBowAttackCoroutine()
    {
        StartCharge();
        yield return new WaitForSeconds(1.0f);
        endCharge(true);
        yield break;
    }

    private void StartCharge()
    {
        _chargeTime = Time.time;
        character.Animator.SetInteger("Charge", 1);
    }

    private void endCharge(bool Double)
    {
        var charged = Time.time - _chargeTime > ClipCharge.length;

        character.Animator.SetInteger("Charge", charged ? 2 : 3);

        if (charged && CreateArrows)
        {
            CreateArrow(Vector3.zero);
            if (Double) CreateArrow(new Vector3(0f, 0.17f, 0f));
        }
    }

    private void CreateArrow(Vector3 increment)
    {
        var arrow = Instantiate(ArrowPrefab, FireTransform);

        // 将玩家信息存储在弓箭的Projectile2D组件中
        arrow.GetComponent<Projectile2D>().player = GetComponent<Player>();
        arrow.GetComponent<Projectile2D>().damage = GetComponent<Player>().attack;

        var sr = arrow.GetComponent<SpriteRenderer>();
        var rb = arrow.GetComponent<Rigidbody2D>();

        arrow.transform.localPosition = Vector3.zero + increment;
        arrow.transform.localRotation = Quaternion.identity;
        arrow.transform.SetParent(transform);
        sr.sprite = character.Bow.Single(j => j.name == "Arrow");
        rb.velocity = speed * FireTransform.right * Mathf.Sign(character.transform.lossyScale.x) * 1f;

        var characterCollider = character.GetComponent<Collider>();
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
