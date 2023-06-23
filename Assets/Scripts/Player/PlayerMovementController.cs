using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class PlayerMovementController : NetworkBehaviour
{
    public float speed = 10f;
    public GameObject playerModel;


    private void Start()
    {
        playerModel.SetActive(false);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            if (!playerModel.activeSelf)
            {
                SetPosition();
                playerModel.SetActive(true);
                // Lock and Hide the Cursor
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

            if (hasAuthority)
            {
                Movement();
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

        Vector3 l_moveDir = new Vector3(l_xDir, 0, l_zDir);
        transform.position += l_moveDir * speed;
    }
}
