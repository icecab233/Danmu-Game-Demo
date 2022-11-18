using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;


public class MegaProjectile : BaseProjectile
{
    public GameObject impactParticle; // Effect spawned when projectile hits a collider
    public GameObject projectileParticle; // Effect attached to the gameobject as child
    public GameObject muzzleParticle; // Effect instantly spawned when gameobject is spawned

    private Transform FXTransform;
    public float scaleFactor;

    new void Start()
    {
        base.Start();

        FXTransform = GameObject.Find("FXTransform").transform;

        projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation, transform) as GameObject;
        projectileParticle.transform.localScale = new Vector3(1f,scaleFactor,1f);
        if (muzzleParticle)
        {
            muzzleParticle = Instantiate(muzzleParticle, transform.position, transform.rotation) as GameObject;
            muzzleParticle.transform.localScale *= scaleFactor;
            muzzleParticle.transform.parent = FXTransform;
            Destroy(muzzleParticle, 1.5f); // 2nd parameter is lifetime of effect in seconds
        }
    }


    public override void BangSelf()
    {
        GameObject impactP = Instantiate(impactParticle, transform.position, Quaternion.LookRotation(Vector3.up)) as GameObject; // Spawns impact effect
        impactP.transform.parent = FXTransform;
        impactP.transform.localScale *= scaleFactor;

        ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>(); // Gets a list of particle systems, as we need to detach the trails
                                                                             //Component at [0] is that of the parent i.e. this object (if there is any)
        for (int i = 1; i < trails.Length; i++) // Loop to cycle through found particle systems
        {
            ParticleSystem trail = trails[i];

            if (trail.gameObject.name.Contains("Trail"))
            {
                trail.transform.SetParent(FXTransform); // Detaches the trail from the projectile
                Destroy(trail.gameObject, 2f); // Removes the trail after seconds
            }
        }

        Destroy(projectileParticle, 3f); // Removes particle effect after delay
        Destroy(impactP, 3.5f); // Removes impact effect after delay
        Destroy(gameObject); // Removes the projectile
    }
}