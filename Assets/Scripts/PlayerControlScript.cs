using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlScript : MonoBehaviour
{
    [SerializeField] private bool IsDebug = false;
    [SerializeField] private float Speed = 15;
    [SerializeField] private float SensitivityX = 1;
    private float _rotationX;
    private CharacterController _controller;
    private Animator _animator;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        if (IsDebug && _controller == null)
        {
            Debug.Log("cannot find character controller");
        }
        _animator = GetComponent<Animator>();
        if (IsDebug && _animator == null)
        {
            Debug.Log("cannot find character animator");
        }
    }
    
    void LateUpdate()
    {
        Move();
    }

    private void Move()
    {
         void LateUpdate()
            {
                Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                _controller.Move(move * (Time.deltaTime * Speed));
    
                _rotationX += Input.GetAxis("Mouse X") * SensitivityX;
                transform.localEulerAngles = new Vector3(0, _rotationX, 0);
    
                if (move != Vector3.zero || _rotationX != 0)
                {
                    WalkAnim();
                }
                _rotationX += Input.GetAxis("Mouse X") * SensitivityX;
                transform.localEulerAngles = new Vector3(0, _rotationX, 0);
            }
    }

    private void WalkAnim()
    {
        //TODO
    }
}
