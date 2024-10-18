using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC_Stats : CoreComponent
{
    public event Action OnHealthZero;

    [SerializeField] private float _maxHealth;
    private float _currentHealth;

    protected override void Awake()
    {
        base.Awake();

        _currentHealth = _maxHealth;
    }

    public void DecreaseHealth(float amount)
    {
        _currentHealth -= amount;

        if (_currentHealth <= 0f)
        {
            _currentHealth = 0f;
            OnHealthZero?.Invoke();
        }
    }

    public void IncreaseHealth(float amount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0f, _maxHealth);
    }
}
