using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// General behaviour for projectiles: bullets, rockets and other.
/// </summary>
public class Projectile2D : MonoBehaviour
{
    public List<Renderer> Renderers;
    public GameObject Trail;
    public GameObject Impact;
    public Rigidbody2D Rigidbody;

    // 发射此发射物的玩家，用于计算击杀
    public Player player;

    public float damage = 1.0f;

    public void Start()
    {
        Destroy(gameObject, 5);
    }

    public void BangSelf()
    {
        Impact.SetActive(true);
        Destroy(GetComponent<SpriteRenderer>());
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<Collider>());

        foreach (var ps in Trail.GetComponentsInChildren<ParticleSystem>())
        {
            ps.Stop();
        }

        foreach (var tr in Trail.GetComponentsInChildren<TrailRenderer>())
        {
            tr.enabled = false;
        }

        Destroy(gameObject);
    }
}