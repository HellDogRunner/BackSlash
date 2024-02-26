using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    CharacterController _controlPLayer;
    [Header("Movement")]
    [SerializeField] private float _speed;
    [SerializeField] private float _sprintSpeed;
    [SerializeField] private float _rotate;
    [SerializeField] private float _jumpSpeed;
    [Header("Gravity")]
    [SerializeField] private float _gravityForce;

    private int _vectorCount;
    private float jSpeed, _verticalMove, _horizontalMove, _vectorsMult;

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
            if (_controlPLayer.isGrounded)
        {
            PlayerInputWASD();
            jSpeed = 0;
            if (Input.GetKeyDown(KeyCode.Space))
                jSpeed = _jumpSpeed;
        }
        jSpeed += -_gravityForce * Time.deltaTime;

        _vectorsMult = _vectorCount > 1 ? 0.8f : 1;
        if (Input.GetKey(KeyCode.LeftControl))
            _vectorsMult *= _sprintSpeed;

        _controlPLayer.Move(new Vector3(_horizontalMove * _vectorsMult, jSpeed, _verticalMove * _vectorsMult) * Time.deltaTime);
    }

    public void PlayerInputWASD()
    {
        _verticalMove = 0;
        _horizontalMove = 0;
        _vectorCount = 0;

        if (Input.GetKey(KeyCode.W))
        {
            _verticalMove += 1 * _speed;
            _vectorCount++;
        }
            
        if (Input.GetKey(KeyCode.S))
        {
            _verticalMove += -1 * 0.8f;
            _vectorCount++;
        }

        if (Input.GetKey(KeyCode.A))
        {
            _horizontalMove += -1 * _speed;
            _vectorCount++;
        }

        if (Input.GetKey(KeyCode.D))
        {
            _horizontalMove += 1 * _speed;
            _vectorCount++;
        }
    }
}
