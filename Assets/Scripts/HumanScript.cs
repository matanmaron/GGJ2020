﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanScript : MonoBehaviour
{
    [SerializeField] bool IsDebug = false;
    [SerializeField] float Speed = 1;
    
    private Rigidbody2D _rigidbody2D;
    private GameManager _gameManager;
    private KeyCode _keyleft;
    private KeyCode _keyrigth;
    private bool stoped = true;
    void Start()
    {
        if (IsDebug) { Debug.Log("*** HumanScript debug is on ***"); }
        _rigidbody2D = GetComponent<Rigidbody2D>();
        if (IsDebug && _rigidbody2D == null) { Debug.Log("cannot find human Rigidbody2D"); }
        var gm = GameObject.Find("GameManager");
        if (IsDebug && gm == null) { Debug.Log("cannot find human GameManager"); }
        _gameManager = gm.GetComponent<GameManager>();
        if (IsDebug && gm == null) { Debug.Log("cannot find human GameManager script"); }
        _keyleft = _gameManager.HumanLeft;
        _keyrigth = _gameManager.HumanRight;
    }
    
    void LateUpdate()
    {
        Move();
    }

    private void Move()
    {
        var move = false;
        if (Input.GetKey(_keyleft))
        {
            if (IsDebug) { Debug.Log("human left"); }
            _rigidbody2D.velocity = new Vector2(Speed * -1, _rigidbody2D.velocity.y);
            stoped = false;
            move = true;
        }
        if (Input.GetKey(_keyrigth))
        {
            if (IsDebug) { Debug.Log("human right"); }
            _rigidbody2D.velocity = new Vector2(Speed, _rigidbody2D.velocity.y);
            stoped = false;
            move = true;
        }

        if (!move && !stoped)
        {
            if (IsDebug) { Debug.Log("human stop"); }
            _rigidbody2D.velocity = Vector2.zero;
            stoped = true;
        }
    }
}
