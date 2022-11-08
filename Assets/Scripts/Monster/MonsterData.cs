using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Monster", menuName = "ScriptableObjects/Monster")]
public class MonsterData : ScriptableObject
{
    // 怪物共有几个等级，如值为x，则下列数组大小都应为x
    public int maxLevel;
    // 刷出每个等级的几率。数组各项和应为100
    public int[] levelChance;

    public int[] maxHealth;
    public float speed;
    public int[] dropExp;

    public int attack;
    // 攻速，实际攻击间隔时间为：x = 10/as
    public float attackSpeed;
}
