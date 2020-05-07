using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CatScript : MonoBehaviour
{
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
        if (GameManager.Instance.IsDebug) { Debug.Log("*** CatScript debug is on ***"); }
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        if (GameManager.Instance.IsDebug && _rigidbody2D == null) { Debug.LogError("cannot find cat Rigidbody2D"); }
        var gm = GameObject.Find("GameManager");
        if (GameManager.Instance.IsDebug && gm == null) { Debug.LogError("cannot find cat GameManager"); }
        _gameManager = gm.GetComponent<GameManager>();
        if (GameManager.Instance.IsDebug && gm == null) { Debug.LogError("cannot find cat GameManager script"); }
        _animator = transform.gameObject.GetComponentInChildren<Animator>();
        if (GameManager.Instance.IsDebug && _animator == null) { Debug.LogError("cannot find cat Animator"); }
        _keyleft = _gameManager.CatLeft;
        _keyright = _gameManager.CatRight;
        _keyup = _gameManager.CatUp;
        _keydown = _gameManager.CatDown;
        _keyjump = _gameManager.CatJump;
        _keyAction = _gameManager.CatAction;
        _animator.Play("Idle");
        GameManager.Instance.CatScoreText.text = "0";
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
            if (GameManager.Instance.IsDebug)
            {
                Debug.Log($"breaking stops !!!");
            }

            _isBreaking = false;
            StopCoroutine(BreakStuff(null)); // TODO really need this?
            GameManager.Instance.CatIcon.SetActive(false);
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
            _rigidbody2D.velocity = new Vector2(GameManager.Instance.CatSpeed * -1, _rigidbody2D.velocity.y);
            _stoped = false;
            move = true;
            _face = Face.Left;
            _changeFace = true;
            _animator.Play("Walk");
            //_audioSource.PlayOneShot(WalkSounds);
            if (!GameManager.Instance.CatWalkSounds.isPlaying)
            {
                GameManager.Instance.CatWalkSounds.Play();
            }
        }
        if (Input.GetKey(_keyright))
        {
            //if (IsDebug) { Debug.Log("cat right"); }
            _rigidbody2D.velocity = new Vector2(GameManager.Instance.CatSpeed, _rigidbody2D.velocity.y);
            _stoped = false;
            move = true;
            _face = Face.Right;
            _changeFace = true;

            _animator.Play("Walk");
            //_audioSource.PlayOneShot(WalkSounds);
            if (!GameManager.Instance.CatWalkSounds.isPlaying)
            {
                GameManager.Instance.CatWalkSounds.Play();
            }
        }
        


        if (!move && !_stoped)
        {
            //if (IsDebug) { Debug.Log("cat stop"); }
            _rigidbody2D.velocity = Vector2.zero;
            _stoped = true;
            _animator.Play("Idle");
            if (GameManager.Instance.CatWalkSounds.isPlaying)
            {
                GameManager.Instance.CatWalkSounds.Stop();
            }
        }
    }

    private void DoJump()
    {
        if (_isGround && Input.GetKey(_keyjump))
        {
            if (GameManager.Instance.IsDebug) { Debug.Log("cat jump"); }
            _rigidbody2D.AddForce(Vector2.up * GameManager.Instance.CatJumpSpeed, ForceMode2D.Force );
            _isGround = false;
            _animator.Play("Jump");  //TODO can we start in middle anim?
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "ground")
        {
            _isGround = true;
            if (GameManager.Instance.IsDebug) { Debug.Log($"cat ground is: {_isGround}"); }
        }
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        //if (IsDebug) { Debug.Log($"cat colliding with {other.gameObject.name}"); }
        if (other.gameObject.tag == "ladder")
        {
            if (Input.GetKey(_keyup))
            {
                GameManager.Instance.HandleLadders(transform, other, GameManager.Instance.CatLadderSpeed, Ladder.Up);
            }
            else if (Input.GetKey(_keydown))
            {
                GameManager.Instance.HandleLadders(transform, other, GameManager.Instance.CatLadderSpeed, Ladder.Down);
            }
        }
        else if (other.gameObject.tag == "item")
        {
            if (!_isBreaking && Input.GetKey(_keyAction))
            {
                if (!GameManager.Instance.CatTalkSounds.isPlaying)
                {
                    GameManager.Instance.CatTalkSounds.Play();
                }
                if (GameManager.Instance.IsDebug)
                {
                    Debug.Log($"breaking starts");
                }
                _animator.Play("sound");
                GameManager.Instance.CatIcon.SetActive(true);
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
            if (GameManager.Instance.IsDebug) { Debug.Log($"cat break {other.gameObject.name} successfully"); }
            var script = other.GetComponent<IItemDestroyAndFixScript>();
            if (script.HitItem(false))
            {
                PlayRandomSFX();
                _gameManager.CatScore+=2;
                ShowScore();
            }
            GameManager.Instance.CatIcon.SetActive(false);
            _isBreaking = false;
        }
    }

    private void ShowScore()
    {
        GameManager.Instance.CatScoreText.text = _gameManager.CatScore.ToString();
    }
    
    private void PlayRandomSFX()
    {
        var rand = Random.Range(0, 3);
        if (!GameManager.Instance.CatSFX[rand].isPlaying)
        {
            GameManager.Instance.CatSFX[rand].Play();
        }
    }
}
