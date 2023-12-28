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
    public Transform localSpawnPosition;


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
        if (_currentHealth <= 0)
        {
            StartCoroutine(PlayerDead());
            if (!dummy)
                ChangeHealthbarvalue();
        }
        else
        {
            print("Changing Health");
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

    private IEnumerator PlayerDead()
    {
        Debug.Log("Player " + gameObject.name + " died");

        transform.position = localSpawnPosition.position;
        _currentHealth = 100;
        yield return null;
    }

    private void ChangeHealthbarvalue()
    {
        _healthBar.value = _currentHealth;
    }
}
