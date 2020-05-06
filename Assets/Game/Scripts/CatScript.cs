﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CatScript : MonoBehaviour
{
    [SerializeField] bool IsDebug = false;
    [SerializeField] float Speed = 1;
    [SerializeField] float LadderSpeed = 2f;
    [SerializeField] float JumpSpeed = 10;
    [SerializeField] GameObject Icon;
    [SerializeField] AudioSource TalkSounds;
    [SerializeField] AudioSource WalkSounds;
    [SerializeField] Text CatScore;
    [SerializeField] AudioSource[] SFX;
    private Rigidbody2D _rigidbody2D;
    private GameManager _gameManager;
    private Animator _animator;
    private AudioSource _audioSource;
    private KeyCode _keyleft;
    private KeyCode _keyright;
    private KeyCode _keyup;
    private KeyCode _keydown;
    private KeyCode _keyjump;
    private KeyCode _keyAction;
    private bool _stoped = true;
    private Face _face = Face.Left;
    private bool _changeFace = false;
    private bool _isGround = false;
    private bool _isBreaking = false;
    
    void Start()
    {
        if (IsDebug) { Debug.Log("*** CatScript debug is on ***"); }
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        if (IsDebug && _rigidbody2D == null) { Debug.Log("cannot find cat Rigidbody2D"); }
        var gm = GameObject.Find("GameManager");
        if (IsDebug && gm == null) { Debug.Log("cannot find cat GameManager"); }
        _gameManager = gm.GetComponent<GameManager>();
        if (IsDebug && gm == null) { Debug.Log("cannot find cat GameManager script"); }
        _animator = transform.gameObject.GetComponentInChildren<Animator>();
        if (IsDebug && _animator == null) { Debug.Log("cannot find cat Animator"); }
        _keyleft = _gameManager.CatLeft;
        _keyright = _gameManager.CatRight;
        _keyup = _gameManager.CatUp;
        _keydown = _gameManager.CatDown;
        _keyjump = _gameManager.CatJump;
        _keyAction = _gameManager.CatAction;
        _animator.Play("Idle");
        CatScore.text = "0";
    }
    
    void LateUpdate()
    {
        if (!_gameManager.GamePaused)
        {
            StopBreak();
            ChangeMove();
            ChangeFace();
            DoJump();
        }
    }

    private void StopBreak()
    {
        if (_isBreaking && IsCatInput() && !Input.GetKey(_keyAction) && Input.anyKey)
        {
            if (IsDebug)
            {
                Debug.Log($"breaking stops !!!");
            }

            _isBreaking = false;
            StopCoroutine(BreakStuff(null)); // TODO really need this?
            Icon.SetActive(false);
        }
    }

    private bool IsCatInput()
    {
        if (Input.GetKey(_keydown) || 
            Input.GetKey(_keyup) ||
            Input.GetKey(_keyleft) ||
            Input.GetKey(_keyright) ||
            Input.GetKey(_keyAction) ||
            Input.GetKey(_keyjump))
        {
            return true;
        }
        return false;
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
            //_audioSource.PlayOneShot(WalkSounds);
            if (!WalkSounds.isPlaying)
            {
                WalkSounds.Play();
            }
        }
        if (Input.GetKey(_keyright))
        {
            //if (IsDebug) { Debug.Log("cat right"); }
            _rigidbody2D.velocity = new Vector2(Speed, _rigidbody2D.velocity.y);
            _stoped = false;
            move = true;
            _face = Face.Right;
            _changeFace = true;

            _animator.Play("Walk");
            //_audioSource.PlayOneShot(WalkSounds);
            if (!WalkSounds.isPlaying)
            {
                WalkSounds.Play();
            }
        }
        


        if (!move && !_stoped)
        {
            //if (IsDebug) { Debug.Log("cat stop"); }
            _rigidbody2D.velocity = Vector2.zero;
            _stoped = true;
            _animator.Play("Idle");
            if (WalkSounds.isPlaying)
            {
                WalkSounds.Stop();
            }
        }
    }

    private void DoJump()
    {
        if (_isGround && Input.GetKey(_keyjump))
        {
            if (IsDebug) { Debug.Log("cat jump"); }
            _rigidbody2D.AddForce(Vector2.up * JumpSpeed, ForceMode2D.Force );
            _isGround = false;
            _animator.Play("Jump");  //TODO can we start in middle anim?
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
        else if (other.gameObject.tag == "item")
        {
            if (!_isBreaking && Input.GetKey(_keyAction))
            {
                if (!TalkSounds.isPlaying)
                {
                    TalkSounds.Play();
                }
                if (IsDebug)
                {
                    Debug.Log($"breaking starts");
                }
                _animator.Play("sound");
                Icon.SetActive(true);
                _isBreaking = true;
                StartCoroutine(BreakStuff(other));
                //_audioSource.PlayOneShot(TalkSounds);
            }
        }
    }

    IEnumerator BreakStuff(Collider2D other)
    {
        yield return new WaitForSeconds(1);
        if (_isBreaking)
        {
            if (IsDebug) { Debug.Log($"cat break {other.gameObject.name} successfully"); }
            var script = other.GetComponent<IItemDestroyAndFixScript>();
            if (script.HitItem(false))
            {
                PlayRandomSFX();
                _gameManager.CatScore+=2;
                ShowScore();
            }
            Icon.SetActive(false);
            _isBreaking = false;
        }
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

        StartCoroutine(MoveToPosition(transform,newpos.gameObject.transform.position));
    }

    public IEnumerator MoveToPosition(Transform transform, Vector3 position)
    {
        var currentPos = transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / LadderSpeed;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
    }

    private void ShowScore()
    {
        CatScore.text = _gameManager.CatScore.ToString();
    }
    
    private void PlayRandomSFX()
    {
        var rand = Random.Range(0, 3);
        if (!SFX[rand].isPlaying)
        {
            SFX[rand].Play();
        }
    }
}
