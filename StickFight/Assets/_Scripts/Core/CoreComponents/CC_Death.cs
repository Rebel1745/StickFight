using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC_Death : CoreComponent
{
    [SerializeField] private GameObject[] _deathParticles;

    public void Die()
    {
        foreach (var particle in _deathParticles)
        {
            ParticleManager.StartParticles(particle);
        }

        _core.transform.parent.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        //Stats.OnHealthZero += Die;
    }

    private void OnDisable()
    {
        //Stats.OnHealthZero -= Die;
    }
}
