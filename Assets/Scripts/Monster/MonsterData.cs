using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Monster", menuName = "ScriptableObjects/Monster")]
public class MonsterData : ScriptableObject
{
    // ���ﹲ�м����ȼ�����ֵΪx�������������С��ӦΪx
    public int maxLevel;
    // ˢ��ÿ���ȼ��ļ��ʡ���������ӦΪ100
    public int[] levelChance;

    public int[] maxHealth;
    public float speed;
    public int[] dropExp;

    public int attack;
    // ���٣�ʵ�ʹ������ʱ��Ϊ��x = 10/as
    public float attackSpeed;
}
