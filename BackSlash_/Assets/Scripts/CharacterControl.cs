using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterControl : MonoBehaviour
{
    CharacterController _controlPLayer;
    [SerializeField] private Transform _firstCamera;
    [Header("Movement")]
    [SerializeField] private float _speed;
    [SerializeField] private float _sprintSpeed;
    [SerializeField] private float _jumpSpeed;
    [Header("Rotate")]
    [SerializeField] private float _turnTime;
    [Header("Gravity")]
    [SerializeField] private float _gravityForce;

    private Vector3 _rotationVector;
    protected int _vectorCount;
    private float jSpeed, _verticalMove, _horizontalMove, _vectorsMult, _turnSpeed;

    private void Start()
    {
        _controlPLayer = GetComponent<CharacterController>();
    }

    private void Update()
    {
        PlayerMove();
    }

    public void PlayerMove()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {

        }

        if (_controlPLayer.isGrounded)
        {
            PlayerInputWASD();
            _rotationVector = _vectorCount == 0 ? Vector3.zero : RotatePlayer();
            jSpeed = 0;
            if (Input.GetKeyDown(KeyCode.Space))
                jSpeed = _jumpSpeed;
        }
        jSpeed += -_gravityForce * Time.deltaTime;

        _vectorsMult = _vectorCount > 1 ? 0.8f : 1;
        if (Input.GetKey(KeyCode.LeftControl))
            _vectorsMult *= _sprintSpeed;

        _controlPLayer.Move((_rotationVector * _vectorsMult * _speed + new Vector3(0f, jSpeed, 0f)) * Time.deltaTime);
    }

    private Vector3 RotatePlayer()
    {
        float _rotationAngle = Mathf.Atan2(_horizontalMove, _verticalMove) * Mathf.Rad2Deg + _firstCamera.eulerAngles.y;
        float _turnAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _rotationAngle, ref _turnSpeed, _turnTime);
        transform.rotation = Quaternion.Euler(0f, _turnAngle, 0f);
        Vector3 _move = Quaternion.Euler(0f, _rotationAngle, 0f) * Vector3.forward;
        return _move.normalized;
    }

    public void PlayerInputWASD()
    {
        _verticalMove = 0;
        _horizontalMove = 0;
        _vectorCount = 0;

        if (Input.GetKey(KeyCode.W))
        {
            _verticalMove += 1;
            _vectorCount++;
        }
            
        if (Input.GetKey(KeyCode.S))
        {
            _verticalMove += -1 * 0.8f;
            _vectorCount++;
        }

        if (Input.GetKey(KeyCode.A))
        {
            _horizontalMove += -1;
            _vectorCount++;
        }

        if (Input.GetKey(KeyCode.D))
        {
            _horizontalMove += 1;
            _vectorCount++;
        }
    }

    public int GetVectorCount()
    {
        return _vectorCount;
    }
}
