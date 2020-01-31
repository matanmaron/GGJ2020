using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;

public class CatScript : MonoBehaviour
{
    [SerializeField] bool IsDebug = false;
    [SerializeField] float Speed = 1;
    [SerializeField] float JumpSpeed = 10;
    [SerializeField] GameObject Icon;
    private Rigidbody2D _rigidbody2D;
    private GameManager _gameManager;
    private Animator _animator;
    private KeyCode _keyleft;
    private KeyCode _keyrigth;
    private KeyCode _keyup;
    private KeyCode _keydown;
    private KeyCode _keyjump;
    private KeyCode _keyAction;
    private bool _stoped = true;
    private Face _face = Face.Left;
    private bool _changeFace = false;
    private bool _isGround = false;
    private bool _breakSuccess = false;

    void Start()
    {
        if (IsDebug) { Debug.Log("*** CatScript debug is on ***"); }
        _rigidbody2D = GetComponent<Rigidbody2D>();
        
        if (IsDebug && _rigidbody2D == null) { Debug.Log("cannot find cat Rigidbody2D"); }
        var gm = GameObject.Find("GameManager");
        if (IsDebug && gm == null) { Debug.Log("cannot find cat GameManager"); }
        _gameManager = gm.GetComponent<GameManager>();
        if (IsDebug && gm == null) { Debug.Log("cannot find cat GameManager script"); }
        _animator = transform.gameObject.GetComponentInChildren<Animator>();
        if (IsDebug && _animator == null) { Debug.Log("cannot find cat Animator"); }
        _keyleft = _gameManager.CatLeft;
        _keyrigth = _gameManager.CatRight;
        _keyup = _gameManager.CatUp;
        _keydown = _gameManager.CatDown;
        _keyjump = _gameManager.CatJump;
        _keyAction = _gameManager.CatAction;
        _animator.Play("Idle");
    }
    
    void LateUpdate()
    {
        if (!_gameManager.GamePaused)
        {
            ChangeMove();
            ChangeFace();
            DoJump();
        }
    }

    private void ChangeFace()
    {
        if (_changeFace)
        {
            if (_face == Face.Left)
            {
                //if (IsDebug) { Debug.Log("cat turn left"); }
                //0 to 180
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (_face == Face.Right)
            {
                //if (IsDebug) { Debug.Log("cat turn right"); }
                //180 t0 0
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            _changeFace = false;
        }
    }


    private void ChangeMove()
    {
        var move = false;
        if (Input.GetKey(_keyleft))
        {
            //if (IsDebug) { Debug.Log("cat left"); }
            _rigidbody2D.velocity = new Vector2(Speed * -1, _rigidbody2D.velocity.y);
            _stoped = false;
            move = true;
            _face = Face.Left;
            _changeFace = true;
            _animator.Play("Walk");
        }
        if (Input.GetKey(_keyrigth))
        {
            //if (IsDebug) { Debug.Log("cat right"); }
            _rigidbody2D.velocity = new Vector2(Speed, _rigidbody2D.velocity.y);
            _stoped = false;
            move = true;
            _face = Face.Right;
            _changeFace = true;
            _animator.Play("Walk");
        }
        


        if (!move && !_stoped)
        {
            //if (IsDebug) { Debug.Log("cat stop"); }
            _rigidbody2D.velocity = Vector2.zero;
            _stoped = true;
            _animator.Play("Idle");
        }
    }

    private void DoJump()
    {
        if (_isGround && Input.GetKey(_keyjump))
        {
            if (IsDebug) { Debug.Log("cat jump"); }
            _rigidbody2D.AddForce(Vector2.up * JumpSpeed, ForceMode2D.Force );
            _isGround = false;
            _animator.Play("Jump");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "ground")
        {
            _isGround = true;
            if (IsDebug) { Debug.Log("cat ground is: "+_isGround); }
        }
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        //if (IsDebug) { Debug.Log($"cat colliding with {other.gameObject.name}"); }
        if (other.gameObject.tag == "ladder")
        {
            if (Input.GetKey(_keyup))
            {
                HandleLadders(other, Ladder.Up);
            }
            else if (Input.GetKey(_keydown))
            {
                HandleLadders(other, Ladder.Down);
            }
        }
        else
        {
            if (!Input.GetKey(_keyAction) && Input.anyKey)
            {
                if (IsDebug)
                {
                    Debug.Log($"breaking stops !!!");
                }

                StopCoroutine(BreakStuff(other));
                _breakSuccess = false;
                Icon.SetActive(false);
            }

            if (Input.GetKey(_keyAction))
            {
                if (IsDebug)
                {
                    Debug.Log($"breaking starts");
                }

                Icon.SetActive(true);
                StartCoroutine(BreakStuff(other));
            }
        }
    }

    IEnumerator BreakStuff(Collider2D other)
    {
        yield return new WaitForSeconds(1);
        _breakSuccess = true;
        if (IsDebug) { Debug.Log($"cat break {other.gameObject.name} successfully"); }
        //break
        _breakSuccess = false;
        Icon.SetActive(false);
    }
    
    private void HandleLadders(Collider2D other, Ladder ladder)
    {
        var num = other.name[1];
        var dir = other.name[2];
        if ((ladder == Ladder.Down && dir == 'D') || (ladder == Ladder.Up && dir == 'U'))
        {
            return;
        }
        var newdir = dir == 'U' ? 'D' : 'U';
        var newlad = "L" + num + newdir;
        var newpos = GameObject.Find(newlad);
        if (IsDebug && newpos == null){ Debug.Log("teleporting problem..."); }

        transform.position = newpos.gameObject.transform.position;
    }
}
