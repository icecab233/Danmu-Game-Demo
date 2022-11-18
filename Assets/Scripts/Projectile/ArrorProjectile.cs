using System.Collections;
using UnityEngine;

public class ArrorProjectile : BaseProjectile
{

    public override void BangSelf()
    {
        Destroy(gameObject);
    }
}