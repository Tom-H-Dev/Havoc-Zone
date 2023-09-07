using Mirror;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : NetworkBehaviour, IHealthable
{
    private float _maxHealth = 100;
    [SerializeField] private float _currentHealth;
    public bool dummy = false;

    [Header("Health bar")]
    [SerializeField] private Slider _healthBar;

    private float _damage = -10;
    private void Start()
    {
        SetMaxHealth();
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("Collision");
        if (collision.gameObject.tag == "Bullet")
        {
            ChangeHealth(_damage);
        }
    }
    public void ChangeHealth(float l_healthChange)
    {
        print("Changing Health");
        if (_currentHealth <= 0)
        {
            PlayerDead();
            if (!dummy)
                ChangeHealthbarvalue();
        }
        else
        {
            _currentHealth += l_healthChange;
            if (!dummy)
                ChangeHealthbarvalue();
        }
    }

    public void SetMaxHealth()
    {
        _currentHealth = _maxHealth;
        if (!dummy)
            ChangeHealthbarvalue();
    }

    private void PlayerDead()
    {
        Debug.Log("Player " + gameObject.name + " died");
        Destroy(gameObject);
    }

    private void ChangeHealthbarvalue()
    {
        _healthBar.value = _currentHealth;
    }
}
