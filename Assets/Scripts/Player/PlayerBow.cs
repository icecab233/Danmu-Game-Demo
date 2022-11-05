using System.Linq;
using System;
using Assets.HeroEditor.Common.CharacterScripts;
using UnityEngine;
using System.Collections;

public class PlayerBow : MonoBehaviour
{
    public Character Character;
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
        Character.GetReady();
        Character.Animator.SetInteger("WeaponType", 3);

        weapon = Character.BowRenderers[3].transform;
    }

    /// <summary>
    /// Called each frame update, weapon to mouse rotation example.
    /// </summary>
    public void LateUpdate()
    {
        if (Character.IsReady())
        {
            RotateArm(ArmL, weapon, ArmL.position + 1000 * Vector3.right, -40, 40);
        }
    }

    // 创造一个默认的拉弓射箭行为
    public void BowAttack()
    {
        StartCoroutine(BowAttackCoroutine());
    }

    IEnumerator BowAttackCoroutine()
    {
        StartCharge();
        yield return new WaitForSeconds(1.0f);
        endCharge();
        yield break;
    }

    private void StartCharge()
    {
        _chargeTime = Time.time;
        Character.Animator.SetInteger("Charge", 1);
    }

    private void endCharge()
    {
        var charged = Time.time - _chargeTime > ClipCharge.length;

        Character.Animator.SetInteger("Charge", charged ? 2 : 3);

        if (charged && CreateArrows)
        {
            CreateArrow();
        }
    }

    private void CreateArrow()
    {
        var arrow = Instantiate(ArrowPrefab, FireTransform);
        var sr = arrow.GetComponent<SpriteRenderer>();
        var rb = arrow.GetComponent<Rigidbody2D>();

        arrow.transform.localPosition = Vector3.zero;
        arrow.transform.localRotation = Quaternion.identity;
        arrow.transform.SetParent(null);
        sr.sprite = Character.Bow.Single(j => j.name == "Arrow");
        rb.velocity = speed * FireTransform.right * Mathf.Sign(Character.transform.lossyScale.x) * 1f;

        var characterCollider = Character.GetComponent<Collider>();

        /*if (characterCollider != null)
        {
            Physics.IgnoreCollision(arrow.GetComponent<Collider>(), characterCollider);
        }

        arrow.gameObject.layer = 31; // TODO: Create layer in your project and disable collision for it (in physics settings)
        Physics.IgnoreLayerCollision(31, 31, true); // Disable collision with other projectiles.*/
    }


    /// <summary>
    /// Selected arm to position (world space) rotation, with limits.
    /// </summary>
    private float AngleToTarget;
    private float AngleToArm;
    public void RotateArm(Transform arm, Transform weapon, Vector2 target, float angleMin, float angleMax) // TODO: Very hard to understand logic.
    {
        target = arm.transform.InverseTransformPoint(target);

        var angleToTarget = Vector2.SignedAngle(Vector2.right, target);
        var angleToArm = Vector2.SignedAngle(weapon.right, arm.transform.right) * Math.Sign(weapon.lossyScale.x);
        var fix = weapon.InverseTransformPoint(arm.transform.position).y / target.magnitude;

        AngleToTarget = angleToTarget;
        AngleToArm = angleToArm;

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
