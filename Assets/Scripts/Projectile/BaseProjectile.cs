using System.Collections;
using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour
{

    // 发射此发射物的玩家，用于计算击杀
    public Player player;

    // 投射物的伤害
    public float damage = 1.0f;

    public void Start()
    {
        Destroy(gameObject, 5);
    }

    // 投射物在遇到刚体后自我销毁使用的方法
    abstract public void BangSelf();
}