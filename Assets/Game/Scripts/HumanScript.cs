using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class HumanScript : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private AudioSource _audioSource;
    private KeyCode _keyleft;
    private KeyCode _keyright;
    private KeyCode _keyup;
    private KeyCode _keydown;
    private KeyCode _keyjump;
    private KeyCode _keyAction;
    private bool stoped = true;
    private Face _face = Face.Left;
    private bool _changeFace = false;
    private bool _isGround = false;
    private bool _tuchedCat = false;
    private bool _isFixing = false;

    void Start()
    {
        if (GameManager.Instance.IsDebug) { Debug.Log("*** HumanScript debug is on ***"); }
        _rigidbody2D = GetComponent<Rigidbody2D>();
        if (GameManager.Instance.IsDebug && _rigidbody2D == null) { Debug.LogError("cannot find human Rigidbody2D"); }
        var gm = GameObject.Find("GameManager");
        if (GameManager.Instance.IsDebug && gm == null) { Debug.LogError("cannot find human GameManager"); }
        if (GameManager.Instance.IsDebug && gm == null) { Debug.LogError("cannot find human GameManager script"); }
        _animator = transform.gameObject.GetComponentInChildren<Animator>();
        if (GameManager.Instance.IsDebug && _animator == null) { Debug.LogError("cannot find human Animator"); }
        _keyleft = GameManager.Instance.HumanLeft;
        _keyright = GameManager.Instance.HumanRight;
        _keyup = GameManager.Instance.HumanUp;
        _keydown = GameManager.Instance.HumanDown;
        _keyjump = GameManager.Instance.HumanJump;
        _keyAction = GameManager.Instance.HumanAction;
        _animator.Play("Idle");
    }

    void LateUpdate()
    {
        if (!GameManager.Instance.GamePaused)
        {
            StopFix();
            Move();
            ChangeFace();
            DoJump();
        }
    }

    private void StopFix()
    {
        if (_isFixing && IsHumanInput() && !Input.GetKey(_keyAction) && Input.anyKey)
        {
            if (GameManager.Instance.IsDebug)
            {
                Debug.Log($"breaking stops !!!");
            }

            _isFixing = false;
            StopCoroutine(FixStuff(null)); // TODO really need this?
            GameManager.Instance.HumanIcon.SetActive(false);
        }
    }

    private bool IsHumanInput()
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
                //if (IsDebug) { Debug.Log("human turn left"); }

                //0 to 180
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (_face == Face.Right)
            {
                //if (IsDebug) { Debug.Log("human turn right"); }
                //180 t0 0
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            _changeFace = false;
        }
    }
    
    private void Move()
    {
        var move = false;
        if (Input.GetKey(_keyleft))
        {
            //if (IsDebug) { Debug.Log("human left"); }
            _rigidbody2D.velocity = new Vector2(GameManager.Instance.HumanSpeed * -1, _rigidbody2D.velocity.y);
            stoped = false;
            move = true;
            _face = Face.Left;
            _changeFace = true;
            _animator.Play("Walk");
            //_audioSource.PlayOneShot(WalkSounds);
            if (!GameManager.Instance.HumanWalkSounds.isPlaying)
            {
                GameManager.Instance.HumanWalkSounds.Play();
            }
        }
        if (Input.GetKey(_keyright))
        {
            //if (IsDebug) { Debug.Log("human right"); }
            _rigidbody2D.velocity = new Vector2(GameManager.Instance.HumanSpeed, _rigidbody2D.velocity.y);
            stoped = false;
            move = true;
            _face = Face.Right;
            _changeFace = true;
            _animator.Play("Walk");
            if (!GameManager.Instance.HumanWalkSounds.isPlaying)
            {
                GameManager.Instance.HumanWalkSounds.Play();
            }
        }

        if (!move && !stoped)
        {
            if (GameManager.Instance.HumanWalkSounds.isPlaying)
            {
                GameManager.Instance.HumanWalkSounds.Stop();
            }
            //if (IsDebug) { Debug.Log("human stop"); }
            _rigidbody2D.velocity = Vector2.zero;
            stoped = true;
            _animator.Play("Idle");
        }
    }
    
    private void DoJump()
    {
        if (_isGround && Input.GetKey(_keyjump))
        {
            if (GameManager.Instance.IsDebug) { Debug.Log("human jump"); }
            _rigidbody2D.AddForce(Vector2.up * GameManager.Instance.HumanJumpSpeed, ForceMode2D.Force );
            _isGround = false;
            _animator.Play("Jump");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "ground")
        {
            _isGround = true;
            if (GameManager.Instance.IsDebug) { Debug.Log("human ground is: "+_isGround); }
        }
        if(collision.gameObject.tag =="cat")
        {
            if (GameManager.Instance.IsDebug) { Debug.Log("human catched the cat"); }
            //collision.transform.Translate(CubePos.transform.position);
            collision.gameObject.transform.position = GameManager.Instance.CubePos.transform.position;
            GameManager.Instance.cage_script.Lock();
            //_audioSource.PlayOneShot(TalkSounds);
            if (!GameManager.Instance.HumanTalkSounds.isPlaying)
            {
                GameManager.Instance.HumanTalkSounds.Play();
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        //if (IsDebug) { Debug.Log($"human colliding with {other.gameObject.name}"); }
        if (other.gameObject.tag == "ladder")
        {
            if (Input.GetKey(_keyup))
            {
                GameManager.Instance.HandleLadders(transform, other, GameManager.Instance.HumanLadderSpeed, Ladder.Up);
            }
            else if (Input.GetKey(_keydown))
            {
                GameManager.Instance.HandleLadders(transform, other, GameManager.Instance.HumanLadderSpeed, Ladder.Down);
            }
        }
        else if (other.gameObject.tag == "item")
        {
            if (!_isFixing && Input.GetKey(_keyAction))
            {
                if (GameManager.Instance.IsDebug)
                {
                    Debug.Log($"fixing starts");
                }
                //_animator.Play("sound");
                GameManager.Instance.HumanIcon.SetActive(true);
                _isFixing = true;
                _animator.Play("Mad");
                if (!GameManager.Instance.HumanTalkSounds.isPlaying)
                {
                    GameManager.Instance.HumanTalkSounds.Play();
                }
                StartCoroutine(FixStuff(other));
            }
        }
    }

    IEnumerator FixStuff(Collider2D other)
    {
        yield return new WaitForSeconds(2);
        if (_isFixing)
        {
            if (GameManager.Instance.IsDebug) { Debug.Log($"human fix {other.gameObject.name} successfully"); }
            var script = other.GetComponent<IItemDestroyAndFixScript>();
            if (script.HitItem(true))
            {
                PlayRandomSFX();
                GameManager.Instance.HumanScore+=3;
                GameManager.Instance.ShowScore();
            }
            GameManager.Instance.HumanIcon.SetActive(false);
            _isFixing = false;
        }
    }

    private void PlayRandomSFX()
    {
        var rand = Random.Range(0, 3);
        if (!GameManager.Instance.HumanSFX[rand].isPlaying)
        {
            GameManager.Instance.HumanSFX[rand].Play();
        }
    }

    public void OnButtonEnd()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
