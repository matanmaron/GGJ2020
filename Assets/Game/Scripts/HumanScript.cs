using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class HumanScript : MonoBehaviour
{
    [SerializeField] bool IsDebug = false;
    [SerializeField] float Speed = 1;
    [SerializeField] private GameObject PausePanel;
    [SerializeField] private GameObject UIPanel;
    [SerializeField] float JumpSpeed = 10;
    [SerializeField] GameObject Icon;
    [SerializeField] CageScript cage_script;
    [SerializeField] GameObject CubePos;
    [SerializeField] AudioSource TalkSounds;
    [SerializeField] AudioSource WalkSounds;
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
    private bool stoped = true;
    private Face _face = Face.Left;
    private bool _changeFace = false;
    private bool _isGround = false;
    private bool _tuchedCat = false;
    private bool _isFixing = false;
    [SerializeField] Text CatScore;
    [SerializeField] Text HumanScore;
    [SerializeField] Text UITimer;
    private float _timer;
    void Start()
    {
        if (IsDebug) { Debug.Log("*** HumanScript debug is on ***"); }
        _rigidbody2D = GetComponent<Rigidbody2D>();
        if (IsDebug && _rigidbody2D == null) { Debug.Log("cannot find human Rigidbody2D"); }
        var gm = GameObject.Find("GameManager");
        if (IsDebug && gm == null) { Debug.Log("cannot find human GameManager"); }
        _gameManager = gm.GetComponent<GameManager>();
        if (IsDebug && gm == null) { Debug.Log("cannot find human GameManager script"); }
        _animator = transform.gameObject.GetComponentInChildren<Animator>();
        if (IsDebug && _animator == null) { Debug.Log("cannot find human Animator"); }
        _keyleft = _gameManager.HumanLeft;
        _keyright = _gameManager.HumanRight;
        _keyup = _gameManager.HumanUp;
        _keydown = _gameManager.HumanDown;
        _keyjump = _gameManager.HumanJump;
        _keyAction = _gameManager.HumanAction;
        _animator.Play("Idle");
        HumanScore.text = "0";
        CatScore.text = "0";
        _timer = 120;
    }

    private void Update()
    {
        _timer = _timer - Time.deltaTime;
        UITimer.text =  Mathf.RoundToInt(_timer).ToString();
        if (_timer<=0)
        {
            //gameover
            if (!_gameManager.GamePaused)
            {//only in human ! no need to copy on cat!
                _gameManager.GamePaused = !_gameManager.GamePaused;
                PausePanel.SetActive(_gameManager.GamePaused);
                UIPanel.SetActive(!_gameManager.GamePaused);
            }
        }
    }

    void LateUpdate()
    {
        if (!_gameManager.GamePaused  && Input.GetKey(KeyCode.Escape))
        {//only in human ! no need to copy on cat!
            _gameManager.GamePaused = !_gameManager.GamePaused;
            PausePanel.SetActive(_gameManager.GamePaused);
            UIPanel.SetActive(!_gameManager.GamePaused);
        }
        if (!_gameManager.GamePaused)
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
            if (IsDebug)
            {
                Debug.Log($"breaking stops !!!");
            }

            _isFixing = false;
            StopCoroutine(FixStuff(null)); // TODO really need this?
            Icon.SetActive(false);
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
            _rigidbody2D.velocity = new Vector2(Speed * -1, _rigidbody2D.velocity.y);
            stoped = false;
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
            //if (IsDebug) { Debug.Log("human right"); }
            _rigidbody2D.velocity = new Vector2(Speed, _rigidbody2D.velocity.y);
            stoped = false;
            move = true;
            _face = Face.Right;
            _changeFace = true;
            _animator.Play("Walk");
            if (!WalkSounds.isPlaying)
            {
                WalkSounds.Play();
            }
        }

        if (!move && !stoped)
        {
            if (WalkSounds.isPlaying)
            {
                WalkSounds.Stop();
            }
            //if (IsDebug) { Debug.Log("human stop"); }
            _rigidbody2D.velocity = Vector2.zero;
            stoped = true;
            _animator.Play("Idle");
        }
    }
    
    public void OnButtonResume()
    {
        _gameManager.GamePaused = !_gameManager.GamePaused;
        PausePanel.SetActive(_gameManager.GamePaused);
        UIPanel.SetActive(!_gameManager.GamePaused);
        if (IsDebug) { Debug.Log("pause is " + _gameManager.GamePaused); }
    }
    
    private void DoJump()
    {
        if (_isGround && Input.GetKey(_keyjump))
        {
            if (IsDebug) { Debug.Log("human jump"); }
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
            if (IsDebug) { Debug.Log("human ground is: "+_isGround); }
        }
        if(collision.gameObject.tag =="cat")
        {
            if (IsDebug) { Debug.Log("human catched the cat"); }
            //collision.transform.Translate(CubePos.transform.position);
            collision.gameObject.transform.position = CubePos.transform.position;
            cage_script.Lock();
            //_audioSource.PlayOneShot(TalkSounds);
            if (!TalkSounds.isPlaying)
            {
                TalkSounds.Play();
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
                HandleLadders(other, Ladder.Up);
            }
            else if (Input.GetKey(_keydown))
            {
                HandleLadders(other, Ladder.Down);
            }
        }
        else if (other.gameObject.tag == "item")
        {
            if (!_isFixing && Input.GetKey(_keyAction))
            {
                if (IsDebug)
                {
                    Debug.Log($"fixing starts");
                }
                //_animator.Play("sound");
                Icon.SetActive(true);
                _isFixing = true;
                _animator.Play("Mad");
                if (!TalkSounds.isPlaying)
                {
                    TalkSounds.Play();
                }
                StartCoroutine(FixStuff(other));
            }
        }
    }

    private void HandleLadders(Collider2D other, Ladder ladder)
    {
        var num = other.name[1];
        if (num == 52 || num == 53)
        {
            return;
        }
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

    IEnumerator FixStuff(Collider2D other)
    {
        yield return new WaitForSeconds(2);
        if (_isFixing)
        {
            if (IsDebug) { Debug.Log($"human fix {other.gameObject.name} successfully"); }
            var script = other.GetComponent<IItemDestroyAndFixScript>();
            if (script.HitItem(true))
            {
                PlayRandomSFX();
                if (_gameManager.CatScore>0)
                {
                    _gameManager.CatScore--;
                }
                _gameManager.HumanScore++;
                ShowScore();
            }
            Icon.SetActive(false);
            _isFixing = false;
        }
    }

    private void ShowScore()
    {
        CatScore.text = _gameManager.CatScore.ToString();
        HumanScore.text = _gameManager.HumanScore.ToString();
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
