using Assets.HeroEditor.Common.CharacterScripts;
using System.Collections;
using UnityEngine;

public abstract class PlayerWeaponBase : MonoBehaviour
{
    protected Character character;
    public Transform FireTransform;
    public GameObject ProjectilePrefab;
    public float speed = 30f;

    protected void Start()
    {
        character = GetComponent<Character>();
        character.GetReady();
    }

    // 进行一次攻击行为
    public abstract void Attack();

    // 狂暴模式下同时进行两次攻击行为
    public abstract void DoubleAttack();
}