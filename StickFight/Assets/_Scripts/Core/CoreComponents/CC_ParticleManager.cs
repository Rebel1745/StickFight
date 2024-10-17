using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC_ParticleManager : CoreComponent
{
    private Transform _particleContainer;

    protected override void Awake()
    {
        base.Awake();

        _particleContainer = GameObject.FindGameObjectWithTag("ParticleContainer").transform;
    }

    public GameObject StartParticles(GameObject prefab, Vector2 position, Quaternion rotation)
    {
        return Instantiate(prefab, position, rotation, _particleContainer);
    }

    public GameObject StartParticles(GameObject prefab)
    {
        return StartParticles(prefab, transform.position, Quaternion.identity);
    }

    public GameObject StartParticlesWithRandomRotation(GameObject prefab, Vector2 position)
    {
        var randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        return StartParticles(prefab, position, randomRotation);
    }
}
