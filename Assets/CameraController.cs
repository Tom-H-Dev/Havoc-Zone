using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class CameraController : NetworkBehaviour
{
    public GameObject cameraHolder;
    public Vector3 offset;

    private float _mouseSensitivity = 2f;
    private float _cameraVerticalRotation = 0f;

    private Transform _player;

    private float _cameraAngle = 0;

    public override void OnStartAuthority()
    {
        cameraHolder.SetActive(true);
        _player = transform;
    }

    private void Start()
    {
        if (_player is null)
        {
            _player = transform;
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            CamMovement();
            //cameraHolder.transform.position = transform.position + offset;
        }
    }

    private void CamMovement()
    {
        // Collect Mouse Input
        float inputX = Input.GetAxis("Mouse X") * _mouseSensitivity;
        print(inputX);
        float inputY = Input.GetAxis("Mouse Y") * _mouseSensitivity;
        _cameraAngle += inputX * Time.deltaTime;

        // Rotate the Camera around its local X axis

        _cameraVerticalRotation -= inputY;
        _cameraVerticalRotation = Mathf.Clamp(_cameraVerticalRotation, -90f, 60f);
        cameraHolder.transform.localEulerAngles = Vector3.right * _cameraVerticalRotation;

        // Rotate the Player Object and the Camera around its Y axis

        _player.Rotate(Vector3.up * _cameraAngle);
    }
}
