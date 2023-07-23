using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using TMPro;
using Steamworks;

public class PlayerShootController : NetworkBehaviour
{
    [SerializeField] private GameObject _shootPos;
    [SerializeField] private GameObject _hitPointIndication;

    [SerializeField] private float _damage;

    [Header("Magazines")]
    [SerializeField] private MagazineSizes _magazineSizes;
    private int _currentAmmo;
    [SerializeField] private TextMeshProUGUI _magText;
    private bool _setStartAmmo = false;

    [Header("Reload")]
    [SerializeField] private bool _reloading = false;
    [SerializeField] private bool _reloadCannotShoot = false;

    [Header("Firing Types")]
    [SerializeField] private FiringTypes _firingTypes;
    public WeaponTypes weaponTypes;
    [SerializeField] private TextMeshProUGUI _fireTypeText;

    [Header("Shooting")]
    [SerializeField] private bool _canShoot = true;
    private float _shootInterval = 0.075f;

    [Header("Sounds")]
    private GunSoundManager _gunSoundManager;
    private AudioSource _audioSource;

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            if (!_setStartAmmo)
            {
                _currentAmmo = _magazineSizes.mag30;
                _audioSource = GetComponentInChildren<AudioSource>();
                _gunSoundManager = GetComponentInChildren<GunSoundManager>();
                _magText.text = _currentAmmo.ToString() + " | " + _magazineSizes.mag30.ToString();
                _setStartAmmo = true;
                _fireTypeText.text = _firingTypes.ToString();
            }

            if (Input.GetMouseButtonDown(0) && _canShoot && !_reloadCannotShoot)
            {
                if (hasAuthority && _firingTypes != FiringTypes.Safe && _canShoot)
                {
                    StartCoroutine(ShotTimer(_shootInterval));
                    if (_currentAmmo <= 0)
                    {
                        //play sound que that mag is empty
                    }
                    else if (_currentAmmo > 0 && _currentAmmo <= _magazineSizes.mag30) //need to change for dynamic mag sizes
                    {
                        ShootGun();
                    }
                }
                else if (_firingTypes == FiringTypes.Safe)
                {
                    Debug.Log("Gun is on safe");
                }
            }
            else if (Input.GetMouseButton(0) && _firingTypes == FiringTypes.Automatic && _canShoot && !_reloadCannotShoot)
            {
                if (hasAuthority && _firingTypes != FiringTypes.Safe && _canShoot)
                {
                    StartCoroutine(ShotTimer(_shootInterval));
                    if (_currentAmmo <= 0)
                    {
                        _canShoot = false;
                        //play sound que that mag is empty
                    }
                    else if (_currentAmmo > 0 && _currentAmmo <= _magazineSizes.mag30) //need to change for dynamic mag sizes
                    {
                        ShootGun();
                    }
                }
                else if (_firingTypes == FiringTypes.Safe)
                {
                    Debug.Log("Gun is on safe");
                }
            }
            else if (Input.GetMouseButtonUp(0) && _reloadCannotShoot && !_canShoot)
            {
                _canShoot = true;
                _reloadCannotShoot = false;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _canShoot = true;
                _reloadCannotShoot = false;
            }


            if (Input.GetKeyDown(KeyCode.R) && _reloading == false)
            {
                Debug.Log("Reload");
                StartCoroutine(Reload());
            }

            if (Input.GetKeyDown(KeyCode.V) && _canShoot && !_reloadCannotShoot)
            {
                Debug.Log("Change Fire Type");
                ChangeFireType();
            }
        }

    }

    private IEnumerator Reload()
    {
        _canShoot = false;
        _reloadCannotShoot = true;
        _reloading = true;
        yield return new WaitForSeconds(1.5f);
        _currentAmmo = _magazineSizes.mag30;
        _magText.text = _currentAmmo.ToString() + " | " + _magazineSizes.mag30.ToString();
        _reloading = false;

        if (!Input.GetMouseButton(0))
        {
            _canShoot = true;
            _reloadCannotShoot = false;
        }
    }

    private void ShootGun()
    {
        _canShoot = false;
        _currentAmmo--;
        _magText.text = _currentAmmo.ToString() + " | " + _magazineSizes.mag30.ToString();

        _audioSource.clip = _gunSoundManager._gunSounds[0];
        _audioSource.Play();

        RaycastHit l_hit;
        if (Physics.Raycast(_shootPos.transform.position, _shootPos.transform.forward, out l_hit, Mathf.Infinity))
        {
            if (l_hit.transform.gameObject.TryGetComponent(out PlayerHealth l_health))
            {
                l_health.ChangeHealth(_damage);
            }
        }
        Debug.DrawRay(_shootPos.transform.position, _shootPos.transform.forward * 10, Color.red, 0.2f);
    }

    private IEnumerator DestroyBulletHitPoint(GameObject l_hitObject, float l_waitTime)
    {
        yield return new WaitForSeconds(l_waitTime);
        Destroy(l_hitObject);
    }

    private void ChangeFireType()
    {
        switch (_firingTypes)
        {
            case FiringTypes.Safe:
                if (weaponTypes == WeaponTypes.Pistol ||
                    weaponTypes == WeaponTypes.SubMachineGun ||
                    weaponTypes == WeaponTypes.Shotgun ||
                    weaponTypes == WeaponTypes.Special ||
                    weaponTypes == WeaponTypes.AssultRifle ||
                    weaponTypes == WeaponTypes.RocketLauncher)
                    _firingTypes = FiringTypes.Semi;
                else if (weaponTypes == WeaponTypes.Sniper)
                    _firingTypes = FiringTypes.Sniper;
                else Debug.LogError("Weapon Type Not Found!");

                //update the text what fire type
                _fireTypeText.text = _firingTypes.ToString();

                break;
            case FiringTypes.Semi:
                if (weaponTypes == WeaponTypes.Pistol ||
                    weaponTypes == WeaponTypes.Shotgun ||
                    weaponTypes == WeaponTypes.RocketLauncher ||
                    weaponTypes == WeaponTypes.Special)
                    _firingTypes = FiringTypes.Safe;
                else if (weaponTypes == WeaponTypes.SubMachineGun)
                    _firingTypes = FiringTypes.Automatic;
                else if (weaponTypes == WeaponTypes.AssultRifle)
                    _firingTypes = FiringTypes.Burst;
                else Debug.LogError("Weapon Type Not Found!");

                //update the text what fire type
                _fireTypeText.text = _firingTypes.ToString();

                break;
            case FiringTypes.Burst:
                if (weaponTypes == WeaponTypes.AssultRifle)
                    _firingTypes = FiringTypes.Automatic;
                else Debug.LogError("Weapon Type Not Found!");

                //update the text what fire type
                _fireTypeText.text = _firingTypes.ToString();

                break;
            case FiringTypes.Automatic:

                if (weaponTypes == WeaponTypes.SubMachineGun || weaponTypes == WeaponTypes.AssultRifle)
                    _firingTypes = FiringTypes.Safe;
                else Debug.LogError("Weapon Type Not Found!");

                //update the text what fire type
                _fireTypeText.text = _firingTypes.ToString();

                break;
            case FiringTypes.Sniper:

                if (weaponTypes == WeaponTypes.Sniper)
                    _firingTypes = FiringTypes.Safe;
                else Debug.LogError("Weapon Type Not Found!");

                //update the text what fire type
                _fireTypeText.text = _firingTypes.ToString();

                break;
        }
    }

    private IEnumerator ShotTimer(float l_shotDelay)
    {
        yield return new WaitForSeconds(l_shotDelay);
        if (_currentAmmo <= 0)
            _canShoot = false;
        else _canShoot = true;
    }
}