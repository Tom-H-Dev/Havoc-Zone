using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class PlayerMovementController : NetworkBehaviour
{
    public float speed;
    public GameObject playerModel;
    [SerializeField]private Rigidbody _rb;
    private float _originalSpeed;
    [Range(1,5)]
    [SerializeField] private float _sprintSpeed;

    private void Start()
    {
        playerModel.SetActive(false);
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            if (!playerModel.activeSelf)
            {

                // TODO: make it so the player does not see their own skin but do other peoples skins\

                playerModel.SetActive(true);
                // Lock and Hide the Cursor
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                _rb.useGravity = false;
            }

            if (hasAuthority || GameManager.instance.devAccess)
            {
                Movement();

                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    Sprinting();
                }
                else if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    ReleaseSprint();
                }
            }
        }
    }

    public void SetPosition()
    {
        transform.position = new Vector3(Random.Range(-5, 5), 1f, Random.Range(-5, 5));
    }

    public void Movement()
    {
        float l_xDir = Input.GetAxis("Horizontal");
        float l_zDir = Input.GetAxis("Vertical");

        Vector3 movement = transform.forward * l_zDir + transform.right * l_xDir;

        transform.position += movement * speed * Time.deltaTime;
        //_rb.velocity += movement *speed * Time.deltaTime;
    }

    private void Sprinting()
    {
        _originalSpeed = speed;
        speed *= _sprintSpeed;
    }

    private void ReleaseSprint()
    {
        speed = _originalSpeed;

    }
}