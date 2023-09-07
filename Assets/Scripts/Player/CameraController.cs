using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using UnityEditor.Rendering;

public class CameraController : NetworkBehaviour
{
    public GameObject cameraHolder;
    public Vector3 offset;
    public GameObject gun;
    public GameObject canvas;

    private float _mouseSensitivity = 5f;
    private float _cameraVerticalRotation = 0f;

    private Transform _player;

    private float _cameraAngle = 0;
    private bool _firstSet = false;
    private Camera _fpsCamera;

    public override void OnStartAuthority()
    {
        cameraHolder.SetActive(true);
        _player = transform;
        _fpsCamera = cameraHolder.GetComponentInChildren<Camera>();
        Rigidbody rb = GetComponentInParent<Rigidbody>();
        rb.useGravity = false;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            if (_player is null)
            {
                _player = transform;
            }
            if (!_firstSet)
            {
                gun.SetActive(true);
                canvas.SetActive(true);
            }
            CamMovement();
        }
    }

    private void CamMovement()
    {
        Debug.DrawRay(_fpsCamera.transform.position, _fpsCamera.transform.forward * 10f, Color.white);

        float l_inputX = Input.GetAxis("Mouse X") * _mouseSensitivity;
        float l_inputY = Input.GetAxis("Mouse Y") * _mouseSensitivity;

        _cameraAngle += l_inputX * Time.deltaTime;

        // Rotate the Camera around its local X axis
        _cameraVerticalRotation -= l_inputY;
        _cameraVerticalRotation = Mathf.Clamp(_cameraVerticalRotation, -90f, 60f);
        cameraHolder.transform.localEulerAngles = Vector3.right * _cameraVerticalRotation;

        _player.rotation *= Quaternion.Euler(0, l_inputX, 0);
    }
}
